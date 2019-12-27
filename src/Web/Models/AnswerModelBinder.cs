using Festispec.Models.Answers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Web.Models
{
    public class AnswerModelBinder : IModelBinder
    {


        private Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public AnswerModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelKindName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(Answer));
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelKindName).FirstValue;

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;
            //if (modelTypeValue == "Laptop")
            //{
            //    (modelMetadata, modelBinder) = binders[typeof(StringAnswer)];
            //}
            //else if (modelTypeValue == "SmartPhone")
            //{
            //    (modelMetadata, modelBinder) = binders[typeof(MultipleChoiceAnswer)];
            //}
            //else
            //{
            //    bindingContext.Result = ModelBindingResult.Failed();
            //    return;
            //}

            if (bindingContext.Model is MultipleChoiceAnswer)
            {
                (modelMetadata, modelBinder) = binders[typeof(MultipleChoiceAnswer)];
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);

            await modelBinder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result] = new ValidationStateEntry
                {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}
