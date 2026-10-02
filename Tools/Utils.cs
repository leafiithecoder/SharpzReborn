using GorillaGameModes;
using GorillaNetworking;

namespace SharpzReborn.Tools
{
    public class Utils
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
    }
}