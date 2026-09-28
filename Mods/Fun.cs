using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using Photon.Pun;
using Photon.Realtime;
using SharpzReborn.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Mods
{
    public class Fun
    {
        public static float delay;
        public static void SendWaterRPC(Vector3 pos, Quaternion rot)
        {
            if (delay < Time.time)
            {
                delay = Time.time + 0.2f;
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", RpcTarget.All, new object[]
                {
                    pos,
                    rot,
                    3f,
                    50f,
                    true,
                    false,
                });
            }
        }

        public static void BetaWaterSplash(
            Vector3 splashPosition,
            Quaternion splashRotation,
            float splashScale = 4f,
            float boundingRadius = 100f,
            bool bigSplash = true,
            bool enteringWater = false,
            object general = null,
            bool bypassDelay = false)
        {
            if (!CanCallWaterSplashNow() && !bypassDelay)
                return;

            try
            {
                general ??= RpcTarget.All;

                object[] parameters =
                {
            splashPosition,
            splashRotation,
            splashScale,
            boundingRadius,
            bigSplash,
            enteringWater
        };

                switch (general)
                {
                    case RpcTarget target:
                        GorillaTagger.Instance.myVRRig.SendRPC(
                            WaterVolume.WaterSplashRPC,
                            target,
                            parameters);
                        break;

                    case NetPlayer player:
                        GorillaTagger.Instance.myVRRig.SendRPC(
                            WaterVolume.WaterSplashRPC,
                            Utilities.RigUtilities.NetPlayerToPlayer(player),
                            parameters);
                        break;

                    case int[] targets:
                        foreach (int target in targets)
                        {
                            GorillaTagger.Instance.myVRRig.SendRPC(
                                WaterVolume.WaterSplashRPC,
                                target,
                                parameters);
                        }
                        break;
                }
            }
            catch
            {
            }
        }

        public static float splashDel;
        public static void WaterSplashHands()
        {
            if (Time.time > splashDel && (rightGrab || leftGrab))
            {
                BetaWaterSplash(rightGrab ? GorillaTagger.Instance.rightHandTransform.position : GorillaTagger.Instance.leftHandTransform.position, rightGrab ? GorillaTagger.Instance.rightHandTransform.rotation : GorillaTagger.Instance.leftHandTransform.rotation);
                splashDel = Time.time + 0.1f;
            }
        }

        public static void GiveWaterSplashHandsGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    if (lockTarget.rightMiddle.calcT > 0.5f || lockTarget.leftMiddle.calcT > 0.5f)
                    {
                        if (Time.time > splashDel)
                        {
                            Vector3 splashPosition = lockTarget.rightMiddle.calcT > 0.5f ? lockTarget.rightHandTransform.position : lockTarget.leftHandTransform.position;
                            Quaternion splashRotation = lockTarget.rightMiddle.calcT > 0.5f ? lockTarget.rightHandTransform.rotation : lockTarget.leftHandTransform.rotation;

                            BetaWaterSplash(splashPosition, splashRotation);
                            splashDel = Time.time + 0.1f;
                        }
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                    gunLocked = false;
            }
        }

        public static bool CanCallWaterSplashNow()
        {
            float time = Time.time;
            int num = -1;
            float num2 = time + 10f;

            for (int i = 0; i < WaterVolume.splashRPCSendTimes.Length; i++)
            {
                if (WaterVolume.splashRPCSendTimes[i] < num2)
                {
                    num2 = WaterVolume.splashRPCSendTimes[i];
                    num = i;
                }
            }

            if (num != -1 && time - 0.5f > num2)
            {
                WaterVolume.splashRPCSendTimes[num] = time;
                return true;
            }
            return false;
        }

        public static void WaterSplashAura() =>
            BetaWaterSplash(VRRig.LocalRig.transform.position + RandomVector3(2f), RandomQuaternion());

        public static void OrbitWaterSplash() =>
            BetaWaterSplash(GorillaTagger.Instance.headCollider.transform.position + new Vector3(MathF.Cos((float)Time.frameCount / 30), 1f, MathF.Sin((float)Time.frameCount / 30)), RandomQuaternion());

        public static void WaterSplashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true))
                    BetaWaterSplash(NewPointer.transform.position, RandomQuaternion());
            }
        }


        public static void WaterSplashAll()
        {
            foreach (VRRig rig in VRRigExtensions.ActiveRigs)
            {
                if (CanCallWaterSplashNow())
                {
                    VRRig.LocalRig.transform.position = rig.transform.position;
                    SendSerialize(VRRig.LocalRig.GetPhotonView(), new RaiseEventOptions { TargetActors = new[] { rig.Creator.ActorNumber } });
                    BetaWaterSplash(rig.head.rigTarget.position, RandomQuaternion(), bypassDelay: true);
                }
            }
        }

        public static void WaterSplashOnTouch()
        {
            foreach (VRRig rig in VRRigExtensions.ActiveRigs)
                if (!rig.IsLocal() && rig.IsBeingTouched())
                    BetaWaterSplash(rig.head.rigTarget.position, RandomQuaternion());
        }

        public static void WaterSplashWalk()
        {
            if (GTPlayer.Instance.IsHandTouching(true))
            {
                RaycastHit ray = GTPlayer.Instance.lastHitInfoHand;
                BetaWaterSplash(GorillaTagger.Instance.leftHandTransform.position, Quaternion.Euler(ray.normal));
            }
            else if (GTPlayer.Instance.IsHandTouching(false))
            {
                RaycastHit ray = GTPlayer.Instance.lastHitInfoHand;
                BetaWaterSplash(GorillaTagger.Instance.rightHandTransform.position, Quaternion.Euler(ray.normal));
            }
        }


        public static void WaterL()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                SendWaterRPC(GTPlayer.Instance.LeftHand.controllerTransform.position, GTPlayer.Instance.LeftHand.controllerTransform.rotation);
            }
        }

        public static void WaterR()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                SendWaterRPC(GTPlayer.Instance.RightHand.controllerTransform.position, GTPlayer.Instance.RightHand.controllerTransform.rotation);
            }
        }

        public static float colorChangerDelay;

        public static int colorChangeType;
        public static bool strobeColor;

        public static void FlashColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.05f;
                strobeColor = !strobeColor;
                ChangeColor(strobeColor ? Color.white : Color.black);
            }
        }

        public static void StrobeColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.05f;
                ChangeColor(RandomColor());
            }
        }

        public static void RainbowColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.05f;
                float h = Time.frameCount / 180f % 1f;
                ChangeColor(Color.HSVToRGB(h, 1f, 1f));
            }
        }

        public static void HardRainbowColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.5f;
                colorChangeType++;
                if (colorChangeType > 3)
                    colorChangeType = 0;

                Color[] colors = {
                    Color.red,
                    Color.green,
                    Color.blue,
                    Color.magenta
                };

                ChangeColor(colors[colorChangeType]);
            }
        }

    }
}
