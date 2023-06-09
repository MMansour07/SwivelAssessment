﻿using System;

namespace Swivel.Core.Interfaces
{
    public interface IResponseModel
    {
        Exception Ex { get; set; }
        string Message { get; set; }
        bool Success { get; set; }
    }
}
