using System;
using System.Collections.Generic;
using QuranicQuizzes.Helpers;

namespace QuranicQuizzes.Models
{

    public class Answer
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string AnswerText { get; set; }
        public bool IsAnswer { get; set; }
        public bool IsArabic { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public double Heights { get; set; }
        public bool IsAnswerSelected { get; set; }
    }

    public class Question
    {
        public int ID { get; set; }
        public List<Answer> Answers { get; set; }
        public string ImageText { get; set; }
        public string ImageURL { get; set; }
        public string Notes { get; set; }
        public string QuestionText { get; set; }
        public string SoundURL { get; set; }
        public double point { get; set; }
        public string FinalImageURL
        {
            get
            {
                return (!string.IsNullOrEmpty(ImageURL))?GlobalConst.ApiUrl + ImageURL:string.Empty;
            }
        }
        public bool IsImage
        {
            get
            {
                return string.IsNullOrEmpty(ImageText)?(string.IsNullOrEmpty(ImageURL)?false:true):false;
            }
        }
        public bool IsImageText
        {
            get
            {
                return string.IsNullOrEmpty(ImageText) ? false : true;
            }
        }
        public string FinalResultIcon
        {
            get
            {
                return (point==0) ? "close" :"done";
            }
        }
    }


    public class QuizzesQuestions
    {
        public int ID { get; set; }
        public int SessionID { get; set; }
        public List<Question> Questions { get; set; }
    }
}
