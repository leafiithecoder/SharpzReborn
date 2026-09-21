using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using SharpzReborn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SharpzReborn.Utilities
{
    public class RigUtilities
    {
        public static VRRig GetVRRigFromPlayer(NetPlayer p) =>
            GorillaGameManager.StaticFindRigForPlayer(p);

        public static NetPlayer GetPlayerFromVRRig(VRRig p) =>
            p.Creator ?? NetworkSystem.Instance.GetPlayer(NetworkSystem.Instance.GetOwningPlayerID(p.rigSerializer.gameObject));

        public static NetPlayer GetPlayerFromID(string id) =>
            PhotonNetwork.PlayerList.FirstOrDefault(player => player.UserId == id);

        public static Player NetPlayerToPlayer(NetPlayer p) =>
            p.GetPlayerRef();

        public static Player GetRandomPlayer(bool includeSelf = false) =>
            includeSelf ?
            PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)] :
            PhotonNetwork.PlayerListOthers[Random.Range(0, PhotonNetwork.PlayerListOthers.Length)];

        private static VRRig rigTarget;
        private static float rigTargetChange;
        public static VRRig GetTargetPlayer(float targetChangeDelay = 1f)
        {
            if (!(Time.time > rigTargetChange) && rigTarget.Active()) return rigTarget;
            rigTargetChange = Time.time + targetChangeDelay;
            rigTarget = GetRandomVRRig();

            return rigTarget;
        }

        public static VRRig GetRandomVRRig(bool includeSelf = false) =>
            GetVRRigFromPlayer(GetRandomPlayer(includeSelf));

        public static NetworkView GetNetworkViewFromVRRig(VRRig p) =>
            p.netView;

        public static PhotonView GetPhotonViewFromVRRig(VRRig p) =>
            GetNetworkViewFromVRRig(p).GetView;

        public static VRRig GetClosestVRRig() =>
            VRRig.LocalRig.GetClosest();

        public static readonly Dictionary<string, float> waitingForCreationDate = new Dictionary<string, float>();
        public static readonly Dictionary<string, string> creationDateCache = new Dictionary<string, string>();
        public static string GetCreationDate(string input, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(input, out string date))
                return date;
            if (!waitingForCreationDate.ContainsKey(input))
            {
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }
            else
            {
                if (!(Time.time > waitingForCreationDate[input])) return "Loading...";
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }

            return "Loading...";
        }

        public static void GetCreationCoroutine(string userId, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(userId, out string date))
            {
                onTranslated?.Invoke(date);
                return;
            }

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = userId }, delegate (GetAccountInfoResult result) // Who designed this
            {
                string creationDate = result.AccountInfo.Created.ToString(format);
                creationDateCache[userId] = creationDate;

                onTranslated?.Invoke(creationDate);
            }, delegate { creationDateCache[userId] = "Error"; onTranslated?.Invoke("Error"); });
        }
    }
}