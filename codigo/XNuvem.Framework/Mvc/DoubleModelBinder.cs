﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XNuvem.Mvc
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext) {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try {
                try {
                    actualValue = Convert.ToDouble(valueResult.AttemptedValue,
                        CultureInfo.InvariantCulture);
                }
                catch (FormatException) {
                    actualValue = Convert.ToDouble(valueResult.AttemptedValue,
                        CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e) {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}
