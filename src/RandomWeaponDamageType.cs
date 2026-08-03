// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomWeaponDamageType
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;

#nullable disable
namespace RngEquipmentMod;

public class RandomWeaponDamageType
{
    public DamageType.Types _type { get; set; }

    public float _damage { get; set; }

    public RandomWeaponDamageType()
    {
    }

    public RandomWeaponDamageType(DamageType.Types type, float value)
    {
        _type = type;
        _damage = (float)Math.Round(value / GetValueMultiplier(type), 1);
    }

    public RandomWeaponDamageType(DamageType damageType)
    {
        _type = damageType.Type;
        _damage = damageType.Damage;
    }

    public float GetValue()
    {
        return GetValueMultiplier(_type) * _damage;
    }

    public static float GetValueMultiplier(DamageType.Types type)
    {
        switch (type)
        {
            case DamageType.Types.Physical:
                return 1f;
            case DamageType.Types.Ethereal:
            case DamageType.Types.Decay:
            case DamageType.Types.Electric:
            case DamageType.Types.Frost:
            case DamageType.Types.Fire:
                return 1.2f;
            case DamageType.Types.Raw:
                return 1.4f;
            default:
                return 1f;
        }
    }
}