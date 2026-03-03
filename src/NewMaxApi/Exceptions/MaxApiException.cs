using System;

namespace NewMaxApi.Exceptions
{
    public class MaxApiException : Exception
    {
        public MaxApiException()
        {
        }

        public MaxApiException(string? message) : base(message)
        {
        }

        public MaxApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
