using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace QuranicQuizzes.Models
{
    public class Dashboard
    {
        public class QuizzesTable
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string QuizStarted { get; set; }
            public string QuizFinished { get; set; }
            public int CorrectQuetions { get; set; }
            public int QuestionsCompleted { get; set; }
            public int TotalQuestions { get; set; }
            public int TotalScore { get; set; }
            public string ProgressLabel { get; set; }
            public string CorrectLabel { get; set; }
            public double BarValue { get; set; }
            public Rectangle FirstBarBounds { get; set; }
            public Rectangle CorrectFirstBarBounds { get; set; }
            public Rectangle CorrectSecondBarBounds { get; set; }
            public int CorrectFirstBarValue { get; set; }
            public int CorrectSecondBarValue { get; set; }

        }

        public class AssignmentTable
        {
            private double _barValue;

            public int ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string QuizStarted { get; set; }
            public string QuizFinished { get; set; }
            public int CorrectQuetions { get; set; }
            public int QuestionsCompleted { get; set; }
            public int TotalQuestions { get; set; }
            public int TotalScore { get; set; }
            public string ProgressLabel { get; set; }
            public string CorrectLabel { get; set; }
            public double BarValue { get; set; }
            public Rectangle FirstBarBounds { get; set; }
            public Rectangle CorrectFirstBarBounds { get; set; }
            public Rectangle CorrectSecondBarBounds { get; set; }
            public int CorrectFirstBarValue { get; set; }
            public int CorrectSecondBarValue { get; set; }
        }

        public class LiveGameTable
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string QuizStarted { get; set; }
            public string QuizFinished { get; set; }
            public int CorrectQuetions { get; set; }
            public int QuestionsCompleted { get; set; }
            public int TotalQuestions { get; set; }
            public int TotalScore { get; set; }
            public string ProgressLabel { get; set; }
            public string CorrectLabel { get; set; }
            public double BarValue { get; set; }
            public Rectangle FirstBarBounds { get; set; }
            public Rectangle CorrectFirstBarBounds { get; set; }
            public Rectangle CorrectSecondBarBounds { get; set; }
            public int CorrectFirstBarValue { get; set; }
            public int CorrectSecondBarValue { get; set; }
        }

        public class Dashboards
        {
            public int Quizzes { get; set; }
            public string AvgTime { get; set; }
            public int AvgScore { get; set; }
            public int TotalScore { get; set; }
            public int LiveGames { get; set; }
            public int Assignments { get; set; }
            public List<string> Labels { get; set; }
            public List<int> QuizzesData { get; set; }
            public List<int> LiveGamesData { get; set; }
            public List<int> AssignmentsData { get; set; }
            public List<int> AvgTimeData { get; set; }
            public List<int> AvgScoreData { get; set; }
            public List<int> TotalScoreData { get; set; }
            public List<QuizzesTable> QuizzesTable { get; set; }
            public List<LiveGameTable> LiveGameTable { get; set; }
            public List<AssignmentTable> AssignmentTable { get; set; }
        }

        public class HomeTab
        {
            public string Icon { get; set; }
            public string dataCount { get; set; }
            public string dataTitle { get; set; }
            public string Color { get; set; }
        }
    }
}
