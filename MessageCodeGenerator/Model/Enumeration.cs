using System.Collections.Generic;

namespace MessageCodeGenerator.Model
{
    public class Enumeration : IDefinition
    {
        public string Name { get; set; }

        public IEnumerable<Enumerator> Enumerators { get; set; }
    }
}