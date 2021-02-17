using PaliBot.Model.Frame;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Model.Event
{
    public class PossessionChangeEvent : GameEvent
    {
        public IPlayer Player { get; private set; }
        public HandSide? Hand { get; private set; }

        public PossessionChangeEvent(IFrame frame, IPlayer player, HandSide? hand) : base(frame)
        {
            Player = player;
            Hand = hand;
        }
    }
}
