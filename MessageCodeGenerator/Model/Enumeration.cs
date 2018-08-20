using System.Collections.Generic;
using System.Linq;

namespace MessageCodeGenerator.Model
{
    public class Enumeration : IDefinition
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<Enumerator> Enumerators { get; set; }

        public override string ToString() => $"Name '{Name}', Tors {Enumerators?.Count() ?? 0}";
    }
}