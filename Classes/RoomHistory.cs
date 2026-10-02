using SharpzReborn.Menu;
using System.Collections.Generic;
using System.Linq;
using static SharpzReborn.Menu.Boards;
using static SharpzReborn.Menu.BoardType;

namespace SharpzReborn.Classes;

public class RoomHistory
{
    private static readonly List<string> History = new();

    public static bool Enabled;

    public static void Add(string roomCode)
    {
        if (string.IsNullOrWhiteSpace(roomCode))
            return;

        roomCode = roomCode.Trim();

        History.Remove(roomCode);
        History.Insert(0, roomCode);

        if (History.Count > 10)
            History.RemoveAt(10);

        if (Enabled)
            Update();
    }

    public static void Update()
    {
        if (!Enabled)
            return;

        SetBoard(
            COCDesc,
            $"<color=purple>Room History</color>\n\n{GetText()}"
        );

        UpdateCOC();
    }

    public static void Enable()
    {
        Enabled = true;
        Update();
    }

    public static void Disable()
    {
        Enabled = false;

        SetBoard(
            COCDesc,
            COCDefault
        );

        UpdateCOC();
    }

    public static void Clear()
    {
        History.Clear();

        if (Enabled)
            Update();
    }

    public static string GetText()
    {
        if (History.Count == 0)
            return "No rooms recorded.";

        return string.Join(
            "\n",
            History.Select((room, index) => $"{index + 1}. {room}")
        );
    }

    public static void SetCOCHistoryBoard()
    {
        Update();
    }
}