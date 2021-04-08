﻿using LevelImposter.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LevelImposter.DB
{
    static class AssetDB
    {
        public static Dictionary<string, TaskData> tasks;
        public static Dictionary<string, UtilData> utils;
        public static Dictionary<string, SabData> sabs;
        public static Dictionary<string, DecData> dec;
        public static Dictionary<string, RoomData> room;

        public static void Init()
        {
            try
            {
                TempDB tempDB = Newtonsoft.Json.JsonConvert.DeserializeObject<TempDB>(
                    Encoding.UTF8.GetString(Properties.Resources.AssetDB, 0, Properties.Resources.AssetDB.Length)
                );

                tasks = tempDB.tasks;
                utils = tempDB.utils;
                sabs = tempDB.sabs;
                dec = tempDB.dec;
                room = tempDB.room;
            }
            catch
            {
                LILogger.LogError("Error during AssetDB JSON Deserialization");
            }
        }

        public static void ImportMap(GameObject map)
        {
            // Ship Status
            ShipStatus shipStatus = map.GetComponent<ShipStatus>();

            // Determine Map Type
            MapType mapType = MapType.Skeld;
            if (map.name == "AprilShip")
                return;
            if (map.name == "MiraShip")
                mapType = MapType.Mira;
            if (map.name == "PolusShip")
                mapType = MapType.Polus;
            if (map.name == "Airship")
                mapType = MapType.Airship;


            // Import Map to Lists
            ImportMap(map, shipStatus, mapType, tasks);
            ImportMap(map, shipStatus, mapType, utils);
            ImportMap(map, shipStatus, mapType, sabs);
            ImportMap(map, shipStatus, mapType, dec);
            ImportMap(map, shipStatus, mapType, room);

            LILogger.LogInfo("..." + map.name + " Loaded");
        }

        private static void ImportMap<T>(GameObject map, ShipStatus shipStatus, MapType mapType, Dictionary<string, T> list) where T : AssetData
        {
            foreach (var elem in list)
            {
                if (elem.Value.MapType == mapType)
                {
                    elem.Value.ImportMap(map, shipStatus);
                }
            }
        }
    }

    class TempDB
    {
        public Dictionary<string, TaskData> tasks;
        public Dictionary<string, UtilData> utils;
        public Dictionary<string, SabData> sabs;
        public Dictionary<string, DecData> dec;
        public Dictionary<string, RoomData> room;
    }
}
