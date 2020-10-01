namespace Nanny.Console.Commands.ExternalServices
{
    public class WorklogRequest
    {
        private static string _worklogPlaceholder = "WORKLOG";
        private string _template1 = 
            @"{
                'timeSpentSeconds': " + _worklogPlaceholder + @",
                'visibility': {
                    'type': 'group',
                    'value': 'jira-developers'
                },
                'comment': {
                    'type': 'doc',
                    'version': 1,
                    'content': [
                        {
                            'type': 'paragraph',
                            'content': [
                                {
                                    'text': 'I did some work here.',
                                    'type': 'text'
                                }
                            ]
                        }
                    ]
                },
                'started': '2020-09-30T01:26:06.145+0000'
            }";
        private string _template2 = 
            @"{
                'timeSpentSeconds': " + _worklogPlaceholder + @",
                'visibility': {
                    'type': 'group',
                    'value': 'jira-developers'
                },
                'started': '2020-09-30T01:26:06.145+0000'
            }";
        private string _template3 = 
            @"{
                'timeSpent': '" + _worklogPlaceholder + @"'
            }";

        private string _worklog;
        public WorklogRequest(string worklog)
        {
            _worklog = worklog;
        }

        public string ToJson()
        {
            return _template3
                .Replace("\'", "\"")
                .Replace(_worklogPlaceholder, _worklog);
        }
    }
}
