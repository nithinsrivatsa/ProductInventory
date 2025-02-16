// <copyright file="ProductIdGenerator.cs" company="Carl Zeiss">
//   Copyright 2025 Carl Zeiss. All rights reserved
// </copyright>

namespace ProductInventoryAPI.Helper
{
    using System;

    public class ProductIdGenerator
    {
        private const int MaxInstances = 10;
        private const long BaseProductId = 100000;
        private const long MaxSequence = 9999;
        private static readonly object LockObj = new object();
        private static long lastTimestamp = -1L;
        private static long sequence = 0L;
        private readonly long nodeId;
        private readonly long epoch = 1640995200000L;

        public ProductIdGenerator(long nodeId)
        {
            if (nodeId < 0 || nodeId >= MaxInstances)
            {
                throw new ArgumentException($"Node ID must be between 0 and {MaxInstances - 1}");
            }

            this.nodeId = nodeId;
        }

        public long GenerateProductId()
        {
            lock (LockObj)
            {
                long timestamp = GetCurrentTimestamp();

                if (timestamp < lastTimestamp)
                {
                    throw new Exception("Clock moved backwards. Refusing to generate ID.");
                }

                if (timestamp == lastTimestamp)
                {
                    sequence = (sequence + 1) % MaxSequence;
                    if (sequence == 0)
                    {
                        timestamp = WaitForNextTimestamp(lastTimestamp);
                    }
                }
                else
                {
                    sequence = 0;
                }

                lastTimestamp = timestamp;

                // Combine timestamp, nodeId, and sequence to generate a unique ID
                long uniqueId = ((timestamp - epoch) % 900000) + BaseProductId + (nodeId * MaxSequence) + sequence;
                return uniqueId;
            }
        }

        private long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private long WaitForNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }

            return timestamp;
        }
    }
}
