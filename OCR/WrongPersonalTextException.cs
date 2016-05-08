using System;
using System.Runtime.Serialization;

namespace OCR
{
    [Serializable]
    internal class WrongPersonalTextException : Exception
    {
        public WrongPersonalTextException()
        {
        }

        public WrongPersonalTextException(string message) : base(message)
        {
        }

        public WrongPersonalTextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongPersonalTextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}