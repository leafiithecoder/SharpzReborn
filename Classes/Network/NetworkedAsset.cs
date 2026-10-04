using Photon.Pun;
using UnityEngine;

namespace SharpzReborn.Classes.Network
{
    public class NetworkedAsset : NetworkedObject
    {
        public string AssetName { get; private set; }

        public static NetworkedAsset Spawn(
            string assetName,
            int networkId,
            int ownerActor,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            GameObject obj =
                NetworkedStupidAssetUtils.LoadGameObject(assetName);

            if (obj == null)
                return null;

            obj.transform.SetPositionAndRotation(
                position,
                rotation
            );

            obj.transform.localScale = scale;

            NetworkedAsset networkedAsset =
                obj.AddComponent<NetworkedAsset>();

            networkedAsset.AssetName = assetName;

            networkedAsset.InitializeAsset(
                networkId,
                ownerActor
            );

            return networkedAsset;
        }

        public static NetworkedAsset Create(
            string assetName,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            int networkId = GetNextNetworkId();

            NetworkedAsset networkedAsset =
                Spawn(
                    assetName,
                    networkId,
                    PhotonNetwork.LocalPlayer.ActorNumber,
                    position,
                    rotation,
                    scale
                );

            if (networkedAsset == null)
                return null;

            SharpzNetwork.ExecuteCommand(
                "net_asset_spawn",
                Photon.Realtime.ReceiverGroup.Others,
                assetName,
                networkId,
                PhotonNetwork.LocalPlayer.ActorNumber,
                position,
                rotation,
                scale
            );

            return networkedAsset;
        }

        public void InitializeAsset(
            int networkId,
            int ownerActor)
        {
            InitializeNetworked(
                networkId,
                ownerActor
            );
        }

        public void NetworkDestroyAsset()
        {
            if (!initialized || !IsOwner)
                return;

            SharpzNetwork.ExecuteCommand(
                "net_destroy",
                Photon.Realtime.ReceiverGroup.Others,
                NetworkId
            );

            NetworkedObject.Remove(NetworkId);
        }
    }
}