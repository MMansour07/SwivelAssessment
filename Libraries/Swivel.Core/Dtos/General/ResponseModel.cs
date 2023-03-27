using Swivel.Core.Interfaces;
using System;

namespace Swivel.Core.Dtos.General
{
    public class ResponseModel<T> : IResponseModel
    {
        public T Data { get; set; }
        public Exception Ex { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
