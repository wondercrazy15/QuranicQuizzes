using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace QuranicQuizzes.Helpers
{
    /// <summary>
    /// This Class is use for Shared preferance data store
    /// </summary>
    public class UserSettings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static bool IsLogin
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsLogin), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsLogin), value);
            }
        }
        public static bool IsPurchased
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsPurchased), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsPurchased), value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(UserId), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(UserId), value);
            }
        }

        public static string AccesToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(AccesToken), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(AccesToken), value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Name), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Name), value);
            }
        }

        public static bool IsAdmin
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsAdmin), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsAdmin), value);
            }
        }

        

        public static bool IsPaidMember
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsPaidMember), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsPaidMember), value);
            }
        }

        public static bool IsTeacher
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsTeacher), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsTeacher), value);
            }
        }
        public static bool IsInstituteAdmin
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsInstituteAdmin), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsInstituteAdmin), value);
            }
        }
        public static bool IsFamilyAdmin
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsFamilyAdmin), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsFamilyAdmin), value);
            }
        }
        public static bool IsStudent
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsStudent), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsStudent), value);
            }
        }

        public static bool IsEnrolled
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsEnrolled), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsEnrolled), value);
            }
        }

        public static string Logo
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Logo), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Logo), value);
            }
        }

    }
}
