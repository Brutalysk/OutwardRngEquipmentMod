// Decompiled with JetBrains decompiler
// Type: OutwardModTemplate.Config
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OutwardModTemplate;

[Serializable]
public class Config
{
    public float DecreaseChanceForAdd = 0.05f;
    public List<ValueRange> Weight = new List<ValueRange>();
    public List<ValueRange> Durability = new List<ValueRange>();
    public List<ValueRange> ColdProtectionModify = new List<ValueRange>();
    public List<ValueRange> ColdProtectionAdd = new List<ValueRange>();
    public List<ValueRange> HeatProtectionModify = new List<ValueRange>();
    public List<ValueRange> HeatProtectionAdd = new List<ValueRange>();
    public List<ValueRange> ImpactResistanceModify = new List<ValueRange>();
    public List<ValueRange> ImpactResistanceAdd = new List<ValueRange>();
    public List<ValueRange> ManaUseModifierModify = new List<ValueRange>();
    public List<ValueRange> ManaUseModifierAdd = new List<ValueRange>();
    public List<ValueRange> PouchCapacityBonusModify = new List<ValueRange>();
    public List<ValueRange> PouchCapacityBonusAdd = new List<ValueRange>();
    public List<ValueRange> StaminaUsePenaltyModify = new List<ValueRange>();
    public List<ValueRange> StaminaUsePenaltyAdd = new List<ValueRange>();
    public List<ValueRange> MovementPenaltyModify = new List<ValueRange>();
    public List<ValueRange> MovementPenaltyAdd = new List<ValueRange>();
    public List<ValueRange> DamageAttackModify = new List<ValueRange>();
    public List<ValueRange> DamageAttackAdd = new List<ValueRange>();
    public List<ValueRange> DamageResistanceModify = new List<ValueRange>();
    public List<ValueRange> DamageResistanceAdd = new List<ValueRange>();
    public List<ValueRange> StamCost = new List<ValueRange>();
    public List<ValueRange> Impact = new List<ValueRange>();
    public List<ValueRange> AttackSpeed = new List<ValueRange>();
    public List<ValueRange> Damage = new List<ValueRange>();
    public List<ValueRange> BagCapacity = new List<ValueRange>();
    public int MinNumberOfDropsInc;
    public int MaxNumberOfDropsInc;
}