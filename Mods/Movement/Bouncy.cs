using Locomotion;
using Banapuchin.Classes;

namespace Banapuchin.Mods.Movement
{
    public class Bouncy : ModBase
    {
        public override string Text => "Bouncy";

        public static float normal;

        public override void OnEnable()
        {
            Player.Instance.climbDrag = 1f;
        }

        public override void OnDisable()
        {
            Player.Instance.climbDrag = normal;
        }
    }
}
