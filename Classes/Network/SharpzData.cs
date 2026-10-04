using PlayFab.ClientModels;
using SharpzReborn.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.Newtonsoft.Json.Linq;

namespace SharpzReborn.Classes.Network
{
    public static class SharpzServerData
    {

        private const string AdminDataUrl =
            "https://raw.githubusercontent.com/leafiithecoder/SharpzData/main/admins.json";

        public static bool Loaded { get; private set; }

        public static void Load()
        {
            CoroutineManager.Instance.StartCoroutine(LoadAdmins());
        }

        private static IEnumerator LoadAdmins()
        {
            using UnityWebRequest request =
                UnityWebRequest.Get(AdminDataUrl);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"Sharpz Reborn // Failed to load admin data: {request.error}"
                );

                yield break;
            }

            try
            {
                JObject json =
                    JObject.Parse(request.downloadHandler.text);

                JArray admins =
                    json["admins"] as JArray;

                if (admins == null)
                {
                    Debug.LogError(
                        "Sharpz Reborn // Admin data does not contain an admins array."
                    );

                    yield break;
                }

                HashSet<string> adminIds = new();

                foreach (JToken admin in admins)
                {
                    string userId =
                        admin["userId"]?.ToString();

                    if (string.IsNullOrEmpty(userId))
                        continue;

                    adminIds.Add(userId);
                }

                AdminManager.SetAdmins(adminIds);

                Loaded = true;

                Debug.Log(
                    $"Sharpz Reborn // Loaded {adminIds.Count} admins."
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    $"Sharpz Reborn // Failed to parse admin data: {ex}"
                );
            }
        }
    }
}