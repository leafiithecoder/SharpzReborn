using TMPro;
using UnityEngine;

namespace SharpzReborn.Menu
{
    public enum BoardType
    {
        MOTDTitle,
        MOTDDesc,
        COCTitle,
        COCDesc
    }

    public class Boards
    {
        public static string Title = "Sharpz Reborn";

        public static string MOTDDefault = "Welcome to Sharpz Reborn! A menu made by Sharpz.\n " +
            "This menu is completely free and open sourced, if you paid for this menu you have been <color=red>scammed.</color>\n" +
            $"There are a total of <color=purple>{Buttons.ButtonCount}</color> mods on this menu.\n" +
            "I am not responsible for any bans using this menu.\n" +
            "If you get banned while using this, it's your responsibility.\n\n<alpha=128>Made with <3 by Sharpz.<alpha=255>\n\n ";

        public static string COCDefault = "Welcome to Sharpz Reborn! A Gorilla Tag menu made by Sharpz.\n" +
            "Suspected Detected Mods:\n" +
            "- Delay Ban Gun\n" +
            "\n" +
            "Meanings:\n" +
            "[<color=purple>M</color>] Requires Master\n" +
            "[<color=purple>M?</color>] Maybe Requires Master\n" +
            "[<color=purple>D</color>] Detected\n" +
            "[<color=purple>D</color>] Maybe Detected \n" +
            "[<color=purple>KB</color>] Half Broken \n" +
            "[<color=purple>BK</color>] Broken \n" +
            "[<color=purple>CS</color>] Client Sided \n" +
            "[<color=purple>CS?</color>] Client sided, but depends \n" +
            "[<color=purple>SS</color>] Server Sided \n";

        public static string MotdSetText = MOTDDefault;
        public static string COCSetText = COCDefault;

        private static readonly string local = "Environment Objects/LocalObjects_Prefab/TreeRoom/";

        public static TMP_Text MotdText;
        public static TMP_Text MotdBodyText;
        public static TMP_Text COCText;
        public static TMP_Text COCBodyText;

        public static void SetLaunchBoards()
        {
            GameObject motdHeading = GameObject.Find(local + "motdHeadingText");
            GameObject motdBody = GameObject.Find(local + "motdBodyText");

            GameObject cocHeading = GameObject.Find(local + "CodeOfConductHeadingText");
            GameObject cocBody = GameObject.Find(local + "COCBodyText_TitleData");

            GameObject MotdTitleObject = Object.Instantiate(
                motdHeading,
                motdHeading.transform.parent
            );

            GameObject MotdBodyObject = Object.Instantiate(
                motdBody,
                motdBody.transform.parent
            );

            motdHeading.SetActive(false);
            motdBody.SetActive(false);

            MotdBodyObject.GetComponent<PlayFabTitleDataTextDisplay>().enabled = false;

            MotdText = MotdTitleObject.GetComponent<TMP_Text>();
            MotdBodyText = MotdBodyObject.GetComponent<TMP_Text>();
            COCText = cocHeading.GetComponent<TMP_Text>();
            COCBodyText = cocBody.GetComponent<TMP_Text>();

            COCBodyText.richText = true;
        }

        public static void SetBoard(BoardType boardType, string text)
        {
            switch (boardType)
            {
                case BoardType.MOTDTitle:
                    Title = text;
                    break;

                case BoardType.MOTDDesc:
                    MotdSetText = text;
                    break;

                case BoardType.COCTitle:
                    Title = text;
                    break;

                case BoardType.COCDesc:
                    COCSetText = text;
                    break;
            }
        }

        public static void UpdateBoards()
        {
            if (MotdText == null || MotdBodyText == null ||
                COCText == null || COCBodyText == null)
                return;

            MotdText.text = Title;
            MotdBodyText.text = MotdSetText;
            COCText.text = Title;
            COCBodyText.text = COCSetText;
        }

        public static void UpdateCOC()
        {
            if (COCBodyText == null)
                return;

            COCBodyText.text = COCSetText;
        }

        public static void ResetBoards()
        {
            MotdSetText = MOTDDefault;
            COCSetText = COCDefault;
            UpdateBoards();
        }
    }
}