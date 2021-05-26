using System;
namespace QuranicQuizzes.Models
{
    public class Login
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public String password { get; set; }
    }
    public class LoginResponse
    {
        public ErrorData errorData { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }


    public class UserInfo
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsInstituteAdmin { get; set; }
        public bool IsFamilyAdmin { get; set; }
        public bool IsPaidMember { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public bool IsEnrolled { get; set; }
        public string Logo { get; set; }
        public string PaymentGateway { get; set; }
        public string PurchaseToken { get; set; }
    }

    public class SubscribeAmount
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Android_pid { get; set; }
        public string iOS_pid { get; set; }
        public bool IsMember { get; set; }

    }
}
