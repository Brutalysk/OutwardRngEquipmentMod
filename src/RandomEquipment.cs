// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomEquipment
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace RngEquipmentMod;

public class RandomEquipment : RandomItem
{
    protected int[] percentValues = new int[32 /*0x20*/]
    {
        0,
        1,
        2,
        3,
        3,
        4,
        5,
        6,
        7,
        8,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        16 /*0x10*/,
        17,
        18,
        19,
        21,
        22,
        24,
        26,
        29,
        32 /*0x20*/,
        35,
        40,
        47,
        58,
        80 /*0x50*/
    };

    public float _coldProtection { get; set; }

    public float _heatProtection { get; set; }

    public float _impactResistance { get; set; }

    public float _pouchCapacityBonus { get; set; }

    public float[] _damageResistance { get; set; }

    public float[] _damageAttack { get; set; }

    public float _manaUseModifier { get; set; }

    public float _manaRegenBonus { get; set; }

    public float _staminaCostReduction { get; set; }

    public float _staminaRegen { get; set; }

    public float _cooldownReductionBonus { get; set; }

    public float _movementPenalty { get; set; }

    public RandomEquipment()
    {
    }

    public RandomEquipment(EquipmentStats equipmentStats)
        : base(equipmentStats)
    {
        _coldProtection = equipmentStats.ColdProtection;
        _heatProtection = equipmentStats.HeatProtection;
        _impactResistance = equipmentStats.ImpactResistance;
        _pouchCapacityBonus = equipmentStats.PouchCapacityBonus;
        _damageResistance = equipmentStats.m_damageResistance;
        _damageAttack = equipmentStats.m_damageAttack;
        _manaUseModifier = equipmentStats.ManaUseModifier * -1f;
        _manaRegenBonus = equipmentStats.ManaRegenBonus;
        _staminaCostReduction = equipmentStats.m_staminaUsePenalty * -1f;
        _staminaRegen = equipmentStats.StaminaRegenModifier;
        _cooldownReductionBonus = equipmentStats.CooldownReduction;
        _movementPenalty = equipmentStats.MovementPenalty * -1f;
    }

    public override void SetItem(ItemStats equipmentStats)
    {
        base.SetItem(equipmentStats);
        GetField<EquipmentStats>("m_coldProtection").SetValue(equipmentStats, _coldProtection);
        GetField<EquipmentStats>("m_heatProtection").SetValue(equipmentStats, _heatProtection);
        GetField<EquipmentStats>("m_impactResistance").SetValue(equipmentStats, _impactResistance);
        GetField<EquipmentStats>("m_pouchCapacityBonus").SetValue(equipmentStats, _pouchCapacityBonus);
        GetField<EquipmentStats>("m_damageResistance").SetValue(equipmentStats, (_damageResistance).ToArray<float>());
        GetField<EquipmentStats>("m_damageAttack").SetValue(equipmentStats, (_damageAttack).ToArray<float>());
        GetField<EquipmentStats>("m_manaUseModifier").SetValue(equipmentStats, (_manaUseModifier * -1f));
        GetField<EquipmentStats>("m_baseManaRegenBonus").SetValue(equipmentStats, _manaRegenBonus);
        GetField<EquipmentStats>("m_staminaUsePenalty").SetValue(equipmentStats, (_staminaCostReduction * -1f));
        GetField<EquipmentStats>("m_baseStaminaRegen").SetValue(equipmentStats, _staminaRegen);
        GetField<EquipmentStats>("m_baseCooldownReductionBonus").SetValue(equipmentStats, _cooldownReductionBonus);
        GetField<EquipmentStats>("m_movementPenalty").SetValue(equipmentStats, (_movementPenalty * -1f));
    }

    public float GetEquipmentScore()
    {
        return 0.0f + GetDamageBonusScore() + GetUtilitiesScore() + GetResourceModifiersScore();
    }

    protected void RandomizeDamageBonus(float availableScore)
    {
        List<int> types = GetTypes(35);
        ClearDamageArray();
        for (int index = 0; index < types.Count - 1; ++index)
        {
            int num1 = types.Count - index;
            float num2 = availableScore / num1 * CalculateModifier(80 /*0x50*/);
            _damageAttack[types[index]] = (float)Math.Round(num2, 1);
            availableScore -= _damageAttack[types[index]];
        }

        _damageAttack[types.Last<int>()] = availableScore;
    }

    protected float GetDamageBonusScore()
    {
        float damageBonusScore = 0.0f;
        for (int index = 0; index < _damageAttack.Length; ++index)
            damageBonusScore += _damageAttack[index];
        return damageBonusScore;
    }

    protected void ClearDamageArray()
    {
        for (int index = 0; index < _damageAttack.Length; ++index)
            _damageAttack[index] = 0.0f;
    }

    protected void RandomizeUtilities(float availableScore)
    {
        float num = availableScore / 2f;
        ClearUtilities();
        float modifier = CalculateModifier(50);
        if (RollOdds(20))
        {
            _pouchCapacityBonus = (float)Math.Round((num / 2f * modifier), 1);
            num -= _pouchCapacityBonus;
        }

        _movementPenalty = (float)Math.Round(num, 1);
    }

    protected float GetUtilitiesScore()
    {
        return (float)(0.0 + _pouchCapacityBonus * 2.0 + _movementPenalty * 2.0);
    }

    protected void ClearUtilities()
    {
        _pouchCapacityBonus = 0.0f;
        _movementPenalty = 0.0f;
    }

    protected void RandomizeResourceModifiers(float availableScore)
    {
        int num1 = RollOdds(40) ? 1 : 0;
        int num2 = RollOdds(20) ? 1 : 0;
        bool flag = availableScore < 0.0;
        ClearResourceModifiers();

        float modifier = CalculateModifier(50);
        if (num2 != 0)
        {
            float num3 = availableScore / 2f * modifier;
            float percentValueFromScore = GetPercentValueFromScore(Math.Abs(num3));
            if (flag)
                percentValueFromScore *= -1f;
            _cooldownReductionBonus = (float)Math.Round(percentValueFromScore, 1);
            availableScore -= num3;
        }

        if (num1 != 0)
        {
            if (RollOdds(3))
                _manaRegenBonus = 1f;
            float percentValueFromScore = GetPercentValueFromScore(Math.Abs(availableScore));
            if (flag)
                percentValueFromScore *= -1f;
            _manaUseModifier = (float)Math.Round(percentValueFromScore, 1);
        }
        else
        {
            if (RollOdds(5))
                _staminaRegen = 1f;
            float percentValueFromScore = GetPercentValueFromScore(Math.Abs(availableScore));
            if (flag)
                percentValueFromScore *= -1f;
            _staminaCostReduction = (float)Math.Round(percentValueFromScore, 1);
        }
    }

    protected float GetResourceModifiersScore()
    {
        return (float)(0.0 + GetScoreFromPercentValue(_cooldownReductionBonus) +
                       GetScoreFromPercentValue(Math.Max(_manaUseModifier, 0.0f)) + _manaRegenBonus * 15.0) +
               GetScoreFromPercentValue(_staminaCostReduction) + _staminaCostReduction + _staminaRegen;
    }

    protected void ClearResourceModifiers()
    {
        _manaUseModifier = 0.0f;
        _manaRegenBonus = 0.0f;
        _staminaCostReduction = 0.0f;
        _staminaRegen = 0.0f;
        _cooldownReductionBonus = 0.0f;
    }

    protected List<int> GetTypes(int typeOdds)
    {
        List<int> types = new List<int>();
        do
        {
            int randomValue = GetRandomValue(0, 5);
            if (!types.Contains(randomValue))
                types.Add(randomValue);
        } while (RollOdds(typeOdds));

        return types;
    }

    protected float GetPercentValueFromScore(float score)
    {
        if (score < 0.0)
            return score;
        int percentValueFromScore = 0;
        for (int index = 1; index < percentValues.Length; ++index)
        {
            if (percentValues[index] <= score)
                percentValueFromScore = index;
        }

        return percentValueFromScore;
    }

    protected float GetScoreFromPercentValue(float value)
    {
        return value < 0.0
            ? value
            : percentValues[Math.Max(Math.Min((int)Math.Round(value), percentValues.Length - 1), 0)];
    }
}