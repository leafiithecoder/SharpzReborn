using Photon.Pun;
using SharpzReborn.Extensions;
using SharpzReborn.Menu;
using SharpzReborn.Mods;
using SharpzReborn.Tools;
using SharpzReborn.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Managers
{
    public class AdminManager
    {
        public static readonly HashSet<string> admins = new();

        private static readonly Dictionary<VRRig, GameObject> adminWatermarks = new();

        private static Texture2D watermarkTexture;
        private static Material watermarkMaterial;
        private static bool givenAdminPanel;

        public static IEnumerator WaitForAdminPanel()
        {
            while (!givenAdminPanel)
            {
                if (PhotonNetwork.LocalPlayer != null &&
                    !string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.UserId))
                {
                    CheckAdminPanel();
                    yield break;
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        public static bool IsAdmin(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            return admins.Contains(userId);
        }

        public static bool IsLocalAdmin(string userId)
        {
            return IsAdmin(userId) &&
                   userId == PhotonNetwork.LocalPlayer.UserId;
        }

        public static bool ShouldShowWatermark(string userId)
        {
            if (!IsAdmin(userId))
                return false;

            if (userId == PhotonNetwork.LocalPlayer.UserId)
                return showLocalAdminIcon;

            return true;
        }

        public static void SetAdmins(HashSet<string> newAdmins)
        {
            admins.Clear();

            foreach (string userId in newAdmins)
                admins.Add(userId);
        }

        public static void LoadWatermark()
        {
            watermarkTexture ??=
                StupidAssetUtils.LoadTexture2D("watermark.png");

            if (watermarkTexture == null)
            {
                Debug.LogError("Sharpz Reborn // Failed to load watermark.png.");
                return;
            }

            if (watermarkMaterial == null)
            {
                Shader shader = Shader.Find("Sprites/Default");

                if (shader == null)
                {
                    Debug.LogError("Sharpz Reborn // Failed to find watermark shader.");
                    return;
                }

                watermarkMaterial = new Material(shader)
                {
                    mainTexture = watermarkTexture,
                    color = Color.white
                };
            }
        }

        public static void FindAdmins()
        {
            if (!NetworkSystem.Instance.InRoom)
            {
                ClearWatermarks();
                return;
            }

            List<VRRig> toRemove = new();

            foreach (var watermark in adminWatermarks)
            {
                VRRig rig = watermark.Key;

                if (rig == null)
                {
                    Object.Destroy(watermark.Value);
                    toRemove.Add(rig);
                    continue;
                }

                var player = rig.Creator?.GetPlayerRef();

                if (player == null || !ShouldShowWatermark(player.UserId))
                {
                    Object.Destroy(watermark.Value);
                    toRemove.Add(rig);
                }
            }

            foreach (VRRig rig in toRemove)
                adminWatermarks.Remove(rig);

            foreach (var netplr in NetworkSystem.Instance.PlayerListOthers)
            {
                if (!ShouldShowWatermark(netplr.UserId))
                    continue;

                VRRig adminRig =
                    RigUtilities.GetVRRigFromPlayer(netplr);

                if (adminRig == null)
                    continue;

                ShowWatermark(adminRig);
            }

            if (showLocalAdminIcon &&
                IsAdmin(PhotonNetwork.LocalPlayer.UserId))
            {
                foreach (VRRig rig in VRRigExtensions.ActiveRigs)
                {
                    if (rig.isLocal)
                    {
                        ShowWatermark(rig);
                        break;
                    }
                }
            }
        }

        public static void ClearWatermarks()
        {
            foreach (GameObject watermark in adminWatermarks.Values)
            {
                if (watermark != null)
                    Object.Destroy(watermark);
            }

            adminWatermarks.Clear();
        }

        public static void ShowWatermark(VRRig adminRig)
        {
            if (adminRig == null)
                return;

            if (!adminWatermarks.TryGetValue(
                adminRig,
                out GameObject watermark))
            {
                watermark =
                    new GameObject("SharpzReborn_AdminWatermark");

                GameObject front =
                    GameObject.CreatePrimitive(PrimitiveType.Quad);

                front.name = "Front";

                GameObject back =
                    GameObject.CreatePrimitive(PrimitiveType.Quad);

                back.name = "Back";

                Object.Destroy(front.GetComponent<Collider>());
                Object.Destroy(back.GetComponent<Collider>());

                front.transform.SetParent(
                    watermark.transform,
                    false);

                back.transform.SetParent(
                    watermark.transform,
                    false);

                back.transform.localRotation =
                    Quaternion.Euler(0f, 180f, 0f);

                front.GetComponent<Renderer>().sharedMaterial =
                    watermarkMaterial;

                back.GetComponent<Renderer>().sharedMaterial =
                    watermarkMaterial;

                adminWatermarks.Add(
                    adminRig,
                    watermark);
            }

            Transform head =
                Visuals.GetNameTagTransform(adminRig);

            watermark.transform.position =
                head.position +
                Vector3.up * (0.6f * adminRig.scaleFactor);

            watermark.transform.Rotate(
                Vector3.up,
                90f * Time.deltaTime,
                Space.World
            );

            watermark.transform.localScale =
                Vector3.one *
                (0.25f * adminRig.scaleFactor);
        }

        public static void CheckAdminPanel()
        {
            if (givenAdminPanel)
                return;

            if (PhotonNetwork.LocalPlayer == null)
                return;

            string userId = PhotonNetwork.LocalPlayer.UserId;

            if (string.IsNullOrEmpty(userId))
                return;

            Debug.Log($"SharpzReborn // Admin UserId: {userId}");

            if (!IsAdmin(userId))
                return;

            givenAdminPanel = true;

            Debug.Log("SharpzReborn // Local player is an admin.");

            Debug.Log("SharpzReborn // Giving admin panel.");

            SetupAdminPanel();
        }
    }
}