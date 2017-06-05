using System;

namespace NaSpacerDo.Logic
{

    [Serializable]
    public class PermissionCompanyException : Exception
    {
        public PermissionCompanyException() { }
        public PermissionCompanyException(string message) : base(message) { }
        public PermissionCompanyException(string message, Exception inner) : base(message, inner) { }
        protected PermissionCompanyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
