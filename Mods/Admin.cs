using Photon.Realtime;
using SharpzReborn.Extensions;
using UnityEngine;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Utilities.RigUtilities;
using exec = SharpzReborn.Classes.Network.SharpzNetwork;

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
                        exec.ExecuteCommand("kick", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }
        public static void AdminHideGun()
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
                        exec.ExecuteCommand("hide", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void AdminHideAll() =>
            exec.ExecuteCommand("hide", ReceiverGroup.Others);

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
                    if (Time.time > adminEventDelay)
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("lag", lockTarget.GetPlayer().ActorNumber, 50);
                        RPCProtection();
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                gunLocked = false;
            }
        }

        public static void AdminLagSpikeAll() =>
            exec.ExecuteCommand("lag", ReceiverGroup.Others, 1000);
        public static void AdminLagAll() =>
            exec.ExecuteCommand("lag", ReceiverGroup.Others, 50);

        public static void AdminLagSpikeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (Time.time > adminEventDelay)
                    {
                        adminEventDelay = Time.time + 0.1f;
                        exec.ExecuteCommand("lag", lockTarget.GetPlayer().ActorNumber, 1000);
                        RPCProtection();
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                gunLocked = false;
            }
        }

        public static void AdminAnnoyLobby()
        {
            exec.ExecuteCommand("amnoy", ReceiverGroup.Others);
        }

        public static void BringAll()
        {
            if (Time.time > adminEventDelay)
            {
                adminEventDelay = Time.time + 0.05f;
                exec.ExecuteCommand("tp", ReceiverGroup.Others, GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward);
            }
        }

        public static void BringGun()
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
                        adminEventDelay = Time.time + 0.05f;
                        exec.ExecuteCommand("tp", GetPlayerFromVRRig(gunTarget).ActorNumber, GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward);
                    }
                }
            }
        }

        public static void AdminAnnoyGun()
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
                        exec.ExecuteCommand("amnoy", GetPlayerFromVRRig(gunTarget).ActorNumber);
                    }
                }
            }
        }

        public static void HideAll() =>
            exec.ExecuteCommand("hide", ReceiverGroup.Others);

        public static void UnhideAll() =>
            exec.ExecuteCommand("unhide", ReceiverGroup.Others);
    }
}
