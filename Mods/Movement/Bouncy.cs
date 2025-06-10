using Locomotion;
using Banapuchin.Classes;
using System;
using System.Collections.Generic;

namespace Banapuchin.Mods.Movement
{
    public class Bouncy : ModBase
    {
        public override string Text => "Bouncy";
        public override List<Type> Incompatibilities => new List<Type> { typeof(UltraBouncy) };

        public static float normal;

        public override void OnEnable()
        {
            base.OnEnable();
            Player.Instance.climbDrag = 1f;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Player.Instance.climbDrag = normal;
        }
    }
}
