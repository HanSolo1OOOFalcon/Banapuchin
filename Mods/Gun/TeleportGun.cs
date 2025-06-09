using Banapuchin.Classes;
using Banapuchin.Libraries;
using Banapuchin.Patches;

namespace Banapuchin.Mods.Gun
{
    public class TeleportGun : ModBase
    {
        public override string Text => "Teleport Gun";

        static GunLibrary gun = new GunLibrary();
        static bool wasFiring;

        public override void OnEnable()
        {
            gun.OnEnable();
        }

        public override void OnDisable()
        {
            gun.OnDisable();
        }

        public override void Update()
        {
            gun.Forever();
            if (gun.isFiring && !wasFiring)
            {
                TeleportPatch.TeleportPlayer(gun.hit.point, true);
            }
            wasFiring = gun.isFiring;
        }
    }
}
