using System.ComponentModel.DataAnnotations;

namespace Nanny.Console.Database
{
    public class Property
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
