using Festispec.Models.Answers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Web.Models
{
    public class AnswerModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var subclasses = new[] { typeof(StringAnswer), typeof(MultipleChoiceAnswer), typeof(NumericAnswer), typeof(FileAnswer) };

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new AnswerModelBinder(binders);
        }
    }
}
