using System.Linq;
using Nanny.Console.Database;

namespace Nanny.Console.Commands
{
    public class LoginCommand : Command
    {
        private Key _key = new Key("login", "l");
        private ApplicationContext _db;

        public LoginCommand(ApplicationContext db)
        {
            _db = db;
        }
        
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override string Output()
        {
            if (_db.Properties.Count() == 0)
            {
                _db.Properties.Add(new Properties
                {
                    Key = "",
                    Value = "password"
                });
                _db.SaveChanges();
                return "";
            }
            else
            {
                return "It's OK";
            }
        }

        public override Key Key()
        {
            return _key;
        }
    }
}
