using SharpzReborn.Classes;
using SharpzReborn.Notifications;
using UnityEngine;
using static SharpzReborn.Menu.Buttons;

namespace SharpzReborn.Mods.Settings;

public class Movement
{
    public static readonly string[] flySpeedNames =
    [
        "Very Slow",
        "Slow",
        "Normal",
        "Fast",
        "Very Fast",
        "Extreme"
    ];

    public static readonly float[] flySpeedValues =
    [
        5f,
        10f,
        15f,
        20f,
        30f,
        50f
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedFlySpeed))]
    public static int flySpeedIndex = 2;

    public static float flySpeed = 15f;

    public static string FlySpeedName => flySpeedNames[flySpeedIndex];

    public static void ChangeFlySpeed(bool increment)
    {
        flySpeedIndex += increment ? 1 : -1;

        if (flySpeedIndex >= flySpeedNames.Length)
            flySpeedIndex = 0;
        else if (flySpeedIndex < 0)
            flySpeedIndex = flySpeedNames.Length - 1;

        ApplySavedFlySpeed();
    }

    public static void SetFlySpeedIndex(int index)
    {
        flySpeedIndex = Mathf.Clamp(index, 0, flySpeedValues.Length - 1);
        ApplySavedFlySpeed();
    }

    private static void ApplySavedFlySpeed()
    {
        flySpeedIndex = Mathf.Clamp(flySpeedIndex, 0, flySpeedValues.Length - 1);
        flySpeed = flySpeedValues[flySpeedIndex];
        GetIndex("Change Fly Speed").overlapText = $"Change Fly Speed [{flySpeedNames[flySpeedIndex]}]";
    }

    public static int speedAmountIndex = 1;
    public static float speedAmount = 7.5f;
    public static float speedMultiplier;

    public static readonly string[] speedNames =
    [
        "Unnoticeable",
        "Normal",
        "Medium",
        "Fast",
        "Ultra Fast"
    ];

    public static readonly float[] speedValues =
    [
        6.5f,
        7.5f,
        8.5f,
        9f,
        20f
    ];

    public static readonly float[] speedMultis =
    [
        1.6f,
        1.1f,
        1.6f,
        2f,
        1f
    ];

    public static string SpeedAmountName => speedNames[speedAmountIndex];

    public static void ChangeSpeedBoostAmount(bool increment)
    {
        speedAmountIndex += increment ? 1 : -1;

        if (speedAmountIndex >= speedNames.Length)
            speedAmountIndex = 0;
        else if (speedAmountIndex < 0)
            speedAmountIndex = speedNames.Length - 1;

        ApplySavedSpeedAmount();
    }

    public static void SetSpeedAmountIndex(int index)
    {
        speedAmountIndex = Mathf.Clamp(index, 0, speedValues.Length - 1);
        ApplySavedSpeedAmount();
    }

    private static void ApplySavedSpeedAmount()
    {
        speedAmountIndex = Mathf.Clamp(speedAmountIndex, 0, speedValues.Length - 1);
        speedAmount = speedValues[speedAmountIndex];
        speedMultiplier = speedMultis[speedAmountIndex];
        GetIndex("Change Speed Boost Amount").overlapText = $"Change Speed Boost Amount [{speedNames[speedAmountIndex]}]";
    }
    public static readonly float[] armLengthAmounts =
    [
    0.75f,
    1.1f,
    1.25f,
    1.5f,
    2f
    ];

    public static readonly string[] armLengthNames =
    [
    "Short",
    "Unnoticeable",
    "Normal",
    "Long",
    "Extreme"
    ];

    [SavedSetting(OnLoaded = nameof(ApplySavedArmLength))]
    public static int armLengthIndex = 2;

    public static float armLength = 1.25f;

    public static string ArmLengthName => armLengthNames[armLengthIndex];

    public static void ChangeArmLength(bool increment)
    {
        armLengthIndex += increment ? 1 : -1;

        if (armLengthIndex >= armLengthNames.Length)
            armLengthIndex = 0;
        else if (armLengthIndex < 0)
            armLengthIndex = armLengthNames.Length - 1;

        ApplySavedArmLength();
    }

    public static void SetArmLengthIndex(int index)
    {
        armLengthIndex = Mathf.Clamp(index, 0, armLengthAmounts.Length - 1);
        ApplySavedArmLength();
    }

    private static void ApplySavedArmLength()
    {
        armLengthIndex = Mathf.Clamp(armLengthIndex, 0, armLengthAmounts.Length - 1);
        armLength = armLengthAmounts[armLengthIndex];
        GetIndex("Change Arm Length").overlapText = $"Change Arm Length [{armLengthNames[armLengthIndex]}]";
    }
}
