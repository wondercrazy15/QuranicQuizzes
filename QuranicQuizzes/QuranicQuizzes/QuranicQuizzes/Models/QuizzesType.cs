using System;
namespace QuranicQuizzes.Models
{
    public class QuizzesType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string IconImage { get; set; }
        public bool IsEnables { get; set; }
        public string LblNote { get; set; }
        public bool IsVisibles { get; set; }
    }

    public class QuizzesQuestionsDesignList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public string IconImage { get; set; }
    }
}
