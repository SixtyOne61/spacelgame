using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class WhaleBuild
    {
        [Tooltip("Param for build map")]
        public SCRWhale ParamWhale;
        [Tooltip("Noise rock param")]
        public SCRNoise ParamRock;

        // rock helper
        public HelperBuildRock HelperBuildRock = new HelperBuildRock();

        // contains all helper
        private List<AbsHelperBuild> _helpers = new List<AbsHelperBuild>();

        public void Init()
        {
            _helpers.Clear();
            HelperBuildRock.ParamWhale = ParamWhale;
            HelperBuildRock.ParamRock = ParamRock;
            _helpers.Add(HelperBuildRock);
        }

        public void Generate()
        {
            // TO DO : change this, make escargot
            // generate all object for world, create prefab
            for (int x = ParamWhale.Width.x; x <= ParamWhale.Width.y; ++x)
            {
                for (int y = ParamWhale.Height.x; y <= ParamWhale.Height.y; ++y)
                {
                    for (int z = ParamWhale.Depth.x; z <= ParamWhale.Depth.y; ++z)
                    {
                        // ... call on all helper
                        foreach (AbsHelperBuild helper in _helpers)
                        {
                            helper.Build(x, y, z);
                        }
                    }
                }
            }

            // export each object to prefab
            foreach (AbsHelperBuild helper in _helpers)
            {
                helper.ExportToPrefab();
            }
        }

    }
}
