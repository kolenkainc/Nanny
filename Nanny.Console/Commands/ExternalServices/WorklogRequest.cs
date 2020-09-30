using System;
using System.Globalization;
using System.Text.RegularExpressions;

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
                'timeSpentSeconds': " + _worklogPlaceholder + @"
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
                .Replace(_worklogPlaceholder, ParseHuman().TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }
        
        public TimeSpan ParseHuman()
        {
            TimeSpan ts = TimeSpan.Zero;
            string currentString = "";
            string currentNumber = "";
            foreach (char ch in _worklog + ' ')
            {
                currentString += ch;
                if (Regex.IsMatch(currentString, @"^(days(\d|\s)|day(\d|\s)|d(\d|\s))", RegexOptions.IgnoreCase)) { ts = ts.Add(TimeSpan.FromDays(int.Parse(currentNumber))); currentString = ""; currentNumber = ""; }
                if (Regex.IsMatch(currentString, @"^(hours(\d|\s)|hour(\d|\s)|h(\d|\s))", RegexOptions.IgnoreCase)) { ts = ts.Add(TimeSpan.FromHours(int.Parse(currentNumber))); currentString = ""; currentNumber = ""; }
                if (Regex.IsMatch(currentString, @"^(ms(\d|\s))", RegexOptions.IgnoreCase)) { ts = ts.Add(TimeSpan.FromMilliseconds(int.Parse(currentNumber))); currentString = ""; currentNumber = ""; }
                if (Regex.IsMatch(currentString, @"^(mins(\d|\s)|min(\d|\s)|m(\d|\s))", RegexOptions.IgnoreCase)) { ts = ts.Add(TimeSpan.FromMinutes(int.Parse(currentNumber))); currentString = ""; currentNumber = ""; }
                if (Regex.IsMatch(currentString, @"^(secs(\d|\s)|sec(\d|\s)|s(\d|\s))", RegexOptions.IgnoreCase)) { ts = ts.Add(TimeSpan.FromSeconds(int.Parse(currentNumber))); currentString = ""; currentNumber = ""; }
                if (Regex.IsMatch(ch.ToString(), @"\d")) { currentNumber += ch; currentString = ""; }
            }
            return ts;
        }
    }
}
