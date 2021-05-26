using System;
using System.Collections.Generic;

namespace QuranicQuizzes.Models
{
    public class CreateAssignment
    {
        public int ID { get; set; }
        public string QuizName { get; set; }
        public string SetDate { get; set; }
        public string DeadLine { get; set; }
        public int assignedCount { get; set; }
        public List<AssignedTo> assignedTo { get; set; }

        public class AssignedTo
        {
            public int ID { get; set; }
            public int HomeworkID { get; set; }
            public string StudentID { get; set; }
            public string Name { get; set; }
        }
        
    }
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAssigned { get; set; }
        public string IconImage { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class AssignedToUser
    {
        public int TotalPages { get; set; }
        public List<User> Users { get; set; }
    }
}
