using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Modules.OnlineExam.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Services
{
    public class OeQuestionService : IBaseService<OeQuestion>
    {
        private readonly OeQuestionRepository _entityRepository;


        public OeQuestionService(OeQuestionRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public OeQuestion Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId, isAsNoTracking, new List<string> { "Exam", "Subject", "Uddipok" });
        }

        public OeQuestion Get(long examId, long subjectId, int uniqueSetNumber, int QuestionSerial, int version)
        {
            return _entityRepository.Get(examId, subjectId, uniqueSetNumber, QuestionSerial, version);
        }

        public OeQuestion Save(OeQuestion entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public OeQuestion Update(OeQuestion entity)
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

        public List<OeQuestion> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch, new List<string> { "Exam", "Subject", "Uddipok" });
        }

        public List<OeQuestion> Load(long examId = 0, long subjectId = 0, int uniqueSetNumber = 0, int questionSerial = 0, int version = 0, string searchKey="")
        {
            return _entityRepository.Load(examId, subjectId, uniqueSetNumber, questionSerial, version, searchKey);
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

        private void CopyNewData(OeQuestion oldEntity, OeQuestion entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.Exam = entity.Exam;
            oldEntity.Subject = entity.Subject;
            oldEntity.Version = entity.Version;
            oldEntity.Uddipok = entity.Uddipok;
            oldEntity.UniqueSetNumber = entity.UniqueSetNumber;
            oldEntity.QuestionSerial = entity.QuestionSerial;
            oldEntity.OptionA = entity.OptionA;
            oldEntity.OptionB = entity.OptionB;
            oldEntity.OptionC = entity.OptionC;
            oldEntity.OptionD = entity.OptionD;
            oldEntity.OptionE = entity.OptionE;
            oldEntity.CorrectAnswer = string.IsNullOrEmpty(entity.CorrectAnswer) ? "" : entity.CorrectAnswer.ToUpper();
            oldEntity.Solve = entity.Solve;
            oldEntity.IsShuffle = entity.IsShuffle;
        }
    }
}