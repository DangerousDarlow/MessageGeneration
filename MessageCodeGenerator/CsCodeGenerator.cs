using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HandlebarsDotNet;
using MessageCodeGenerator.Model;
using Newtonsoft.Json;

namespace MessageCodeGenerator
{
    // ReSharper disable once UnusedMember.Global
    public class CsCodeGenerator : ILanguageCodeGenerator
    {
        private Func<object, string> MessageTemplate { get; } =
            Handlebars.Compile(File.ReadAllText("CsTemplates/Message.template"));

        private Func<object, string> EnumerationTemplate { get; } =
            Handlebars.Compile(File.ReadAllText("CsTemplates/Enumeration.template"));

        public void GenerateCode(IEnumerable<Namespace> model) =>
            model?.ToList().ForEach(GenerateNamespace);

        private void GenerateNamespace(Namespace nspace)
        {
            Directory.CreateDirectory(nspace.Name);
            nspace.Messages?.ToList().ForEach(GenerateMessage);
            nspace.Enumerations?.ToList().ForEach(GenerateEnumeration);
        }

        private void GenerateMessage(Message message)
        {
            var path = Path.Combine(message.Namespace, message.Name);
            File.WriteAllText($"{path}.cs", MessageTemplate(new CsMessage(message)));
        }

        private void GenerateEnumeration(Enumeration enumeration)
        {
            var path = Path.Combine(enumeration.Namespace, enumeration.Name);
            File.WriteAllText($"{path}.cs", EnumerationTemplate(new CsEnumeration(enumeration)));
        }
    }

    public class CsMessage
    {
        public CsMessage(Message message)
        {
            Name = message.Name;
            Namespace = message.Namespace;
            Properties = message.Properties?.Select(property => new CsProperty(property, Namespace));

            var json = JsonConvert.SerializeObject(message);
            var bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(json));
            Schema = bytes.Select(b => b.ToString("X2")).Aggregate((s1, s2) => s1 + s2);
        }

        public string Name { get; }

        public string Namespace { get; }

        public IEnumerable<CsProperty> Properties { get; }

        public string Schema { get; }
    }

    public class CsProperty
    {
        public CsProperty(Property property, string nspace)
        {
            Name = property.Name;
            Type = PropertyTypeToString(property.Type, nspace);
        }

        public string Name { get; }

        public string Type { get; }

        public static string PropertyTypeToString(PropertyType propertyType, string nspace)
        {
            switch (propertyType.Type)
            {
                case PropertyTypeEnum.Definition:
                    return DefinitionName(propertyType.Definition, nspace);

                case PropertyTypeEnum.Integer:
                    return "int";

                case PropertyTypeEnum.String:
                    return "string";

                case PropertyTypeEnum.DateTime:
                    return "DateTime";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string DefinitionName(IDefinition definition, string nspace) =>
            definition.Namespace == nspace
            ? definition.Name
            : $"{definition.Namespace}.{definition.Name}";
    }

    public class CsEnumeration
    {
        public CsEnumeration(Enumeration enumeration)
        {
            Name = enumeration.Name;
            Namespace = enumeration.Namespace;
            Enumerators = enumeration.Enumerators.Select(enumerator => new CsEnumerator(enumerator));
        }

        public string Name { get; }

        public string Namespace { get; }

        public IEnumerable<CsEnumerator> Enumerators { get; }
    }

    public class CsEnumerator
    {
        public CsEnumerator(Enumerator enumerator)
        {
            TemplateString = enumerator.Value.HasValue
                ? $"{enumerator.Name} = {enumerator.Value.Value}"
                : enumerator.Name;
        }

        public string TemplateString { get; }
    }
}