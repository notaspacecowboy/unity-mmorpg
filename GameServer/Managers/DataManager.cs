using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using Newtonsoft.Json;

namespace GameServer.Managers
{
    class DataManager: Singleton<DataManager>
    {
        internal string DataPath;
        internal Dictionary<int, MapDefine> Maps = null;
        internal Dictionary<int, CharacterDefine> Characters = null;
        internal Dictionary<int, TeleporterDefine> Teleporters = null;
        internal Dictionary<int, ItemDefine> Items = null;
        internal Dictionary<int, EquipDefine> Equips = null;
        internal Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
        internal Dictionary<int, Dictionary<int, SpawnRuleDefine>> SpawnRules = null;
        internal Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;
        internal Dictionary<int, QuestDefine> Quests = null;

        public DataManager()
        {
            Log.InfoFormat("DataManager > DataManager()");
            this.DataPath = "Data/";
        }


        public void Load()
        {
            Log.InfoFormat("DataManager > Load()");
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
            this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);
            
            json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
            this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

            json = File.ReadAllText(this.DataPath + "QuestDefine.txt");
            this.Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);

        }
    }
}
