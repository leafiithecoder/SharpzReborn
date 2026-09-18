using System.ComponentModel;
using BepInEx;
using SharpzReborn.Classes;
using SharpzReborn.Managers;
using SharpzReborn.Patches;

namespace SharpzReborn;

[Description(Constants.Description)]
[BepInPlugin(
        Constants.Guid,
        Constants.Name,
        Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Preferences.Load();
        
        gameObject.AddComponent<CoroutineManager>();

        GorillaTagger.OnPlayerSpawned(
                OnPlayerSpawned);
    }

    private void OnApplicationQuit() => Preferences.Save();

    private void OnPlayerSpawned()
    {
        PatchHandler.PatchAll();

        Preferences.ApplyButtonStates();
    }
}