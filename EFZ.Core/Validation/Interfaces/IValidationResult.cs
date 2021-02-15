namespace EFZ.Core.Validation.Interfaces
{
    public interface IValidationResult
    {

        bool IsValid { get; }

        string Message { get; }
    }
}