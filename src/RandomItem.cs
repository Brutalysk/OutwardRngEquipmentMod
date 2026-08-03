// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomItem
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
using System.Reflection;
using System.Security.Cryptography;

#nullable disable
namespace RngEquipmentMod;

public class RandomItem
{
    public UID _UID { get; set; }

    public int _maxDurability { get; set; }

    public float _rawWeight { get; set; }

    public RandomItem()
    {
    }

    public RandomItem(ItemStats itemStats)
    {
        _maxDurability = itemStats.MaxDurability;
        _rawWeight = itemStats.RawWeight;
        _UID = itemStats.m_item.UID;
    }

    public virtual void Randomize()
    {
    }

    public static FieldInfo GetField<T>(string name)
    {
        return typeof(T).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public virtual void SetItem(ItemStats itemStats)
    {
        Plugin.Log.LogMessage($"[SetItem]::SetItem - weight : {_rawWeight}, durability : {_maxDurability}");
        GetField<ItemStats>("m_rawWeight").SetValue(itemStats, _rawWeight);
        GetField<ItemStats>("m_baseMaxDurability").SetValue(itemStats, _maxDurability);
    }

    public bool RollOdds(int percentChances)
    {
        RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        byte[] data = new byte[4];
        randomNumberGenerator.GetBytes(data);
        return Math.Abs(BitConverter.ToInt32(data, 0)) % 100 <= percentChances;
    }

    public int GetRandomValue(int low, int high)
    {
        RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        byte[] data = new byte[4];
        randomNumberGenerator.GetBytes(data);
        int num1 = Math.Abs(BitConverter.ToInt32(data, 0));
        int num2 = high - low;
        return low + num1 % num2;
    }

    public float CalculateModifier(int divergance)
    {
        return (float)((100.0 - (divergance - GetRandomValue(0, 2 * divergance))) / 100.0);
    }
}