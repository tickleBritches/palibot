using PaliBot.Model.Frame;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Model.Play
{
    class GrabDiscPlay : GamePlay
    {
        public GrabDiscPlay(IPlayer player, HandSide hand) : base(0)
        {
            Player = player;
            Hand = hand;
        }

        public IPlayer Player { get; }
        public HandSide Hand { get; }

        public override string Describe()
        {
            return $"{Player.Name} grabs the disc with the {Hand.ToString().ToLower()} hand";
        }
    }
}
