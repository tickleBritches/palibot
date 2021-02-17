using PaliBot.Model.Frame;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PaliBot.Model.Event
{
    public class ThrowEvent : GameEvent
    {
        public IPlayer Player { get; private set; }

        public ThrowEvent(IFrame frame, IPlayer player) : base(frame)
        {
            Player = player;
        }
    }
}
