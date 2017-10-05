using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.StudentBoardResult.Models;
using NetCoreCMS.Modules.StudentBoardResult.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.StudentBoardResult.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Board Result", IconCls = "fa-database", Order = 110)]
    public class SbrStudentController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private SbrBoardService _sbrBoardService;
        private SbrGroupService _sbrGroupService;
        private SbrExamService _sbrExamService;
        private SbrExamSubjectService _sbrExamSubjectService;
        private SbrStudentService _sbrStudentService;
        private SbrStudentResultService _sbrStudentResultService;
        private SbrStudentResultDetailsService _sbrStudentResultDetailsService;

        private SbrSettings sbrSettings;

        public SbrStudentController(NccSettingsService nccSettingsService, ILoggerFactory factory, SbrBoardService sbrBoardService, SbrGroupService sbrGroupService, SbrExamService sbrExamService, SbrExamSubjectService sbrExamSubjectService, SbrStudentService sbrStudentService, SbrStudentResultService sbrStudentResultService, SbrStudentResultDetailsService sbrStudentResultDetailsService)
        {
            _logger = factory.CreateLogger<SbrStudentController>();
            sbrSettings = new SbrSettings();

            _nccSettingsService = nccSettingsService;
            _sbrBoardService = sbrBoardService;
            _sbrGroupService = sbrGroupService;
            _sbrExamService = sbrExamService;
            _sbrExamSubjectService = sbrExamSubjectService;
            _sbrStudentService = sbrStudentService;
            _sbrStudentResultService = sbrStudentResultService;
            _sbrStudentResultDetailsService = sbrStudentResultDetailsService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_Sbr_Settings");
                if (tempSettings != null)
                {
                    sbrSettings = JsonConvert.DeserializeObject<SbrSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Student", Url = "/SbrStudent/Manage", IconCls = "fa-arrow-right", Order = 10)]
        public ActionResult Manage(int page = 0, int count = 10)
        {
            var itemList = _sbrStudentService.LoadAll(false).OrderByDescending(x => x.Id).Skip(page * count).Take(count).ToList(); ;
            return View(itemList);
        }

        public ActionResult CreateEdit(long Id = 0)
        {
            var groupList = _sbrGroupService.LoadAll(true);
            List <string> sscYear = new List<string>();
            List<string> hscYear = new List<string>();
            for (int i = 2; i < 9; i++)
            {
                sscYear.Add((DateTime.Now.Year - i).ToString());
            }
            for (int i = 0; i < 5; i++)
            {
                hscYear.Add((DateTime.Now.Year - i).ToString());
            }

            var GenderList = Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            var ReligionList = Enum.GetValues(typeof(Religion)).Cast<Religion>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            var BoardList = _sbrBoardService.LoadAll(true);
            ViewBag.SscYearList = new SelectList(sscYear, 0);
            ViewBag.HscYearList = new SelectList(hscYear, 0);
            //ViewBag.ExamList = new SelectList(_sbrExamService.LoadAll(true), "Id", "Name", 0);
            ViewBag.SscGroupList = new SelectList(groupList, "Id", "Name", 0);
            ViewBag.HscGroupList = new SelectList(groupList, "Id", "Name", 0);
            ViewBag.SscBoardList = new SelectList(BoardList, "Id", "Name", 0);
            ViewBag.HscBoardList = new SelectList(BoardList, "Id", "Name", 0);
            ViewBag.GenderList = new SelectList(GenderList, "Value", "Text");
            ViewBag.ReligionList = new SelectList(ReligionList, "Value", "Text");


            SbrStudentViewModel item = new SbrStudentViewModel();
            SbrStudent student = new SbrStudent();

            if (Id > 0)
            {
                student = _sbrStudentService.Get(Id);
                item.StudentId = student.Id;
                item.Name = student.Name;
                item.Status = student.Status;
                item.FatherName = student.FatherName;
                item.MotherName = student.MotherName;
                item.MobileNumber = student.MobileNumber;
                item.DateOfBirth = student.DateOfBirth;
                item.Imagepath = student.Imagepath;
                item.GenderId = (int)student.Gender;
                item.ReligionId = (int)student.Religion;

                SbrExam sscExam = _sbrExamService.LoadAll(true, -1, "SSC").FirstOrDefault();
                SbrStudentResult ssc = _sbrStudentResultService.Get(student.Id, sscExam.Id);
                if (ssc != null)
                {
                    item.SscResultId = ssc.Id;
                    item.SscBoardId = ssc.Board.Id;
                    item.SscGroupId = ssc.Group.Id;
                    item.SscRollNo = ssc.RollNo;
                    item.SscRegistrationNo = ssc.RegistrationNo;
                    item.SscYear = ssc.Year;
                    item.SscGpa = ssc.Gpa;
                    item.SscGpaWithout4th = ssc.GpaWithout4th;
                    item.SscGrade = ssc.Grade;
                }
                SbrExam hscExam = _sbrExamService.LoadAll(true, -1, "HSC").FirstOrDefault();
                SbrStudentResult hsc = _sbrStudentResultService.Get(student.Id, hscExam.Id);
                if (hsc != null)
                {
                    item.HscResultId = hsc.Id;
                    item.HscBoardId = hsc.Board.Id;
                    item.HscGroupId = hsc.Group.Id;
                    item.HscRollNo = hsc.RollNo;
                    item.HscRegistrationNo = hsc.RegistrationNo;
                    item.HscYear = hsc.Year;
                    item.HscGpa = hsc.Gpa;
                    item.HscGpaWithout4th = hsc.GpaWithout4th;
                    item.HscGrade = hsc.Grade;
                }
                ViewBag.SscYearList = new SelectList(sscYear, item.SscYear);
                ViewBag.HscYearList = new SelectList(hscYear, item.HscYear);
                //ViewBag.ExamList = new SelectList(_sbrExamService.LoadAll(true), "Id", "Name", 0);
                ViewBag.SscGroupList = new SelectList(groupList, "Id", "Name", item.SscGroupId);
                ViewBag.HscGroupList = new SelectList(groupList, "Id", "Name", item.HscGroupId);
                ViewBag.SscBoardList = new SelectList(BoardList, "Id", "Name", item.SscBoardId);
                ViewBag.HscBoardList = new SelectList(BoardList, "Id", "Name", item.HscBoardId);
                ViewBag.GenderList = new SelectList(GenderList, "Value", "Text", item.GenderId);
                ViewBag.ReligionList = new SelectList(ReligionList, "Value", "Text", item.ReligionId);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(SbrStudentViewModel model, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";
            if (ModelState.IsValid)
            {
                SbrStudent stu = model.StudentId > 0 ? _sbrStudentService.Get(model.StudentId) : new SbrStudent();
                stu.Id = model.StudentId;
                stu.Name = model.Name;
                stu.Status = model.Status;
                stu.FatherName = model.FatherName;
                stu.MotherName = model.MotherName;
                stu.MobileNumber = model.MobileNumber;
                stu.DateOfBirth = model.DateOfBirth;
                stu.Imagepath = model.Imagepath;
                stu.Gender = (Gender)model.GenderId;
                stu.Religion = (Religion)model.ReligionId;

                SbrExam sscExam = _sbrExamService.LoadAll(true, -1, "SSC").FirstOrDefault();
                SbrStudentResult ssc = model.SscResultId > 0 ? _sbrStudentResultService.Get(model.SscResultId) : new SbrStudentResult();
                ssc.Id = model.SscResultId;
                ssc.Exam = sscExam;
                ssc.Board = _sbrBoardService.Get(model.SscBoardId);
                ssc.Group = _sbrGroupService.Get(model.SscGroupId);
                ssc.RollNo = model.SscRollNo;
                ssc.RegistrationNo = model.SscRegistrationNo;
                ssc.Year = model.SscYear;
                ssc.Gpa = model.SscGpa;
                ssc.GpaWithout4th = model.SscGpaWithout4th;
                ssc.Grade = model.SscGrade;

                SbrExam hscExam = _sbrExamService.LoadAll(true, -1, "HSC").FirstOrDefault();
                SbrStudentResult hsc = model.HscResultId > 0 ? _sbrStudentResultService.Get(model.HscResultId) : new SbrStudentResult();
                hsc.Id = model.HscResultId;
                hsc.Exam = hscExam;
                hsc.Board = _sbrBoardService.Get(model.HscBoardId);
                hsc.Group = _sbrGroupService.Get(model.HscGroupId);
                hsc.RollNo = model.HscRollNo;
                hsc.RegistrationNo = model.HscRegistrationNo;
                hsc.Year = model.HscYear;
                hsc.Gpa = model.HscGpa;
                hsc.GpaWithout4th = model.HscGpaWithout4th;
                hsc.Grade = model.HscGrade;

                if (model.StudentId > 0)
                {
                    _sbrStudentService.Update(stu);
                    isSuccess = true;
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Data updated successfull.";
                }
                else
                {
                    _sbrStudentService.Save(stu);

                    isSuccess = true;
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Data saved successfull.";
                }
                if (isSuccess)
                {
                    ssc.Student = stu;
                    hsc.Student = stu;
                    if (model.SscResultId > 0)
                    {
                        _sbrStudentResultService.Update(ssc);
                    }
                    else
                    {
                        _sbrStudentResultService.Save(ssc);
                    }
                    if (model.HscResultId > 0)
                    {
                        _sbrStudentResultService.Update(hsc);
                    }
                    else
                    {
                        _sbrStudentResultService.Save(hsc);
                    }
                }

            }
            if (isSuccess == true)
            {
                if (save == "Save")
                {
                    return RedirectToAction("Manage");
                }
                else
                {
                    model = new SbrStudentViewModel();
                }
            }

            #region default value set
            var groupList = _sbrGroupService.LoadAll(true);
            List<string> sscYear = new List<string>();
            List<string> hscYear = new List<string>();
            for (int i = 2; i < 9; i++)
            {
                sscYear.Add((DateTime.Now.Year - i).ToString());
            }
            for (int i = 0; i < 5; i++)
            {
                hscYear.Add((DateTime.Now.Year - i).ToString());
            }

            var GenderList = Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            var ReligionList = Enum.GetValues(typeof(Religion)).Cast<Religion>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            var BoardList = _sbrBoardService.LoadAll(true);

            ViewBag.SscYearList = new SelectList(sscYear, model.SscYear);
            ViewBag.HscYearList = new SelectList(hscYear, model.HscYear);
            ViewBag.SscGroupList = new SelectList(groupList, "Id", "Name", model.SscGroupId);
            ViewBag.HscGroupList = new SelectList(groupList, "Id", "Name", model.HscGroupId);
            ViewBag.SscBoardList = new SelectList(BoardList, "Id", "Name", model.SscBoardId);
            ViewBag.HscBoardList = new SelectList(BoardList, "Id", "Name", model.HscBoardId);
            ViewBag.GenderList = new SelectList(GenderList, "Value", "Text", model.GenderId);
            ViewBag.ReligionList = new SelectList(ReligionList, "Value", "Text", model.ReligionId);            
            #endregion

            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _sbrStudentService.Get(Id);
                if (item.Status == EntityStatus.Active)
                {
                    item.Status = EntityStatus.Inactive;
                    ViewBag.Message = "In-activated successfull.";
                }
                else
                {
                    item.Status = EntityStatus.Active;
                    ViewBag.Message = "Activated successfull.";
                }

                _sbrStudentService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            SbrStudent item = _sbrStudentService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _sbrStudentService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}