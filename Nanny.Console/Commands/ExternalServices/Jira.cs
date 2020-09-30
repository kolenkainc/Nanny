using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Nanny.Console.Database;

namespace Nanny.Console.Commands.ExternalServices
{
    public class Jira : IJira
    {
        private ApplicationContext _db;
        private HttpClient _httpClient;

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
                var response = _httpClient.PostAsync(
                    $"/rest/api/3/issue/{issue}/worklog",
                    new StringContent(
                        new WorklogRequest(worklog).ToJson(),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json));
            }
            catch (Exception e)
            {
                throw new JiraException(e.Message);
            }
        }

        private void ReadConfiguration()
        {
            var domain = _db.Properties.First(p => p.Key == Constants.JiraDomain).Value;
            var token = _db.Properties.First(p => p.Key == Constants.JiraToken).Value;
            var email = _db.Properties.First(p => p.Key == Constants.JiraLogin).Value;
            _httpClient.BaseAddress = new Uri(domain);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{email}:{token}")));
        }
    }
}