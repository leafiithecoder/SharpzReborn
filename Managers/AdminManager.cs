using Photon.Pun;
using SharpzReborn.Mods;
using SharpzReborn.Tools;
using SharpzReborn.Utilities;
using System.Collections.Generic;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Managers
{
    public class AdminManager
    {
        private static readonly Dictionary<VRRig, GameObject> adminWatermarks = new();

        private static Texture2D watermarkTexture;
        private static Material watermarkMaterial;

        public static bool IsAdmin(string userId)
        {
            return admins.Contains(userId);
        }

        public static bool IsLocalAdmin(string userId)
        {
            return IsAdmin(userId) && userId == PhotonNetwork.LocalPlayer.UserId;
        }

        public static bool ShouldShowWatermark(string userId)
        {
            return IsAdmin(userId) && !IsLocalAdmin(userId);
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

            if (watermarkTexture == null || watermarkMaterial == null)
                LoadWatermark();

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

                VRRig adminRig = RigUtilities.GetVRRigFromPlayer(netplr);

                if (adminRig == null)
                    continue;

                if (!adminWatermarks.TryGetValue(adminRig, out GameObject watermark))
                {
                    watermark = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    watermark.name = "SharpzReborn_AdminWatermark";

                    Object.Destroy(watermark.GetComponent<Collider>());

                    watermark.GetComponent<Renderer>().sharedMaterial =
                        watermarkMaterial;

                    adminWatermarks.Add(adminRig, watermark);
                }

                Transform head = Visuals.GetNameTagTransform(adminRig);

                watermark.transform.position =
                    head.position +
                    head.up * (0.70f * adminRig.scaleFactor);

                float aspect =
                    watermarkTexture.width /
                    (float)watermarkTexture.height;

                const float height = 0.25f;

                watermark.transform.localScale =
                    new Vector3(
                        height * aspect,
                        height,
                        1f);

                watermark.transform.LookAt(
                    GorillaTagger.Instance.headCollider.transform.position
                );
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
    }
}