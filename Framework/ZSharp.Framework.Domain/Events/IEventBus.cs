﻿using System.Collections.Generic;

namespace ZSharp.Framework.Domain
{
    public interface IEventBus
    {
        void Publish(Envelope<IEvent> @event);

        void Publish(IEnumerable<Envelope<IEvent>> events);
    }
}
