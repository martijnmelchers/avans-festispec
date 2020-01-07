using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Enums;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Festispec.DomainServices.Services
{
    public class SyncService<T> where T : Entity
    {
        private readonly FestispecContext _db;
        private readonly string _jsonFile;
        private JObject _jsonObject;

        public SyncService(FestispecContext db)
        {
            _db = db;
            _jsonFile = FestispecPaths.FestispecOfflineStoragePath + "\\" + typeof(T).Name + ".json";
            
            // disable lazy loading on this specific context to avoid pulling in our entire database as json.
            _db.Configuration.LazyLoadingEnabled = false;
            
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    SerializationBinder = new EntityFrameworkSerializationBinder(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
        }

        private JObject JsonObject
        {
            get
            {
                if (_jsonObject == null)
                    Initialise();
                
                return _jsonObject;
            }
            set => _jsonObject = value;
        }

        private void Initialise()
        {
            if (!File.Exists(_jsonFile))
            {
                using (FileStream fileStream = File.Create(_jsonFile)) fileStream.Dispose();
                Flush();
            }

            ReadFile();
        }

        private void ReadFile()
        {
            JsonObject = JObject.Parse(File.ReadAllText(_jsonFile));
        }

        public IEnumerable<T> GetAll()
        {
            return ((JArray) JsonObject["items"]).Select(i =>
                JsonConvert.DeserializeObject<T>(i.ToString()));
        }

        public T GetEntity(int entityId)
        {
            var jObject = (JObject) ((JArray) JsonObject["items"])
                .First(j => j is JObject jO 
                            && jO.ContainsKey("Id") 
                            && (long) ((JValue) jO["Id"]).Value == entityId);

            if (jObject == null)
                throw new EntityNotFoundException();

            return JsonConvert.DeserializeObject<T>(jObject.ToString());
        }

        public async Task<T> GetEntityAsync(int entityId)
        {
            return await Task.Run(() => GetEntity(entityId));
        } 

        public void AddEntity(T entity)
        {
            var jArray = (JArray) JsonObject["items"];
            jArray.Add(JObject.Parse(JsonConvert.SerializeObject(entity)));
        }

        public void AddEntities(IEnumerable<T> entities)
        {
            foreach (T entity in entities) AddEntity(entity);
        }

        public void SaveChanges()
        {
            JsonObject["updatedAt"] = new JValue(DateTime.Now);
            File.WriteAllText(_jsonFile, JsonObject.ToString());
        }

        public async void SaveChangesAsync()
        {
            JsonObject["updatedAt"] = new JValue(DateTime.Now);
            await File.WriteAllTextAsync(_jsonFile, JsonObject.ToString());
        }

        public FestispecContext GetSyncContext() => _db;

        public void Flush()
        {
            var initialObject = new JObject {{"createdAt", new JValue(DateTime.Now)}, {"items", new JArray()}};
            File.WriteAllText(_jsonFile, initialObject.ToString());
            ReadFile();
        }

        private class EntityFrameworkSerializationBinder : DefaultSerializationBinder
        {
            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                // prevent the deserializer from looking in the dynamic proxies assembly, but instead look in our own.
                assemblyName = serializedType.Assembly.GetName().Name != "EntityFrameworkDynamicProxies-Models"
                    ? serializedType.Assembly.FullName
                    : serializedType.BaseType?.Assembly.FullName;

                typeName = serializedType.Namespace != "System.Data.Entity.DynamicProxies"
                    ? serializedType.FullName
                    : serializedType.BaseType?.FullName;
            }
        }
    }
}