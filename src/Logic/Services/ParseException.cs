using System;

namespace PL_Resolution.Logic.Services
{
    public class ParseException : Exception
    {
        public ParseException(int line, string message)
        {
            Message = $"Line {line}: {message}!";
        }

        public string Message { get; }
    }
}