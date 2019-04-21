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

        // size param
        public float LargeSize;
        public float Ray;

        // center of the box
        public Vector3 Center;

        public BoxParam()
        {

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
        }

        public bool HasContact(BoxParam other)
        {
            return x.HasContact(other.x) && y.HasContact(other.y) && z.HasContact(other.z);
        }
    }
}
