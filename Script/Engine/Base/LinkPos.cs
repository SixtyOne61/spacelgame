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
        public SerializableDictionary<Neighbor, UnitPos> Neighbors = new SerializableDictionary<Neighbor, UnitPos>();
        public int Life = int.MaxValue;

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
            if (Neighbors.ContainsKey(where))
            {
                Debug.LogError("Linkpos already know this neighbor");
            }
            Neighbors.Add(where, pos);
            AddMask(where);
        }

        public void Remove(Neighbor where)
        {
            if(!Neighbors.ContainsKey(where))
            {
                Debug.LogError("Remove neighbor but doesn't exist. " + Center + " " + where);
                return;
            }
            Neighbors.Remove(where);
            RemoveMask(where);
        }

        public bool Has(Neighbor where)
        {
            return Neighbors.ContainsKey(where);
        }

        public bool Has(UnitPos pos)
        {
            foreach(KeyValuePair<Neighbor, UnitPos> our in Neighbors)
            {
                if(pos == our.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddMask(Neighbor where)
        {
            switch (where)
            {
                case Neighbor.Top:
                    Mask |= Face.Top;
                    break;

                case Neighbor.Bottom:
                    Mask |= Face.Bot;
                    break;

                case Neighbor.Left:
                    Mask |= Face.Left;
                    break;

                case Neighbor.Right:
                    Mask |= Face.Right;
                    break;

                case Neighbor.Front:
                    Mask |= Face.Front;
                    break;

                case Neighbor.Back:
                    Mask |= Face.Back;
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
    }
}
