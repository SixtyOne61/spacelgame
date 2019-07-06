using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class BoxParam
    {
        // info on each axis
        public PosClamp x = new PosClamp();
        public PosClamp y = new PosClamp();
        public PosClamp z = new PosClamp();

        // each edge
        public List<Vector3> points = new List<Vector3>();

        // size param
        public float LargeSize;
        public float Ray;

        // center of the box
        public Vector3 Center;

        public BoxParam()
        {

        }

        public BoxParam(BoxParam param)
        {
            x = new PosClamp(param.x);
            y = new PosClamp(param.y);
            z = new PosClamp(param.z);
            points.AddRange(param.points);
            LargeSize = param.LargeSize;
            Ray = param.Ray;
            Center = new Vector3(param.Center.x, param.Center.y, param.Center.z);
        }

        public BoxParam(Vector3 center)
        {
            Center = center;
        }

        public void Terminate(float size)
        {
            x.Terminate(size);
            z.Terminate(size);
            y.Terminate(size);

            LargeSize = Mathf.Max(x.Size, y.Size, z.Size);
            Center = new Vector3(x.Center, y.Center, z.Center);
            Ray = Vector3.Distance(Center, new Vector3(x.Clamp.y, y.Clamp.y, z.Clamp.y));

            points.Add(new Vector3(x.Clamp.y, y.Clamp.x, z.Clamp.y));
            points.Add(new Vector3(x.Clamp.y, y.Clamp.x, z.Clamp.x));
            points.Add(new Vector3(x.Clamp.x, y.Clamp.x, z.Clamp.x));
            points.Add(new Vector3(x.Clamp.x, y.Clamp.x, z.Clamp.y));

            points.Add(new Vector3(x.Clamp.y, y.Clamp.y, z.Clamp.y));
            points.Add(new Vector3(x.Clamp.y, y.Clamp.y, z.Clamp.x));
            points.Add(new Vector3(x.Clamp.x, y.Clamp.y, z.Clamp.x));
            points.Add(new Vector3(x.Clamp.x, y.Clamp.y, z.Clamp.y));
        }

        public bool HasContact(BoxParam other)
        {
            return x.HasContact(other.x) && y.HasContact(other.y) && z.HasContact(other.z);
        }

        public bool IsAround(BoxParam other)
        {
            return x.HasInContact(other.x) && y.HasInContact(other.y) && z.HasInContact(other.z);
        }

        public float MinDistance(Vector3 edge)
        {
            float distance = Vector3.Distance(edge, points[0]);

            for (int i = 1; i < points.Count; ++i)
            {
                distance = Mathf.Min(distance, Vector3.Distance(edge, points[i]));
            }

            return distance;
        }

        public float MinDistance(BoxParam other)
        {
            float distance = other.MinDistance(points[0]);

            for(int i = 1; i < points.Count; ++i)
            {
                distance = Mathf.Min(distance, other.MinDistance(points[i]));
            }

            return distance;
        }
    }
}
