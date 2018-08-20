using System.Collections.Generic;
using System.Linq;
using MessageCodeGenerator.Model;

namespace MessageCodeGenerator
{
    public interface ICodeGenerator
    {
        void GenerateCode(IEnumerable<Namespace> namespaces);
    }

    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator(IEnumerable<ILanguageCodeGenerator> languageCodeGenerators)
        {
            LanguageCodeGenerators = languageCodeGenerators;
        }

        private IEnumerable<ILanguageCodeGenerator> LanguageCodeGenerators { get; }

        public void GenerateCode(IEnumerable<Namespace> namespaces) =>
            LanguageCodeGenerators.ToList().ForEach(x => x.GenerateCode(namespaces));
    }
}