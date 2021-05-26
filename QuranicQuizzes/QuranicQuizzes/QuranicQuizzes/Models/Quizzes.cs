using System;
using System.Collections.Generic;

namespace QuranicQuizzes.Models
{
    public class Quizze
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int NumberPlayed { get; set; }
        public int NumberQuestions { get; set; }
        public string Created { get; set; }
        public bool LockQuiz { get; set; }
        public bool ShowIcon { get; set; }
        public bool ShowLearnIcon { get; set; }
        public bool IsLearnTabFree { get; set; }
        public bool CourseLockQuiz { get; set; }
        public bool CourseQuizPassed { get; set; }
        public int CategoryID { get; set; }
        public bool ShowHint { get; set; }
        public bool TestOnly { get; set; }
        public bool IsTeacher { get; set; }
        public bool ShowLockQuizPassed { get; set; }
        public bool NotShowLockQuizPassed { get; set; }
        public bool ShowLearnTab { get; set; }
        public string LearnTabSound { get; set; }
        public bool ShowPlayTab { get; set; }
    }

    public class Quizzes
    {
        public int totalPages { get; set; }
        public List<Quizze> quizzes { get; set; }
    }
}
