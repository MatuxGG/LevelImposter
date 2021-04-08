﻿using BepInEx.Logging;
using LevelImposter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LevelImposter.Map
{
    static class MapHandler
    {
        public static MapData mapData;
        public static string mapDir;
        public static bool mapLoaded = false;

        public static bool Load()
        {
            if (mapLoaded)
                return true;

            LILogger.LogInfo("Deserializing Map Data...");

            // Get Directory
            string dllDir = System.Reflection.Assembly.GetAssembly(typeof(LevelImposter.MainHarmony)).Location;
            mapDir = Path.Combine(Path.GetDirectoryName(dllDir), "map.json");

            // Load File
            if (!File.Exists(mapDir))
            {
                LILogger.LogError("Could not find map at " + mapDir);
                return false;
            }
            string mapJson = File.ReadAllText(mapDir);

            // Deserialize
            mapData = Newtonsoft.Json.JsonConvert.DeserializeObject<MapData>(mapJson);

            // Return
            mapLoaded = true;
            LILogger.LogInfo("Map Data has been Loaded!");
            return true;
        }

        public static MapData GetMap()
        {
            return mapData;
        }

        public static MapAsset GetById(UInt64 id)
        {
            return mapData.objs.FirstOrDefault(obj => obj.id == id);
        }
    }
}
