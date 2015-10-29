﻿using System;
using ZSharp.Framework.SqlDb;

namespace ZSharp.Framework.Domain
{
    public class MessageEntity : EfEntity<long>
    {
        public string Body { get; set; }

        public string BodyTypeName { get; set; }

        public DateTimeOffset DeliveryDate { get; set; }

        public string CorrelationId { get; set; }

        public TimeSpan? Delay { get; set; }

        public TimeSpan? TimeToLive { get; set; }

        public string SysCode { get; set; }

        public string Topic { get; set; }
    }
}
