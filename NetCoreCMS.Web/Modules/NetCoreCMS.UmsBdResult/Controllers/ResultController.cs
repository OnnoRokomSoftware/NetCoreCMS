/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.UmsBdResult.Models;
using NetCoreCMS.UmsBdResult.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.UmsBdResult.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "UmsBdResult", IconCls = "fa-cogs", Order = 100)]
    [SiteMenu(Name = "Result", Order = 100)]
    public class ResultController : NccController
    {
        #region Initialization
        private UmsBdResultService _umsBdResultService;
        private UmsBdResultApiService _umsBdResultApiService;
        private UmsBdResultSettings _settings;
        private NccSettingsService _nccSettingsService;
        public ResultController(UmsBdResultService umsBdResultService, NccSettingsService nccSettingsService, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<ResultController>();
            _umsBdResultService = umsBdResultService;
            _umsBdResultApiService = new UmsBdResultApiService();
            //_settings = _umsBdResultService.GetSettings();
            _nccSettingsService = nccSettingsService;
            var tempSettings = _nccSettingsService.GetByKey("UmsBdResult_Settings");
            if (tempSettings != null)
            {
                _settings = JsonConvert.DeserializeObject<UmsBdResultSettings>(tempSettings.Value);
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Settings", Url = "/Result/Settings", IconCls = "fa-arrow-right", Order = 1)]
        public ActionResult Settings()
        {
            if (_settings == null)
                _settings = new UmsBdResultSettings();
            return View(_settings);
        }

        [HttpPost]
        public ActionResult Settings(UmsBdResultSettings model)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                _nccSettingsService.SetByKey("UmsBdResult_Settings", JsonConvert.SerializeObject(model));
                ViewBag.MessageType = "SuccessMessage";
                ViewBag.Message = "Settings updated successfull.";
                //if (model.Id > 0)
                //{
                //    _umsBdResultService.Update(model);
                //    ViewBag.MessageType = "SuccessMessage";
                //    ViewBag.Message = "Settings updated successfull.";
                //}
                //else
                //{
                //    _umsBdResultService.Save(model);
                //    ViewBag.MessageType = "SuccessMessage";
                //    ViewBag.Message = "Settings save successfull.";
                //}
            }

            return View(model);
        }
        #endregion

        #region Public Site
        [AllowAnonymous]
        [SiteMenuItem(Name = "Result", Url = "/Result", Order = 1)]
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(string.Empty, "Value", "Text");
            ViewBag.ExamList = new SelectList(string.Empty, "Value", "Text");

            return View(new UmsBdResultViewModel());
        }

        #region Ajax Helper
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> AjaxRequestForCourse(string prnNo)
        {
            try
            {
                if (string.IsNullOrEmpty(prnNo))
                    return Json(new { returnList = "Invalid Program Roll", IsSuccess = false });

                prnNo = prnNo.Trim();

                var resultList = await _umsBdResultApiService.LoadCourse(_settings.BaseApi, _settings.ApiKey, _settings.OrgBusinessId, prnNo);
                if (resultList.Data != null)
                {
                    var se = JsonConvert.SerializeObject(resultList.Data);
                    var finalResponse = JsonConvert.DeserializeObject<List<CommonObjectApi>>(se);
                    List<SelectListItem> resultListItem = new SelectList(finalResponse, "Id", "Name").ToList();
                    return Json(new { IsSuccess = true, returnList = resultListItem });
                }
                return Json(new { returnList = "Invalid Program Roll", IsSuccess = false });
            }
            catch (Exception ex)
            {
                return Json(new { returnList = ex.Message, IsSuccess = false });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> AjaxRequestForExamInformation(string courseId, string prnNo)
        {
            try
            {
                if (String.IsNullOrEmpty(prnNo))
                    return Json(new { returnList = "Invalid Program Roll", IsSuccess = false });

                if (String.IsNullOrEmpty(courseId))
                    return Json(new { returnList = "Please select a Course", IsSuccess = false });
                prnNo = prnNo.Trim();

                var resultList = await _umsBdResultApiService.LoadExam(_settings.BaseApi, _settings.ApiKey, _settings.OrgBusinessId, courseId, prnNo);
                if (resultList.Data != null)
                {
                    var se = JsonConvert.SerializeObject(resultList.Data);
                    var finalResponse = JsonConvert.DeserializeObject<List<CommonObjectApi>>(se);
                    List<SelectListItem> resultListItem = new SelectList(finalResponse, "Id", "Name").ToList();
                    return Json(new { IsSuccess = true, returnList = resultListItem });
                }
                return Json(new { returnList = "No exam found", IsSuccess = false });
            }
            catch (Exception ex)
            {
                return Json(new { returnList = ex.Message, IsSuccess = false });
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AjaxRequestForReport(string prnNo, string courseId, string examId)
        {
            try
            {
                if (!String.IsNullOrEmpty(prnNo) && !String.IsNullOrEmpty(courseId) && !String.IsNullOrEmpty(examId))
                {
                    IList<UmsBdResultIndivisualViewModel> ivmList = new List<UmsBdResultIndivisualViewModel>();
                    var resultList = await _umsBdResultApiService.GetStudentResult(_settings.BaseApi, _settings.ApiKey, _settings.OrgBusinessId, courseId, prnNo, examId);
                    if (resultList.Data != null)
                    {
                        var se = JsonConvert.SerializeObject(resultList.Data);
                        ivmList = JsonConvert.DeserializeObject<List<UmsBdResultIndivisualViewModel>>(se);
                    }
                    ViewBag.Result = ivmList;
                    ViewBag.PrnNo = prnNo.Trim();
                    ViewBag.ExamId = examId;
                    ViewBag.IsWord = ivmList.Select(x => x.IsWord).FirstOrDefault();
                    ViewBag.McqAnswer = (string.IsNullOrEmpty(ivmList.Select(x => x.McqMarks).FirstOrDefault()) == false) ? false : true;

                    return PartialView("_indivisualResult");
                }
                else
                {
                    return Json(new { returnList = "Please fill all fields", IsSuccess = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { returnList = ex.ToString(), IsSuccess = false });
            }
        }
        #endregion

        [AllowAnonymous]
        public async Task<FileResult> DownloadSolveSheet(string prnNo, string examId, string version)
        {
            try
            {
                if (!String.IsNullOrEmpty(prnNo) && !String.IsNullOrEmpty(examId) && !String.IsNullOrEmpty(version))
                {
                    string visitorIp = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    UmsBdResultSolveSheetViewModel solveSheet = new UmsBdResultSolveSheetViewModel();
                    var resultList = await _umsBdResultApiService.GetSolveSheetPdf(_settings.BaseApi, _settings.ApiKey, _settings.OrgBusinessId, prnNo, examId, version, visitorIp);
                    if (resultList.Data != null)
                    {
                        var se = JsonConvert.SerializeObject(resultList.Data);
                        solveSheet = JsonConvert.DeserializeObject<UmsBdResultSolveSheetViewModel>(se);
                        //System.IO.File.WriteAllBytes(@"D:\" + Guid.NewGuid().ToString() + ".pdf", Convert.FromBase64String(resultList.Data));
                        return File(Convert.FromBase64String(solveSheet.Content), "application/pdf", prnNo + "_" + solveSheet.ExamName + "_" + solveSheet.ExamVersion + ".pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return null;
        }

        [AllowAnonymous]
        [HttpGet("/Result/SolveSheetDownload/{codeNo}", Name = "Products_List")]
        public async Task<FileResult> SolveSheetDownload(string codeNo)
        {
            try
            {
                if (!String.IsNullOrEmpty(codeNo))
                {
                    string visitorIp = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    UmsBdResultSolveSheetViewModel solveSheet = new UmsBdResultSolveSheetViewModel();
                    var resultList = await _umsBdResultApiService.GetSolveSheetPdfByCode(_settings.BaseApi, _settings.ApiKey, _settings.OrgBusinessId, codeNo, visitorIp);
                    if (resultList.Data != null)
                    {
                        var se = JsonConvert.SerializeObject(resultList.Data);
                        solveSheet = JsonConvert.DeserializeObject<UmsBdResultSolveSheetViewModel>(se);
                        //System.IO.File.WriteAllBytes(@"D:\" + Guid.NewGuid().ToString() + ".pdf", Convert.FromBase64String(resultList.Data));
                        return File(Convert.FromBase64String(solveSheet.Content), "application/pdf", codeNo + "_" + solveSheet.ExamName + ".pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return File(new Byte[10], "application/pdf", codeNo + ".pdf");
        }
        #endregion
    }
}