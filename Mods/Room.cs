using GorillaNetworking;
using Photon.Pun;
using SharpzReborn.Managers;
using SharpzReborn.Notifications;
using System.Collections;
using UnityEngine;
using static SharpzReborn.Menu.Buttons;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Mods
{
    public class Room : MonoBehaviourPunCallbacks
    {
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Sharpz Reborn // Join failed: {returnCode} | {message}");

            if (returnCode == 32765)
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> Room is full.");
        }

        public static string lastRoom = "";

        public override void OnJoinedRoom()
        {
            lastRoom = PhotonNetwork.CurrentRoom.Name;
        }

        public override void OnLeftRoom()
        {
            //obsolete
            Debug.Log("pun succeeded");
            Visuals.ClearLinePool(true);
        }

        public static void JoinLastRoom()
        {
            if (PhotonNetwork.InRoom || string.IsNullOrEmpty(lastRoom))
                return;

            QueueRoom(lastRoom);
        }

        public static void JoinRandomRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();
                CoroutineManager.Instance.StartCoroutine(JoinRandomDelay());
                return;
            }

            GorillaNetworkJoinTrigger trigger = PhotonNetworkController.Instance.currentJoinTrigger ?? GorillaComputer.instance.GetJoinTriggerForZone("forest");
            PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger);
        }

        public static IEnumerator JoinRandomDelay()
        {
            yield return new WaitForSeconds(1.5f);
            JoinRandomRoom();
        }

        public static void ReconnectAndRejoin()
        {
            if (PhotonNetwork.InRoom)
                return;

            PhotonNetwork.ReconnectAndRejoin();
        }
        
        public static void AmIMaster()
        {
            string masterName = PhotonNetwork.MasterClient.NickName;
            if (!PhotonNetwork.IsMasterClient)
            {
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=red>FAIL</color><color=grey>] </color><color=white>You are not the master client, " + masterName + " is the current master client.</color>");
                return;
            }
            Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>] </color><color=white>You are the master client.</color>");
        }
        public static Coroutine queueCoroutine;
        public static int reconnectDelay = 1;
        public static bool roomFull;

        public static IEnumerator QueueRoomCoroutine(string roomName)
        {
            NetworkSystemPUN instance = NetworkSystem.Instance as NetworkSystemPUN;

            if (instance == null)
            {
                Debug.LogError("Sharpz Reborn // NetworkSystemPUN is null");
                yield break;
            }

            roomFull = false;

            if (instance.InRoom)
                instance.ReturnToSinglePlayer();

            yield return new WaitUntil(() => instance.netState == NetSystemState.Idle);
            yield return new WaitForSeconds(0.5f);

            while (!instance.InRoom && !roomFull)
            {
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomName, JoinType.Solo);
                yield return new WaitForSeconds(reconnectDelay);
            }

            queueCoroutine = null;
        }
        public static void QueueRoom(string roomName)
        {
            Debug.Log("Sharpz Reborn // QueueRoom started");

            if (CoroutineManager.Instance == null)
            {
                Debug.LogError("Sharpz Reborn // CoroutineManager.instance is null");
                return;
            }

            if (queueCoroutine != null)
                CoroutineManager.Instance.StopCoroutine(queueCoroutine);

            queueCoroutine = CoroutineManager.Instance.StartCoroutine(QueueRoomCoroutine(roomName));

            Debug.Log("Sharpz Reborn // Queue coroutine started");
        }

        public static void Reconnect()
        {
            string roomName = NetworkSystem.Instance.RoomName;

            NetworkSystem.Instance.ReturnToSinglePlayer();
            QueueRoom(roomName);
        }

        public static void JoinMenuRoom()
        {
            PhotonNetworkController.Instance.AttemptToAutoJoinSpecificRoom("sharpzReborn$", JoinType.Solo);
        }

        public static void QueueJoinRoom(string roomName)
        {
            if (NetworkSystem.Instance == null)
                return;

            QueueRoom(roomName.ToUpper());
        }

        public static string PrivateRoomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static void JoinRandomPriv()
        {

        }
    }
}