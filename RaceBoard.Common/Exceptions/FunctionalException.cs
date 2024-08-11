using RaceBoard.Common.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RaceBoard.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class FunctionalException : Exception
    {
        public ErrorType FunctionalError { get; }
        public List<string> Errors { get; }

        public FunctionalException(ErrorType functionalError, List<string> errors, bool setErrorsToBaseExceptionMessage = false) : base(GetErrorsMessage(errors, setErrorsToBaseExceptionMessage))
        {
            FunctionalError = functionalError;
            Errors = errors;
        }

        public FunctionalException(ErrorType functionalError, string message, bool setErrorsToBaseExceptionMessage = false) : base(GetErrorsMessage(new List<string>() { message }, setErrorsToBaseExceptionMessage))
        {
            FunctionalError = functionalError;
            Errors = new List<string> { message };
        }

        public FunctionalException(ErrorType functionalError) : base()
        {
            FunctionalError = functionalError;
            Errors = new List<string>();
        }

        private static string GetErrorsMessage(List<string> errors, bool setErrorsToBaseExceptionMessage)
        {
            if (setErrorsToBaseExceptionMessage && errors != null && errors.Any())
            {
                return string.Join(",", errors);
            }

            return null;
        }

        protected FunctionalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var errorType = info.GetString("FunctionalError");
            var errors = info.GetString("Errors");

            if (!string.IsNullOrEmpty(errorType) && errors != null)
            {
                this.FunctionalError = Enum.Parse<ErrorType>(errorType);
                this.Errors = new List<string>() { errors };
            }

            if (this.Errors == null)
            {
                this.Errors = new List<string>();
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("FunctionalError", (int)this.FunctionalError);
            info.AddValue("Errors", GetErrorsMessage(this.Errors, true));

            base.GetObjectData(info, context);
        }
    }
}
