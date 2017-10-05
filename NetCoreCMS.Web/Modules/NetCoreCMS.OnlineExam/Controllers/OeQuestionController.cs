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
    public class OeQuestionController : NccController
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

        public OeQuestionController(NccSettingsService nccSettingsService, ILoggerFactory factory, OeSubjectService oeSubjectService, OeExamService oeExamService, OeQuestionService oeQuestionService, OeStudentService oeStudentService, OeStudentQuestionSetService oeStudentQuestionSetService, OeStudentQuestionSetDetailsService oeStudentQuestionSetDetailsService)
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
        [AdminMenuItem(Name = "Manage Question", Url = "/OeQuestion/Manage", IconCls = "fa-question", Order = 3)]
        public ActionResult Manage(long examId = 0, int version = 0, int uniqueSet = 0, string searchKey = "")
        {
            var examList = _oeExamService.LoadAll();
            if (examId == 0 && examList.Count > 0)
                examId = examList.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            ViewBag.ExamList = new SelectList(examList, "Id", "Name", examId);
            ViewBag.Version = version;
            ViewBag.UniqueSet = uniqueSet;
            ViewBag.SearchKey = searchKey;
            //var itemList = _oeQuestionService.LoadAll(false).OrderByDescending(x => x.Id).ToList();
            var itemList = _oeQuestionService.Load(examId, 0, uniqueSet, 0, version, searchKey).OrderBy(x => x.Id).ToList();
            return View(itemList);
        }


        //[AdminMenuItem(Name = "New Question", Url = "/OeQuestion/CreateEdit", IconCls = "fa-plus", Order = 4)]
        public ActionResult CreateEdit(long Id = 0)
        {
            OeViewQuestion item = new OeViewQuestion();
            string subjectId = "0";
            string examId = "0";

            if (Id > 0)
            {
                var item1 = _oeQuestionService.Get(Id);
                var item2 = _oeQuestionService.Get(item1.Exam.Id, item1.Subject.Id, item1.UniqueSetNumber, item1.QuestionSerial, item1.Version == 1 ? 2 : 1);

                item = fillViewModel(item1, item2);
                subjectId = item1.Subject.Id.ToString();
                examId = item1.Exam.Id.ToString();
            }
            ViewBag.SubjectList = new SelectList(_oeSubjectService.LoadAll(), "Id", "Name", subjectId);
            ViewBag.ExamList = new SelectList(_oeExamService.LoadAll(), "Id", "Name", examId);
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(OeViewQuestion model, string SubjectId, string ExamId, string save)
        {
            bool isSuccess = false;
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                var isValid = true;
                var exam = _oeExamService.Get(Convert.ToInt64(ExamId));
                var subject = _oeSubjectService.Get(Convert.ToInt64(SubjectId));

                //Max Min Unique Set Number check
                if (model.UniqueSetNumber < 1 || model.UniqueSetNumber > exam.TotalUniqueSet)
                {
                    isValid = false;
                    ViewBag.Message = "Enter a valid Unique Set.";
                }

                //Max Min Question serial check
                if (isValid == true && (model.QuestionSerial < 1 || model.QuestionSerial > exam.TotalQuestion))
                {
                    isValid = false;
                    ViewBag.Message = "Enter a valid Question Serial.";
                }

                //Duplicate Serial and cross subject check
                if (isValid == true)
                {
                    var tempQList = _oeQuestionService.Load(exam.Id, 0, 0, model.QuestionSerial).Where(x => x.Id != model.BnId && x.Id != model.EnId).ToList();
                    if (tempQList != null)
                    {
                        //Duplicate Serial check
                        if (tempQList.Where(x => x.UniqueSetNumber == model.UniqueSetNumber).Count() > 0)
                        {
                            isValid = false;
                            ViewBag.Message = "Duplicate Question Serial.";
                        }

                        if (isValid)
                        {
                            //Cross unique set subject check                
                            foreach (var item in tempQList)
                            {
                                if (item.Subject.Id != subject.Id)
                                {
                                    isValid = false;
                                    ViewBag.Message = "Unique set subject mismatch.";
                                    break;
                                }
                            }
                        }
                    }
                }

                //Cross unique set uddipok check
                //Not implemented 

                if (isValid)
                {
                    #region Question Model Creation from view movel
                    OeQuestion oeqBn = new OeQuestion();
                    if (model.BnId > 0)
                        oeqBn = _oeQuestionService.Get(model.BnId);
                    oeqBn.Version = 1;
                    oeqBn.Exam = exam;
                    oeqBn.Subject = subject;
                    oeqBn.UniqueSetNumber = model.UniqueSetNumber;
                    oeqBn.QuestionSerial = model.QuestionSerial;
                    oeqBn.CorrectAnswer = model.CorrectAnswer == null ? "" : model.CorrectAnswer.Trim();
                    oeqBn.IsShuffle = model.IsShuffle;
                    //oeqBn.Uddipok = model.BnUddipok;
                    oeqBn.Name = model.BnQuestion;
                    oeqBn.OptionA = model.BnOptionA;
                    oeqBn.OptionB = model.BnOptionB;
                    oeqBn.OptionC = model.BnOptionC;
                    oeqBn.OptionD = model.BnOptionD;
                    oeqBn.OptionE = model.BnOptionE;
                    oeqBn.Solve = model.BnSolve;

                    OeQuestion oeqEn = new OeQuestion();
                    if (model.EnId > 0)
                        oeqEn = _oeQuestionService.Get(model.EnId);
                    oeqEn.Version = 2;
                    oeqEn.Exam = exam;
                    oeqEn.Subject = subject;
                    oeqEn.UniqueSetNumber = model.UniqueSetNumber;
                    oeqEn.QuestionSerial = model.QuestionSerial;
                    oeqEn.CorrectAnswer = model.CorrectAnswer == null ? "" : model.CorrectAnswer.Trim();
                    oeqEn.IsShuffle = model.IsShuffle;
                    //oeqEn.Uddipok = model.EnUddipok;
                    oeqEn.Name = model.EnQuestion;
                    oeqEn.OptionA = model.EnOptionA;
                    oeqEn.OptionB = model.EnOptionB;
                    oeqEn.OptionC = model.EnOptionC;
                    oeqEn.OptionD = model.EnOptionD;
                    oeqEn.OptionE = model.EnOptionE;
                    oeqEn.Solve = model.EnSolve;
                    #endregion

                    if (exam.IsBangla)
                    {
                        if (model.BnId > 0)
                            _oeQuestionService.Update(oeqBn);
                        else
                            _oeQuestionService.Save(oeqBn);
                    }
                    if (exam.IsEnglish)
                    {
                        if (model.EnId > 0)
                            _oeQuestionService.Update(oeqEn);
                        else
                            _oeQuestionService.Save(oeqEn);
                    }

                    isSuccess = true;
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Data saved successfull.";
                }
            }

            if (isSuccess == true && save == "Save")
            {
                return RedirectToAction("Manage");
            }
            else if (isSuccess == true)
            {
                int QuestionSerial = model.QuestionSerial + 1;
                int UniqueSetNumber = model.UniqueSetNumber;
                model = new OeViewQuestion();
                model.QuestionSerial = QuestionSerial;
                model.UniqueSetNumber = UniqueSetNumber;
            }

            ViewBag.SubjectList = new SelectList(_oeSubjectService.LoadAll(), "Id", "Name", SubjectId);
            ViewBag.ExamList = new SelectList(_oeExamService.LoadAll(), "Id", "Name", ExamId);

            return View(model);
        }

        public ActionResult Delete(long Id)
        {
            OeQuestion item = _oeQuestionService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, string action)
        {
            var item1 = _oeQuestionService.Get(Id);
            if (action == "Delete All")
            {
                var item2 = _oeQuestionService.Get(item1.Exam.Id, item1.Subject.Id, item1.UniqueSetNumber, item1.QuestionSerial, item1.Version == 1 ? 2 : 1);
                if (item2 != null)
                    _oeQuestionService.DeletePermanently(item2.Id);
            }
            _oeQuestionService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        [AllowAnonymous]
        public ActionResult Index(string prn = "", string reg = "", long Id = 0, int version = 0, bool isReExam = false)
        {
            ViewBag.ErrorMessage = "";
            if (reg == null) { reg = ""; ViewBag.ErrorMessage = "Please enter Registration Number."; }
            if (prn == null) { prn = ""; ViewBag.ErrorMessage = "Please enter Roll Number."; }
            ViewBag.prn = prn.Trim();
            ViewBag.reg = reg.Trim();
            ViewBag.Student = null;
            ViewBag.ExamList = new List<OeExam>();
            ViewBag.Exam = null;
            ViewBag.QuestionList = new List<OeQuestion>();

            if (!string.IsNullOrEmpty(prn) && !string.IsNullOrEmpty(reg))
            {
                OeStudent Student = _oeStudentService.Get(prn, reg);
                if (Student == null)
                {
                    ViewBag.ErrorMessage = "Invalid Roll / Registration Number.";
                }
                else
                {
                    ViewBag.Student = Student;
                    if (Id > 0)
                    {
                        ViewBag.Exam = _oeExamService.Get(Id);
                        ViewBag.QuestionList = _oeQuestionService.Load(Id, 0, 1, 0, version).OrderBy(x => x.QuestionSerial).ToList();

                        var questionSetDetailsList = _oeStudentQuestionSetDetailsService.GenerateSet(Student.Id, Id, version, isReExam);
                        var studentQuestionSet = questionSetDetailsList.FirstOrDefault().StudentQuestionSet;
                        if (studentQuestionSet.Correct + studentQuestionSet.Incorrect > 0)
                        {
                            return RedirectToAction("Result", new { Id = studentQuestionSet.Id });
                        }
                        ViewBag.QuestionSetDetailsList = questionSetDetailsList;
                    }
                    else
                    {
                        List<OeExam> examList = _oeExamService.LoadAll().Where(x => x.IsPublished == true).OrderByDescending(x => x.PublishDateTime).ToList();
                        List<OeStudentQuestionSet> studentResultList = _oeStudentQuestionSetService.LoadAll().Where(x => x.Student.Id == Student.Id).OrderByDescending(x => x.CreationDate).ToList();
                        List<OeExam> examList1 = new List<OeExam>();
                        foreach (var item in examList)
                        {
                            int count = studentResultList.Where(x => x.Exam.Id == item.Id).Count();
                            if (count <= 0)
                                examList1.Add(item);
                        }

                        ViewBag.ExamList = examList1;
                        ViewBag.StudentResultList = studentResultList;
                    }
                }
            }



            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(long studentQuestionSetId, string[] StudentAnswer)
        {
            OeStudentQuestionSet studentQuestionSet = _oeStudentQuestionSetService.Get(studentQuestionSetId);
            if (studentQuestionSet != null && studentQuestionSet.Id > 0)
            {
                studentQuestionSet.NotAnswered = studentQuestionSet.Exam.TotalQuestion;
                studentQuestionSet.Correct = 0;
                studentQuestionSet.Incorrect = 0;
                studentQuestionSet.TotalMarks = 0;


                OeStudent student = studentQuestionSet.Student;
                OeExam exam = studentQuestionSet.Exam;
                List<OeStudentQuestionSetDetails> studentQuestionSetDetailsList = _oeStudentQuestionSetDetailsService.Load(studentQuestionSet.Id);


                foreach (var item in StudentAnswer)
                {
                    var temp = item.Split('-');
                    var sq = studentQuestionSetDetailsList.Where(x => x.SetSerial == Convert.ToInt32(temp[0])).FirstOrDefault();
                    if (string.IsNullOrEmpty(sq.StudentAnswer))
                    {
                        sq.StudentAnswer = temp[1];
                    }
                    else
                    {
                        sq.StudentAnswer += temp[1];
                    }
                }

                foreach (var item in studentQuestionSetDetailsList)
                {
                    var correctAnswer = string.IsNullOrEmpty(item.CorrectAnswer) ? "" : item.CorrectAnswer.ToUpper();
                    if (item.StudentAnswer == correctAnswer)
                    {
                        //correct
                        studentQuestionSet.NotAnswered -= 1;
                        studentQuestionSet.Correct += 1;
                        studentQuestionSet.TotalMarks += exam.MarksPerQuestion;
                        _oeStudentQuestionSetDetailsService.Update(item);
                    }
                    else if (string.IsNullOrEmpty(item.StudentAnswer))
                    {
                        //not answered
                    }
                    else
                    {
                        //incorrect
                        studentQuestionSet.NotAnswered -= 1;
                        studentQuestionSet.Incorrect += 1;
                        studentQuestionSet.TotalMarks -= exam.NegativeMarks;
                        _oeStudentQuestionSetDetailsService.Update(item);
                    }
                }

                _oeStudentQuestionSetService.Update(studentQuestionSet);
                return RedirectToAction("Result", new { Id = studentQuestionSet.Id });
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Result(long Id) //studentQuestionSetId
        {

            OeStudentQuestionSet studentQuestionSet = _oeStudentQuestionSetService.Get(Id);
            if (studentQuestionSet != null && studentQuestionSet.Id > 0)
            {
                OeStudent student = studentQuestionSet.Student;
                ViewBag.prn = student.PrnNo;
                ViewBag.reg = student.RegistrationNo;
                List<OeStudentQuestionSetDetails> studentQuestionSetDetailsList = _oeStudentQuestionSetDetailsService.Load(studentQuestionSet.Id);

                ViewBag.StudentQuestionSet = studentQuestionSet;
                ViewBag.StudentQuestionSetDetailsList = studentQuestionSetDetailsList;
                return View();
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helper
        private OeViewQuestion fillViewModel(OeQuestion item1, OeQuestion item2)
        {
            OeViewQuestion item = new OeViewQuestion();
            item.Exam = item1.Exam;
            item.Subject = item1.Subject;
            item.UniqueSetNumber = item1.UniqueSetNumber;
            item.QuestionSerial = item1.QuestionSerial;
            item.CorrectAnswer = item1.CorrectAnswer;
            item.IsShuffle = item1.IsShuffle;

            OeQuestion bn = new OeQuestion();
            OeQuestion en = new OeQuestion();
            if (item1.Version == 1)
                bn = item1;
            else
                en = item1;

            if (item2 != null)
            {
                if (item2.Version == 1)
                    bn = item2;
                else
                    en = item2;
            }

            if (bn != null)
            {
                item.BnId = bn.Id;
                item.BnUddipok = bn.Uddipok;
                item.BnQuestion = bn.Name;
                item.BnOptionA = bn.OptionA;
                item.BnOptionB = bn.OptionB;
                item.BnOptionC = bn.OptionC;
                item.BnOptionD = bn.OptionD;
                item.BnOptionE = bn.OptionE;
                item.BnSolve = bn.Solve;
            }

            if (en != null)
            {
                item.EnId = en.Id;
                item.EnUddipok = en.Uddipok;
                item.EnQuestion = en.Name;
                item.EnOptionA = en.OptionA;
                item.EnOptionB = en.OptionB;
                item.EnOptionC = en.OptionC;
                item.EnOptionD = en.OptionD;
                item.EnOptionE = en.OptionE;
                item.EnSolve = en.Solve;
            }


            return item;
        }
        #endregion
    }
}