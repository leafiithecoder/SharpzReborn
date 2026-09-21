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
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                    VRRig.LocalRig.transform.rotation = rotation;

                    VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + RandomVector3();
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + RandomVector3();

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();
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

            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (!lockTarget.IsTagged())
                    {
                        VRRig.LocalRig.enabled = false;
                            Vector3 position = lockTarget.transform.position + RandomVector3();

                            VRRig.LocalRig.transform.position = position;

                            VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                            VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();
                            VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();

                            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                            VRRig.LocalRig.leftIndex.calcT = 0f;
                            VRRig.LocalRig.leftMiddle.calcT = 0f;
                            VRRig.LocalRig.leftThumb.calcT = 0f;

                            VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                            VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                            VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                            VRRig.LocalRig.rightIndex.calcT = 0f;
                            VRRig.LocalRig.rightMiddle.calcT = 0f;
                            VRRig.LocalRig.rightThumb.calcT = 0f;

                            VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                            VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                            VRRig.LocalRig.rightThumb.LerpFinger(1f, false);

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

            static void TurnOff()
            {
                Buttons.GetIndex("Tag All").enabled = false;
                RecreateMenu();
            }

            if (NetworkSystem.Instance.IsMasterClient)
            {
                foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                    AddInfected(v);

                TurnOff();
                NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> Everyone is tagged!");
            }
            else
            {

                if (!VRRig.LocalRig.IsTagged())
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must be tagged.");
                    TurnOff();
                }
                else
                {
                    bool isInfectedPlayers = VRRigExtensions.ActiveRigs.Any(vrrig => !vrrig.IsTagged());
                    if (isInfectedPlayers)
                    {
                        foreach (var vrrig in VRRigExtensions.ActiveRigs.Where(vrrig => !vrrig.IsTagged()))
                        {
                            VRRig.LocalRig.enabled = false;
                                Vector3 position = vrrig.transform.position + RandomVector3();

                                VRRig.LocalRig.transform.position = position;
                                VRRig.LocalRig.transform.rotation = RandomQuaternion();

                                VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                                VRRig.LocalRig.leftHand.rigTarget.transform.position = vrrig.transform.position + RandomVector3();
                                VRRig.LocalRig.rightHand.rigTarget.transform.position = vrrig.transform.position + RandomVector3();

                                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                                VRRig.LocalRig.leftIndex.calcT = 0f;
                                VRRig.LocalRig.leftMiddle.calcT = 0f;
                                VRRig.LocalRig.leftThumb.calcT = 0f;

                                VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                                VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                                VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                                VRRig.LocalRig.rightIndex.calcT = 0f;
                                VRRig.LocalRig.rightMiddle.calcT = 0f;
                                VRRig.LocalRig.rightThumb.calcT = 0f;

                                VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                                VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                                VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
                            if (ValidateTag(vrrig))
                                ReportTag(vrrig);
                        }
                    }
                    else
                    {
                        NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> Everyone is tagged!");
                        VRRig.LocalRig.enabled = true;
                        TurnOff();
                    }
                }
            }
        }

        public static void UntagGun()
        {
            if (!rightGrab) return;

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;

            if (Ray.collider == null)
            {
                Debug.Log("[GunDebug] Raycast hit nothing.");
            }
            else
            {
                Debug.Log($"[GunDebug] Hit: {Ray.collider.name}");
                Debug.Log($"[GunDebug] Layer: {Ray.collider.gameObject.layer}");
                Debug.Log($"[GunDebug] Parent VRRig: " +
                    (Ray.collider.GetComponentInParent<VRRig>() != null));
            }

            if (!rightTriggerPressed) return;

            VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

            if (gunTarget && !gunTarget.IsLocal() && gunTarget.IsTagged())
            {
                if (PhotonNetwork.IsMasterClient)
                    RemoveInfected(GetPlayerFromVRRig(gunTarget));
                else
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
            }
        }

        public static void FlickTagGun()
        {
            if (!rightGrab || !rightTriggerPressed) return;

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;

            if (Ray.collider == null)
            {
                Debug.Log("[GunDebug] Raycast hit nothing.");
            }
            else
            {
                Debug.Log($"[GunDebug] Hit: {Ray.collider.name}");
                Debug.Log($"[GunDebug] Layer: {Ray.collider.gameObject.layer}");
                Debug.Log($"[GunDebug] Parent VRRig: " +
                    (Ray.collider.GetComponentInParent<VRRig>() != null));
            }
            GameObject NewPointer = GunData.NewPointer;

            GTPlayer.Instance.GetControllerTransform(false).position = NewPointer.transform.position;

            if (Vector3.Distance(
                GTPlayer.Instance.GetControllerTransform(false).position,
                GorillaTagger.Instance.bodyCollider.transform.position) > 4f)
            {
                GTPlayer.Instance.GetControllerTransform(false).position =
                    GorillaTagger.Instance.bodyCollider.transform.position +
                    (GTPlayer.Instance.GetControllerTransform(false).position -
                     GorillaTagger.Instance.bodyCollider.transform.position) * 4f;
            }
        }
    }
}
