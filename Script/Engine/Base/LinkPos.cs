using System.Collections.Generic;
using UnityEngine;
using System;

namespace Engine
{
    [System.Serializable, Flags]
    public enum Face
    {
        None = 0,
        Top = 1,
        Bot = 2,
        Front = 4,
        Back = 8,
        Right = 16,
        Left = 32
    }

    [System.Serializable]
    public class LinkPos
    {
    	[System.Serializable]
        public enum Neighbor : int
        {
            Top = 1,
            Bottom = -1,
            Left = -2,
            Right = 2,
            Back = 3,
            Front = -3
        }

        public UnitPos Center;
        public Face Mask = Face.None;
        public int Life = int.MaxValue;

        [SerializeField]
        private UnitPos Top = new UnitPos(0, 0, 0);
        [SerializeField]
        private UnitPos Bottom = new UnitPos(0, 0, 0);
        [SerializeField]
        private UnitPos Left = new UnitPos(0, 0, 0);
        [SerializeField]
        private UnitPos Right = new UnitPos(0, 0, 0);
        [SerializeField]
        private UnitPos Back = new UnitPos(0, 0, 0);
        [SerializeField]
        private UnitPos Front = new UnitPos(0, 0, 0);

        public LinkPos(UnitPos center, int life)
        {
            Center = new UnitPos(center);
            Life = life;
        }

        public LinkPos(UnitPos center)
        {
            Center = new UnitPos(center);
        }

        public void Add(Neighbor where, UnitPos pos)
        {
            if (Has(where))
            {
                Debug.LogError("Linkpos already know this neighbor");
            }
            AddMask(where, pos);
        }

        public void Remove(Neighbor where)
        {
            if(!Has(where))
            {
                Debug.LogError("Remove neighbor but doesn't exist. " + Center + " " + where);
                return;
            }
            RemoveMask(where);
        }

        public bool Has(Neighbor where)
        {
        	switch (where)
            {
                case Neighbor.Top:
                    return (Mask & Face.Top) == Face.Top;

                case Neighbor.Bottom:
                    return (Mask & Face.Bot) == Face.Bot;
     
                case Neighbor.Left:
                    return (Mask & Face.Left) == Face.Left;
                   
                case Neighbor.Right:
                    return (Mask & Face.Right) == Face.Right;
     
                case Neighbor.Front:
                    return (Mask & Face.Front) == Face.Front;
                    
                case Neighbor.Back:
                    return (Mask & Face.Back) == Face.Back;
                    
                default:
                    break;
            }
            return false;
        }

        public bool Has(UnitPos pos)
        {
        	if(Has(Neighbor.Top) && pos == Top)
        	{
        		return true;
        	}
        	if(Has(Neighbor.Bottom) && pos == Bottom)
        	{
        		return true;
        	}
        	if(Has(Neighbor.Left) && pos == Left)
        	{
        		return true;
        	}
        	if(Has(Neighbor.Right) && pos == Right)
        	{
        		return true;
        	}
        	if(Has(Neighbor.Front) && pos == Front)
        	{
        		return true;
        	}
        	if(Has(Neighbor.Back) && pos == Back)
        	{
        		return true;
        	}
        	return false;
        }

        private void AddMask(Neighbor where, UnitPos pos)
        {
            switch (where)
            {
                case Neighbor.Top:
                    Mask |= Face.Top;
                    Top = pos;
                    break;

                case Neighbor.Bottom:
                    Mask |= Face.Bot;
                    Bottom = pos;
                    break;

                case Neighbor.Left:
                    Mask |= Face.Left;
                    Left = pos;
                    break;

                case Neighbor.Right:
                    Mask |= Face.Right;
                    Right = pos;
                    break;

                case Neighbor.Front:
                    Mask |= Face.Front;
                    Front = pos;
                    break;

                case Neighbor.Back:
                    Mask |= Face.Back;
                    Back = pos;
                    break;

                default:
                    break;
            }
        }

        private void RemoveMask(Neighbor where)
        {
            switch (where)
            {
                case Neighbor.Top:
                    Mask ^= Face.Top;
                    break;

                case Neighbor.Bottom:
                    Mask ^= Face.Bot;
                    break;

                case Neighbor.Left:
                    Mask ^= Face.Left;
                    break;

                case Neighbor.Right:
                    Mask ^= Face.Right;
                    break;

                case Neighbor.Front:
                    Mask ^= Face.Front;
                    break;

                case Neighbor.Back:
                    Mask ^= Face.Back;
                    break;

                default:
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            return Center.Equals(((LinkPos)obj).Center);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(LinkPos c1, LinkPos c2)
        {
            return c1.Center.Equals(c2.Center);
        }

        public static bool operator !=(LinkPos c1, LinkPos c2)
        {
            return !c1.Center.Equals(c2.Center);
        }

        public Dictionary<LinkPos.Neighbor, UnitPos> getNeighbor()
        {
            Dictionary<LinkPos.Neighbor, UnitPos> neighbor = new Dictionary<Neighbor, UnitPos>();

            if (Has(Neighbor.Top))
            {
                neighbor.Add(Neighbor.Top, Top);
            }
            if (Has(Neighbor.Bottom))
            {
                neighbor.Add(Neighbor.Bottom, Bottom);
            }
            if (Has(Neighbor.Left))
            {
                neighbor.Add(Neighbor.Left, Left);
            }
            if (Has(Neighbor.Right))
            {
                neighbor.Add(Neighbor.Right, Right);
            }
            if (Has(Neighbor.Front))
            {
                neighbor.Add(Neighbor.Front, Front);
            }
            if (Has(Neighbor.Back))
            {
                neighbor.Add(Neighbor.Back, Back);
            }

            return neighbor;
        }

        public bool HasContact(Vector3 pos, float size)
        {
            return HasContact(Center.x - size, Center.x + size, pos.x)
                && HasContact(Center.y - size, Center.y + size, pos.y)
                && HasContact(Center.z - size, Center.z + size, pos.z);
        }

        private bool HasContact(float min, float max, float start)
        {
            return min <= start && max >= start;
        }
    }
}
    