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

    [Serializable]
    public class CenaeCreationException : Exception
    {
        public CenaeCreationException()
        {
        }

        public CenaeCreationException(string message) : base(message)
        {
        }

        public CenaeCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CenaeCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class SocialReasonException : Exception
    {
        public SocialReasonException()
        {
        }

        public SocialReasonException(string message) : base(message)
        {
        }

        public SocialReasonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SocialReasonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class NifException : Exception
    {
        public NifException()
        {
        }

        public NifException(string message) : base(message)
        {
        }

        public NifException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NifException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}