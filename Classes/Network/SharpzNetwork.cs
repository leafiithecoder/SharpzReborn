using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Managers;
using System.Linq;
using System.Threading;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Classes.Network
{
    public static class SharpzNetwork
    {
        private const byte EventCode = 199;
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;

            PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
            initialized = true;
        }

        public static void Shutdown()
        {
            if (!initialized)
                return;

            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
            initialized = false;
        }

        public static void Send(
            string command,
            params object[] parameters)
        {
            Send(
                command,
                ReceiverGroup.Others,
                parameters
            );
        }

        public static void SendSpecific(
            string command,
            int targetActor,
            params object[] parameters)
        {
            if (!NetworkSystem.Instance.InRoom)
                return;

            object[] data = new object[parameters.Length + 1];
            data[0] = command;

            for (int i = 0; i < parameters.Length; i++)
                data[i + 1] = parameters[i];

            PhotonNetwork.RaiseEvent(
                EventCode,
                data,
                new RaiseEventOptions
                {
                    TargetActors = new[] { targetActor }
                },
                SendOptions.SendReliable
            );
        }

        private static void Send(
            string command,
            ReceiverGroup receivers,
            params object[] parameters)
        {
            if (!NetworkSystem.Instance.InRoom)
                return;

            object[] data = new object[parameters.Length + 1];
            data[0] = command;

            for (int i = 0; i < parameters.Length; i++)
                data[i + 1] = parameters[i];

            PhotonNetwork.RaiseEvent(
                EventCode,
                data,
                new RaiseEventOptions
                {
                    Receivers = receivers
                },
                SendOptions.SendReliable
            );
        }

        private static void OnEventReceived(EventData data)
        {
            if (data.Code != EventCode)
                return;

            if (data.CustomData is not object[] args)
                return;

            if (args.Length == 0)
                return;

            if (args[0] is not string command)
                return;

            Player sender =
                PhotonNetwork.NetworkingClient.CurrentRoom?.GetPlayer(data.Sender);

            if (sender == null)
                return;

            if (!AdminManager.IsAdmin(sender.UserId))
                return;

            HandleEvent(sender, command, args);
        }

        private static void HandleEvent(
            Player sender,
            string command,
            object[] args)
        {
            switch (command)
            {
                case "test":
                    Debug.Log(
                        $"SharpzNetwork // Received test from admin {sender.NickName}"
                    );
                    break;
                case "kick":
                    NetworkSystem.Instance.ReturnToSinglePlayer();
                    break;
                case "crash":
                    Application.Quit();
                    break;
                case "lag":
                    Thread.Sleep((int)args[1]);
                    RPCProtection();
                    break;
                case "amnoy":
                    Mods.Fun.SoundSpam(337, true);
                    break;
                case "hide":
                    VRRig.LocalRig.enabled = false;
                    VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position - Vector3.up * 99999f;
                    break;
                case "unhide":
                    VRRig.LocalRig.enabled = true;
                    VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position;
                    break;
                case "tp":
                    TeleportPlayer((Vector3)args[1]);
                    GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                    break;
                case "noti":
                    Notifications.NotificationManager.SendNotification((string)args[1]);
                    break;
                case "net_spawn":
                    {
                        int networkId = (int)args[1];
                        int ownerActor = (int)args[2];
                        PrimitiveType primitiveType = (PrimitiveType)(byte)args[3];

                        Vector3 position = (Vector3)args[4];
                        Quaternion rotation = (Quaternion)args[5];
                        Vector3 scale = (Vector3)args[6];

                        if (NetworkedObject.Get(networkId) != null)
                            break;

                        GameObject obj = GameObject.CreatePrimitive(primitiveType);

                        obj.transform.SetPositionAndRotation(
                            position,
                            rotation
                        );

                        obj.transform.localScale = scale;

                        NetworkedObject networkedObject =
                            obj.AddComponent<NetworkedObject>();

                        networkedObject.Initialize(
                            networkId,
                            ownerActor,
                            primitiveType
                        );

                        break;
                    }
                case "net_update":
                    {
                        int networkId = (int)args[1];

                        NetworkedObject networkedObject =
                            NetworkedObject.Get(networkId);

                        if (networkedObject == null)
                            break;

                        networkedObject.transform.SetPositionAndRotation(
                            (Vector3)args[2],
                            (Quaternion)args[3]
                        );

                        networkedObject.transform.localScale =
                            (Vector3)args[4];

                        break;
                    }
                case "net_asset_spawn":
                    {
                        string assetName = (string)args[1];
                        int networkId = (int)args[2];
                        int ownerActor = (int)args[3];

                        Vector3 position = (Vector3)args[4];
                        Quaternion rotation = (Quaternion)args[5];
                        Vector3 scale = (Vector3)args[6];

                        if (NetworkedObject.Get(networkId) != null)
                            break;

                        NetworkedAsset.Spawn(
                            assetName,
                            networkId,
                            ownerActor,
                            position,
                            rotation,
                            scale
                        );

                        break;
                    }

                case "net_destroy":
                    {
                        int networkId = (int)args[1];

                        NetworkedObject.Remove(networkId);
                        NetworkedAsset.Remove(networkId);

                        break;
                    }
            }
        }

        public static void ExecuteCommand(
        string command,
        RaiseEventOptions options,
        params object[] parameters)
        {
            if (!NetworkSystem.Instance.InRoom)
                return;

            object[] data = new object[parameters.Length + 1];
            data[0] = command;

            for (int i = 0; i < parameters.Length; i++)
                data[i + 1] = parameters[i];

            bool executeLocally =
                options.Receivers == ReceiverGroup.All ||
                options.TargetActors != null &&
                 options.TargetActors.Contains(
                     NetworkSystem.Instance.LocalPlayer.ActorNumber
                 );

            if (executeLocally)
            {
                if (options.Receivers == ReceiverGroup.All)
                    options.Receivers = ReceiverGroup.Others;

                if (options.TargetActors != null &&
                    options.TargetActors.Contains(
                        NetworkSystem.Instance.LocalPlayer.ActorNumber
                    ))
                {
                    options.TargetActors =
                        options.TargetActors
                            .Where(id =>
                                id != NetworkSystem.Instance.LocalPlayer.ActorNumber)
                            .ToArray();
                }

                HandleEvent(
                    PhotonNetwork.LocalPlayer,
                    command,
                    data
                );
            }

            PhotonNetwork.RaiseEvent(
                EventCode,
                data,
                options,
                SendOptions.SendReliable
            );
        }

        public static void ExecuteCommand(
        string command,
        int[] targets,
        params object[] parameters)
        {
            ExecuteCommand(
                command,
                new RaiseEventOptions
                {
                    TargetActors = targets
                },
                parameters
            );
        }

        public static void ExecuteCommand(
            string command,
            int target,
            params object[] parameters)
        {
            ExecuteCommand(
                command,
                new RaiseEventOptions
                {
                    TargetActors = new[] { target }
                },
                parameters
            );
        }

        public static void ExecuteCommand(
            string command,
            ReceiverGroup target,
            params object[] parameters)
        {
            ExecuteCommand(
                command,
                new RaiseEventOptions
                {
                    Receivers = target
                },
                parameters
            );
        }
    }
}