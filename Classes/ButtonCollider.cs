using UnityEngine;
using static SharpzReborn.Menu.Main;

namespace SharpzReborn.Classes
{
    public class Button : MonoBehaviour
    {

        public static float      buttonCooldown;
        public        bool       incremental;
        public        bool       positive;
        public        ButtonInfo relatedButton;

        public void OnTriggerEnter(Collider collider)
        {
            if (Time.time     <= buttonCooldown ||
                menu          == null           ||
                relatedButton == null)
                return;

            if (!TryGetReferenceHand(
                        collider,
                        out bool pressingRightHand))
                return;

            buttonCooldown =
                    Time.time +
                    0.2f;

            GorillaTagger.Instance.StartVibration(
                    pressingRightHand,
                    GorillaTagger.Instance.tagHapticStrength / 2f,
                    GorillaTagger.Instance.tagHapticDuration / 2f);

            VRRig.LocalRig.PlayHandTapLocal(
                    8,
                    pressingRightHand,
                    0.4f);

            bool? incrementDirection =
                    incremental
                            ? positive
                            : (bool?)null;

            Toggle(
                    relatedButton,
                    incrementDirection);
        }
    }
}