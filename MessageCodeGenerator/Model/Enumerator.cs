namespace MessageCodeGenerator.Model
{
    public class Enumerator
    {
        public string Name { get; set; }

        public int? Value { get; set; }

        public override string ToString() => $"Name '{Name}' Value {Value}";
    }
}