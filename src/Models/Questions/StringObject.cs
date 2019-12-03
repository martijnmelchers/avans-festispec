namespace Festispec.Models.Questions
{
    public class StringObject
    {
        public StringObject()
        {

        }
        public StringObject(string value)
        {
            Value = value;
        }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}