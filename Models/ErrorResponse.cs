﻿namespace Swagger_Demo.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public string? Details { get; set; } 
    }
}
