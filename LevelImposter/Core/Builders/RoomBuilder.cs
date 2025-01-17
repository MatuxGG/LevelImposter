﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LevelImposter.Core
{
    public class RoomBuilder : Builder
    {
        private int roomId = 1;
        private static Dictionary<Guid, SystemTypes> systemDB = new Dictionary<Guid, SystemTypes>();

        public void Build(LIElement elem, GameObject obj)
        {
            if (elem.type != "util-room")
                return;

            SystemTypes systemType = (SystemTypes)roomId;
            roomId++;

            PlainShipRoom shipRoom = obj.AddComponent<PlainShipRoom>();
            shipRoom.RoomId = systemType;
            shipRoom.roomArea = obj.GetComponent<Collider2D>();
            shipRoom.roomArea.isTrigger = true;

            MapUtils.Rename(systemType, elem.name);
            systemDB.Add(elem.id, systemType);
        }

        public void PostBuild()
        {
            systemDB.Clear();
            MapUtils.Rename((SystemTypes)0, "Default Room");
            roomId = 1;
        }

        public static SystemTypes GetSystem(Guid id)
        {
            return systemDB.GetValueOrDefault(id);
        }

        public static SystemTypes[] GetAllSystems()
        {
            SystemTypes[] arr = new SystemTypes[systemDB.Count];
            systemDB.Values.CopyTo(arr, 0);
            return arr;
        }
    }
}
