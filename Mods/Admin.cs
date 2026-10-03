using Photon.Realtime;
using SharpzReborn.Extensions;
using UnityEngine;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Utilities.RigUtilities;
using exec = SharpzReborn.Classes.SharpzNetwork;

namespace SharpzReborn.Mods
{
    public class Admin // only for admins
    {
        private static float adminEventDelay;
        public static void AdminKickGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("kick", ReceiverGroup.All, GetPlayerFromVRRig(gunTarget).UserId);
                    }
                }
            }
        }

        public static void AdminKickAll() =>
            exec.ExecuteCommand("kick", ReceiverGroup.Others);

        public static void AdminCrashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > adminEventDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("crash", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void AdminCrashAll() =>
            exec.ExecuteCommand("crash", ReceiverGroup.Others);

        public static void AdminLagGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (!lockTarget.IsLocal())
                    {
                        gunLocked = false;
                        lockTarget = null;
                        exec.ExecuteCommand("unlag", ReceiverGroup.All, lockTarget);
                    }
                }

                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

                    if (gunTarget && gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                        exec.ExecuteCommand("lag", ReceiverGroup.All, lockTarget);
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                    exec.ExecuteCommand("unlag", ReceiverGroup.All, lockTarget);
                }
            }
        }

        public static void AdminLagAll() =>
            exec.ExecuteCommand("lag", ReceiverGroup.Others);

        public static void AdminUnlagAll() =>
            exec.ExecuteCommand("unlag", ReceiverGroup.All);

        public static void AdminAnnoyLobby()
        {
            exec.ExecuteCommand("amnoy", ReceiverGroup.Others);
        }
    }
}
