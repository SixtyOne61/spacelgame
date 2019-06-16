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

        public abstract void Generate();
        public abstract void ExportToPrefab();

        public bool IsValidCoord(int x, int y, int z)
        {
            return !(x < Bornx.x || x > Bornx.y
                || y < Borny.x || y > Borny.y
                || z < Bornz.x || z > Bornz.y);
        }
    }
}
