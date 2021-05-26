using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuranicQuizzes.Models;
using static QuranicQuizzes.Models.Dashboard;

namespace QuranicQuizzes.Interfaces
{
    public interface IClientAPI
    {
        Task<UserInfo> GetUserInfo();
        Task<LoginResponse> DoLogin(Login data);
        Task<List<Categories>> GetCategories();
        Task<List<Coures>> GetCourses(int pageNo);
        Task<Quizzes> GetCategoriesByQuizzes(int id,int pageNo);
        Task<Quizzes> GetSubCategoriesByQuizzes(int id, int pageNo);
        
        Task<Quizzes> GetCoursesByQuizzes(int id, int pageNo);
        Task<Quizzes> GetSearchData(int CategoryId,int SubCategoryId,string Search,int pageNo);
        Task<QuizzesQuestions> GetQuestionsOfQuizzes(int id, int categoryID, bool testMode, bool qShuffle, bool removeDuplicates, bool resume, bool isCourse, int courseID, bool retakeWrong, bool isHomework, int homeworkID);
        Task<CheckSession> GetCheckSession(int id,bool isCourse,int homeworkID);
        Task<Int32> GetEvaluateAnswer(int SessionId, int QuestionId, int AnswerId, int Points);
        Task<Dashboards> getDashboardData(string startDate, string endDate);
        Task<Int32> EndQuiz(int SessionId);
        Task<List<Assignment>> GetAssignments();
        Task<List<Assignment>> GetNewAssignments();
        Task<List<CreateAssignment>> GetCreateAssignments(string qid);
        Task<AssignedToUser> GetAssignedToUserList(int HomworkId, int pageNo);
        Task<AssignedToUser> GetSearchAssignedToUserList(int HomworkId, int pageNo, string Search);
        Task<Int32> AssignHomeworkToUser(int HomworkId, string UserId);
        Task<DashboardDetails> getDashboardDetails(string qid);
        Task<Int32> AssignHomeworkToAllUser(int HomworkId);
        Task<CreateAssignment> CreateAssignment(int quizId,int CategoryId, string deadLine);
        Task<bool> PurchaseItem(string token, string plan, string period, int? seats,string platform);
        Task<bool> PurchaseItemCancle(string token);
    }
}
