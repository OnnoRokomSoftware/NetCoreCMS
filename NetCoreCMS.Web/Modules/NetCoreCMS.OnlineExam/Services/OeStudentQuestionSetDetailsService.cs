using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.OnlineExam.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;
using System;

namespace NetCoreCMS.Modules.OnlineExam.Services
{
    public class OeStudentQuestionSetDetailsService : IBaseService<OeStudentQuestionSetDetails>
    {
        private readonly OeStudentQuestionSetDetailsRepository _entityRepository;

        private readonly OeExamRepository _examRepository;
        private readonly OeStudentRepository _studentRepository;
        private readonly OeQuestionRepository _questionRepository;
        private readonly OeStudentQuestionSetRepository _studentQuestionSetRepository;

        public OeStudentQuestionSetDetailsService(OeStudentQuestionSetDetailsRepository entityRepository, OeExamRepository examRepository, OeStudentRepository studentRepository, OeQuestionRepository questionRepository, OeStudentQuestionSetRepository studentQuestionSetRepository)
        {
            _entityRepository = entityRepository;

            _examRepository = examRepository;
            _studentRepository = studentRepository;
            _studentQuestionSetRepository = studentQuestionSetRepository;
            _questionRepository = questionRepository;
        }

        public OeStudentQuestionSetDetails Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string> { "StudentQuestionSet", "Question" });
        }

        public OeStudentQuestionSetDetails Save(OeStudentQuestionSetDetails entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public OeStudentQuestionSetDetails Update(OeStudentQuestionSetDetails entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id, false);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<OeStudentQuestionSetDetails> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string> { "StudentQuestionSet", "Question" });
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(OeStudentQuestionSetDetails oldEntity, OeStudentQuestionSetDetails entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.StudentQuestionSet = entity.StudentQuestionSet;
            oldEntity.Question = entity.Question;
            oldEntity.SetSerial = entity.SetSerial;
            oldEntity.StudentAnswer = entity.StudentAnswer;
            oldEntity.CorrectAnswer = entity.CorrectAnswer;
            oldEntity.OptionA = entity.OptionA;
            oldEntity.OptionB = entity.OptionB;
            oldEntity.OptionC = entity.OptionC;
            oldEntity.OptionD = entity.OptionD;
            oldEntity.OptionE = entity.OptionE;
        }

        public List<OeStudentQuestionSetDetails> Load(long studentQuestionSetId)
        {
            return _entityRepository.Load(studentQuestionSetId);
        }

        public List<OeStudentQuestionSetDetails> GenerateSet(long studentId, long examId, int version, bool isReExam = false)
        {
            OeStudent student = _studentRepository.Get(studentId);
            OeExam exam = _examRepository.Get(examId);

            List<OeStudentQuestionSetDetails> studentQuestionSetDetailsList = new List<OeStudentQuestionSetDetails>();

            //Check if set exists then show return that set
            OeStudentQuestionSet studentQuestionSet = _studentQuestionSetRepository.Get(studentId, examId, version);
            /*special check for running exam*/
            if (isReExam == true && studentQuestionSet != null && studentQuestionSet.Id > 0)
            {
                if (studentQuestionSet.CreationDate.AddMinutes(studentQuestionSet.Exam.TotalTimeInMin) > DateTime.Now)
                {
                    isReExam = false;
                }
            }
            /*special check for running exam*/

            if (isReExam == false && studentQuestionSet != null && studentQuestionSet.Id > 0)
            {
                studentQuestionSetDetailsList = _entityRepository.Load(studentQuestionSet.Id);
            }
            else
            {
                studentQuestionSet = new OeStudentQuestionSet();
                studentQuestionSet.Student = student;
                studentQuestionSet.Exam = exam;
                studentQuestionSet.NotAnswered = exam.TotalQuestion;
                studentQuestionSet.Version = version;
                _studentQuestionSetRepository.Add(studentQuestionSet);
                _studentQuestionSetRepository.SaveChange();

                List<OeQuestion> questionList = _questionRepository.Load(examId, 0, 0, 0, version);

                var groups = questionList.GroupBy(x => x.QuestionSerial);
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int randomNumber = 0;
                foreach (var group in groups)
                {
                    randomNumber = random.Next(0, group.Count());
                    OeStudentQuestionSetDetails sqSetDetails = new OeStudentQuestionSetDetails();
                    sqSetDetails.StudentQuestionSet = studentQuestionSet;
                    sqSetDetails.Question = group.ElementAt(randomNumber);
                    //sqSetDetails.SetSerial = SetSerial;
                    if (sqSetDetails.Question.IsShuffle == true)
                    {
                        List<int> tempIntList = new List<int>() { 1, 2, 3, 4 };
                        tempIntList = tempIntList.OrderBy(x => random.Next()).ToList();
                        var answerString = "";
                        foreach (var item in tempIntList)
                        {
                            answerString += item.ToString();
                        }
                        answerString = answerString.Replace("1", "A").Replace("2", "B").Replace("3", "C").Replace("4", "D");
                        sqSetDetails.OptionA = answerString[0].ToString();
                        sqSetDetails.OptionB = answerString[1].ToString();
                        sqSetDetails.OptionC = answerString[2].ToString();
                        sqSetDetails.OptionD = answerString[3].ToString();

                        //FUNCTION FOR SHUFFLING CORRECT ANSWER
                        SetCorrectAnsOfGeneratedSet(sqSetDetails.Question.CorrectAnswer, sqSetDetails);
                    }
                    else
                    {
                        sqSetDetails.CorrectAnswer = sqSetDetails.Question.CorrectAnswer;
                        sqSetDetails.OptionA = "A";
                        sqSetDetails.OptionB = "B";
                        sqSetDetails.OptionC = "C";
                        sqSetDetails.OptionD = "D";
                    }
                    studentQuestionSetDetailsList.Add(sqSetDetails);
                    //SetSerial += 1;
                }

                studentQuestionSetDetailsList = studentQuestionSetDetailsList.OrderBy(item => random.Next()).ToList();

                int SetSerial = 1;
                foreach (var item in studentQuestionSetDetailsList)
                {
                    item.SetSerial = SetSerial;
                    _entityRepository.Add(item);
                    SetSerial += 1;
                }
                _entityRepository.SaveChange();
            }
            return studentQuestionSetDetailsList.OrderBy(x => x.SetSerial).ToList();
        }

        public static void SetCorrectAnsOfGeneratedSet(string uniqueSetAns, OeStudentQuestionSetDetails details)
        {
            details.CorrectAnswer = "";
            if (string.IsNullOrWhiteSpace(uniqueSetAns))
                return;

            //generate new ans
            string[] orAns = uniqueSetAns.Split('|').ToArray();
            for (int i = 0; i < orAns.Length; i++)
            {
                var newAns = orAns[i];
                if (string.IsNullOrEmpty(details.OptionA) == false)
                    newAns = newAns.Replace(details.OptionA, "1");
                if (string.IsNullOrEmpty(details.OptionB) == false)
                    newAns = newAns.Replace(details.OptionB, "2");
                if (string.IsNullOrEmpty(details.OptionC) == false)
                    newAns = newAns.Replace(details.OptionC, "3");
                if (string.IsNullOrEmpty(details.OptionD) == false)
                    newAns = newAns.Replace(details.OptionD, "4");
                if (string.IsNullOrEmpty(details.OptionE) == false)
                    newAns = newAns.Replace(details.OptionE, "5");

                newAns = newAns.Replace("1", "A").Replace("2", "B").Replace("3", "C").Replace("4", "D").Replace("5", "E");
                orAns[i] = new string(newAns.OrderBy(c => c).ToArray());
            }

            //set ans
            details.CorrectAnswer = string.Join("|", orAns);
        }
    }
}