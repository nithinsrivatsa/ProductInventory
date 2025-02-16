// <copyright file="NotFoundServiceException.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Services.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotFoundException : ServiceException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound, "The requested resource was not found.")
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(HttpStatusCode.NotFound, message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
