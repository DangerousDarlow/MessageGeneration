namespace MessageCodeGenerator.Model
{
    public enum PropertyTypeEnum
    {
        Definition,
        Integer,
        String
    }

    public class PropertyType
    {
        private IDefinition _definition;

        public IDefinition Definition
        {
            get => _definition;
            set
            {
                Type = PropertyTypeEnum.Definition;
                _definition = value;
            }
        }

        public PropertyTypeEnum Type { get; set; }
    }
}