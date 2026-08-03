// Decompiled with JetBrains decompiler
// Type: OutwardModTemplate.ValueRange
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
using Random = UnityEngine.Random;

#nullable disable
namespace OutwardModTemplate;

[Serializable]
public class ValueRange
{
    public float Min;
    public float Max;
    public float Chance;

    public ValueRange()
    {
    }

    public ValueRange(float min, float max, float chance)
    {
        Min = min;
        Max = max;
        Chance = chance;
    }

    internal float ChangeValue(float value)
    {
        float num = Random.Range(Min, Max);
        return value * num;
    }

    internal float GetValue() => Random.Range(Min, Max);
}