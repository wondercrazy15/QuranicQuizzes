using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using static QuranicQuizzes.Models.Dashboard;

namespace QuranicQuizzes.Helpers
{
    /// <summary>
    /// Api calling
    /// </summary>
    public class ClientAPI: IClientAPI
    {
       //Login
        public async Task<LoginResponse> DoLogin(Login data)
        {
            try
            {
                LoginResponse loginResponse = new LoginResponse();
                HttpClient client = new HttpClient() { BaseAddress = new Uri(GlobalConst.ApiUrl + "token?" + GlobalConst.ApiUrlKey) };
                var vals = new List<KeyValuePair<string, string>>();
                vals.Add(new KeyValuePair<string, string>("grant_type", "password"));
                vals.Add(new KeyValuePair<string, string>("username", data.username));
                vals.Add(new KeyValuePair<string, string>("password", data.password));
                var response = await client.PostAsync(GlobalConst.ApiUrl + "token?"+GlobalConst.ApiUrlKey, new FormUrlEncodedContent(vals), CancellationToken.None);

                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);
                    return loginResponse;
                }
                else
                {
                    ErrorData Error = JsonConvert.DeserializeObject<ErrorData>(content);
                    loginResponse.errorData = Error;
                    return loginResponse;
                }
            }
            catch (Exception ex)
            {
                
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //Get all Categories
        public async Task<List<Categories>> GetCategories()
        {
            List<Categories> _categories = new List<Categories>();
            var requestUrl = GlobalConst.ApiUrl + "api/Category?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl,UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _categories = JsonConvert.DeserializeObject<List<Categories>>(responseData);
                    return _categories;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //GetCategoriesByQuizzes
        public async Task<Quizzes> GetCategoriesByQuizzes(int id,int pageNo)
        {
            Quizzes _quizzes = new Quizzes();
            var requestUrl = GlobalConst.ApiUrl + "api/Category/"+id+"/"+ pageNo+"?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzes = JsonConvert.DeserializeObject<Quizzes>(responseData);
                    return _quizzes;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //Get all Courses
        public async Task<List<Coures>> GetCourses(int pageNo)
        {
            List<Coures> _quizzes = new List<Coures>();
            var requestUrl = GlobalConst.ApiUrl + "api/Course" + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzes = JsonConvert.DeserializeObject<List<Coures>>(responseData);
                    return _quizzes;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //GetCoursesByQuizzes
        public async Task<Quizzes> GetCoursesByQuizzes(int id, int pageNo)
        {
            Quizzes _quizzes = new Quizzes();
            var requestUrl = GlobalConst.ApiUrl + "api/Course/" + id + "/" + pageNo + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzes = JsonConvert.DeserializeObject<Quizzes>(responseData);
                    return _quizzes;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //GetSearchData of categories
        public async Task<Quizzes> GetSearchData(int CategoryId, int SubCategoryId,string Search, int pageNo)
        {
            Quizzes _quizzes = new Quizzes();
            var requestUrl = GlobalConst.ApiUrl + "api/Search/" + CategoryId + "/"+ SubCategoryId+"/"+Search+"/" + pageNo + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzes = JsonConvert.DeserializeObject<Quizzes>(responseData);
                    return _quizzes;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //GetSubCategoriesByQuizzes
        public async Task<Quizzes> GetSubCategoriesByQuizzes(int id, int pageNo)
        {
            Quizzes _quizzes = new Quizzes();
            var requestUrl = GlobalConst.ApiUrl + "api/SubCategory/" + id + "/" + pageNo + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzes = JsonConvert.DeserializeObject<Quizzes>(responseData);
                    return _quizzes;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        //Get Questions List
        public async Task<QuizzesQuestions> GetQuestionsOfQuizzes(int id,int categoryID,bool testMode,bool qShuffle,bool removeDuplicates,bool resume,bool isCourse,int courseID,bool retakeWrong,bool isHomework,int homeworkID)
        {
            QuizzesQuestions _quizzesQuestions = new QuizzesQuestions();
            var requestUrl = GlobalConst.ApiUrl + "api/Play/" + id + "?" + GlobalConst.ApiUrlKey+"&categoryID="+categoryID+"&testMode="+testMode+"&qShuffle=" + qShuffle + "&removeDuplicates=" + removeDuplicates + "&resume=" + resume + "&isCourse=" + isCourse + "&courseID=" + courseID + "&retakeWrong=" + retakeWrong+"&isHomework="+ isHomework + "&homeworkID="+ homeworkID;

            //categoryID=2&testMode=true&qShuffle=false&removeDuplicates=false&resume=false&isCourse=false&courseID=0&retakeWrong=false&isHomework=false&homeworkID=0
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _quizzesQuestions = JsonConvert.DeserializeObject<QuizzesQuestions>(responseData);
                    return _quizzesQuestions;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<CheckSession> GetCheckSession(int id,bool isCourse,int homeworkID)
        {
            CheckSession _CheckSession = new CheckSession();
            var requestUrl = GlobalConst.ApiUrl + "api/CheckSession/"+id+"?isCourse="+isCourse+"&homeworkID="+ homeworkID + "&" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _CheckSession = JsonConvert.DeserializeObject<CheckSession>(responseData);
                    return _CheckSession;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<int> GetEvaluateAnswer(int SessionId, int QuestionId, int AnswerId, int Points)
        {
            
            var requestUrl = GlobalConst.ApiUrl + "api/Evaluate/" + SessionId + "/" + QuestionId + "/" + AnswerId +"/"+ Points+ "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                return statusCode;


            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return 0;
        }

        public async Task<int> EndQuiz(int SessionId)
        {
            var requestUrl = GlobalConst.ApiUrl + "api/EndQuiz/" + SessionId + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                return statusCode;


            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return 0;
        }

        public async Task<UserInfo> GetUserInfo()
        {
            UserInfo _UserInfo = new UserInfo();
            var requestUrl = GlobalConst.ApiUrl + "api/Account/UserInfo?"+GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _UserInfo = JsonConvert.DeserializeObject<UserInfo>(responseData);
                    return _UserInfo;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<List<Assignment>> GetAssignments()
        {
            List<Assignment> _assignmentsList = new List<Assignment>();
            var requestUrl = GlobalConst.ApiUrl + "api/CurrentAssignments?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _assignmentsList = JsonConvert.DeserializeObject<List<Assignment>>(responseData);
                    return _assignmentsList;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<List<Assignment>> GetNewAssignments()
        {
            List<Assignment> _assignmentsList = new List<Assignment>();
            var requestUrl = GlobalConst.ApiUrl + "api/NewAssignments?" + GlobalConst.ApiUrlKey;

            HttpClient client = CreateHttpClient((false, null));
            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _assignmentsList = JsonConvert.DeserializeObject<List<Assignment>>(responseData);
                    return _assignmentsList;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<List<CreateAssignment>> GetCreateAssignments(string qid)
        {
            List<CreateAssignment> _assignmentsList = new List<CreateAssignment>();
            var requestUrl = GlobalConst.ApiUrl + "api/Assignment/" + qid + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _assignmentsList = JsonConvert.DeserializeObject<List<CreateAssignment>>(responseData);
                    return _assignmentsList;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<AssignedToUser> GetAssignedToUserList(int HomworkId, int pageNo)
        {
            AssignedToUser _AssignedToUser = new AssignedToUser();
            var requestUrl = GlobalConst.ApiUrl + "api/AssignedUsers/"+HomworkId+"/"+pageNo +"?"+ GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _AssignedToUser = JsonConvert.DeserializeObject<AssignedToUser>(responseData);
                    return _AssignedToUser;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<int> AssignHomeworkToUser(int HomworkId, string UserId)
        {
            var requestUrl = GlobalConst.ApiUrl + "api/AssignUser/" + HomworkId + "/" + UserId + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);
            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                return statusCode;

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return 0;
        }

        public async Task<int> AssignHomeworkToAllUser(int HomworkId)
        {
            var requestUrl = GlobalConst.ApiUrl + "api/AssignUsers/" + HomworkId + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token); var statusCode = ((int)result.StatusCode);
                return statusCode;

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return 0;
        }

        public async Task<CreateAssignment> CreateAssignment(int quizId, int CategoryId, string deadLine)
        {
            CreateAssignment _CreateAssignment = new CreateAssignment();
            var requestUrl = GlobalConst.ApiUrl + "api/CreateAssignment/" + quizId + "/" + CategoryId + "?Deadline=" + deadLine + "&" + GlobalConst.ApiUrlKey;

            //categoryID=2&testMode=true&qShuffle=false&removeDuplicates=false&resume=false&isCourse=false&courseID=0&retakeWrong=false&isHomework=false&homeworkID=0
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("POST");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _CreateAssignment = JsonConvert.DeserializeObject<CreateAssignment>(responseData);
                    return _CreateAssignment;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<Dashboards> getDashboardData(string startDate, string endDate)
        {
            try
            {
                Dashboards _dashboard = new Dashboards();
                var requestUrl = GlobalConst.ApiUrl + "api/Reports?startDate=" + startDate + "&endDate=" + endDate + "&" + GlobalConst.ApiUrlKey;
                //categoryID=2&testMode=true&qShuffle=false&removeDuplicates=false&resume=false&isCourse=false&courseID=0&retakeWrong=false&isHomework=false&homeworkID=0
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage();
                request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
                request.Method = new HttpMethod("POST");
                request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                if (result != null)
                {
                    var statusCode = ((int)result.StatusCode);
                    if (statusCode == 200)
                    {
                        var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        _dashboard = JsonConvert.DeserializeObject<Dashboards>(responseData);
                        return _dashboard;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };
                Crashes.TrackError(ex, properties);
            }
            return null;
        }

        public async Task<AssignedToUser> GetSearchAssignedToUserList(int HomworkId, int pageNo, string Search)
        {
            AssignedToUser _AssignedToUser = new AssignedToUser();
            var requestUrl = GlobalConst.ApiUrl + "api/SearchUsers/" + HomworkId + "/" + pageNo + "/" + Search + "?" + GlobalConst.ApiUrlKey;
            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
            request.Method = new HttpMethod("GET");
            request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {

                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _AssignedToUser = JsonConvert.DeserializeObject<AssignedToUser>(responseData);
                    return _AssignedToUser;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<DashboardDetails> getDashboardDetails(string qid)
        {
            try
            {
                DashboardDetails _details= new DashboardDetails();

                var requestUrl = GlobalConst.ApiUrl + "api/SessionReport/" + qid + "?" + GlobalConst.ApiUrlKey;

                HttpClient client = new HttpClient();

                var request = new HttpRequestMessage();
                request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
                request.Method = new HttpMethod("POST");
                request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);

                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {
                   
                    var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _details = JsonConvert.DeserializeObject<DashboardDetails>(responseData);
                    return _details;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

            return null;
        }

        public async Task<bool> PurchaseItem(string token, string plan, string period, int? seats,string platform)
        {
            try
            {
                var requestUrl = GlobalConst.ApiUrl + "api/Account/IAP" + "?plan="+ plan + "&period=" + period
                   + "&seats="+ seats + "&paymentgateway="+platform + "&" + GlobalConst.ApiUrlKey;
                // var requestUrl = "https://quranicquizzes.com/api/Account/IAP?plan=96b3efec-d266-4895-a819-c45bcc591ee1&period=1 Y&seats=&paymentgateway=apple&ak=9yT6MRqbdBYbEvQQ7tweG";
                HttpClient client = CreateHttpClient((false, null));
                var request = new HttpRequestMessage();
                request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
                //request.Headers.TryAddWithoutValidation("content-type", "application/json");
                request.Method = new HttpMethod("POST");
                request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);
                
                request.Content = new StringContent(JsonConvert.SerializeObject(token), System.Text.Encoding.UTF8, "application/json");
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {
                    Analytics.TrackEvent("In app purchase Done successfully  "+ UserSettings.Name, new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });

                    return true;
                }
                else
                {
                    Analytics.TrackEvent("Issue with In app purchase  "+ UserSettings.Name, new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message+" Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
                Analytics.TrackEvent("Error in Api call of In app purchase  " + UserSettings.Name, new Dictionary<string, string>
                {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                });
            }

            return false;
        }

        public async Task<bool> PurchaseItemCancle(string token)
        {
            try
            {
                

                var requestUrl = GlobalConst.ApiUrl + "api/Account/IAPCancel" + "?" + GlobalConst.ApiUrlKey;

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);
                var request = new HttpRequestMessage();
                request.Headers.TryAddWithoutValidation("Authorization", UserSettings.AccesToken);
                request.Method = new HttpMethod("POST");
                request.RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);
                request.Content = new StringContent(JsonConvert.SerializeObject(token), System.Text.Encoding.UTF8, "application/json");

                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var result = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, tokenSource.Token);
                var statusCode = ((int)result.StatusCode);
                if (statusCode == 200)
                {
                    Analytics.TrackEvent("In app purchase Cancel Succesfully  "+ UserSettings.Name, new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });
                    return true;
                }
                else
                {
                    Analytics.TrackEvent("Issues In app purchase Cancle "+ UserSettings.Name, new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Error in Api call of In app purchase Cancle " + UserSettings.Name, new Dictionary<string, string>
                {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                 });
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message+"Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") },
                    { "StackTrace", ex.StackTrace }
                };
            }

            return false;
        }

        private static HttpClient CreateHttpClient((bool allowRedirects, IWebProxy proxy) properties)
        {
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = properties.allowRedirects,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                UseCookies = false
            };
            if (properties.proxy != null)
            {
                if (!clientHandler.SupportsProxy)
                    throw new Exception("Proxy not supported.");
                clientHandler.UseProxy = true;
                clientHandler.Proxy = properties.proxy;
            }
            return new HttpClient(clientHandler)
            {
                Timeout = TimeSpan.FromMilliseconds(Int32.MaxValue)
            };
        }






    }
}
