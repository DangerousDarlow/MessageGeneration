using System.Collections.Generic;
using System.Linq;

namespace MessageCodeGenerator.Model
{
    public class Message : IDefinition
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<Property> Properties { get; set; }

        public override string ToString() => $"Name '{Name}', Props {Properties?.Count() ?? 0}";
    }
}