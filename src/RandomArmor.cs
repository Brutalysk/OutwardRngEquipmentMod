// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomArmor
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
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
        base.Randomize();
        float score = GetScore();

        ClearDamageArray();
        ClearUtilities();
        ClearResourceModifiers();

        bool hasDamageBonusModifiers = RollOdds(35);
        bool hasUtilitiesModifiers = RollOdds(35);
        bool hasResourceModifiers = RollOdds(60);

        int numberOfModifiers = 1;
        if (hasDamageBonusModifiers) ++numberOfModifiers;
        if (hasUtilitiesModifiers) ++numberOfModifiers;
        if (hasResourceModifiers) ++numberOfModifiers;

        if (hasResourceModifiers)
        {
            float modifier = CalculateModifier(60);
            float negativeAttributeModifier = RollOdds(50) ? 1.0f : -1.0f;
            float availableScore = score / numberOfModifiers * modifier * negativeAttributeModifier;
            availableScore = availableScore < 0.0f ? Math.Min(availableScore, GetProposedNegativeValue() * modifier) : availableScore;

            RandomizeResourceModifiers(availableScore);
            --numberOfModifiers;
            score -= availableScore;
        }

        if (hasUtilitiesModifiers)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / numberOfModifiers * modifier;
            RandomizeUtilities(availableScore);
            --numberOfModifiers;
            score -= availableScore;
        }

        if (hasDamageBonusModifiers)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / numberOfModifiers * modifier;
            RandomizeDamageBonus(availableScore);
            score -= availableScore;
        }

        RandomizeProtectionBonus(score);
    }

    private float GetScore()
    {
        // Clamp the equipment score to 0 to give a chance for heavy helmets to have some score left.
        float equipmentScore = GetEquipmentScore();
        return equipmentScore + GetProtectionScore();
    }

    protected void RandomizeProtectionBonus(float availableScore)
    {
        List<int> resistanceList = new List<int>();
        resistanceList.AddRange(GetTypes(35));
        if (RollOdds(90) && !resistanceList.Contains(0))
            resistanceList.Add(0);

        if (RollOdds(25))
        {
            List<int> resistanceDebuffList = new List<int>();
            resistanceDebuffList.AddRange(GetTypes(35));
            resistanceDebuffList.RemoveAll(item => resistanceList.Contains(item));
            ClearResistanceArray();

            foreach(int resistanceDebuff in resistanceDebuffList)
            {
                int num = resistanceList.Count;
            
                float modifier = CalculateModifier(30);
                float score = -(availableScore / num * modifier);
                score = Math.Min(score, GetProposedNegativeValue() * modifier);
                _damageResistance[resistanceDebuff] = GetPercentValueFromScore(score);
                availableScore -= score;
            }
        }

        for (int index = 0; index < resistanceList.Count - 1; ++index)
        {
            int num = resistanceList.Count - index;
            float score = availableScore / num * CalculateModifier(30);
            _damageResistance[resistanceList[index]] = GetPercentValueFromScore(score);
            availableScore -= score;
        }
        _damageResistance[resistanceList[resistanceList.Count - 1]] = GetPercentValueFromScore(availableScore);
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