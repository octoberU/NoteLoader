using Newtonsoft.Json;
using System;
using System.Numerics;
namespace AudicaTools
{
    [Serializable]
    public class Cue
    {
        public int tick;
        public int tickLength;
        public int pitch;
        public int velocity;
        public GridOffset gridOffset;
        public float zOffset;
        public HandType handType;
        public Behavior behavior;

        [JsonConstructor]
        public Cue(int tick, int tickLength, int pitch, int velocity, GridOffset gridOffset, float zOffset, int handType, int behavior)
        {
            this.tick = tick;
            this.tickLength = tickLength;
            this.pitch = pitch;
            this.velocity = velocity;
            this.gridOffset = gridOffset;
            this.zOffset = zOffset;
            this.handType = (HandType)handType;
            this.behavior = (Behavior)behavior;
        }

        public Cue(int tick, int tickLength, int pitch, int velocity, GridOffset gridOffset, float zOffset, HandType handType, Behavior behavior)
        {
            this.tick = tick;
            this.tickLength = tickLength;
            this.pitch = pitch;
            this.velocity = velocity;
            this.gridOffset = gridOffset;
            this.zOffset = zOffset;
            this.handType = handType;
            this.behavior = behavior;
        }

        [Serializable]
        public struct GridOffset
        {
            public float x;
            public float y;
        }

        public enum Behavior
        {
            Standard,
            Vertical,
            Horizontal,
            Hold,
            ChainStart,
            Chain,
            Melee,
            Dodge
        }

        public enum HandType
        {
            Either,
            Right,
            Left,
            None
        }
    }

}