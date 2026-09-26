using GorillaGameModes;
using GorillaLocomotion;
using Photon.Pun;
using SharpzReborn.Extensions;
using SharpzReborn.Menu;
using SharpzReborn.Notifications;
using System.Linq;
using UnityEngine;
using static SharpzReborn.Classes.RigManager;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Utilities.GameModeUtilities;

namespace SharpzReborn.Mods
{
    public class Advantages
    {
        public static void TagSelf()
        {
            static void TurnOff()
            {
                Buttons.GetIndex("Tag Self").SetEnabled(false);
                RecreateMenu();
            }
            if (PhotonNetwork.IsMasterClient)
            {
                AddInfected(PhotonNetwork.LocalPlayer);
                NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> You have been tagged.");
                TurnOff();
            }
            else
            {
                if (InfectedList().Contains(PhotonNetwork.LocalPlayer))
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> You have been tagged.");
                    VRRig.LocalRig.enabled = true;
                    TurnOff();
                }
                else
                {
                    VRRig rig = VRRigExtensions.ActiveRigs
                        .Where(r => !r.IsLocal() && r.IsTagged())
                        .OrderBy(r => r.Distance(VRRig.LocalRig)
                                    + r.LatestVelocity().magnitude)
                        .FirstOrDefault();

                        if (!rig.IsTagged()) return;
                        VRRig.LocalRig.enabled = false;
                        if (rig != null) VRRig.LocalRig.transform.position = rig.rightHandTransform.position;

                }
            }
        }

        public static void UntagAll()
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                NotifiLib.SendNotification($"{fail} You are not master client.");
            else
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    RemoveInfected(v);
            }
        }
        public static void DisableTags()
        {
            Notifications.NotifiLib.SendNotification($"{warning} You should Flush RPCs after this.");
            if (!NetworkSystem.Instance.IsMasterClient)
                NotifiLib.SendNotification($"{fail} You are not master client.");
            else
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    RemoveInfected(v);
            }
        }

        public static bool ValidateTag(VRRig Rig) =>
            Vector3.Distance(ServerSyncPos, Rig.transform.position) < 6f;

        private static float reportTagDelay;
        public static void ReportTag(VRRig rig)
        {
            if (Time.time > reportTagDelay)
            {
                reportTagDelay = Time.time + 0.1f;
                GameMode.ReportTag(rig.GetPlayer());
            }
        }

        public static void TagGun()
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (!lockTarget.IsTagged())
                    {
                        VRRig.LocalRig.enabled = false;
                        VRRig.LocalRig.transform.position = lockTarget.transform.position - new Vector3(0f, 3f, 0f);

                        if (ValidateTag(lockTarget))
                            ReportTag(lockTarget);
                    }
                    else
                    {
                        gunLocked = false;
                        VRRig.LocalRig.enabled = true;
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        if (PhotonNetwork.IsMasterClient)
                            AddInfected(GetPlayerFromVRRig(gunTarget));
                        else
                        {
                            if (!VRRig.LocalRig.IsTagged()) return;
                            gunLocked = true;
                            lockTarget = gunTarget;
                        }
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }
        public static void TagAll()
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (NetworkSystem.Instance.IsMasterClient)
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    AddInfected(v);

                NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> Everyone is tagged!");
            }
            else
            {

                if (!VRRig.LocalRig.IsTagged())
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must be tagged.");
                }
                else
                {
                    bool isInfectedPlayers = VRRigExtensions.ActiveRigs.Any(vrrig => !vrrig.IsTagged());
                    if (isInfectedPlayers)
                    {
                        foreach (var vrrig in VRRigExtensions.ActiveRigs.Where(vrrig => !vrrig.IsTagged()))
                        {
                            VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = lockTarget.transform.position - new Vector3(0f, 3f, 0f);
                            if (ValidateTag(vrrig))
                                ReportTag(vrrig);
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification($"{success} Everyone is tagged!");
                        VRRig.LocalRig.enabled = true;
                    }
                }
            }
        }

        public static void UntagGun()
        {
            if (!GetGunInput(false))
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                    VRRig.LocalRig.enabled = true;
                }

                return;
            }

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;

            if (gunLocked && lockTarget != null)
            {
                if (lockTarget.IsTagged())
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        NotifiLib.SendNotification($"{fail} You are not the master client.");
                        gunLocked = false;
                        lockTarget = null;
                        return;
                    }

                    RemoveInfected(GetPlayerFromVRRig(lockTarget));

                    gunLocked = false;
                    lockTarget = null;
                }
                else
                {
                    gunLocked = false;
                    lockTarget = null;
                    VRRig.LocalRig.enabled = true;
                }

                return;
            }

            if (GetGunInput(true) && Ray.collider != null)
            {
                VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

                if (gunTarget && !gunTarget.IsLocal() && gunTarget.IsTagged())
                {
                    gunLocked = true;
                    lockTarget = gunTarget;
                }
            }
        }

        public static void FlickTagGun()
        {
            if (!GetGunInput(false))
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                }

                return;
            }

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;
            GameObject NewPointer = GunData.NewPointer;

            if (gunLocked && lockTarget != null)
            {
                Transform controller = GTPlayer.Instance.GetControllerTransform(false);

                Vector3 bodyPosition = GorillaTagger.Instance.bodyCollider.transform.position;
                Vector3 targetPosition = lockTarget.transform.position;

                Vector3 offset = targetPosition - bodyPosition;

                if (offset.sqrMagnitude > 16f)
                    targetPosition = bodyPosition + offset.normalized * 4f;

                controller.position = targetPosition;

                return;
            }

            if (GetGunInput(true) && Ray.collider != null)
            {
                VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

                if (gunTarget && !gunTarget.IsLocal())
                {
                    gunLocked = true;
                    lockTarget = gunTarget;
                }
            }
        }
    }
}
