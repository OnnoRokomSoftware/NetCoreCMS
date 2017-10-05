using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.UmsBdRedirect.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.UmsBdRedirect.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "UmsBdRedirect", IconCls = "", Order = 100)]
    public class UmsBdRedirectController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;

        private NccUmsBdRedirect nccUmsBdRedirect;

        public UmsBdRedirectController(NccSettingsService nccSettingsService, ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<UmsBdRedirectController>();
            _nccSettingsService = nccSettingsService;
            var tempSettings = _nccSettingsService.GetByKey("UmsBdRedirect_Settings");
            if (tempSettings != null)
            {
                nccUmsBdRedirect = JsonConvert.DeserializeObject<NccUmsBdRedirect>(tempSettings.Value);
            }
        }
        #endregion

        #region Admin Panel
        #endregion

        #region User Panel
        [AllowAnonymous]
        [HttpGet("/r/{codeNo}", Name = "Udvash")]
        public ActionResult UdvashResult(string codeNo)
        {
            return Redirect("https://new.udvash.com/Result/SolveSheetDownload/" + codeNo);
        }

        [AllowAnonymous]
        [HttpGet("/s/{codeNo}", Name = "Unmesh")]
        public ActionResult UnmeshResult(string codeNo)
        {
            return Redirect("http://unmeshbd.com/Result/SolveSheetDownload/" + codeNo);
        }
        #endregion
    }
}