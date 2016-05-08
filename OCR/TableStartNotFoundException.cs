using System;
using System.Runtime.Serialization;

namespace OCR
{
    [Serializable]
    internal class TableStartNotFoundException : Exception
    {
        public TableStartNotFoundException()
        {
        }

        public TableStartNotFoundException(string message) : base(message)
        {
        }

        public TableStartNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TableStartNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}