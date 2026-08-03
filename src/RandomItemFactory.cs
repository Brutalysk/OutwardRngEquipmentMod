// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomItemFactory
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

#nullable disable
namespace RngEquipmentMod;

internal static class RandomItemFactory
{
    public static RandomItem CreateRandomItem(Item item, ItemStats itemStats)
    {
        RandomItem randomItem;
        switch (item)
        {
            case Weapon _:
                randomItem = new RandomWeapon((WeaponStats)itemStats);
                break;
            case Armor _:
                randomItem = new RandomArmor((EquipmentStats)itemStats);
                break;
            case Bag _:
                Plugin.Log.LogMessage("[RandomItemFactory]::CreateRandomItem create random bag");
                randomItem = new RandomBag((EquipmentStats)itemStats);
                break;
            default:
                randomItem = new RandomItem(itemStats);
                break;
        }

        randomItem.Randomize();
        return randomItem;
    }
}