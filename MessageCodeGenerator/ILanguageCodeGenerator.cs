using System.Collections.Generic;
using MessageCodeGenerator.Model;

namespace MessageCodeGenerator
{
    public interface ILanguageCodeGenerator
    {
        void GenerateCode(IEnumerable<Namespace> model);
    }
}