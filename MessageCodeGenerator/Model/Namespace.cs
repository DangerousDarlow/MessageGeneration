using System.Collections.Generic;

namespace MessageCodeGenerator.Model
{
    public class Namespace
    {
        public string Name { get; set; }

        public IEnumerable<Enumeration> Enumerations { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}