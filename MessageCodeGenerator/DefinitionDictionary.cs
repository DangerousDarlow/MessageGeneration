using System.Collections.Generic;
using MessageCodeGenerator.Model;

namespace MessageCodeGenerator
{
    public interface IDefinitionDictionary
    {
        void Add(IDefinition definition, string nspace);

        IDefinition Get(string name, string nspace);
    }

    // ReSharper disable once UnusedMember.Global
    public class DefinitionDictionary : IDefinitionDictionary
    {
        private Dictionary<string, IDefinition> Definitions { get; } = new Dictionary<string, IDefinition>();

        public void Add(IDefinition definition, string nspace) =>
            Definitions.Add(CreateKey(definition.Name, nspace), definition);

        public IDefinition Get(string name, string nspace)
        {
            // Assume name is unqualified; append namespace
            var key = CreateKey(name, nspace);
            if (Definitions.ContainsKey(key))
                return Definitions[key];

            // Assume name is qualified
            if (Definitions.ContainsKey(name))
                return Definitions[name];

            throw new KeyNotFoundException(name);
        }

        private static string CreateKey(string name, string nspace) => $"{nspace}.{name}";
    }
}