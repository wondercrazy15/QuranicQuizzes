using System;
using System.Collections.Generic;
using System.Drawing;

namespace QuranicQuizzes.Models
{
    public class Assignment
    {
        public int ID { get; set; }
        public int HWID { get; set; }
        public int CategoryID { get; set; }
        public int Attempts { get; set; }
        public string QuizName { get; set; }
        public DateTime SetTime { get; set; }
        public DateTime Deadline { get; set; }
        public string DeadlineLabel { get; set; }
        public string SetTimeLabel { get; set; }
        public Color TextColor { get; set; }
    }
}
