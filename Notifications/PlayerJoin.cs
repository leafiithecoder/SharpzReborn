using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Managers;
using SharpzReborn.Notifications;
using UnityEngine;

namespace SharpzReborn.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    public class JoinPatch : MonoBehaviour
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                if (AdminManager.IsAdmin(newPlayer.UserId))
                {NotificationManager.SendNotification("<color=grey>[</color><color=green>ADMIN JOIN</color><color=grey>] </color><color=white>Name: " + newPlayer.NickName + "</color>");}
                else { NotificationManager.SendNotification("<color=grey>[</color><color=green>JOIN</color><color=grey>] </color><color=white>Name: " + newPlayer.NickName + "</color>");oldnewplayer = newPlayer;}
            }
        }

        private static Player oldnewplayer;
    }
}