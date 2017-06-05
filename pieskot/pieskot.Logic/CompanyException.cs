using System;
using System.Runtime.Serialization;

namespace NaSpacerDo.Logic
{
    [Serializable]
    public class CompanyException : Exception
    {
        public CompanyException()
        {
        }

        public CompanyException(string message) : base(message)
        {
        }

        public CompanyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CompanyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}