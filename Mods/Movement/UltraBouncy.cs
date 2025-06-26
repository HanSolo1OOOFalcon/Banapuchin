using Banapuchin.Classes;
using Il2CppLocomotion;

namespace Banapuchin.Mods.Movement
{
    public class UltraBouncy : ModBase
    {
        public override string Text => "Ultra Bouncy";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Bouncy) };

        public static float Normal;

        public override void OnEnable()
        {
            base.OnEnable();
            Player.Instance.climbDrag = 0.1f;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Player.Instance.climbDrag = Normal;
        }
    }
}