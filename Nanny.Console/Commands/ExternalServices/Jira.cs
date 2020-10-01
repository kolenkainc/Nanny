using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Logging;
using Nanny.Console.Database;

namespace Nanny.Console.Commands.ExternalServices
{
    public class Jira : IJira
    {
        private ApplicationContext _db;
        private HttpClient _httpClient;
        private ILogger<IJira> _logger;

        public Jira(HttpClient httpClient, ApplicationContext db, ILogger<IJira> logger)
        {
            _httpClient = httpClient;
            _db = db;
            _logger = logger;
        }

        public void Worklog(string issue, string worklog)
        {
            try
            {
                ReadConfiguration();
                var json = new WorklogRequest(worklog).ToJson();
                _logger.LogDebug("Send the following json:");
                _logger.LogDebug(json);
                var response = _httpClient.PostAsync(
                    $"/rest/api/3/issue/{issue}/worklog",
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json));
                _logger.LogDebug("Response was: {0}", response.Result.Content.ReadAsStringAsync().Result);
                if (!response.Result.IsSuccessStatusCode)
                {
                    var errorMessage = response.Result.Content.ReadAsStringAsync().Result;
                    throw new JiraException(errorMessage);
                }
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