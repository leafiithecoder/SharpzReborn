using Photon.Pun;
using SharpzReborn.Classes.Network;
using System.Collections.Generic;
using UnityEngine;

namespace SharpzReborn.Classes
{
    public class NetworkedObject : MonoBehaviour
    {
        private static readonly Dictionary<int, NetworkedObject> Objects = new();

        private static int nextLocalId = 1;

        public int NetworkId { get; private set; }

        public int OwnerActor { get; private set; }

        public PrimitiveType PrimitiveType { get; private set; }

        public bool IsOwner =>
            OwnerActor == PhotonNetwork.LocalPlayer.ActorNumber;

        protected bool initialized;

        public static NetworkedObject Create(
            PrimitiveType primitiveType,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            int networkId = GetNextNetworkId();

            GameObject obj = GameObject.CreatePrimitive(primitiveType);

            obj.transform.SetPositionAndRotation(
                position,
                rotation
            );

            obj.transform.localScale = scale;

            NetworkedObject networkedObject =
                obj.AddComponent<NetworkedObject>();

            networkedObject.Initialize(
                networkId,
                PhotonNetwork.LocalPlayer.ActorNumber,
                primitiveType
            );

            SharpzNetwork.ExecuteCommand(
                "net_spawn",
                Photon.Realtime.ReceiverGroup.Others,
                networkId,
                PhotonNetwork.LocalPlayer.ActorNumber,
                (byte)primitiveType,
                position,
                rotation,
                scale
            );

            return networkedObject;
        }

        public void Initialize(
            int networkId,
            int ownerActor,
            PrimitiveType primitiveType)
        {
            PrimitiveType = primitiveType;

            InitializeNetworked(
                networkId,
                ownerActor
            );
        }

        protected void InitializeNetworked(
            int networkId,
            int ownerActor)
        {
            NetworkId = networkId;
            OwnerActor = ownerActor;

            Objects[NetworkId] = this;

            initialized = true;
        }

        public void NetworkUpdate()
        {
            if (!initialized || !IsOwner)
                return;

            SharpzNetwork.ExecuteCommand(
                "net_update",
                Photon.Realtime.ReceiverGroup.Others,
                NetworkId,
                transform.position,
                transform.rotation,
                transform.localScale
            );
        }

        public void NetworkDestroy()
        {
            if (!initialized || !IsOwner)
                return;

            SharpzNetwork.ExecuteCommand(
                "net_destroy",
                Photon.Realtime.ReceiverGroup.Others,
                NetworkId
            );

            Remove(NetworkId);
        }

        private void OnDestroy()
        {
            if (NetworkId != 0)
                Objects.Remove(NetworkId);
        }

        public static NetworkedObject Get(int networkId)
        {
            Objects.TryGetValue(
                networkId,
                out NetworkedObject obj
            );

            return obj;
        }

        public static void Remove(int networkId)
        {
            if (!Objects.TryGetValue(
                networkId,
                out NetworkedObject obj))
                return;

            Objects.Remove(networkId);

            if (obj != null)
                UnityEngine.Object.Destroy(obj.gameObject);
        }

        protected static int GetNextNetworkId()
        {
            while (Objects.ContainsKey(nextLocalId))
                nextLocalId++;

            return nextLocalId++;
        }

        private float updateTimer;

        public float updateRate = 0.05f;

        private void Update()
        {
            if (!initialized || !IsOwner)
                return;

            updateTimer += Time.deltaTime;

            if (updateTimer < updateRate)
                return;

            updateTimer = 0f;

            NetworkUpdate();
        }

        public static void SyncToPlayer(Photon.Realtime.Player player)
        {
            foreach (NetworkedObject obj in Objects.Values)
            {
                if (obj == null || !obj.initialized)
                    continue;

                if (!obj.IsOwner)
                    continue;

                SharpzNetwork.ExecuteCommand(
                    "net_spawn",
                    new Photon.Realtime.RaiseEventOptions
                    {
                        TargetActors = new[] { player.ActorNumber }
                    },
                    obj.NetworkId,
                    obj.OwnerActor,
                    (byte)obj.PrimitiveType,
                    obj.transform.position,
                    obj.transform.rotation,
                    obj.transform.localScale
                );
            }
        }
    }
}