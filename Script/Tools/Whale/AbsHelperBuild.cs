using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public abstract class AbsHelperBuild
    {
        [Tooltip("Param for build map")]
        public SCRWhale ParamWhale;
        [Tooltip("born x")]
        public Vector2 Bornx;
        [Tooltip("born y")]
        public Vector2 Borny;
        [Tooltip("born z")]
        public Vector2 Bornz;

        publoc abstract void Generate();
        public abstract void Build(int x, int y, int z);
        public abstract void ExportToPrefab();

        public bool IsValidCoord(int x, int y, int z)
        {
            return !(x < ParamWhale.Width.x || x > ParamWhale.Width.y
                || y < ParamWhale.Height.x || y > ParamWhale.Height.y
                || z < ParamWhale.Depth.x || z > ParamWhale.Depth.y);
        }
    }
}
