using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Managers;
using SharpzReborn.Notifications;
using UnityEngine;

namespace SharpzReborn.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    public class LeavePatch : MonoBehaviour
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                if (AdminManager.IsAdmin(otherPlayer.UserId))
                {
                    NotificationManager.SendNotification("<color=grey>[</color><color=red>ADMIN LEAVE</color><color=grey>] </color><color=white>Name: " + otherPlayer.NickName + "</color>");
                }
                else
                {
                    NotificationManager.SendNotification("<color=grey>[</color><color=red>LEAVE</color><color=grey>]</color> <color=white>Name: " + otherPlayer.NickName + "</color>");
                    a = otherPlayer;
                }
            }
        }

        private static Player a;
    }
}