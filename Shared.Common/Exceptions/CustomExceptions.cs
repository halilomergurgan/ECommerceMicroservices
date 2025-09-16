namespace Shared.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public List<string> Errors { get; }
    
    public ValidationException(List<string> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
