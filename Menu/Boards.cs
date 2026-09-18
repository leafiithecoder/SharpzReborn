using TMPro;
using UnityEngine;

namespace SharpzReborn.Menu
{
    public class Boards
    {
        public static string Title = "Sharpz Reborn";
        public static string MotdSetText = "Welcome to Sharpz Reborn! A menu made by Sharpz. " +
            "This menu is completely free and open sourced, if you paid for this menu you have been <color=red>scammed.</color> " +
            "I am not responsible for any bans using this menu. " +
            "If you get banned while using this, it's your responsibility.\n\n<alpha=128>Made with <3 Sharpz.<alpha=255>\n\n ";

        public static string COCSetText = "Welcome to Sharpz Reborn! A menu made by Sharpz. " +
        "This menu is completely free and open sourced, if you paid for this menu you have been <color=red>scammed.</color> " +
        "I am not responsible for any bans using this menu. " +
        "If you get banned while using this, it's your responsibility.\n\n<alpha=128>Made with <3 Sharpz.<alpha=255>\n\n ";

        private static TMP_Text MotdText;
        private static TMP_Text MotdBodyText;
        private static TMP_Text COCText;
        private static TMP_Text COCBodyText;

        private static GameObject MotdTitleObject;
        private static GameObject MotdBodyObject;

        private static readonly string local = "Environment Objects/LocalObjects_Prefab/TreeRoom/";

        public static void DoBoards()
        {
            GameObject motdHeading = GameObject.Find(local + "motdHeadingText");
            GameObject motdBody = GameObject.Find(local + "motdBodyText");

            GameObject cocHeading = GameObject.Find(local + "CodeOfConductHeadingText");
            GameObject cocBody = GameObject.Find(local + "COCBodyText_TitleData");

            MotdTitleObject = Object.Instantiate(motdHeading, motdHeading.transform.parent);
            MotdBodyObject = Object.Instantiate(motdBody, motdBody.transform.parent);

            motdHeading.SetActive(false);
            motdBody.SetActive(false);

            MotdBodyObject.GetComponent<PlayFabTitleDataTextDisplay>().enabled = false;

            MotdText = MotdTitleObject.GetComponent<TMP_Text>();
            MotdBodyText = MotdBodyObject.GetComponent<TMP_Text>();
            COCText = cocHeading.GetComponent<TMP_Text>();
            COCBodyText = cocBody.GetComponent<TMP_Text>();
            COCBodyText.richText = true;

            ApplyTexts();
        }

        public static void UpdateBoards()
        {
            if (MotdText == null || MotdBodyText == null)
                return;

            MotdText.text = Title;
            MotdBodyText.text = MotdSetText;
            COCText.text = Title;
            COCBodyText.text = COCSetText;
        }

        private static void ApplyTexts()
        {
            MotdText.text = Title;
            MotdBodyText.text = MotdSetText;
            COCText.text = Title;
            COCBodyText.text = COCSetText;
        }
    }
}