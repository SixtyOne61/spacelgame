using UnityEngine;
using System;

namespace Engine
{
    public class PosClamp
    {
        // x was min and y was max
        public Vector2 Clamp;
        public float Size;
        public float Half;
        public float Center;
        public bool IsInit = false;
        
        public PosClamp()
        {

        }

        public PosClamp(PosClamp param)
        {
            Clamp = new Vector2(param.Clamp.x, param.Clamp.y);
            Size = param.Size;
            Half = param.Half;
            Center = param.Center;
            IsInit = param.IsInit;
        }

        public void AddBest(float val)
        {
            if(!IsInit)
            {
                IsInit = true;
                Clamp.x = val;
                Clamp.y = val;
            }
            else if (Clamp.x > val)
            {
                Clamp.x = val;
            }
            else if (Clamp.y < val)
            {
                Clamp.y = val;
            }
        }

        public void Terminate(float size)
        {
            Clamp.y += size;
            Size = Clamp.y - Clamp.x;
            Half = Size / 2.0f;
            Center = (Clamp.x + Clamp.y) / 2.0f;
        }

        public bool HasContact(PosClamp other)
        {
            return Clamp.x >= other.Clamp.x ? Clamp.x <= other.Clamp.y : Clamp.y >= other.Clamp.x;
        }

        public bool HasInContact(PosClamp other)
        {
            return Clamp.x <= other.Clamp.x && Clamp.y >= other.Clamp.y;
        }

        public bool HasContact(float start, float end)
        {
            return Clamp.x >= start ? Clamp.x <= end : Clamp.y >= start;
        }

        public Vector2 GetVolumeContact(Vector2 other)
        {
            float start = Clamp.x >= other.x ? Clamp.x : other.x;
            float end = Clamp.y <= other.y ? Clamp.y : other.y;

            return new Vector2(start, end);
        }
    }
}
