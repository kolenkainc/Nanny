using System.Net.Http;

namespace Nanny.Console.Commands.ExternalServices
{
    public class Jira : IJira
    {
        private HttpClient _httpClient;
        
        public Jira(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
