// <copyright file="DataAccessException.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Services.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    [Serializable]
    public class DataAccessException : ServiceException
    {
        public DataAccessException()
            : base(HttpStatusCode.InternalServerError, "A data access error occurred.")
        {
        }

        public DataAccessException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public DataAccessException(string message, Exception innerException)
            : base(HttpStatusCode.InternalServerError, message, innerException)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
