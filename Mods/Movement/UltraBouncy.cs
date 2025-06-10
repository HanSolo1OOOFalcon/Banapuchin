using Banapuchin.Classes;
using Locomotion;
using System.Collections.Generic;
using System;

namespace Banapuchin.Mods.Movement
{
    public class UltraBouncy : ModBase
    {
        public override string Text => "Ultra Bouncy";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Bouncy) };

        public static float normal;

        public override void OnEnable()
        {
            Player.Instance.climbDrag = 0.1f;
        }

        public override void OnDisable()
        {
            Player.Instance.climbDrag = normal;
        }
    }
}
