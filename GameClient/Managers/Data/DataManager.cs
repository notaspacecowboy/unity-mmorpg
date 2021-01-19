//=============================
//Author: Zack Yang 
//Created Date: 10/17/2020 20:11
//=============================
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Common.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
    public string DataPath;
    public Dictionary<int, MapDefine> Maps = null;
    public Dictionary<int, CharacterDefine> Characters = null;
    public Dictionary<int, TeleporterDefine> Teleporters = null;
    public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
    public Dictionary<int, NpcDefine> Npcs = null;
    public Dictionary<int, ItemDefine> Items = null;
    public Dictionary<int, EquipDefine> Equips = null;
    public Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;
    public Dictionary<int, QuestDefine> Quests = null;

    public DataManager()
    {
        Debug.LogFormat("DataManager -> DataManager()");
        DataPath = "Data/";
    }

    /// <summary>
    /// load data from json files synchronously
    /// </summary>
    public void Load()
    {
        Debug.LogFormat("DataManager -> Load()");

        //load map resources
        string json = File.ReadAllText(DataPath + "MapDefine.txt");
        Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

        //load character information
        json = File.ReadAllText(DataPath + "CharacterDefine.txt");
        Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

        //load teleporter information
        json = File.ReadAllText(DataPath + "TeleporterDefine.txt");
        Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

        //load spawnpoints information
        //json = File.ReadAllText(DataPath + "SpawnPointDefine.txt");
        //SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);

        //load npc information
        json = File.ReadAllText(DataPath + "NpcDefine.txt");
        Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

        //load item information
        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

        //load equip information
        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

        //load item information
        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

        //load quest information
        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);

    }

    public void LoadAsync()
    {
        MonoManager.Instance.StartCoroutine(LoadData());
    }

    /// <summary>
    /// load data from json files asynchronously
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadData()
    {
        Debug.LogFormat("DataManager -> LoadAsync()");

        //load map resources
        string json = File.ReadAllText(DataPath + "MapDefine.txt");
        Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);
        //yield return null;

        //load character information
        json = File.ReadAllText(DataPath + "CharacterDefine.txt");
        Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
        yield return null;

        //load teleporter information
        json = File.ReadAllText(DataPath + "TeleporterDefine.txt");
        Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);
        yield return null;

        //load spawnpoints information
        //json = File.ReadAllText(DataPath + "SpawnPointDefine.txt");
        //SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);
        //yield return null;

        //load npc information
        json = File.ReadAllText(DataPath + "NpcDefine.txt");
        Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);
        yield return null;

        //load item information
        json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
        this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
        yield return null;

        //load shopitem information
        json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
        this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);
        yield return null;

        //load equip information
        json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
        this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);
        yield return null;

        //load equip information
        json = File.ReadAllText(this.DataPath + "QuestDefine.txt");
        this.Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);
        yield return null;
    }

#if UNITY_EDITOR
    public void SaveTeleporters()
    {
        string json = JsonConvert.SerializeObject(this.Teleporters, Formatting.Indented);
        File.WriteAllText(this.DataPath + "TeleporterDefine.txt", json);
    }

    public void SaveSpawnPoints()
    {
        string json = JsonConvert.SerializeObject(this.SpawnPoints, Formatting.Indented);
        File.WriteAllText(this.DataPath + "SpawnPointDefine.txt", json);
    }

#endif
}
