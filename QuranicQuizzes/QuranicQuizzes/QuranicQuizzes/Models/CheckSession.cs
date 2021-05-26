using System;
namespace QuranicQuizzes.Models
{
    public class CheckSession
    {
        public bool HasSession { get; set; }
        public int Correct { get; set; }
        public int Completed { get; set; }
        public int TotalScore { get; set; }
        public bool IsTest { get; set; }
    }
}
