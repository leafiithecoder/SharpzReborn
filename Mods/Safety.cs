using BepInEx;
using Photon.Pun;
using SharpzReborn.Classes;
using SharpzReborn.Extensions;
using SharpzReborn.Notifications;
using System;
using System.Linq;
using UnityEngine;
using static SharpzReborn.Classes.RigManager;
using static SharpzReborn.Menu.Main;
namespace SharpzReborn.Mods
{
    public class Safety
    {
        public static VRRig reportRig;

        public static float antiReportDelay;
        public static void AntiReport(Action<VRRig, Vector3> onReport)
        {
            if (!NetworkSystem.Instance.InRoom) return;

            if (reportRig != null)
            {
                onReport?.Invoke(reportRig, reportRig.transform.position);
                reportRig = null;

                return;
            }

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                Transform report = line.reportButton.gameObject.transform;

                foreach (VRRig vrrig in from vrrig in VRRigCache.m_activeRigs where !vrrig.isLocal let D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position) let D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position) where D1 < 0.35f || D2 < 0.35f select vrrig)
                    onReport?.Invoke(vrrig, report.transform.position);
            }
        }
        public static void AntiReportDisconnect() =>
                AntiReport((vrrig, position) =>
                           {
                               NetworkSystem.Instance.ReturnToSinglePlayer();

                               if (!(Time.time > antiReportDelay)) return;
                               antiReportDelay = Time.time + 1f;
                               NotifiLib.SendNotification($"{ModInfoNotif("ANTI-REPORT")} " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected.");
                           });
        public static void NoFingerMovement()
        {
            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
            ControllerInputPoller.instance.rightControllerGripFloat = 0f;
            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
            ControllerInputPoller.instance.leftControllerPrimaryButton = false;
            ControllerInputPoller.instance.leftControllerSecondaryButton = false;
            ControllerInputPoller.instance.rightControllerPrimaryButton = false;
            ControllerInputPoller.instance.rightControllerSecondaryButton = false;
            ControllerInputPoller.instance.leftControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.leftControllerSecondaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerSecondaryButtonTouch = false;
        }

        public static float flushCooldown;
        public static void FlushRPCs()
        {
            if (Time.time > flushCooldown)
            {
                Menu.Main.RPCProtection();
                flushCooldown = Time.time + 5f;
                return;
            }
            NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not meant to spam Flush RPCs. Only call it once after you are done spamming RPCs.");
        }

        public static void AntiModerator()
        {
            foreach (var vrrig in VRRigExtensions.ActiveRigs.Where(vrrig => !vrrig.isOfflineVRRig && vrrig.Cosmetics().Contains("LBAAK") || vrrig.Cosmetics().Contains("LBAAD") || vrrig.Cosmetics().Contains("LMAPY")))
            {
                try
                {
                    VRRig plr = vrrig;
                    NetPlayer player = GetPlayerFromVRRig(plr);
                }
                catch { }
                NetworkSystem.Instance.ReturnToSinglePlayer();
                NotifiLib.SendNotification($"{ModInfoNotif("ANTI-MODERATOR")} {vrrig.GetName()} is a moderator, you have been disconnected.");
            }
        }
    }
}