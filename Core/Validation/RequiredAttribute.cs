namespace Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute
    {
        public string ErrorMessage { get; set; } = "Field is required.";
        public RequiredAttribute() { }
        public RequiredAttribute(string errorMessage) => ErrorMessage = errorMessage;
    }
}
