using System;

namespace HikNAS
{
    public class HikException : System.Exception
    {
        public HikException() : base()
        {
        }

        public HikException(String message) : base(message)
        {
        }

        public HikException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

