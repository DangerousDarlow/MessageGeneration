using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MessageCodeGenerator
{
    public interface ICodeGenerator
    {
        void GenerateCode(IEnumerable<JObject> idl);
    }

    // ReSharper disable once UnusedMember.Global
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator(
            IIdlToModel idlToModel,
            IEnumerable<ILanguageCodeGenerator> languageCodeGenerators)
        {
            IdlToModel = idlToModel;
            LanguageCodeGenerators = languageCodeGenerators;
        }

        private IIdlToModel IdlToModel { get; }

        private IEnumerable<ILanguageCodeGenerator> LanguageCodeGenerators { get; }

        public void GenerateCode(IEnumerable<JObject> idl)
        {
            var model = IdlToModel.Transform(idl);
            LanguageCodeGenerators.ToList().ForEach(generator => generator.GenerateCode(model));
        }
    }
}