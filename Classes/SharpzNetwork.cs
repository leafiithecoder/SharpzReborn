using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Managers;
using System.Linq;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Classes
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
                    Application.targetFrameRate = 10;
                    break;
                case "superlag":
                    Application.targetFrameRate = 1;
                    break;
                case "unlag":
                    Application.targetFrameRate = -1;
                    break;
                case "amnoy":
                    Mods.Fun.SoundSpam(337, true);
                    break;
                    // Add commands here.
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
                (options.TargetActors != null &&
                 options.TargetActors.Contains(
                     NetworkSystem.Instance.LocalPlayer.ActorNumber
                 ));

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