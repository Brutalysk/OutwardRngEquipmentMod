// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomArmor
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System.Collections.Generic;

#nullable disable
namespace RngEquipmentMod;

public class RandomArmor : RandomEquipment
{
    public EquipmentSlot.EquipmentSlotIDs _armorType { get; set; }

    public RandomArmor()
    {
    }

    public RandomArmor(EquipmentStats equipmentStats)
        : base(equipmentStats)
    {
        _armorType = ((Armor)(equipmentStats).m_item).m_baseData.EquipSlot;
    }

    public override void Randomize()
    {
        float score = getScore();
        bool flag1 = RollOdds(50);
        int num1 = RollOdds(35) ? 1 : 0;
        bool flag2 = RollOdds(35);
        bool flag3 = RollOdds(60);
        int num2 = 1;
        if (num1 != 0)
            ++num2;
        if (flag2)
            ++num2;
        if (flag3)
            ++num2;
        if (flag3)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / num2 * modifier;
            if (flag1)
                availableScore *= -1f;
            RandomizeResourceModifiers(availableScore);
            --num2;
            score -= availableScore;
        }
        else
            ClearResourceModifiers();

        if (flag2)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / num2 * modifier;
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
            score -= availableScore;
        }
        else
            ClearDamageArray();

        RandomizeProtectionBonus(score);
    }

    private float getScore() => 0.0f + GetEquipmentScore() + GetProtectionScore();

    protected void RandomizeProtectionBonus(float availableScore)
    {
        List<int> intList = new List<int>();
        intList.AddRange(GetTypes(35));
        if (RollOdds(90) && !intList.Contains(0))
            intList.Add(0);
        ClearResistanceArray();
        for (int index = 0; index < intList.Count; ++index)
        {
            int num = intList.Count - index;
            float score = availableScore / num * CalculateModifier(80 /*0x50*/);
            _damageResistance[intList[index]] = GetPercentValueFromScore(score);
            availableScore -= score;
        }
    }

    protected float GetProtectionScore()
    {
        float impactResistance = _impactResistance;
        for (int index = 0; index < _damageResistance.Length; ++index)
            impactResistance += GetScoreFromPercentValue(_damageResistance[index]);
        return impactResistance;
    }

    private void ClearResistanceArray()
    {
        for (int index = 0; index < _damageResistance.Length; ++index)
            _damageResistance[index] = 0.0f;
    }
}