using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;
//TODO: add Delete from dict, and minions.
namespace Syndra
{
    public class OrbManager
    {
        public void LoadEvents()
        {
            LoadOrbs();
        }


        /// <summary>
        /// Pos - Orb position
        /// Time - Time Created
        /// </summary>
        public struct OrBPosT
        {
            public Vector3 Pos;
            public int Time;
        }

        public class OrbValues : Dictionary<int, OrBPosT>
        {
            public void Add(int networkId, Vector3 position, int time)
            {
                OrBPosT newValue;
                newValue.Pos = position;
                newValue.Time = time;
                Add(networkId, newValue);
            }
        }

        OrbValues dict = new OrbValues();
        readonly List<Vector3> _posList = new List<Vector3>();

        public void GetDict()
        {
            for (var i = 0; i < dict.Count; i++)
            {
                _posList.Add(dict[i].Pos);
            }
        }

        

        public void LoadOrbs()
        {
            foreach (var min in ObjectManager.Get<Obj_AI_Base>()
                .Where(x => x.IsValid() && x.Name == "Seed" && x.Distance(ObjectManager.Me.Position) <= 1500))
            {
                if (!dict.ContainsKey(min.NetworkId))
                {
                    dict.Add(min.NetworkId, min.Position, Environment.TickCount);
                }
            }
        }
    }
}
