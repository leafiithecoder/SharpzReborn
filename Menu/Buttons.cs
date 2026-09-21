using GorillaLocomotion;
using Pathfinding.RVO.Sampled;
using SharpzReborn.Classes;
using SharpzReborn.Mods;
using SharpzReborn.Mods.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Menu
{
    public static class Buttons
    {
            public static ButtonInfo[] MainMods =
            [
                new() { buttonText = "Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the settings." },
                new() { buttonText = "Photon", method = () => SetCategory("Room Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the room mods." },
                new() { buttonText = "Movement", method = () => SetCategory("Movement Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the movement mods." },
                new() { buttonText = "Safety", method = () => SetCategory("Safety Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the safety mods." },
                new() { buttonText = "Visual", method = () => SetCategory("Visual Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the visual mods." },
                new() { buttonText = "Player", method = () => SetCategory("Player Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the player mods." },
                new() { buttonText = "Advantage", method = () => SetCategory("Advantage Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the player mods." },
                new() { buttonText = "Important", method = () => SetCategory("Important Mods"), mode = ButtonMode.Action, isCategory = true,toolTip = "Opens the important mods." },
            ];

            public static ButtonInfo[] Settings =
            [
                new() { buttonText = "Exit Settings", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Menu Settings", method = () => SetCategory("Menu Settings"), mode = ButtonMode.Action, toolTip = "Opens the menu settings." },
                new() { buttonText = "Movement Settings", method = () => SetCategory("Movement Settings"), mode = ButtonMode.Action, toolTip = "Opens the movement settings." },
            ];

            private static readonly ButtonInfo[] MenuSettings =
            [
                new() { buttonText = "Exit Menu Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main settings page." },
                new() { buttonText = "Right Handed Menu", enableMethod = () => Menu.Settings.rightHanded = true, disableMethod = () => Menu.Settings.rightHanded = false, enabled = Menu.Settings.rightHanded, mode = ButtonMode.Toggle, toolTip = "Moves the menu to your right hand." },
                new() { buttonText = "Disable Notifications", enableMethod = () => Menu.Settings.disableNotifications = true, disableMethod = () => Menu.Settings.disableNotifications = false, enabled = Menu.Settings.disableNotifications, mode = ButtonMode.Toggle, toolTip = "Disables notifications." },
                new() { buttonText = "Disable FPS Counter", enableMethod = () => Menu.Settings.fpsCounter = false, disableMethod = () => Menu.Settings.fpsCounter = true, enabled = !Menu.Settings.fpsCounter, mode = ButtonMode.Toggle, toolTip = "Disables the FPS counter." },
                new() { buttonText = "Disable Disconnect Button", enableMethod = () => Menu.Settings.disconnectButton = false, disableMethod = () => Menu.Settings.disconnectButton = true, enabled = !Menu.Settings.disconnectButton, mode = ButtonMode.Toggle, toolTip = "Disables the disconnect button." },
                new() { buttonText = "Incremental Buttons", enableMethod = () => Menu.Settings.incrementalButtons = true, disableMethod = () => Menu.Settings.incrementalButtons = false, enabled = Menu.Settings.incrementalButtons, mode = ButtonMode.Toggle, toolTip = "Shows separate minus and plus controls for incremental settings." },
                new() { buttonText = "Rounded Menu", enableMethod = () => Menu.Settings.roundedButtons = true, disableMethod = () => Menu.Settings.roundedButtons = false, enabled = Menu.Settings.roundedButtons, mode = ButtonMode.Toggle, toolTip = "Rounds the menu, buttons, search keyboard, and other menu objects." },
                new() { buttonText = "Drop Menu", enableMethod = () =>Menu.Settings. dropMenu = true, disableMethod = () => Menu.Settings.dropMenu = false, enabled = Menu.Settings.dropMenu, mode = ButtonMode.Toggle, toolTip = "Makes the menu open while held and drop when released." },
                new() { buttonText = "Animated Menu", enableMethod = () => Menu.Settings.animateMenu = true, disableMethod = () => Menu.Settings.animateMenu = false, enabled = Menu.Settings.animateMenu, mode = ButtonMode.Toggle, toolTip = "Adds a simple grow and shrink animation to the menu." },
                new() { buttonText = "Gradient Animation", incrementMethod = Menu.Settings.ChangeGradientAnimation, displayText = () => $"Gradient Animation [{Menu.Settings.GradientAnimationName}]", mode = ButtonMode.Incremental, toolTip = "Changes how animated gradients move." },
                new() { buttonText = "Rainbow Colours", enableMethod = () => Menu.Settings.SetRainbowColors(true), disableMethod = () => Menu.Settings.SetRainbowColors(false), enabled = Menu.Settings.rainbowColors, mode = ButtonMode.Toggle, toolTip = "Uses animated rainbow colours instead of the selected colour scheme." },
                new() { buttonText = "Theme Changer", incrementMethod = Menu.Settings.ChangeTheme, displayText = () => $"Theme Changer [{Menu.Settings.ThemeName}]", mode = ButtonMode.Incremental, toolTip = "Changes the colour scheme used by the menu." },
                new() { buttonText = "Outlines", enableMethod = () => Menu.Settings.outlines = true, disableMethod = () => Menu.Settings.outlines = false, enabled = Menu.Settings.outlines, mode = ButtonMode.Toggle, toolTip = "Adds outlines around the menu and its buttons." },
                new() { buttonText = "Button Gradients", enableMethod = () => Menu.Settings.buttonGradients = true, disableMethod = () => Menu.Settings.buttonGradients = false, enabled = Menu.Settings.buttonGradients, mode = ButtonMode.Toggle, toolTip = "Toggles spatial gradients on buttons." },
                new() { buttonText = "Vertical Gradients", enableMethod = () => Menu.Settings.verticalButtonGradients = true, disableMethod = () => Menu.Settings.verticalButtonGradients = false, enabled = Menu.Settings.verticalButtonGradients, mode = ButtonMode.Toggle, toolTip = "Changes button gradients between horizontal and vertical." },
                new() { buttonText = "Top Page Buttons", enableMethod = () => Menu.Settings.pageButtonsAtTop = true, disableMethod = () => Menu.Settings.pageButtonsAtTop = false, enabled = Menu.Settings.pageButtonsAtTop, mode = ButtonMode.Toggle, toolTip = "Moves the page buttons into the first two normal button slots." },
            ];

            public static ButtonInfo[] MovementSettings =
            [
                new() { buttonText = "Exit Movement Settings", method = () => SetCategory("Settings"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main settings page." },
                new() { buttonText = "Change Fly Speed", overlapText = $"Change Fly Speed [{Mods.Settings.Movement.flySpeedNames[Mods.Settings.Movement.flySpeedIndex]}]", incrementMethod = Mods.Settings.Movement.ChangeFlySpeed, mode = ButtonMode.Incremental, toolTip = "Changes the fly speed." },
                new() { buttonText = "Change Arm Length", overlapText = $"Change Arm Length [{Mods.Settings.Movement.armLengthNames[Mods.Settings.Movement.armLengthIndex]}]", incrementMethod = Mods.Settings.Movement.ChangeArmLength, mode = ButtonMode.Incremental, toolTip = "Changes the length of your arms." },
                new() { buttonText = "Change Speed Boost Amount", overlapText = $"Change Speed Boost Amount [{Mods.Settings.Movement.speedNames[Mods.Settings.Movement.speedAmountIndex]}]", incrementMethod = Mods.Settings.Movement.ChangeSpeedBoostAmount, mode = ButtonMode.Incremental, toolTip = "Changes the amount of your speed boost." },
                new() { buttonText = "Grip Speed Boost", enableMethod = () => gripSpeedBoost = true, disableMethod = () => gripSpeedBoost = false, mode = ButtonMode.Toggle, toolTip = "Binds the speed boost mod to your grip." },
                new() { buttonText = "Trigger Speed Boost", enableMethod = () => triggerSpeedBoost = true, disableMethod = () => triggerSpeedBoost = false, mode = ButtonMode.Toggle, toolTip = "Binds the speed boost mod to your trigger." },
                new() { buttonText = "Trigger Platforms", enableMethod = () => triggerPlatforms = true, disableMethod = () => triggerPlatforms = false, mode = ButtonMode.Toggle, toolTip = "Spawns platforms on your hands when you press trigger." },
                new() { buttonText = "Non-Sticky Platforms", enableMethod = () => nonStickyPlatforms = true, disableMethod = () => nonStickyPlatforms = false, mode = ButtonMode.Toggle, toolTip = "Makes your hands not stick to the platforms in the platforms mod." },
                new() { buttonText = "Constant Noclip", enableMethod = () => constantNoclip = true, disableMethod = () => constantNoclip = false, mode = ButtonMode.Toggle, toolTip = "Keeps the noclip mod always on." },
                new() { buttonText = "Grip Noclip", enableMethod = () => gripNoclip = true, disableMethod = () => gripNoclip = false, mode = ButtonMode.Toggle, toolTip = "Binds the noclip mod to your grip." },
            ];

            public static ButtonInfo[] RoomMods =
            [
                new() { buttonText = "Exit Photon Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Disconnect", method = () => NetworkSystem.Instance.ReturnToSinglePlayer(), mode = ButtonMode.Action, toolTip = "Disconnects you from the current room." },
                new() { buttonText = "Join Last Room", method = Room.JoinLastRoom, mode = ButtonMode.Action, toolTip = "Joins the last room you were in." },
                new() { buttonText = "Join Random Pub", method = Room.JoinRandomRoom, mode = ButtonMode.Action, toolTip = "Joins a random available room." },
                new() { buttonText = "Reconnect", method = Room.Reconnect, mode = ButtonMode.Action, toolTip = "Reconnects you to the Photon server." },
                new() { buttonText = "Reconnect & Rejoin", method = Room.ReconnectAndRejoin, mode = ButtonMode.Action, toolTip = "Reconnects to Photon and attempts to rejoin your previous room." },
                new() { buttonText = "Join Menu Room", method = Room.JoinMenuRoom, mode = ButtonMode.Action, toolTip = "Joins the BRANDED menu room." },
                new() { buttonText = "Check Master", method = Room.AmIMaster, mode = ButtonMode.Action, toolTip = "Checks if you are the master client of the room." },
                new() { buttonText = "Queue Rooms", method =() => SetCategory("Queue Room Mods"), mode = ButtonMode.Action, toolTip = "Opens the Queue Rooms tab."}
            ];

            public static ButtonInfo[] QueueRoomMods =
            [
                new() { buttonText = "Exit Queue Rooms", method = () => SetCategory("Room"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Queue Room [MODS]", method =() => Room.QueueJoinRoom("mods"), mode = ButtonMode.Action, toolTip = "Queues the room MODS." },
                new() { buttonText = "Queue Room [MOD]", method =() => Room.QueueJoinRoom("mod"), mode = ButtonMode.Action, toolTip = "Queues the room MOD." },
                new() { buttonText = "Queue Room [MODDER]", method =() => Room.QueueJoinRoom("modder"), mode = ButtonMode.Action, toolTip = "Queues the room MODDER." },
                new() { buttonText = "Queue Room [BOT]", method =() => Room.QueueJoinRoom("bot"), mode = ButtonMode.Action, toolTip = "Queues the room BOT." },
                new() { buttonText = "Queue Room [DAISY09]", method =() => Room.QueueJoinRoom("daisy09"), mode = ButtonMode.Action, toolTip = "Queues the room DAISY09." },
                new() { buttonText = "Queue Room [DAISY]", method =() => Room.QueueJoinRoom("daisy"), mode = ButtonMode.Action, toolTip = "Queues the room DAISY." },
                new() { buttonText = "Queue Room [PBBV]", method = () => Room.QueueJoinRoom("pbbv"), mode = ButtonMode.Action, toolTip = "Queues the room PBBV." },
                new() { buttonText = "Queue Room [RUN]", method = () => Room.QueueJoinRoom("run"), mode = ButtonMode.Action, toolTip = "Queues the room RUN." },
                new() { buttonText = "Queue Room [J3VU]", method = () => Room.QueueJoinRoom("j3vu"), mode = ButtonMode.Action, toolTip = "Queues the room J3VU." },
                new() { buttonText = "Queue Room [GHOST]", method = () => Room.QueueJoinRoom("ghost"), mode = ButtonMode.Action, toolTip = "Queues the room GHOST." },
                new() { buttonText = "Queue Room [TIPTOE]", method = () => Room.QueueJoinRoom("tiptoe"), mode = ButtonMode.Action, toolTip = "Queues the room TIPTOE." },
                new() { buttonText = "Queue Room [SREN17]", method = () => Room.QueueJoinRoom("sren17"), mode = ButtonMode.Action, toolTip = "Queues the room SREN17." },
                new() { buttonText = "Queue Room [SREN18]", method = () => Room.QueueJoinRoom("sren18"), mode = ButtonMode.Action, toolTip = "Queues the room SREN18." },
                new() { buttonText = "Queue Room [SREN16]", method = () => Room.QueueJoinRoom("sren16"), mode = ButtonMode.Action, toolTip = "Queues the room SREN16." },
                new() { buttonText = "Queue Room [HELP]", method = () => Room.QueueJoinRoom("help"), mode = ButtonMode.Action, toolTip = "Queues the room HELP." },
                new() { buttonText = "Queue Room [HIDE]", method = () => Room.QueueJoinRoom("hide"), mode = ButtonMode.Action, toolTip = "Queues the room HIDE." },
                new() { buttonText = "Queue Room [H3LP]", method = () => Room.QueueJoinRoom("h3lp"), mode = ButtonMode.Action, toolTip = "Queues the room H3LP." },
                new() { buttonText = "Queue Room [BANSHEE]", method = () => Room.QueueJoinRoom("banshee"), mode = ButtonMode.Action, toolTip = "Queues the room BANSHEE." },
                new() { buttonText = "Queue Room [PAUL]", method = () => Room.QueueJoinRoom("paul"), mode = ButtonMode.Action, toolTip = "Queues the room PAUL." },
                new() { buttonText = "Queue Room [ECHO]", method = () => Room.QueueJoinRoom("echo"), mode = ButtonMode.Action, toolTip = "Queues the room ECHO." },
                new() { buttonText = "Queue Room [WARNING]", method = () => Room.QueueJoinRoom("warning"), mode = ButtonMode.Action, toolTip = "Queues the room WARNING." },
            ];

            public static ButtonInfo[] MovementMods =
            [
                new() { buttonText = "Exit Movement Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, isCategory = true,toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Platforms [G]", method = Mods.Movement.Platforms, mode = ButtonMode.Toggle, toolTip = "Spawns platforms on your hands when you press grip." },
                new() { buttonText = "Fly [A]", method = Mods.Movement.Fly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding A." },
                new() { buttonText = "Hand Fly [A]", method = Mods.Movement.HandFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward in the direction your hand is pointing while holding A." },
                new() { buttonText = "Noclip Fly [A]", method = Mods.Movement.NoclipFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding A and lets you phase through walls." },
                new() { buttonText = "Trigger Fly [T]", method = Mods.Movement.TriggerFly, mode = ButtonMode.Toggle, toolTip = "Moves you forward while holding your right trigger." },
                new() { buttonText = "WASD Fly [WASD]", enableMethod = Mods.Movement.EnableWASDFly, method = Mods.Movement.WASDFly, disableMethod = () => GTPlayer.Instance.GetControllerTransform(false).parent.rotation = Quaternion.Euler(0, 0, 0), mode = ButtonMode.Toggle, toolTip = "Moves you around with WASD." },
                new() { buttonText = "Teleport Gun", method = Mods.Movement.TeleportGun, mode = ButtonMode.Toggle, toolTip = "Teleports you to wherever your pointer is when you press trigger." },
                new() { buttonText = "Speed Boost", method = Mods.Movement.SpeedBoost, mode = ButtonMode.Toggle, toolTip = "Increases your movement speed." },
                new() { buttonText = "Noclip [T]", method = Mods.Movement.Noclip, mode = ButtonMode.Toggle, toolTip = "Allows you to move through walls." },
                new() { buttonText = "Steam Long Arms", enableMethod = Mods.Movement.EnableSteamLongArms, disableMethod = Mods.Movement.DisableSteamLongArms, method = Mods.Movement.EnableSteamLongArms, mode = ButtonMode.Toggle, toolTip = "Simulates SteamVR's world scale to make your arms longer." },
                new() { buttonText = "Sticky Hands", enableMethod = Mods.Movement.StickyHands, disableMethod = Mods.Movement.DisableStickyHands, method = Mods.Movement.StickyHands, mode = ButtonMode.Toggle, toolTip = "Makes your hands sticky." },
                new() { buttonText = "Slide Control", enableMethod = Mods.Movement.EnableSlideControl, disableMethod = Mods.Movement.DisableSlideControl, mode = ButtonMode.Toggle, toolTip = "Allows you to be able to control your sliding." },
                new() { buttonText = "Moon Walk", method = Mods.Movement.LowGravity, mode = ButtonMode.Toggle, toolTip = "Reduces the gravity on you." },
                new() { buttonText = "No Gravity", method = Mods.Movement.ZeroGravity, mode = ButtonMode.Toggle, toolTip = "Removes gravity from you." },
                new() { buttonText = "Jupiter Walk", method = Mods.Movement.HighGravity, mode = ButtonMode.Toggle, toolTip = "Increases the gravity on you." },
                new() { buttonText = "Up And Down", method = Mods.Movement.UpAndDown, mode = ButtonMode.Toggle, toolTip = "Moves you up and down." },
                new() { buttonText = "Left And Right", method = Mods.Movement.LeftAndRight, mode = ButtonMode.Toggle, toolTip = "Moves you left and right." },
                new() { buttonText = "Forwards And Backwards", method = Mods.Movement.ForwardsAndBackwards, mode = ButtonMode.Toggle, toolTip = "Moves you forwards and backwards." },
                new() { buttonText = "Force Tag Freeze", method = Mods.Movement.ForceTagFreeze, mode = ButtonMode.Toggle, toolTip = "Forces tag freeze." },
                new() { buttonText = "No Tag Freeze", method = Mods.Movement.NoTagFreeze, mode = ButtonMode.Toggle, toolTip = "Removes tag freeze." },
                new() { buttonText = "Wall Walk [G]", method = Mods.Movement.WallWalk, mode = ButtonMode.Toggle, toolTip = "Allows you to walk on walls." },
            ];

            public static ButtonInfo[] SafetyMods =
            [
                new() { buttonText = "Exit Safety Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Anti Report [Disconnect]", method = Safety.AntiReportDisconnect, mode = ButtonMode.Toggle, toolTip = "Disconnects when you are reported." },
                new() { buttonText = "Anti Moderator [Disconnect]", method = Safety.AntiModerator, mode = ButtonMode.Toggle, toolTip = "Disconnects when there is a moderator in the lobby." },
                new() { buttonText = "No Finger Movement", method = Safety.NoFingerMovement, mode = ButtonMode.Toggle, toolTip = "Disables finger movement." },
                new() { buttonText = "Flush RPCs", method = Safety.FlushRPCs, mode = ButtonMode.Action, toolTip = "Flushes RPCs." },
            ];

            public static ButtonInfo[] VisualMods =
            [
                new() { buttonText = "Exit Visual Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Bone ESP", method = Visuals.BoneESP, mode = ButtonMode.Toggle, toolTip = "Shows player bones." },
                new() { buttonText = "Tracers", method = Visuals.Tracers, disableMethod =() => {Visuals.isLineRenderQueued = false; Visuals.ClearLinePool(); }, mode = ButtonMode.Toggle, toolTip = "Shows lines from you to other players." },
            ];

            public static ButtonInfo[] PlayerMods =
            [
                new() { buttonText = "Exit Player Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Fix Head", method = Player.FixHead, mode = ButtonMode.Action, toolTip = "Fixes any bugs with your head." },
                new() { buttonText = "Grab Rig [G]", method = Player.GrabRig, mode = ButtonMode.Toggle, toolTip = "Lets you grab your rig." },
                new() { buttonText = "Ghost [A]", method = Player.Ghost, mode = ButtonMode.Toggle, toolTip = "Makes you a ghost." },
                new() { buttonText = "Invisible [B]", method = Player.Invisible, mode = ButtonMode.Toggle, toolTip = "Makes you invisible." },
                new() { buttonText = "Spin Head X", method = () => Player.SpinHead("x"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the X axis." },
                new() { buttonText = "Spin Head Y", method = () => Player.SpinHead("Y"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the Y axis." },
                new() { buttonText = "Spin Head Z", method = () => Player.SpinHead("Z"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spins your head on the Z axis." },
                new() { buttonText = "Backwards Head", method = Player.BackwardsHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head backwards." },
                new() { buttonText = "Sideways Head", method = Player.SidewaysHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head sideways." },
                new() { buttonText = "Upside Down Head", method = Player.UpsideDownHead, disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Turns your head upside down." },
                new() { buttonText = "Spaz Head X", method = () => Player.SpazHead("x"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the X axis." },
                new() { buttonText = "Spaz Head Y", method = () => Player.SpazHead("y"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the Y axis." },
                new() { buttonText = "Spaz Head Z", method = () => Player.SpazHead("z"), disableMethod = Player.FixHead, mode = ButtonMode.Toggle, toolTip = "Spazzes your head on the Z axis." },
                new() { buttonText = "Spaz Head", method = Player.SpazHeadXYZ, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Spazzes your head on all axes." },
                new() { buttonText = "Broken Neck", method = Player.BrokenNeck, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Breaks your neck." },
                new() { buttonText = "Head Bang", method = Player.HeadBang, mode = ButtonMode.Toggle, disableMethod = Player.FixHead, toolTip = "Makes your head bang." },
            ];

        
            public static ButtonInfo[] AdvantageMods =
            [
                new() { buttonText = "Exit Advantage Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Tag Self", method = Advantages.TagSelf, mode = ButtonMode.Action, toolTip = "Adds the tag state to you." },
                new() { buttonText = "Tag Gun", method = Advantages.TagGun, mode = ButtonMode.Toggle, toolTip = "Allows you to tag anyone when tagged with a gun." },
                new() { buttonText = "Tag All", method = Advantages.TagAll, mode = ButtonMode.Action, toolTip = "Tags everyone." },
                new() { buttonText = "Untag All [M]", method = Advantages.UntagAll, mode = ButtonMode.Action, toolTip = "Removes the tag state from everyone." },
                new() { buttonText = "Untag Gun [M]", method = Advantages.UntagGun, mode = ButtonMode.Toggle, toolTip = "Removes the tag state from anyone with a gun." },
                new() { buttonText = "Disable Tags [M]", method = Advantages.DisableTags, mode = ButtonMode.Toggle, toolTip = "Constantly removes the tag state from everyone." },
                new() { buttonText = "Flick Tag Gun", method = Advantages.FlickTagGun, mode = ButtonMode.Toggle, toolTip = "Simulates a flick tag." },
            ];

            public static ButtonInfo[] ImportantMods =
            [
                new() { buttonText = "Exit Important Mods", method = () => SetCategory("Main"), mode = ButtonMode.Action, toolTip = "Returns to the main page of the menu." },
                new() { buttonText = "Crash", method = Application.Quit, mode = ButtonMode.Action, toolTip = "Closes the game." },
            ];

            public static ButtonCategory[] Categories =
            [
                new() { name = "Main", buttons = MainMods },
                new() { name = "Settings", buttons = Settings },
                new() { name = "Menu Settings", buttons = MenuSettings },
                new() { name = "Movement Settings", buttons = MovementSettings },
                new() { name = "Room Mods", buttons = RoomMods },
                new() { name = "Queue Room Mods", buttons = QueueRoomMods},
                new() { name = "Movement Mods", buttons = MovementMods },
                new() { name = "Safety Mods", buttons = SafetyMods },
                new() { name = "Visual Mods", buttons = VisualMods },
                new() { name = "Player Mods", buttons = PlayerMods },
                new() { name = "Advantage Mods", buttons = AdvantageMods },
                new() { name = "Important Mods", buttons = ImportantMods },
        ];


        private static readonly Dictionary<string, ButtonCategory> CategoryLookup = BuildCategoryLookup();
        private static readonly Dictionary<string, ButtonInfo> ButtonLookup = BuildButtonLookup();

        private static readonly Dictionary<string, ButtonInfo> SavedToggleButtonLookup =
                        BuildSavedToggleButtonLookup();

        public static IReadOnlyDictionary<string, ButtonInfo> SavedToggleButtons =>
                        SavedToggleButtonLookup;

        public static ButtonInfo[] AllButtons { get; } = BuildButtonArray();

        public static int ButtonCount =>
            AllButtons.Count(button => button != null && !button.isCategory);

        public static ButtonCategory GetCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return null;

            CategoryLookup.TryGetValue(categoryName, out ButtonCategory category);

            return category;
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            if (string.IsNullOrWhiteSpace(buttonText))
                return null;

            ButtonLookup.TryGetValue(buttonText, out ButtonInfo button);

            return button;
        }

        private static Dictionary<string, ButtonCategory> BuildCategoryLookup()
        {
            Dictionary<string, ButtonCategory> lookup = new(StringComparer.OrdinalIgnoreCase);

            foreach (ButtonCategory category in Categories)
            {
                if (string.IsNullOrWhiteSpace(category.name))
                    throw new InvalidOperationException("A button category does not have a name.");

                if (!lookup.TryAdd(category.name, category))
                    throw new InvalidOperationException($"Duplicate button category named {category.name}.");

            }

            return lookup;
        }

        private static Dictionary<string, ButtonInfo> BuildButtonLookup()
        {
            Dictionary<string, ButtonInfo> lookup = new(StringComparer.OrdinalIgnoreCase);

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    if (string.IsNullOrWhiteSpace(button.buttonText))
                        continue;

                    // Duplicate names such as "Return to Main" are allowed.
                    // Actual menu presses reference their ButtonInfo directly,
                    // so this lookup is only used when something explicitly requests a button by name.
                    lookup.TryAdd(button.buttonText, button);
                }
            }

            return lookup;
        }

        private static ButtonInfo[] BuildButtonArray()
        {
            int buttonCount = Categories.Sum(category => category.buttons.Length);

            ButtonInfo[] result = new ButtonInfo[buttonCount];

            int index = 0;

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    result[index] = button;
                    index++;
                }
            }

            return result;
        }

        private static Dictionary<string, ButtonInfo> BuildSavedToggleButtonLookup()
        {
            Dictionary<string, ButtonInfo> result =
                            new(
                                            StringComparer.Ordinal);

            foreach (ButtonCategory category in Categories)
            {
                foreach (ButtonInfo button in category.buttons)
                {
                    if (button.mode != ButtonMode.Toggle)
                        continue;

                    string key =
                                    category.name +
                                    "/" +
                                    button.buttonText;

                    if (!result.TryAdd(key, button))
                    {
                        throw new InvalidOperationException(
                                        $"Duplicate saved toggle button key {key}.");
                    }

                }
            }

            return result;
        }
    }
}