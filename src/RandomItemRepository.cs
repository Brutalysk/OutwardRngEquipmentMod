// Decompiled with JetBrains decompiler
// Type: RngEquipmentMod.RandomItemRepository
// Assembly: RngEquipmentMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC41F823-2535-484D-AE60-4EB1D1607286
// Assembly location: C:\Users\Xavie\Downloads\RngEquipmentMod.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#nullable disable
namespace RngEquipmentMod;

internal class RandomItemRepository
{
    private static Dictionary<UID, RandomItem> _data;
    private string _path = "randomitem_data.json";
    private string _networkPath = "./rngModSave/";

    private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = (TypeNameHandling)3
    };

    public RandomItemRepository() => _data = new Dictionary<UID, RandomItem>();

    public bool Contains(UID UID)
    {
        return _data.ContainsKey(UID) || Load(UID);
    }

    public RandomItem Get(UID UID)
    {
        if (!_data.ContainsKey(UID))
            Plugin.Log.LogError($"[RandomItemRepository]::Get - RandomItem with UID : {UID} isn't in the repository.");
        return _data[UID];
    }

    public void Add(RandomItem randomItem)
    {
        _data[randomItem._UID] = randomItem;
        Save(randomItem._UID);
    }

    public void Save()
    {
        try
        {
            Dictionary<UID, RandomItem>.KeyCollection keys = _data.Keys;
            foreach (UID key in _data.Keys)
                Save(key);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError(ex);
        }
    }

    public void Load()
    {
        try
        {
            if (!File.Exists(_path))
                Plugin.Log.LogError($"[RandomItemRepository]::Load - File {_path} doesn't exists");
            else
                _data = JsonConvert.DeserializeObject<Dictionary<UID, RandomItem>>(File.ReadAllText(_path), new JsonSerializerSettings()
                {
                    TypeNameHandling = (TypeNameHandling)4
                });
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError(ex);
        }
    }

    public void Save(UID UID)
    {
        try
        {
            string contents = JsonConvert.SerializeObject(_data[UID], _jsonSerializerSettings);
            Plugin.Log.LogMessage(contents);
            File.WriteAllText(_networkPath + UID, contents);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError(ex);
        }
    }

    public bool Load(UID UID)
    {
        try
        {
            if (!File.Exists(_networkPath + UID))
            {
                Plugin.Log.LogError($"[RandomItemRepository]::Load - File {_networkPath}{UID} doesn't exists");
                return false;
            }

            string str = File.ReadAllText(_networkPath + UID);
            Plugin.Log.LogMessage(("[RandomItemRepository]::Load - json : " + str));
            _data[UID] = JsonConvert.DeserializeObject<RandomItem>(str, _jsonSerializerSettings);
            Plugin.Log.LogMessage($"[RandomItemRepository]::Load - object : {_data[UID]._UID}");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError(ex);
            return false;
        }

        return true;
    }

    public bool LoadMath(UID UID)
    {
        try
        {
            for (int index = 5; !File.Exists(_networkPath + UID) && index > 0; --index)
            {
                Plugin.Log.LogMessage($"[RandomItemRepository]::LoadMath - {UID} wasn't found, waiting 300ms");
                Thread.Sleep(100);
            }

            Plugin.Log.LogMessage($"[RandomItemRepository]::LoadMath - Finished loading {UID}");
            string str = File.ReadAllText(_networkPath + UID);
            _data[UID] = JsonConvert.DeserializeObject<RandomItem>(str, _jsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError(ex);
            return false;
        }

        return true;
    }
}