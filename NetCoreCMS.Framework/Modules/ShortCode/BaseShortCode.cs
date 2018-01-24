using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Modules.ShortCode
{
    public class BaseShortCode : IShortCode
    {
        private IViewRenderService _viewRenderService;
        public Type ModuleController { get; set; }
        private DateTime _lastRenderTime;
        private int _minCacheDuration = 10;
        private string _htmlContent = "";

        public BaseShortCode(Type moduleControllerType, string name, string viewFileName)
        {
            ModuleController = moduleControllerType;
            ShortCodeName = name;
            ViewFileName = viewFileName;
        }

        public string ShortCodeName { get; }
        public string ViewFileName { get; }


        public virtual object PrepareViewModel(params object[] paramiters)
        {
            return null;
        }

        public string Render(params object[] paramiters)
        {
            if (string.IsNullOrEmpty(_htmlContent) || (GlobalContext.WebSite.EnableCache == false && (_lastRenderTime - DateTime.Now >= new TimeSpan(0, 0, _minCacheDuration))))
            {
                _lastRenderTime = DateTime.Now;
                var model = PrepareViewModel(paramiters);
                _viewRenderService = GlobalContext.GetViewRenerService();
                _htmlContent = _viewRenderService.RenderToString(ModuleController, ViewFileName, model);
            }

            return _htmlContent;
        }
    }
}