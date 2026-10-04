using SharpzReborn.Tools;
using UnityEngine;

namespace SharpzReborn.Classes.Network
{
    public static class NetworkedStupidAssetUtils
    {
        public static GameObject LoadGameObject(string assetName)
        {
            AssetBundle bundle =
                StupidAssetUtils.LoadAssetBundle(assetName);

            if (bundle == null)
                return null;

            GameObject[] assets =
                bundle.LoadAllAssets<GameObject>();

            if (assets.Length == 0)
            {
                bundle.Unload(false);
                return null;
            }

            GameObject obj =
                Object.Instantiate(assets[0]);

            bundle.Unload(false);

            return obj;
        }
    }
}