using GorillaGameModes;
using GorillaNetworking;
using SharpzReborn.Classes;
using SharpzReborn.Classes.Network;
using SharpzReborn.Managers;
using SharpzReborn.Menu;
using UnityEngine;

namespace SharpzReborn.Tools
{
    public class Utilities
    {
        public static string GetCurrentGamemode()
        {
            switch (GorillaGameManager.instance.GameType())
            {
                case GorillaGameModes.GameModeType.HuntDown:
                    return "Hunt";

                case GorillaGameModes.GameModeType.FreezeTag:
                    return "Freeze Tag";

                case GorillaGameModes.GameModeType.Ghost:
                    return "Ghost Tag";

                case GorillaGameModes.GameModeType.PropHunt:
                    return "Prop Hunt";

                case GorillaGameModes.GameModeType.InfectionCompetitive:
                    return "Competitive";

                case GorillaGameModes.GameModeType.SuperInfect:
                    return "Super Infection";

                case GorillaGameModes.GameModeType.SuperCasual:
                    return "Super Casual";

                case GorillaGameModes.GameModeType.None:
                    return "ERROR";

                default:
                    return GorillaGameManager.instance.GameType().ToString();
            }
        }

        public static string GetCurrentGamemodeColor()
        {
            switch (GorillaGameManager.instance.GameType())
            {
                case GorillaGameModes.GameModeType.Guardian:
                    return "<color=yellow>Guardian</color>";

                case GorillaGameModes.GameModeType.Paintbrawl:
                    return "<color=orange>Paintbrawl</color>";

                case GorillaGameModes.GameModeType.Ambush:
                    return "<color=silver>Ambush</color>";

                case GorillaGameModes.GameModeType.Infection:
                    return "<color=red>Infection</color>";

                case GorillaGameModes.GameModeType.Casual:
                    return "<color=white>Casual</color>";

                case GorillaGameModes.GameModeType.HuntDown:
                    return "<color=cyan>Hunt</color>";

                case GorillaGameModes.GameModeType.FreezeTag:
                    return "<color=cyan>Freeze Tag</color>";

                case GorillaGameModes.GameModeType.Ghost:
                    return "<color=#add8e6ff>Ghost Tag</color>";

                case GorillaGameModes.GameModeType.PropHunt:
                    return "<color=green>Prop Hunt</color>";

                case GorillaGameModes.GameModeType.InfectionCompetitive:
                    return "<color=#800000ff>Competitive</color>";

                case GorillaGameModes.GameModeType.SuperInfect:
                    return "<color=magenta>Super Infection</color>";

                case GorillaGameModes.GameModeType.SuperCasual:
                    return "<color=teal>Super Casual</color>";

                case GorillaGameModes.GameModeType.None:
                    return "<color=red>ERROR</color>";

                default:
                    return GorillaGameManager.instance.GameType().ToString();
            }
        }

        public static void BroadcastRoom(string roomName, bool create, string key, string shuffler)
        {
            string text = NetworkSystem.ShuffleRoomName(roomName, shuffler.Substring(2, 8), true) + "|" + NetworkSystem.ShuffleRoomName("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".Substring(NetworkSystem.Instance.currentRegionIndex, 1), shuffler[..2], true);

            BroadcastMyRoomRequest broadcastMyRoomRequest = new BroadcastMyRoomRequest
            {
                KeyToFollow = key,
                RoomToJoin = text,
                Set = create
            };

            GorillaServer.Instance.BroadcastMyRoom(broadcastMyRoomRequest, delegate { }, delegate { });
        }

        public static string Map(VRRig r)
        {
            return EnumUtilExt.GetName<GTZone>(r.zoneEntity.currentZone);
        }

        public static void Init()
        {
            CoroutineManager.Instance.StartCoroutine(AdminManager.WaitForAdminPanel());
            SharpzServerData.Load();
            SharpzNetwork.Initialize();
            AdminManager.LoadWatermark();
            Boards.SetLaunchBoards();
            Debug.Log($"SharpzReborn // Initialized {Buttons.ButtonCount} buttons.");

        }

        public static void Shutdown()
        {
            SharpzNetwork.Shutdown();
            Preferences.Save();
        }
    }
}