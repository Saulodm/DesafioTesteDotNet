namespace Core.Validation
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public IList<ValidationError> Errors { get; } = new List<ValidationError>();
    }
    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
