using System;
using System.Runtime.Serialization;

namespace OCR
{
    [Serializable]
    public class ContributionPeriodCreationException : Exception
    {
        public ContributionPeriodCreationException()
        {
        }

        public ContributionPeriodCreationException(string message) : base(message)
        {
        }

        public ContributionPeriodCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContributionPeriodCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}