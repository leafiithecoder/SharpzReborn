using static SharpzReborn.Menu.Main;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharpzReborn.Mods
{
    public class Player
    {
        public static void GrabRig()
        {
            if (rightGrab)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }
            else
            {
                VRRig.LocalRig.enabled = true;
            }
        }

        private static bool ghostMonke;
        private static bool lastHit;

        public static void Ghost()
        {
            bool hit = rightPrimary || Mouse.current.leftButton.isPressed;

            VRRig.LocalRig.enabled = !ghostMonke;

            if (hit && !lastHit)
                ghostMonke = !ghostMonke;

            lastHit = hit;
        }

        private static bool wasDisabledAlready;
        private static bool invisMonke;
        private static bool lastHit2;

        public static void Invisible()
        {
            bool hit = rightSecondary || Mouse.current.rightButton.isPressed;

            if (invisMonke)
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position - Vector3.up * 99999f;
            }

            if (hit && !lastHit2)
            {
                invisMonke = !invisMonke;

                if (invisMonke)
                    wasDisabledAlready = VRRig.LocalRig.enabled;
                else
                    VRRig.LocalRig.enabled = wasDisabledAlready;
            }

            lastHit2 = hit;
        }

        private static readonly float headSpinSpeed = 10f;
        public static void SpinHead(string axis)
        {
            if (VRRig.LocalRig.enabled)
            {
                switch (axis.ToLower())
                {
                    case "x":
                        VRRig.LocalRig.head.trackingRotationOffset.x += headSpinSpeed;
                        break;
                    case "y":
                        VRRig.LocalRig.head.trackingRotationOffset.y += headSpinSpeed;
                        break;
                    case "z":
                        VRRig.LocalRig.head.trackingRotationOffset.z += headSpinSpeed;
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (axis.ToLower())
                {
                    case "x":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(headSpinSpeed, 0f, 0f));
                        break;
                    case "y":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, headSpinSpeed, 0f));
                        break;
                    case "z":
                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, 0f, headSpinSpeed));
                        break;
                    default:
                        return;
                }
            }
        }

        public static void SpazHead(string axis)
        {
            int offset = UnityEngine.Random.Range(0, 360);
            switch (axis.ToLower())
            {
                case "x":
                    VRRig.LocalRig.head.trackingRotationOffset.x = offset;
                    break;
                case "y":
                    VRRig.LocalRig.head.trackingRotationOffset.y = offset;
                    break;
                case "z":
                    VRRig.LocalRig.head.trackingRotationOffset.z = offset;
                    break;
                default:
                    return;
            }
        }
        public static void SpazHeadXYZ()
        {
            if (VRRig.LocalRig.enabled)
            {
                VRRig.LocalRig.head.trackingRotationOffset.x = UnityEngine.Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.y = UnityEngine.Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.z = UnityEngine.Random.Range(0f, 360f);
            }
            else
                VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
        }

        public static void FixHead()
        {
            VRRig.LocalRig.head.trackingRotationOffset.x = 0f;
            VRRig.LocalRig.head.trackingRotationOffset.y = 0f;
            VRRig.LocalRig.head.trackingRotationOffset.z = 0f;
            VRRig.LocalRig.head.rigTarget.transform.localScale = Vector3.one;
        }

        public static void UpsideDownHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.z = 180f;

        public static void BrokenNeck() =>
            VRRig.LocalRig.head.trackingRotationOffset.z = 90f;

        public static void BackwardsHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.y = 180f;

        public static void SidewaysHead() =>
            VRRig.LocalRig.head.trackingRotationOffset.y = 90f;

        public static void BigHead() =>
            VRRig.LocalRig.head.rigTarget.transform.localScale = Vector3.one * 2f;

        public static void SmallHead() =>
            VRRig.LocalRig.head.rigTarget.transform.localScale = Vector3.one * 0.3f;

        public static float lastBangTime;
        public static void HeadBang()
        {
            if (Time.time > lastBangTime)
            {
                VRRig.LocalRig.head.trackingRotationOffset.x = 50f;
                lastBangTime = Time.time + 60f / 159f;
            }
            else
                VRRig.LocalRig.head.trackingRotationOffset.x = Mathf.Lerp(VRRig.LocalRig.head.trackingRotationOffset.x, 0f, 0.1f);
        }
    }
}
