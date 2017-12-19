using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.Provider
{
    public class NccApplicationModelProvider : IApplicationModelProvider
    {
        public NccApplicationModelProvider()
        {
            
        }

        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            var areaKey = "Areas";

            // This code is called only once per tenant during the construction of routes
            foreach (var controller in context.Result.Controllers)
            {
                var controllerType = controller.ControllerType.AsType();
                var controllerModule = controllerType.Assembly.GetName();
                var module = GlobalContext.GetModuleByAssemblyName(controllerModule);

                if (module != null)
                {
                    var areaName = module.Area;
                    if(string.IsNullOrWhiteSpace(areaName) == false)
                    {   
                        controller.RouteValues.Add("area", areaName);                     
                    }
                }                
            }
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
        }
    }
}
