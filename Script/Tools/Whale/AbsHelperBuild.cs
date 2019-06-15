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
        public abstract void Build(int x, int y, int z);
        public abstract void ExportToPrefab();

        public bool IsValidCoord(int x, int y, int z)
        {
            Vector2Int Width = new Vector2Int(0, ParamWhale.SizeChunck * ParamWhale.NbChunck);
            Vector2Int Height = new Vector2Int(0, ParamWhale.SizeChunck * ParamWhale.NbChunck);
            Vector2Int Depth = new Vector2Int(0, ParamWhale.SizeChunck * ParamWhale.NbChunck);

            return !(x < Width.x || x > Width.y
                || y < Height.x || y > Height.y
                || z < Depth.x || z > Depth.y);
        }
    }
}
