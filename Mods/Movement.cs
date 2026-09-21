using GorillaLocomotion;
using SharpzReborn.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static SharpzReborn.Menu.Main;
using static SharpzReborn.Menu.Buttons;
using SharpzReborn.Menu;

namespace SharpzReborn.Mods;

public abstract class Movement
{

    private static GameObject platl;
    private static GameObject platr;


    public static bool previousTeleportTrigger;
    public static void Fly()
    {
        if (!ControllerInputPoller.instance.rightControllerPrimaryButton)
            return;

        GTPlayer.Instance.transform.position            += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
        GorillaTagger.Instance.rigidbody.linearVelocity =  Vector3.zero;
    }

    public static void TriggerFly()
    {
        if (rightTriggerPressed)
        {
            GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
        }
    }

    public static void HandFly()
    {
        if (rightPrimary)
        {
            GTPlayer.Instance.transform.position += TrueRightHand().forward * (Time.deltaTime * Settings.Movement.flySpeed);
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
        }
    }

    public static bool noclip;
    public static void NoclipFly()
    {
        if (rightPrimary)
        {
            GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * (Time.deltaTime * Settings.Movement.flySpeed);
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            if (!noclip)
            {
                noclip = true;
                UpdateClipColliders(false);
            }
        }
        else
        {
            if (noclip)
            {
                noclip = false;
                UpdateClipColliders(true);
            }
        }
    }

    public static void Platforms()
    {
        bool leftGrip =
                ControllerInputPoller.instance.leftGrab;

        bool rightGrip =
                ControllerInputPoller.instance.rightGrab;

        if (leftGrip)
        {
            if (platl == null)
            {
                platl =
                        GameObject.CreatePrimitive(
                                PrimitiveType.Cube);

                platl.transform.localScale =
                        new Vector3(
                                0.025f,
                                0.3f,
                                0.4f);

                platl.transform.position =
                        TrueLeftHand().position + TrueLeftHand().right * 0.05f;

                platl.transform.rotation =
                        TrueLeftHand().rotation;

                FixStickyColliders(
                        platl);

                ColorChanger colorChanger =
                        platl.AddComponent<ColorChanger>();

                colorChanger.colors =
                        Menu.Settings.backgroundColor;
            }
        }
        else if (platl != null)
        {
            Object.Destroy(
                    platl);

            platl =
                    null;
        }

        if (rightGrip)
        {
            if (platr == null)
            {
                platr =
                        GameObject.CreatePrimitive(
                                PrimitiveType.Cube);

                platr.transform.localScale =
                        new Vector3(
                                0.025f,
                                0.3f,
                                0.4f);

                platr.transform.position =
                        TrueRightHand().position - TrueRightHand().right * 0.05f;

                platr.transform.rotation =
                        TrueRightHand().rotation;
                
                FixStickyColliders(
                        platr);

                ColorChanger colorChanger =
                        platr.AddComponent<ColorChanger>();

                colorChanger.colors =
                        Menu.Settings.backgroundColor;
            }
        }
        else if (platr != null)
        {
            Object.Destroy(
                    platr);

            platr =
                    null;
        }
    }
    public static void TeleportGun()
    {
        if (!ControllerInputPoller.instance.rightGrab)
            return;

        (RaycastHit Ray, GameObject NewPointer) GunData    = RenderGun();
        GameObject                              NewPointer = GunData.NewPointer;

        if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && !previousTeleportTrigger)
        {
            GTPlayer.Instance.TeleportTo(NewPointer.transform.position + Vector3.up, GTPlayer.Instance.transform.rotation);
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
        }

        previousTeleportTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;
    }


    public static void SpeedBoost()
    {
        bool hasControl = gripSpeedBoost || triggerSpeedBoost;

        if (!hasControl)
        {
            GTPlayer.Instance.maxJumpSpeed = Settings.Movement.speedAmount;
            GTPlayer.Instance.jumpMultiplier = Settings.Movement.speedMultiplier;
            return;
        }

        if (gripSpeedBoost && (rightGrab || leftGrab))
        {
            GTPlayer.Instance.maxJumpSpeed = Settings.Movement.speedAmount;
            GTPlayer.Instance.jumpMultiplier = Settings.Movement.speedMultiplier;
            return;
        }

        if (triggerSpeedBoost && (rightTriggerPressed || leftTriggerPressed))
        {
            GTPlayer.Instance.maxJumpSpeed = Settings.Movement.speedAmount;
            GTPlayer.Instance.jumpMultiplier = Settings.Movement.speedMultiplier;
            return;
        }
    }
    public static void UpdateClipColliders(bool enabled)
    {
        foreach (MeshCollider v in Resources.FindObjectsOfTypeAll<MeshCollider>())
            v.enabled = enabled;
    }
    public static void Noclip()
    {
        bool gripNoclip = GetIndex("Grip Noclip").enabled;
        if (gripNoclip ? rightGrab : rightTrigger > 0.5f || GetIndex("Constant Noclip").enabled)
        {
            if (!noclip)
            {
                noclip = true;
                UpdateClipColliders(false);
            } 
        }
        else
        {
            if (noclip)
            {
                noclip = false;
                UpdateClipColliders(true);
            }
        }
    }
    public static void EnableSteamLongArms() =>
        GTPlayer.Instance.transform.localScale = Vector3.one * VRRig.LocalRig.NativeScale * Settings.Movement.armLength;
    public static void DisableSteamLongArms() =>
        GTPlayer.Instance.transform.localScale = Vector3.one * VRRig.LocalRig.NativeScale;
    public static GameObject stickpart;
    public static void StickyHands()
    {
        if (stickpart == null)
        {
            stickpart = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            FixStickyColliders(stickpart);
            stickpart.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            stickpart.GetComponent<Renderer>().enabled = false;
        }
        if (GTPlayer.Instance.IsHandTouching(true))
            stickpart.transform.position = TrueLeftHand().position;

        if (GTPlayer.Instance.IsHandTouching(false))
            stickpart.transform.position = TrueRightHand().position;

        if (GTPlayer.Instance.IsHandTouching(true) && GTPlayer.Instance.IsHandTouching(false))
            stickpart.transform.position = Vector3.zero;
    }
    public static void DisableStickyHands()
    {
        if (stickpart != null)
        {
            Object.Destroy(stickpart);
            stickpart = null;
        }
    }
    public static float oldSlide;
    public static void EnableSlideControl()
    {
        oldSlide = GTPlayer.Instance.slideControl;
        GTPlayer.Instance.slideControl = 1f;
    }
    public static void DisableSlideControl() =>
        GTPlayer.Instance.slideControl = oldSlide;
    public static void LowGravity() =>
        GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * 6.66f, ForceMode.Acceleration);
    public static void ZeroGravity() =>
        GorillaTagger.Instance.rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
    public static void HighGravity() =>
        GorillaTagger.Instance.rigidbody.AddForce(Vector3.down * 7.77f, ForceMode.Acceleration);
    public static void UpAndDown()
    {
        if (rightTrigger > 0.5f || rightGrab)
            ZeroGravity();

        if (rightTrigger > 0.5f)
            GorillaTagger.Instance.rigidbody.linearVelocity += Vector3.up * (Time.deltaTime * Settings.Movement.flySpeed * 3f);

        if (rightGrab)
            GorillaTagger.Instance.rigidbody.linearVelocity += Vector3.up * (Time.deltaTime * Settings.Movement.flySpeed * -3f);
    }
    public static void LeftAndRight()
    {
        if (rightTrigger > 0.5f || rightGrab)
            ZeroGravity();

        if (rightTrigger > 0.5f)
            GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.bodyCollider.transform.right * (Time.deltaTime * Settings.Movement.flySpeed * -3f);

        if (rightGrab)
            GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.bodyCollider.transform.right * (Time.deltaTime * Settings.Movement.flySpeed * 3f);
    }
    public static void ForwardsAndBackwards()
    {
        if (rightTrigger > 0.5f || rightGrab)
            ZeroGravity();

        if (rightTrigger > 0.5f)
            GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.bodyCollider.transform.forward * (Time.deltaTime * Settings.Movement.flySpeed * 3f);

        if (rightGrab)
            GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.bodyCollider.transform.forward * (Time.deltaTime * Settings.Movement.flySpeed * -3f);
    }
    public static void ForceTagFreeze() =>
        GTPlayer.Instance.disableMovement = true;
    public static void NoTagFreeze() =>
        GTPlayer.Instance.disableMovement = false;

    public static float startX = -1f;
    public static float startY = -1f;

    public static float subThingy;
    public static float subThingyZ;

    public static Vector3 lastPosition = Vector3.zero;

    public static void EnableWASDFly()
    {
        lastPosition = GorillaTagger.Instance.rigidbody.transform.position;

        if (XRSettings.isDeviceActive && Time.time < timeMenuStarted + 1f)
            Toggle("WASD Fly");
    }

    public static void WASDFly()
    {
        if (Keyboard.current == null || Mouse.current == null)
            return;

        ZeroGravity();

        bool W = Keyboard.current.wKey.isPressed;
        bool A = Keyboard.current.aKey.isPressed;
        bool S = Keyboard.current.sKey.isPressed;
        bool D = Keyboard.current.dKey.isPressed;
        bool Space = Keyboard.current.spaceKey.isPressed;
        bool Ctrl = Keyboard.current.leftCtrlKey.isPressed;
        bool Shift = Keyboard.current.leftShiftKey.isPressed;
        bool Alt = Keyboard.current.leftAltKey.isPressed;

        bool LeftArrow = Keyboard.current.leftArrowKey.isPressed;
        bool RightArrow = Keyboard.current.rightArrowKey.isPressed;
        bool UpArrow = Keyboard.current.upArrowKey.isPressed;
        bool DownArrow = Keyboard.current.downArrowKey.isPressed;

        if (!menu)
        {
            Transform parentTransform = GTPlayer.Instance.GetControllerTransform(false).parent;

            float turnSpeed = 250f;

            if (LeftArrow)
                parentTransform.eulerAngles += new Vector3(0f, -turnSpeed, 0f) * Time.deltaTime;

            if (RightArrow)
                parentTransform.eulerAngles += new Vector3(0f, turnSpeed, 0f) * Time.deltaTime;

            if (UpArrow)
                parentTransform.eulerAngles += new Vector3(-turnSpeed, 0f, 0f) * Time.deltaTime;

            if (DownArrow)
                parentTransform.eulerAngles += new Vector3(turnSpeed, 0f, 0f) * Time.deltaTime;

            if (Mouse.current.rightButton.isPressed)
            {
                Quaternion currentRotation = parentTransform.rotation;
                Vector3 euler = currentRotation.eulerAngles;

                if (startX < 0)
                {
                    startX = euler.y;
                    subThingy = Mouse.current.position.value.x / Screen.width;
                }

                if (startY < 0)
                {
                    startY = euler.x;
                    subThingyZ = Mouse.current.position.value.y / Screen.height;
                }

                float newX = startY - (Mouse.current.position.value.y / Screen.height - subThingyZ) * 360f * 1.33f;
                float newY = startX + (Mouse.current.position.value.x / Screen.width - subThingy) * 360f * 1.33f;

                newX = newX > 180f ? newX - 360f : newX;
                newX = Mathf.Clamp(newX, -90f, 90f);

                parentTransform.rotation = Quaternion.Euler(newX, newY, euler.z);
            }
            else
            {
                startX = -1f;
                startY = -1f;
            }

            float speed = Settings.Movement.flySpeed;

            if (Shift)
                speed *= 2f;
            else if (Alt)
                speed /= 2f;

            float yaw = parentTransform.eulerAngles.y;

            Vector3 forward = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0f, yaw, 0f) * Vector3.right;

            if (W)
                GorillaTagger.Instance.rigidbody.transform.position += forward * (Time.deltaTime * speed);

            if (S)
                GorillaTagger.Instance.rigidbody.transform.position -= forward * (Time.deltaTime * speed);

            if (A)
                GorillaTagger.Instance.rigidbody.transform.position -= right * (Time.deltaTime * speed);

            if (D)
                GorillaTagger.Instance.rigidbody.transform.position += right * (Time.deltaTime * speed);

            if (Space)
                GorillaTagger.Instance.rigidbody.transform.position += Vector3.up * (Time.deltaTime * speed);

            if (Ctrl)
                GorillaTagger.Instance.rigidbody.transform.position += Vector3.down * (Time.deltaTime * speed);

            VRRig.LocalRig.head.rigTarget.transform.rotation =
                GorillaTagger.Instance.headCollider.transform.rotation;
        }

        if (!W && !A && !S && !D && !Space && !Ctrl && lastPosition != Vector3.zero)
            GorillaTagger.Instance.rigidbody.transform.position = lastPosition;
        else
            lastPosition = GorillaTagger.Instance.rigidbody.transform.position;
    }
    private static Vector3 walkPos;
    private static Vector3 walkNormal;
    private static float wallWalkStrength = 9.81f;

    public static void WallWalk()
    {
        if (GTPlayer.Instance.IsHandTouching(true) || GTPlayer.Instance.IsHandTouching(false))
        {
            RaycastHit ray = GTPlayer.Instance.lastHitInfoHand;
            walkPos = ray.point;
            walkNormal = ray.normal;
        }

        bool wallWalkKey = rightGrab || leftGrab;

        if (walkPos != Vector3.zero && wallWalkKey)
        {
            GorillaTagger.Instance.rigidbody.AddForce(walkNormal * -wallWalkStrength, ForceMode.Acceleration);
            ZeroGravity();
        }
    }
}