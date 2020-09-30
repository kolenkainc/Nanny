namespace Nanny.Console.Commands.ExternalServices
{
    public class WorklogRequest
    {
        private string template1 = 
            @"{
                'timeSpentSeconds': {0},
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
        private string template2 = 
            @"{
                'timeSpentSeconds': {0},
                'visibility': {
                    'type': 'group',
                    'value': 'jira-developers'
                },
                'started': '2020-09-30T01:26:06.145+0000'
            }";

        private string _worklog;
        public WorklogRequest(string worklog)
        {
            _worklog = worklog;
        }

        public string ToJson()
        {
            return string.Format(template2, _worklog);
        }
    }
}
