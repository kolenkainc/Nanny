using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Nanny.Console.Database;

namespace Nanny.Console.Commands.ExternalServices
{
    public class Jira : IJira
    {
        private HttpClient _httpClient;
        private ApplicationContext _db;
        
        public Jira(HttpClient httpClient, ApplicationContext db)
        {
            _httpClient = httpClient;
            _db = db;
        }

        public void Worklog(string issue, string worklog)
        {
            try
            {
                ReadConfiguration();
                _httpClient.PostAsync($"/rest/api/3/issue/{issue}/worklog", new StringContent(""));
            }
            catch (Exception e)
            {
                throw new JiraException(e.Message);
            }
        }

        private void ReadConfiguration()
        {
            var domain = _db.Properties.First(p => p.Key == "JiraDomain").Value;
            var token = _db.Properties.First(p => p.Key == "JiraToken").Value;
            var email = _db.Properties.First(p => p.Key == "JiraEmail").Value;
            _httpClient.BaseAddress = new Uri(domain);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                            $"{email}:{token}")));
        }
    }
}
