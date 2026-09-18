using Photon.Pun;
using SharpzReborn.Extensions;
using SharpzReborn.Menu;
using SharpzReborn.Notifications;
using UnityEngine;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Utilities.GameModeUtilities;
using static SharpzReborn.Classes.RigManager;
using GorillaGameModes;

namespace SharpzReborn.Mods
{
    public class Advantages
    {
        static void TurnOff()
        {
            Buttons.GetIndex("Tag Self").enabled = false;
            RecreateMenu();
        }
        public static void TagSelf()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                AddInfected(PhotonNetwork.LocalPlayer);
                NotifiLib.SendNotification($"{success} You have been tagged.");
                TurnOff();
            }
            else
            {
                if (InfectedList().Contains(PhotonNetwork.LocalPlayer))
                {
                    NotifiLib.SendNotification($"{success} You have been tagged.");
                    VRRig.LocalRig.enabled = true;
                    TurnOff();
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

            if (!rightTriggerPressed)
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (!lockTarget.IsTagged())
                    {
                        VRRig.LocalRig.enabled = false;

                        if (!Buttons.GetIndex("Obnoxious Tag").enabled)
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
                if (rightTriggerPressed)
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
    }
}
