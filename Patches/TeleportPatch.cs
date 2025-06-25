using Locomotion;
using HarmonyLib;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Banapuchin.Patches
{
    [HarmonyPatch(typeof(Player), "FixedUpdate")]
    internal class TeleportPatch
    {
        private static bool _isTeleporting;
        private static bool _killVelocity;
        private static Vector3 _teleportDestination;

        private static void Postfix(Player __instance)
        {
            try
            {
                if (_isTeleporting)
                {
                    __instance.disableMovement = true;
                    __instance.transform.position = _teleportDestination;
                    __instance.disableMovement = false;
                    if (_killVelocity)
                    {
                        __instance.playerRigidbody.velocity = Vector3.zero;
                    }
                    Task.Delay(250).ContinueWith(task =>
                    {
                        _isTeleporting = false;
                        _killVelocity = false;
                        Player.Instance.disableMovement = false;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"TeleportPatch Postfix Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void TeleportPlayer(Vector3 destination, bool killVelocity)
        {
            _teleportDestination = destination;
            _killVelocity = killVelocity;
            _isTeleporting = true;
        }
    }
}
