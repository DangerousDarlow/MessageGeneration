namespace MessageCodeGenerator.Model
{
    public class Property
    {
        public string Name { get; set; }

        public PropertyType Type { get; set; }

        public override string ToString() => $"Name '{Name}', Type {Type}";
    }
}