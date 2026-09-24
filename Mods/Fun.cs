using GorillaLocomotion;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Mods
{
    public class Fun
    {
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
        public static float splashDel;
        public static void GiveWaterBendingGun()
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

                            SendWaterRPC(splashPosition, splashRotation);
                            splashDel = Time.time + 0.1f;
                        }
                    }
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                    if (gunTarget && !gunTarget.isLocal)
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


        public static void WaterSplashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true) && Time.time > splashDel)
                {
                    splashDel = Time.time + 0.1f;
                    SendWaterRPC(NewPointer.transform.position, RandomQuaternion());
                }
            }
        }
    }
}
