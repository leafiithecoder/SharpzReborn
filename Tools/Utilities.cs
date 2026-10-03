using GorillaGameModes;
using GorillaNetworking;
using SharpzReborn.classes;
using SharpzReborn.Classes;
using SharpzReborn.Managers;
using SharpzReborn.Menu;

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
        }

        public static void Shutdown()
        {
            SharpzNetwork.Shutdown();
            Preferences.Save();
        }
    }
}