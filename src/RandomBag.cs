// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomBag
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;

#nullable disable
namespace RngEquipmentMod;

public class RandomBag : RandomEquipment
{
    public float _bagCapacity { get; set; }

    public RandomBag()
    {
    }

    public RandomBag(EquipmentStats equipmentStats)
        : base(equipmentStats)
    {
        _bagCapacity = ((Bag)(equipmentStats).m_item).BagCapacity;
    }

    public override void SetItem(ItemStats equipmentStats)
    {
        base.SetItem(equipmentStats);
        (((Bag)equipmentStats.m_item).m_container).m_baseContainerCapacity = _bagCapacity;
    }

    public override void Randomize()
    {
        float score = GetScore();
        int num1 = RollOdds(20) ? 1 : 0;
        bool flag1 = RollOdds(20);
        bool flag2 = RollOdds(20);
        int num2 = 2;
        if (num1 != 0)
            ++num2;
        if (flag1)
            ++num2;
        if (flag2)
            ++num2;
        if (flag1)
        {
            int num3 = RollOdds(30) ? 1 : 0;
            float modifier = CalculateModifier(60);
            float availableScore = score / num2 * modifier;
            if (num3 != 0)
                availableScore *= -1f;
            RandomizeUtilities(availableScore);
            --num2;
            score -= availableScore;
        }
        else
            ClearUtilities();

        if (num1 != 0)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / num2 * modifier;
            RandomizeDamageBonus(availableScore);
            --num2;
            score -= availableScore;
        }
        else
            ClearDamageArray();

        if (flag2)
        {
            int num4 = RollOdds(30) ? 1 : 0;
            float modifier = CalculateModifier(60);
            float availableScore = score / num2 * modifier;
            if (num4 != 0)
                availableScore *= -1f;
            RandomizeResourceModifiers(availableScore);
            score -= availableScore;
        }
        else
            ClearResourceModifiers();

        _bagCapacity = (float)Math.Round(score, 1);
    }

    protected float GetScore() => 0.0f + GetEquipmentScore() + _bagCapacity;
}