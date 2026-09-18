using System;
using SharpzReborn.Classes;
using UnityEngine;

namespace SharpzReborn.Menu;

public enum GradientAnimationMode
{
    Scroll,
    Pulse,
}

public class Settings
{
    /*
     * These are the settings for the menu.
     *
     * To change the colors, you need to modify the ExtGradient variables.
     * Here are some examples on how to use ExtGradient:
     *
     * Solid Color:
     *  new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) }
     *
     * Simple Gradient:
     *  new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.white) }
     *
     * Rainbow Color:
     *   new ExtGradient { rainbow = true }
     *
     * Epileptic Color (random color every frame):
     *   new ExtGradient { epileptic = true }
     *
     * Self Color:
     *   new ExtGradient { copyRigColor = true }
     *
     * To change the font, you may use the following code:
     *   Font.CreateDynamicFontFromOSFont("Comic Sans MS", 24)
     */

    [SavedSetting] public static bool dropMenu    = true;
    [SavedSetting] public static bool animateMenu = true;

    [SavedSetting] public static float menuAnimationSpeed = 14f;

    [SavedSetting] public static GradientAnimationMode gradientAnimationMode =
            GradientAnimationMode.Scroll;

    public static ExtGradient backgroundColor;

    public static readonly ExtGradient[] buttonColors =
    {
            new(),
            new(),
    };

    public static ExtGradient outlineColor;

    public static readonly Color[] textColors =
    {
            Color.white,
            Color.white,
    };

    // Themes
    public static readonly MenuTheme[] themes =
    {
            new()
            {
                    name = "Purple",

                    backgroundPrimary   = new Color32(23, 16, 46, 255),
                    backgroundSecondary = new Color32(38, 25, 73, 255),

                    disabledButtonPrimary   = new Color32(17, 13, 32, 255),
                    disabledButtonSecondary = new Color32(31, 23, 54, 255),

                    enabledButtonPrimary   = new Color32(95,  75,  216, 255),
                    enabledButtonSecondary = new Color32(139, 109, 255, 255),

                    outline = new Color32(9, 7, 18, 255),
            },

            new()
            {
                    name = "Blue",

                    backgroundPrimary   = new Color32(10, 24, 44, 255),
                    backgroundSecondary = new Color32(17, 47, 78, 255),

                    disabledButtonPrimary   = new Color32(10, 19, 31, 255),
                    disabledButtonSecondary = new Color32(16, 34, 55, 255),

                    enabledButtonPrimary   = new Color32(32, 105, 196, 255),
                    enabledButtonSecondary = new Color32(74, 158, 255, 255),

                    outline = new Color32(5, 12, 22, 255),
            },

            new()
            {
                    name = "Red",

                    backgroundPrimary   = new Color32(42, 12, 17, 255),
                    backgroundSecondary = new Color32(73, 19, 28, 255),

                    disabledButtonPrimary   = new Color32(28, 12, 15, 255),
                    disabledButtonSecondary = new Color32(49, 19, 24, 255),

                    enabledButtonPrimary   = new Color32(184, 46, 65,  255),
                    enabledButtonSecondary = new Color32(245, 85, 105, 255),

                    outline = new Color32(20, 5, 8, 255),
            },

            new()
            {
                    name = "Mint",

                    backgroundPrimary   = new Color32(9,  36, 33, 255),
                    backgroundSecondary = new Color32(14, 61, 54, 255),

                    disabledButtonPrimary   = new Color32(8,  27, 25, 255),
                    disabledButtonSecondary = new Color32(13, 45, 41, 255),

                    enabledButtonPrimary   = new Color32(31, 171, 140, 255),
                    enabledButtonSecondary = new Color32(83, 226, 192, 255),

                    outline = new Color32(4, 18, 16, 255),
            },

            new()
            {
                    name = "Monochrome",

                    backgroundPrimary   = new Color32(18, 18, 18, 255),
                    backgroundSecondary = new Color32(35, 35, 35, 255),

                    disabledButtonPrimary   = new Color32(24, 24, 24, 255),
                    disabledButtonSecondary = new Color32(45, 45, 45, 255),

                    enabledButtonPrimary   = new Color32(100, 100, 100, 255),
                    enabledButtonSecondary = new Color32(160, 160, 160, 255),

                    outline = new Color32(5, 5, 5, 255),
            },

            new()
            {
                name = "Lavender",

                backgroundPrimary   = new Color32(31, 25, 47, 255),
                backgroundSecondary = new Color32(49, 39, 72, 255),

                disabledButtonPrimary   = new Color32(26, 21, 39, 255),
                disabledButtonSecondary = new Color32(42, 34, 59, 255),

                enabledButtonPrimary   = new Color32(128, 105, 174, 255),
                enabledButtonSecondary = new Color32(174, 148, 221, 255),

                outline = new Color32(13, 10, 21, 255),
            },

            new()
            {
                name = "Sunset",

                backgroundPrimary   = new Color32(43, 26, 35, 255),
                backgroundSecondary = new Color32(69, 38, 49, 255),

                disabledButtonPrimary   = new Color32(33, 21, 29, 255),
                disabledButtonSecondary = new Color32(53, 31, 41, 255),

                enabledButtonPrimary   = new Color32(190, 91, 89, 255),
                enabledButtonSecondary = new Color32(239, 139, 119, 255),

                outline = new Color32(21, 11, 17, 255),
            },

            new()
            {
                name = "Sage",

                backgroundPrimary   = new Color32(25, 35, 31, 255),
                backgroundSecondary = new Color32(39, 54, 47, 255),

                disabledButtonPrimary   = new Color32(20, 29, 25, 255),
                disabledButtonSecondary = new Color32(32, 44, 38, 255),

                enabledButtonPrimary   = new Color32(91, 139, 112, 255),
                enabledButtonSecondary = new Color32(139, 184, 153, 255),

                outline = new Color32(10, 16, 13, 255),
            },

            new()
            {
                name = "Rose",

                backgroundPrimary   = new Color32(42, 24, 34, 255),
                backgroundSecondary = new Color32(64, 36, 51, 255),

                disabledButtonPrimary   = new Color32(33, 20, 28, 255),
                disabledButtonSecondary = new Color32(51, 31, 42, 255),

                enabledButtonPrimary   = new Color32(174, 85, 124, 255),
                enabledButtonSecondary = new Color32(221, 130, 164, 255),

                outline = new Color32(20, 10, 16, 255),
            },

            new()
            {
                name = "ii's Stupid Menu",

                backgroundPrimary   = new Color32(255, 128, 0, 128),
                backgroundSecondary = new Color32(255, 102, 0, 128),

                disabledButtonPrimary   = new Color32(170, 85, 0, 255),
                disabledButtonSecondary = new Color32(170, 85, 0, 255),

                enabledButtonPrimary   = new Color32(85, 42, 0, 255),
                enabledButtonSecondary = new Color32(85, 42, 0, 255),

                outline = new Color32(70, 30, 0, 255),
            },

            new()
            {
                name = "Blue Magenta",

                backgroundPrimary   = Color.blue,
                backgroundSecondary = Color.magenta,

                disabledButtonPrimary   = Color.black,
                disabledButtonSecondary = Color.black,

                enabledButtonPrimary   = Color.blue,
                enabledButtonSecondary = Color.blue,

                outline = new Color32(35, 0, 70, 255),
            },

            new()
            {
                name = "Cyber Lime",

                backgroundPrimary   = new Color32(20, 45, 18, 255),
                backgroundSecondary = new Color32(45, 85, 25, 255),

                disabledButtonPrimary   = new Color32(15, 30, 12, 255),
                disabledButtonSecondary = new Color32(28, 48, 20, 255),

                enabledButtonPrimary   = new Color32(100, 200, 35, 255),
                enabledButtonSecondary = new Color32(180, 255, 65, 255),

                outline = new Color32(5, 18, 4, 255),
            },

            new()
            {
                name = "Cotton Candy",

                backgroundPrimary   = new Color32(55, 30, 75, 255),
                backgroundSecondary = new Color32(95, 45, 105, 255),

                disabledButtonPrimary   = new Color32(40, 25, 55, 255),
                disabledButtonSecondary = new Color32(65, 35, 80, 255),

                enabledButtonPrimary   = new Color32(220, 100, 180, 255),
                enabledButtonSecondary = new Color32(140, 130, 255, 255),

                outline = new Color32(30, 12, 45, 255),
            },

            new()
            {
                name = "Gold",

                backgroundPrimary   = new Color32(48, 35, 12, 255),
                backgroundSecondary = new Color32(85, 60, 18, 255),

                disabledButtonPrimary   = new Color32(35, 26, 10, 255),
                disabledButtonSecondary = new Color32(60, 45, 15, 255),

                enabledButtonPrimary   = new Color32(190, 135, 30, 255),
                enabledButtonSecondary = new Color32(255, 205, 75, 255),

                outline = new Color32(25, 16, 4, 255),
            },

            new()
            {
                name = "Crimson",

                backgroundPrimary   = new Color32(40, 8, 20, 255),
                backgroundSecondary = new Color32(75, 12, 35, 255),

                disabledButtonPrimary   = new Color32(28, 8, 16, 255),
                disabledButtonSecondary = new Color32(50, 12, 27, 255),

                enabledButtonPrimary   = new Color32(170, 20, 55, 255),
                enabledButtonSecondary = new Color32(245, 45, 85, 255),

                outline = new Color32(20, 3, 10, 255),
            },
            new()
            {
                name = "Purple Blue",

                backgroundPrimary   = new Color32(35, 20, 85, 255),
                backgroundSecondary = new Color32(15, 55, 115, 255),

                disabledButtonPrimary   = new Color32(24, 17, 55, 255),
                disabledButtonSecondary = new Color32(17, 32, 70, 255),

                enabledButtonPrimary   = new Color32(115, 65, 230, 255),
                enabledButtonSecondary = new Color32(55, 145, 255, 255),

                outline = new Color32(12, 7, 35, 255),
            },

            new()
            {
                name = "Pink Orange",

                backgroundPrimary   = new Color32(115, 25, 75, 255),
                backgroundSecondary = new Color32(145, 55, 20, 255),

                disabledButtonPrimary   = new Color32(65, 20, 48, 255),
                disabledButtonSecondary = new Color32(75, 32, 18, 255),

                enabledButtonPrimary   = new Color32(245, 75, 155, 255),
                enabledButtonSecondary = new Color32(255, 145, 65, 255),

                outline = new Color32(45, 8, 28, 255),
            },

            new()
            {
                name = "Cyan Purple",

                backgroundPrimary   = new Color32(0, 75, 105, 255),
                backgroundSecondary = new Color32(65, 25, 115, 255),

                disabledButtonPrimary   = new Color32(10, 42, 58, 255),
                disabledButtonSecondary = new Color32(38, 20, 65, 255),

                enabledButtonPrimary   = new Color32(0, 200, 220, 255),
                enabledButtonSecondary = new Color32(155, 85, 255, 255),

                outline = new Color32(5, 15, 38, 255),
            },

            new()
            {
                name = "Red Orange",

                backgroundPrimary   = new Color32(100, 15, 30, 255),
                backgroundSecondary = new Color32(125, 40, 5, 255),

                disabledButtonPrimary   = new Color32(55, 12, 22, 255),
                disabledButtonSecondary = new Color32(65, 25, 8, 255),

                enabledButtonPrimary   = new Color32(230, 35, 65, 255),
                enabledButtonSecondary = new Color32(255, 125, 25, 255),

                outline = new Color32(40, 5, 10, 255),
            },

            new()
            {
                name = "Teal Blue",

                backgroundPrimary   = new Color32(5, 65, 75, 255),
                backgroundSecondary = new Color32(15, 40, 105, 255),

                disabledButtonPrimary   = new Color32(10, 38, 45, 255),
                disabledButtonSecondary = new Color32(15, 27, 60, 255),

                enabledButtonPrimary   = new Color32(20, 190, 175, 255),
                enabledButtonSecondary = new Color32(55, 115, 245, 255),

                outline = new Color32(3, 18, 30, 255),
            },

            new()
            {
                name = "Yellow Pink",

                backgroundPrimary   = new Color32(105, 70, 15, 255),
                backgroundSecondary = new Color32(110, 30, 75, 255),

                disabledButtonPrimary   = new Color32(58, 43, 15, 255),
                disabledButtonSecondary = new Color32(60, 22, 45, 255),

                enabledButtonPrimary   = new Color32(255, 195, 55, 255),
                enabledButtonSecondary = new Color32(245, 85, 165, 255),

                outline = new Color32(38, 12, 25, 255),
            },

            new()
            {
                name = "Old RGB",

                backgroundPrimary   = Color.red,
                backgroundMiddle    = Color.green,
                backgroundSecondary = Color.blue,

                disabledButtonPrimary   = Color.black,
                disabledButtonMiddle    = Color.black,
                disabledButtonSecondary = Color.black,

                enabledButtonPrimary   = Color.red,
                enabledButtonMiddle    = Color.green,
                enabledButtonSecondary = Color.blue,
            },

            new()
            {
                name = "Pitch Black",

                backgroundPrimary   = Color.black,
                backgroundMiddle    = Color.black,
                backgroundSecondary = Color.black,

                disabledButtonPrimary   = Color.black,
                disabledButtonMiddle    = Color.black,
                disabledButtonSecondary = Color.black,

                enabledButtonPrimary   = new Color32(76, 0, 143, 255),
                enabledButtonSecondary   = new Color32(76, 0, 143, 255),
            },
    };

    [SavedSetting(OnLoaded = nameof(ApplyTheme))]
    public static int themeIndex;

    [SavedSetting(OnLoaded = nameof(ApplyTheme))]
    public static bool rainbowColors = true;
    [SavedSetting] public static bool outlines;

    [SavedSetting] public static float menuCornerRadius = 0.06f;
    [SavedSetting] public static float outlineThickness = 0.018f;

    public static Font currentFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

    [SavedSetting] public static bool fpsCounter       = true;
    [SavedSetting] public static bool disconnectButton = true;
    [SavedSetting] public static bool rightHanded;
    [SavedSetting] public static bool disableNotifications;

    [SavedSetting] public static KeyCode keyboardButton = KeyCode.Q;

    [SavedSetting] public static Vector3 menuSize       = new(0.1f, 1f, 1f); // Depth, width, height
    [SavedSetting] public static int     buttonsPerPage = 8;

    [SavedSetting] public static bool incrementalButtons      = true;
    [SavedSetting] public static bool buttonGradients         = true;
    [SavedSetting] public static bool roundedButtons          = true;
    [SavedSetting] public static bool verticalButtonGradients = false;

    [SavedSetting] public static bool pageButtonsAtTop;

    [SavedSetting] public static float searchKeyboardFollowSpeed = 5f;

    [SavedSetting] public static Vector3 buttonSize = new(0.09f, 0.9f, 0.08f);

    [SavedSetting] public static float buttonSpacing      = 0.1f;
    [SavedSetting] public static float buttonCornerRadius = 0.018f;

    [SavedSetting] public static int buttonCornerSegments = 5;

    [SavedSetting] public static float gradientSpeed = 0.5f; // Speed of colors

    [SavedSetting] public static GunSettings gunSettings = new();

    static Settings() =>
            ApplyTheme();

    public static string GradientAnimationName =>
            gradientAnimationMode.ToString();

    public static string ThemeName => themes[themeIndex].name;

    public static void ChangeGradientAnimation(bool increment)
    {
        int count = Enum.GetValues(typeof(GradientAnimationMode)).Length;

        int index =
                (int)gradientAnimationMode +
                (increment ? 1 : -1);

        if (index >= count)
            index = 0;
        else if (index < 0)
            index = count - 1;

        gradientAnimationMode =
                (GradientAnimationMode)index;

        ColorChanger.ClearGradientCache();
    }

    public static void ChangeTheme(bool increment)
    {
        themeIndex += increment ? 1 : -1;

        if (themeIndex >= themes.Length)
            themeIndex = 0;
        else if (themeIndex < 0)
            themeIndex = themes.Length - 1;

        ApplyTheme();
    }

    public static void SetRainbowColors(bool enabled)
    {
        rainbowColors = enabled;

        ApplyTheme();
    }

    public static void ApplyTheme()
    {
        if (themes.Length == 0)
            return;

        themeIndex = Mathf.Clamp(themeIndex, 0, themes.Length - 1);
        MenuTheme theme = themes[themeIndex];

        backgroundColor = rainbowColors
            ? new ExtGradient { rainbow = true }
            : CreateGradient(theme.backgroundPrimary, theme.backgroundSecondary, theme.backgroundMiddle);

        buttonColors[0] = CreateGradient(theme.disabledButtonPrimary, theme.disabledButtonSecondary, theme.disabledButtonMiddle);

        buttonColors[1] = rainbowColors
            ? new ExtGradient { rainbow = true }
            : CreateGradient(theme.enabledButtonPrimary, theme.enabledButtonSecondary, theme.enabledButtonMiddle);

        outlineColor = new ExtGradient
        {
            colors = ExtGradient.GetSolidGradient(theme.outline),
        };

        textColors[0] = theme.disabledText;
        textColors[1] = theme.enabledText;

        ColorChanger.ClearGradientCache();
    }

    private static ExtGradient CreateGradient(Color first, Color last, Color? middle = null)
    {
        return new ExtGradient
        {
            colors = middle.HasValue
                ? new[]
                {
                    new GradientColorKey(first, 0f),
                    new GradientColorKey(middle.Value, 0.25f),
                    new GradientColorKey(last, 0.5f),
                    new GradientColorKey(middle.Value, 0.75f),
                    new GradientColorKey(first, 1f),
                }
                : ExtGradient.GetSimpleGradient(first, last),
        };
    }
}