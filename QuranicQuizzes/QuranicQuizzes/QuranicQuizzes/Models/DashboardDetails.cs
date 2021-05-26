using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QuranicQuizzes.Helpers;

namespace QuranicQuizzes.Models
{
    public class DashboardDetails
    {
        public class DashboardQuizze
        {
            public int ID { get; set; }
            public string QuestionText { get; set; }
            public string ImageURL { get; set; }
            public string ImageText { get; set; }
            public bool AnsweredCorrectly { get; set; }
            public int Points { get; set; }
            public string FinalImageURL
            {
                get
                {
                    return (!string.IsNullOrEmpty(ImageURL)) ? GlobalConst.ApiUrl + ImageURL : string.Empty;
                }
            }
            public bool IsImage
            {
                get
                {
                    return string.IsNullOrEmpty(ImageText) ? (string.IsNullOrEmpty(ImageURL) ? false : true) : false;
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
                    return (Points == 0) ? "Round_close" : "Round_yes";
                }
            }
            public string TintColor
            {
                get
                {
                    return (Points == 0) ? "#dc3545" : "#28a745";
                }
            }
        }

        public class LeaderBoards
        {
            public string Name { get; set; }
            public int Pos { get; set; }
            public int TotalScore { get; set; }
        }

       public List<DashboardQuizze> Quizzes { get; set; }
       public List<LeaderBoards> LeaderBoard { get; set; }
       
    }
}
