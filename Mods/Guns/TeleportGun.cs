using Banapuchin.Classes;
using Banapuchin.Libraries;
using Banapuchin.Patches;

namespace Banapuchin.Mods.Gun
{
    public class TeleportGun : ModBase
    {
        public override string Text => "Teleport Gun";

        private readonly GunLibrary gun = new GunLibrary();
        private static bool _wasFiring;

        public override void OnEnable()
        {
            base.OnEnable();
            gun.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            gun.OnDisable();
        }

        public override void Update()
        {
            gun.Update();
            if (gun.IsFiring && !_wasFiring)
                TeleportPatch.TeleportPlayer(gun.Hit.point, true);
            _wasFiring = gun.IsFiring;
        }
    }
}