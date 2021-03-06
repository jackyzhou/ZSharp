﻿using System;

namespace ZSharp.Framework.Extensions
{
    public static class MiscExtensions
    {
        public static void Dump(this Exception exc)
        {
            exc.StackTrace.Dump();
            exc.Message.Dump();
        }

        public static TimeSpan Double(this TimeSpan timeSpan)
        {
            return timeSpan + timeSpan;
        }
    }
}
