using EFZ.Core.Validation.Interfaces;

namespace EFZ.Core.Validation.Impl
{
    public class ValidationResult : IValidationResult
    {
        #region  Constructors

        private ValidationResult()
        {
        }

        #endregion

        #region  Properties

        public bool IsValid { get; private set; }
        public string Message { get; private set; }

        #endregion

        public static IValidationResult ResultOk()
        {
            return new ValidationResult
            {
                IsValid = true
            };
        }

        public static IValidationResult ResultFailed(string message)
        {
            return new ValidationResult
            {
                IsValid = false,
                Message = message
            };
        }
    }
}