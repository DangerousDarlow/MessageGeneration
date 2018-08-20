using System.Collections.Generic;
using System.Linq;

namespace MessageCodeGenerator.Model
{
    public class Namespace
    {
        public string Name { get; set; }

        public IEnumerable<Enumeration> Enumerations { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        public override string ToString() =>
            $"Name '{Name}', Enums {Enumerations?.Count() ?? 0}, Msgs {Messages?.Count() ?? 0}";
    }
}