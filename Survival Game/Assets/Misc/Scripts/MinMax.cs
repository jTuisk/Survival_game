using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Misc
{
    public class MinMax
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMax()
        {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public void AddValue(float x)
        {
            if (x < Min)
            {
                Min = x;
            }
            if (x > Max)
            {
                Max = x;
            }
        }
    }
}