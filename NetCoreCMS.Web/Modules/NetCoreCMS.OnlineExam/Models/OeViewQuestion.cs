using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.OnlineExam.Models
{
    public class OeViewQuestion
    {
        public OeViewQuestion()
        {
            CorrectAnswer = "";
            IsShuffle = true;
            BnId = 0;
            EnId = 0;
            UniqueSetNumber = 1;
            QuestionSerial = 1;
        }

        public OeExam Exam { get; set; }
        public OeSubject Subject { get; set; }
        public int UniqueSetNumber { get; set; }
        public int QuestionSerial { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsShuffle { get; set; }

        public long BnId { get; set; }
        public OeUddipok BnUddipok { get; set; }
        public string BnQuestion { get; set; }
        public string BnOptionA { get; set; }
        public string BnOptionB { get; set; }
        public string BnOptionC { get; set; }
        public string BnOptionD { get; set; }
        public string BnOptionE { get; set; }
        public string BnSolve { get; set; }

        public long EnId { get; set; }
        public OeUddipok EnUddipok { get; set; }
        public string EnQuestion { get; set; }
        public string EnOptionA { get; set; }
        public string EnOptionB { get; set; }
        public string EnOptionC { get; set; }
        public string EnOptionD { get; set; }
        public string EnOptionE { get; set; }
        public string EnSolve { get; set; }
    }
}