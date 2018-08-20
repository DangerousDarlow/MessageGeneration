using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ninject;
using Ninject.Extensions.Conventions;

namespace MessageCodeGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            // Ninject?! Surely using it is overkill for such a simple app?
            // Perhaps but I wanted to see if I could use Ninject to discover language
            // specific generators. Adding support for a new language should just mean
            // adding a code generator implementation for that language; no manual
            // wiring necessary.
            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .BindAllInterfaces());

            // A real code generator would make the source of definitions dynamic
            var idlFiles = Directory.GetFiles("Idl", "*.idl.json").ToList();
            var idl = idlFiles.Select(path => JObject.Parse(File.ReadAllText(path)));

            var generator = kernel.Get<ICodeGenerator>();
            generator.GenerateCode(idl);
        }
    }
}