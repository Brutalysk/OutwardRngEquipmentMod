// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomWeapon
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace RngEquipmentMod;

public class RandomWeapon : RandomEquipment
{
    private const float IMPACT_MODIFIER = 0.75f;

    private DamageType.Types[] allowedDamageTypes = new[]
    {
        DamageType.Types.Physical,
        DamageType.Types.Ethereal,
        DamageType.Types.Decay,
        DamageType.Types.Electric,
        DamageType.Types.Frost,
        DamageType.Types.Fire,
        DamageType.Types.Raw
    };

    public List<RandomWeaponDamageType> _weaponDamageTypes { get; set; }

    public float _attackSpeed { get; set; }

    public float _impact { get; set; }

    public float _stamCost { get; set; }

    public Weapon.WeaponType _type { get; set; }

    public RandomWeapon()
    {
    }

    public RandomWeapon(WeaponStats weaponStats)
        : base(weaponStats)
    {
        _attackSpeed = weaponStats.AttackSpeed;
        _impact = weaponStats.Impact;
        _stamCost = weaponStats.StamCost;

        Weapon weapon = (Weapon)weaponStats.m_item;
        _type = weapon.Type;

        _weaponDamageTypes = new List<RandomWeaponDamageType>();
        foreach (DamageType damageType in weaponStats.BaseDamage.List)
        {
            _weaponDamageTypes.Add(new RandomWeaponDamageType(damageType));
        }
    }

    public override void SetItem(ItemStats itemStats)
    {
        base.SetItem(itemStats);
        WeaponStats weaponStats = (WeaponStats)itemStats;
        List<float> attacksModifiers = new List<float>();
        SetAttackModifiers(weaponStats, attacksModifiers);
        GetField<WeaponStats>("AttackSpeed").SetValue(weaponStats, _attackSpeed);
        GetField<WeaponStats>("Impact").SetValue(weaponStats, _impact);
        GetField<WeaponStats>("StamCost").SetValue(weaponStats, _stamCost);
        DamageList damageList = new DamageList();
        Weapon weapon = (Weapon)(weaponStats).m_item;
        List<DamageType> damageTypes = new List<DamageType>();
        _weaponDamageTypes.ForEach(damageType =>
            damageTypes.Add(new DamageType(damageType._type, damageType._damage)));
        damageList.Add(damageTypes);
        GetField<WeaponStats>("BaseDamage").SetValue(weaponStats, damageList);
        GetField<Weapon>("baseDamage").SetValue(weapon, damageList);
        for (int index = 0; index < weaponStats.Attacks.Length; ++index)
            SetAttackDamage(weaponStats.Attacks[index], attacksModifiers[index]);
    }

    private void SetAttackDamage(WeaponStats.AttackData attack, float modifier)
    {
        attack.Damage = new List<float>();
        _weaponDamageTypes.Sort(
            (Comparison<RandomWeaponDamageType>)((x, y) => x._type.CompareTo(y._type)));
        foreach (RandomWeaponDamageType weaponDamageType in _weaponDamageTypes)
        {
            for (int index = 0; index < _weaponDamageTypes.Count; ++index)
            {
                float num = _weaponDamageTypes[index]._damage * modifier;
                attack.Damage.Add(num);
            }
        }
    }

    private void SetAttackModifiers(WeaponStats weaponStats, List<float> attacksModifiers)
    {
        float totalDamage = weaponStats.BaseDamage.TotalDamage;
        for (int index = 0; index < weaponStats.Attacks.Length; ++index)
        {
            float num = CalculateAttackDamage(weaponStats.Attacks[index]) / totalDamage;
            attacksModifiers.Add(num);
        }
    }

    private float CalculateAttackDamage(WeaponStats.AttackData attack)
    {
        float attackDamage = 0.0f;
        for (int index = 0; index < attack.Damage.Count; ++index)
            attackDamage += attack.Damage[index];
        return attackDamage;
    }


    public override void Randomize()
    {
        bool flag = RollOdds(10);
        float score = GetScore();

        Weapon.WeaponType type = _type;
        if (type <= Weapon.WeaponType.Chakram_OH)
        {
            if (type > Weapon.WeaponType.Mace_1H)
            {
                if (type == Weapon.WeaponType.Dagger_OH || type == Weapon.WeaponType.Chakram_OH)
                    goto label_10;
                goto label_11;
            }
        }
        else if (type <= Weapon.WeaponType.FistW_2H)
        {
            if (type == Weapon.WeaponType.Pistol_OH)
            {
                goto label_10;
            }
        }
        else
        {
            if (type != Weapon.WeaponType.Shield)
            {
                if (type == Weapon.WeaponType.Arrow)
                    return;
                goto label_11;
            }

            goto label_10;
        }

        RandomizeAttackSpeed();
        goto label_11;
        label_10:
        flag = RollOdds(25);
        label_11:
        int num1 = RollOdds(20) ? 1 : 0;
        int num2 = 2;
        if (flag)
            ++num2;
        if (num1 != 0)
            ++num2;
        if (flag)
        {
            float modifier = CalculateModifier(60);
            float availableScore = score / (float)num2 * modifier;
            RandomizeDamageBonus(availableScore);
            --num2;
            score -= availableScore;
        }
        else
            ClearDamageArray();

        if (num1 != 0)
        {
            int num3 = RollOdds(35) ? 1 : 0;
            float modifier = CalculateModifier(60);
            float availableScore = score / (float)num2 * modifier;
            if (num3 != 0)
                availableScore *= -1f;
            RandomizeResourceModifiers(availableScore);
            score -= availableScore;
        }
        else
            ClearResourceModifiers();

        if (_type != Weapon.WeaponType.Shield && _type != Weapon.WeaponType.Bow)
        {
            score /= _attackSpeed;
        }

        float modifier1 = CalculateModifier(60);
        float num5 = score / 2f * modifier1;
        _impact = (float)Math.Round((num5 / 0.75f), 1);
        RandomizeDamage(score - num5);
    }

    private void RandomizeAttackSpeed()
    {
        float num = CalculateModifier(50);
        if (num > 1.0)
        {
            num = 1f + (float)((num - 1.0) * 2.0);
        }

        _attackSpeed = (float)Math.Round((double)num, 1);
    }

    private void RandomizeDamage(float value)
    {
        List<RandomWeaponDamageType> weaponDamageTypeList = new List<RandomWeaponDamageType>();
        List<DamageType.Types> typesList = new List<DamageType.Types>();

        if (RollOdds(65))
        {
            typesList.Add(DamageType.Types.Physical);
        }
        else
        {
            typesList.Add((DamageType.Types)(int)allowedDamageTypes[GetRandomValue(1, allowedDamageTypes.Length - 1)]);
        }

        while (RollOdds(35))
        {
            DamageType.Types allowedDamageType = (DamageType.Types)(int)allowedDamageTypes[GetRandomValue(0, allowedDamageTypes.Length - 1)];

            if (!typesList.Contains(allowedDamageType))
            {
                typesList.Add(allowedDamageType);
            }
        }

        for (int index = 0; index < typesList.Count - 1; ++index)
        {
            int typesRemaining = typesList.Count - index;
            float typeValue = value / typesRemaining * CalculateModifier(50);
            weaponDamageTypeList.Add(new RandomWeaponDamageType(typesList[index], typeValue));
            value -= typeValue;
        }

        weaponDamageTypeList.Add(new RandomWeaponDamageType(typesList[typesList.Count - 1], value));

        _weaponDamageTypes = weaponDamageTypeList;
    }

    private float GetScore()
    {
        float score = _impact * 0.75f;
        foreach (var weaponDamageType in _weaponDamageTypes)
        {
            score += weaponDamageType.GetValue();
        }

        if (_type != Weapon.WeaponType.Shield && _type != Weapon.WeaponType.Bow)
        {
            score *= _attackSpeed;
        }

        return score + GetEquipmentScore();
    }
}