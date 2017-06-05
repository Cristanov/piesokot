using System;

namespace NaSpacerDo.Logic
{
    public class Result<T>
    {
        public Result(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public Result(T data, string message) : this(data)
        {
            Message = message;
        }

        public Result(T data)
        {
            Data = data;
        }

        public T Data { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
