using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class ShipPart
    {
        public ScriptableCube Param
        {
            get; private set;
        }
        public List<UnitPos> Cubes
        {
            get; private set;
        }

        public ShipPart(ScriptableCube param)
        {
            Cubes = new List<UnitPos>();
            Param = param;
        }

        public void Add(UnitPos pos)
        {
            Cubes.Add(pos);
        }
    }
}