namespace Nanny.Console.Commands
{
    public class Key
    {
        private string _longName;
        private string _shortName;
        private string _prefix;
        
        public Key(string longName, string shortName)
        {
            _longName = longName;
            _shortName = shortName;
            _prefix = "--";
        }

        public bool IsSuite(string fromUser)
        {
            return fromUser == _prefix + _longName 
                   || fromUser == _prefix + _shortName;
        }

        public override string ToString()
        {
            return $"{_prefix}{_longName} or {_prefix}{_shortName}";
        }
    }
}