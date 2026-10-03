using System.ComponentModel;
using BepInEx;
using SharpzReborn.Classes;
using SharpzReborn.Managers;
using SharpzReborn.Patches;

namespace SharpzReborn;

[Description(PluginInfo.Description)]
[BepInPlugin(
        PluginInfo.GUID,
        PluginInfo.Name,
        PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Preferences.Load();
        
        gameObject.AddComponent<CoroutineManager>();

        GorillaTagger.OnPlayerSpawned(
                OnPlayerSpawned);
    }

    private void OnApplicationQuit() { Tools.Utilities.Shutdown(); }

    private void OnPlayerSpawned()
    {
        PatchHandler.PatchAll();

        Preferences.ApplyButtonStates();
    }
}