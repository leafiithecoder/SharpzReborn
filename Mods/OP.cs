using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Extensions;
using SharpzReborn.Managers;
using SharpzReborn.Menu;
using SharpzReborn.Notifications;
using SharpzReborn.Patches.Internal;
using SharpzReborn.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static SharpzReborn.Menu.Main;
using Random = UnityEngine.Random;
using GorillaTag;
using System;

namespace SharpzReborn.Mods
{
    public class OP
    {// alot of credits to some other menu most of these methods aren't mine
        #region Misc

        private static float reportDelay;
        public static void DelayBanGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;
                

                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal() && !gunLocked)
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;

                        if (VRRig.LocalRig.IsTagged())
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must not be tagged.");
                            return;
                        }

                        if (!lockTarget.IsTagged())
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> The target must be tagged.");
                            return;
                        }

                        if (PhotonNetwork.IsMasterClient)
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must not be master client.");
                            return;
                        }

                        if (Time.time > reportDelay)
                        {
                            reportDelay = Time.time + 0.5f;
                            GorillaPlayerScoreboardLine.ReportPlayer(lockTarget.GetPlayer().UserId, GorillaPlayerLineButton.ButtonType.Cheating, lockTarget.GetPlayer().NickName);
                        }

                        SerializePatch.OverrideSerialization = () =>
                        {
                            lockTarget.GetPlayer();
                            MassSerialize(true, new[] { VRRig.LocalRig.GetPhotonView() });

                            Vector3 positionArchive = VRRig.LocalRig.transform.position;
                            SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = PhotonNetwork.PlayerList.Where(plr => !(new[] { PhotonNetwork.MasterClient.ActorNumber, lockTarget.GetPlayer().ActorNumber }).Contains(plr.ActorNumber)).Select(plr => plr.ActorNumber).ToArray() });

                            VRRig.LocalRig.transform.position = new Vector3(99999f, 99999f, 99999f);
                            SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = new[] { PhotonNetwork.MasterClient.ActorNumber } });

                            VRRig.LocalRig.transform.position = lockTarget.rightHandTransform.position;
                            SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = new[] { lockTarget.GetPlayer().ActorNumber } });

                            RPCProtection();
                            VRRig.LocalRig.transform.position = positionArchive;

                            return false;
                        };
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    SerializePatch.OverrideSerialization = null;
                }
            }
        }
        public static void BetaSetStatus(RoomSystem.StatusEffects state, RaiseEventOptions reo)
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
            else
            {
                object[] statusSendData = new object[1];
                statusSendData[0] = (int)state;
                object[] sendEventData = new object[3];
                sendEventData[0] = NetworkSystem.Instance.ServerTimestamp;
                sendEventData[1] = (byte)2;
                sendEventData[2] = statusSendData;
                PhotonNetwork.RaiseEvent((byte)Constants.Network.ROOM_SYSTEM, sendEventData, reo, SendOptions.SendUnreliable);
            }
        }

        private static float vibrateDelay;

        public static void VibrateGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (GetGunInput(true) && Time.time > vibrateDelay)
                    {
                        NetPlayer owner = Classes.RigManager.GetPlayerFromVRRig(lockTarget);

                        BetaSetStatus(
                            RoomSystem.StatusEffects.JoinedTaggedTime,
                            new RaiseEventOptions
                            {
                                TargetActors = new[] { owner.ActorNumber }
                            });

                        RPCProtection();
                        vibrateDelay = Time.time + 0.5f;
                    }
                }

                if (GetGunInput(true) && !gunLocked)
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
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                }
            }
        }

        private static float slowDelay;

        public static void SlowGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (GetGunInput(true) && Time.time > slowDelay)
                    {
                        NetPlayer player = Classes.RigManager.GetPlayerFromVRRig(lockTarget);

                        BetaSetStatus(
                            RoomSystem.StatusEffects.TaggedTime,
                            new RaiseEventOptions
                            {
                                TargetActors = new[] { player.ActorNumber }
                            });

                        RPCProtection();
                        slowDelay = Time.time + 1f;
                    }
                }

                if (GetGunInput(true) && !gunLocked)
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
                if (gunLocked)
                {
                    gunLocked = false;
                    lockTarget = null;
                }
            }
        }

        public static float delay;

        public static void BetaNearbyFollowCommand(GorillaFriendCollider friendCollider, Photon.Realtime.Player player)
        {
            PhotonNetworkController.Instance.FriendIDList.Add(player.UserId);

            object[] groupJoinSendData = new object[2];
            groupJoinSendData[0] = PhotonNetworkController.Instance.shuffler;
            groupJoinSendData[1] = PhotonNetworkController.Instance.keyStr;
            NetEventOptions netEventOptions = new NetEventOptions { TargetActors = new[] { player.ActorNumber } };

            if (friendCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId) && friendCollider.playerIDsCurrentlyTouching.Contains(player.UserId) && player != PhotonNetwork.LocalPlayer)
                RoomSystem.SendEvent(4, groupJoinSendData, netEventOptions, false);
            else if (!friendCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId))
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not in stump.");
        }

        public static IEnumerator StumpKickDelay(Action action, Action action2, float extraDelay = 0f, bool changeQueue = false)
        {
            PhotonNetworkController.Instance.FriendIDList.Clear();
            yield return new WaitForSeconds(extraDelay);

            bool joinedRoomPatchEnabled = JoinedRoomPatch.enabled;

            string queueArchive = GorillaComputer.instance.currentQueue;
            if (changeQueue)
                GorillaComputer.instance.currentQueue = RandomString();

            action?.Invoke();
            yield return new WaitForSeconds(0.3f);
            action2?.Invoke();
            yield return new WaitForSeconds(1f);

            if (changeQueue)
                GorillaComputer.instance.currentQueue = queueArchive;

            yield return new WaitForSeconds(30f);

            JoinedRoomPatch.enabled = joinedRoomPatchEnabled;
        }

        public static void CreateKickRoom()
        {
             Tools.Utils.BroadcastRoom(RandomString(), true, PhotonNetworkController.Instance.keyToFollow, PhotonNetworkController.Instance.shuffler);
             Room.Reconnect();
        }

        private static float kickDelay;
        public static void StumpKickGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true) && Time.time > kickDelay)
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        NetPlayer player = RigUtilities.GetPlayerFromVRRig(gunTarget);
                        kickDelay = Time.time + 0.5f;

                        if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(player.UserId))
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> The player must be in stump.");
                            return;
                        }

                        if (!NetworkSystem.Instance.SessionIsPrivate)
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must be in a private room.");
                            return;
                        }

                        CoroutineManager.Instance.StartCoroutine(StumpKickDelay(() =>
                        {
                            PhotonNetworkController.Instance.shuffler = Random.Range(0, 99).ToString().PadLeft(2, '0') + Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                            PhotonNetworkController.Instance.keyStr = Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                            BetaNearbyFollowCommand(GorillaComputer.instance.friendJoinCollider, RigUtilities.NetPlayerToPlayer(player));
                            RPCProtection();
                        }, () =>
                        {
                            CreateKickRoom();
                        }));
                    }
                }
            }
        }

        public static void StumpKickAll()
        {
            if (NetworkSystem.Instance.InRoom)
            {
                if (!NetworkSystem.Instance.SessionIsPrivate)
                {
                    NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You must be in a private room.");
                    return;
                }

                CoroutineManager.Instance.StartCoroutine(StumpKickDelay(() =>
                {
                    PhotonNetworkController.Instance.shuffler = Random.Range(0, 99).ToString().PadLeft(2, '0') + Random.Range(0, 99999999).ToString().PadLeft(8, '0');
                    PhotonNetworkController.Instance.keyStr = Random.Range(0, 99999999).ToString().PadLeft(8, '0');

                    foreach (VRRig rig in VRRigExtensions.ActiveRigs.Where(rig => !rig.IsLocal() && GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(rig.GetPlayer().UserId)))
                        BetaNearbyFollowCommand(GorillaComputer.instance.friendJoinCollider, RigUtilities.NetPlayerToPlayer(rig.GetPlayer()));

                    RPCProtection();
                }, () =>
                {
                    CreateKickRoom();
                }));
            }
            else
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not in a room.");
        }

        #endregion
        #region Game Modes
        public static void InfectionToTag()
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
            else
            {
                GorillaTagManager gorillaTagManager = (GorillaTagManager)GorillaGameManager.instance;
                gorillaTagManager.infectedModeThreshold = PhotonNetwork.CurrentRoom.MaxPlayers + 1;
            }
        }

        public static void TagToInfection()
        {
            if (!NetworkSystem.Instance.IsMasterClient)
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
            else
            {
                GorillaTagManager gorillaTagManager = (GorillaTagManager)GorillaGameManager.instance;
                gorillaTagManager.infectedModeThreshold = 1;
            }
        }


        public static void FixThreshold()
        {
            GorillaTagManager gorillaTagManager = (GorillaTagManager)GorillaGameManager.instance;
            gorillaTagManager.infectedModeThreshold = 4;
        }
        #endregion
        #region VIM
        public static void VIMAction(int type, VRRig target)
        {
            if (target == null || target.IsLocal())
            {
                return;
            }

            if (admins.Contains(target.GetPlayer().UserId))
            {
                return;
            } 

            switch (type)
            {
                case 0:
                    RoomControls.KickPlayer(target.GetPlayer().ActorNumber);
                    break;
                case 1:
                    RoomControls.KickAndBlockPlayer(target.GetPlayer().ActorNumber);
                    break;
                case 2:
                    RoomControls.MutePlayer(target.GetPlayer().ActorNumber);
                    break;
            }
        }

        public static void VIMActionGun(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            if (!GetGunInput(false))
                return;

            var GunData = RenderGun();
            RaycastHit Ray = GunData.Ray;

            if (!GetGunInput(true))
                return;

            VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();

            if (gunTarget && !gunTarget.IsLocal())
                VIMAction(type, gunTarget);
        }

        public static void VIMActionAll(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            NetworkSystem.Instance.PlayerListOthers.ForEach(v =>
                VIMAction(type, RigUtilities.GetVRRigFromPlayer(v)));
        }

        public static void VIMActionOnTouch(int type, bool undo = false)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            foreach (VRRig rig in VRRigExtensions.ActiveRigs)
            {
                if (!rig.IsLocal() && rig.IsBeingTouched())
                    VIMAction(type, rig);
            }
        }

        public static void VIMActionRandom(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            List<VRRig> validRigs = VRRigExtensions.ActiveRigs
                .Where(rig => rig != null && !rig.IsLocal())
                .ToList();

            if (validRigs.Count == 0)
                return;

            VRRig target = validRigs[Random.Range(0, validRigs.Count)];

            VIMAction(type, target);
        }

        public static void VIMActionTagged(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            foreach (VRRig rig in VRRigExtensions.ActiveRigs)
            {
                if (rig != null && !rig.IsLocal() && rig.IsTagged())
                    VIMAction(type, rig);
            }
        }
        public static void VIMActionUntagged(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            foreach (VRRig rig in VRRigExtensions.ActiveRigs)
            {
                if (rig != null && !rig.IsLocal() && !rig.IsTagged())
                    VIMAction(type, rig);
               
            }
        }

        public static void VIMActionClosest(int type)
        {
            if (!VRRig.LocalRig.IsVIMSubscriber())
            {
                NotifiLib.SendNotification($"{warning} You are not a VIM subscriber, so this mod will not function.");
                return;
            }

            VIMAction(type, RigUtilities.GetClosestVRRig());
        }

        #endregion
        #region Rope
        public static Coroutine RopeCoroutine;
        public static IEnumerator RopeEnableRig()
        {
            yield return new WaitForSeconds(0.3f);
            VRRig.LocalRig.enabled = true;
        }
        public static void BetaSetRopeVelocity(int RopeId, Vector3 Velocity)
        {
            Velocity = Velocity.ClampMagnitudeSafe(15f);

            if (RopeSwingManager.instance.ropes.TryGetValue(RopeId, out GorillaRopeSwing Rope))
            {
                var ClosestNode = Rope.nodes
                    .Skip(1)
                    .Select((v, i) => new
                    {
                        index = i,
                        transform = v,
                        distance = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, v.transform.position)
                    })
                    .OrderBy(x => x.distance)
                    .First();

                if (ClosestNode.distance > 5f)
                {
                    if (RopeCoroutine != null)
                        CoroutineManager.Instance.StopCoroutine(RopeCoroutine);

                    RopeCoroutine = CoroutineManager.Instance.StartCoroutine(RopeEnableRig());

                    VRRig.LocalRig.enabled = false;
                    VRRig.LocalRig.transform.position = ClosestNode.transform.position;
                }

                if (Vector3.Distance(ServerPos, ClosestNode.transform.position) < 5f)
                    RopeSwingManager.instance.SendSetVelocity_RPC(RopeId, ClosestNode.index, Velocity, true);
                else
                    RopeDelay = 0f;

                RPCProtection();
            }
        }

        public static void BetaSetRopeVelocity(GorillaRopeSwing Rope, Vector3 Velocity)
        {
            BetaSetRopeVelocity(RopeSwingManager.instance.ropes.FirstOrDefault(x => x.Value == Rope).Key, Velocity);
        }

        private static float randomRopeDelay;
        private static GorillaRopeSwing randomRope;
        public static GorillaRopeSwing GetRandomRope()
        {
            if (Time.time > randomRopeDelay)
            {
                randomRopeDelay = Time.time + 0.5f;
                randomRope = RopeSwingManager.instance.ropes.Values.OrderBy(_ => Random.value).FirstOrDefault();
            }
            return randomRope;
        }

        private static float RopeDelay;

        public static void SpazRopeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true))
                {
                    GorillaRopeSwing gunTarget = Ray.collider.GetComponentInParent<GorillaRopeSwing>();
                    if (gunTarget && Time.time > RopeDelay)
                    {
                        RopeDelay = Time.time + 0.25f;
                        BetaSetRopeVelocity(gunTarget, RandomVector3(100f));
                    }
                }
            }
        }

        public static void SpazAllRopes()
        {
            if (rightTrigger > 0.5f && Time.time > RopeDelay)
            {
                RopeDelay = Time.time + 0.125f;

                GorillaRopeSwing rope = GetRandomRope();
                BetaSetRopeVelocity(rope, RandomVector3(100f));
            }
        }

        public static void SpazGrabbedRopes()
        {
            if (Time.time > RopeDelay)
            {
                RopeDelay = Time.time + 0.125f;
                VRRig randomRig = VRRigExtensions.ActiveRigs
                    .Where(rig => rig.currentRopeSwing != null)
                    .OrderBy(_ => Random.value)
                    .FirstOrDefault();

                if (randomRig != null)
                    BetaSetRopeVelocity(randomRig.currentRopeSwing, RandomVector3(100f));
            }
        }

        public static void FlingRopeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true))
                {
                    GorillaRopeSwing gunTarget = Ray.collider.GetComponentInParent<GorillaRopeSwing>();

                    if (gunTarget && Time.time > RopeDelay)
                    {
                        RopeDelay = Time.time + 0.125f;
                        BetaSetRopeVelocity(gunTarget, RandomVector3(100f));
                    }
                }
            }
        }

        public static void FlingAllRopesGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true) && Time.time > RopeDelay)
                {
                    RopeDelay = Time.time + 0.125f;

                    GorillaRopeSwing rope = GetRandomRope();
                    BetaSetRopeVelocity(rope, (NewPointer.transform.position - rope.transform.position).normalized * 100f);
                }
            }
        }

        #endregion

    }
}
