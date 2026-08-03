// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.Plugin
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

#nullable disable
namespace RngEquipmentMod;

[BepInPlugin("brutalysk.rngequipment", "RNG Equipment", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public const string GUID = "brutalysk.rngequipment";
    public const string NAME = "RNG Equipment";
    public const string VERSION = "1.0.0";
    internal static ManualLogSource Log;
    internal static RandomItemRepository RandomItemRepository;
    public static ConfigEntry<bool> ExampleConfig;

    internal void Awake()
    {
        Log = Logger;
        RandomItemRepository = new RandomItemRepository();
        ExampleConfig = Config.Bind<bool>("ExampleCategory", "ExampleSetting", false, "This is an example setting.");
        new Harmony("brutalysk.rngequipment").PatchAll();
    }

    internal void Update()
    {
    }
}