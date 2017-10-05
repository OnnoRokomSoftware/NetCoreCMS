using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.OnlineExam.Models;
using NetCoreCMS.Modules.OnlineExam.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.OnlineExam.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Online Exam", IconCls = "fa-link", Order = 100)]
    public class OeStudentQuestionSetController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private OeSubjectService _oeSubjectService;
        private OeExamService _oeExamService;
        private OeQuestionService _oeQuestionService;
        private OeStudentService _oeStudentService;
        private OeStudentQuestionSetService _oeStudentQuestionSetService;
        private OeStudentQuestionSetDetailsService _oeStudentQuestionSetDetailsService;


        private OnlineExamSettings nccOnlineExamSettings;

        public OeStudentQuestionSetController(NccSettingsService nccSettingsService, ILoggerFactory factory, OeSubjectService oeSubjectService, OeExamService oeExamService, OeQuestionService oeQuestionService, OeStudentService oeStudentService, OeStudentQuestionSetService oeStudentQuestionSetService, OeStudentQuestionSetDetailsService oeStudentQuestionSetDetailsService)
        {
            _logger = factory.CreateLogger<OeExamController>();
            nccOnlineExamSettings = new OnlineExamSettings();

            _nccSettingsService = nccSettingsService;
            _oeSubjectService = oeSubjectService;
            _oeExamService = oeExamService;
            _oeQuestionService = oeQuestionService;
            _oeStudentService = oeStudentService;
            _oeStudentQuestionSetService = oeStudentQuestionSetService;
            _oeStudentQuestionSetDetailsService = oeStudentQuestionSetDetailsService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_OnlineExam_Settings");
                if (tempSettings != null)
                {
                    nccOnlineExamSettings = JsonConvert.DeserializeObject<OnlineExamSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Student Result", Url = "/OeStudentQuestionSet/Manage", IconCls = "fa-file", Order = 5)]
        public ActionResult Manage()
        {
            var itemList = _oeStudentQuestionSetService.LoadAll().OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }
        #endregion

        #region User Panel
        #endregion

        #region Helper
        #endregion
    }
}