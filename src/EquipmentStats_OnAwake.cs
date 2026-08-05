// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.EquipmentStats_OnAwake
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using HarmonyLib;
using System;
using UnityEngine;

#nullable disable
namespace RngEquipmentMod;

[HarmonyPatch(typeof(ItemStats), "OnAwake")]
internal class EquipmentStats_OnAwake
{
    [HarmonyPrefix]
    public static void Prefix(ItemStats __instance)
    {
        switch (__instance.m_item)
        {
            case Weapon _:
            case Armor _:
            case Bag _:
                InitItem(__instance);
                break;
        }
    }

    private static void InitItem(ItemStats itemStats)
    {
        UID UID = itemStats.m_item.UID;
        if (!Plugin.RandomItemRepository.Contains(UID))
        {
            try
            {
                RandomItem randomItem = RandomItemFactory.CreateRandomItem(itemStats.m_item, itemStats);
                if(randomItem != null)
                {
                    Plugin.RandomItemRepository.Add(randomItem);
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError(ex);
            }
        }

        Plugin.RandomItemRepository.Get(UID).SetItem(itemStats);
    }
}