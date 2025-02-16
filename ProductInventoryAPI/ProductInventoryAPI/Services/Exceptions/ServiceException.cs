// <copyright file="ServiceException.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Services.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        protected ServiceException(HttpStatusCode statusCode)
            : base($"An error occurred with status code {statusCode}.") // Default message
        {
            StatusCode = statusCode;
        }

        protected ServiceException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        protected ServiceException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected ServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            StatusCode = (HttpStatusCode)info.GetValue(nameof(StatusCode), typeof(HttpStatusCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(StatusCode), StatusCode);
        }
    }
}
