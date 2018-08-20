using System;
using System.Collections.Generic;
using System.Linq;
using MessageCodeGenerator.Model;
using Newtonsoft.Json.Linq;

namespace MessageCodeGenerator
{
    public interface IIdlToModel
    {
        IEnumerable<Namespace> Transform(IEnumerable<JObject> idl);
    }

    // ReSharper disable once UnusedMember.Global
    public class IdlToModel : IIdlToModel
    {
        public IdlToModel(IDefinitionDictionary definitionDictionary)
        {
            DefinitionDictionary = definitionDictionary;
        }

        private IDefinitionDictionary DefinitionDictionary { get; }

        public IEnumerable<Namespace> Transform(IEnumerable<JObject> idl)
        {
            var idlList = idl.ToList();

            // First iteration through idl namespaces creates all message and
            // enumeration objects. This is so they can be referenced by other
            // messages when building the model.
            idlList.ForEach(CreateMessageAndEnumerationStubs);

            var namespaces = new List<Namespace>();

            // Second iteration through idl namespaces transforms
            // the idl definitions into model definitions
            idlList.ForEach(i => namespaces.Add(Transform(i)));

            return namespaces;
        }

        private void CreateMessageAndEnumerationStubs(JObject namespaceIdl)
        {
            var nspace = namespaceIdl["namespace"]?.Value<string>();
            namespaceIdl["messages"]?.ToList().ForEach(token => CreateMessageStub(token, nspace));
            namespaceIdl["enumerations"]?.ToList().ForEach(token => CreateEnumerationStub(token, nspace));
        }

        private void CreateMessageStub(JToken messageIdl, string nspace) =>
            DefinitionDictionary.Add(new Message
                {
                    Name = messageIdl["name"]?.Value<string>(),
                    Namespace = nspace
                },
                nspace);

        private void CreateEnumerationStub(JToken enumerationIdl, string nspace) =>
            DefinitionDictionary.Add(new Enumeration
                {
                    Name = enumerationIdl["name"]?.Value<string>(),
                    Namespace = nspace
                },
                nspace);

        private Namespace Transform(JObject namespaceIdl)
        {
            var nspace = namespaceIdl["namespace"]?.Value<string>();

            return new Namespace
            {
                Name = nspace,
                Messages = namespaceIdl["messages"]?.Select(token => TransformMessage(token, nspace)).ToList()
            };
        }

        private Message TransformMessage(JToken messageIdl, string nspace)
        {
            try
            {
                var message = DefinitionDictionary.Get(messageIdl["name"]?.Value<string>(), nspace) as Message;
                if (message == null)
                    throw new NullReferenceException(messageIdl["name"]?.Value<string>());

                message.Properties = messageIdl["properties"]?.Select(
                    token => TransformProperty(token, nspace)).ToList();

                return message;
            }
            catch (Exception e)
            {
                throw new AggregateException(
                    $"Failed to transform message {nspace}.{messageIdl["name"]?.Value<string>()}",
                    e);
            }
        }

        private Property TransformProperty(JToken propertyIdl, string nspace)
        {
            try
            {
                return new Property
                {
                    Name = propertyIdl["name"]?.Value<string>(),
                    Type = TransformPropertyType(propertyIdl["type"]?.Value<string>(), nspace)
                };
            }
            catch (Exception e)
            {
                throw new AggregateException(
                    $"Failed to transform property {propertyIdl["name"]?.Value<string>()}",
                    e);
            }
        }

        private PropertyType TransformPropertyType(string idl, string nspace)
        {
            if (Enum.TryParse(idl, out PropertyTypeEnum propertyTypeEnum))
                return new PropertyType
                {
                    Type = propertyTypeEnum
                };

            return new PropertyType
            {
                Type = PropertyTypeEnum.Definition,
                Definition = DefinitionDictionary.Get(idl, nspace)
            };
        }
    }
}