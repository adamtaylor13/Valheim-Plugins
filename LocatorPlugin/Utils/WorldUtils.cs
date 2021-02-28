﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Purps.Valheim.LocateMerchant.Utils;
using Purps.Valheim.Locator.Patches;
using UnityEngine;

namespace Purps.Valheim.Locator.Utils {
    public static class WorldUtils {
        private static readonly List<Vector3> MapPoints = generateMapPoints();

        private static List<Minimap.PinData> MapPins =>
            (List<Minimap.PinData>) Traverse.Create(Minimap.instance).Field("m_pins").GetValue();

        private static List<ZoneSystem.LocationInstance> MapLocations => ZoneSystem.instance.GetLocationList().ToList();

        private static List<Vector3> generateMapPoints() {
            const int radius = 10000;
            const int cycle = 360;
            const int pointCount = 6;

            var mapPoints = new List<Vector3>();
            for (var r = radius / pointCount; r <= radius; r += radius / pointCount * 2)
            for (var d = 0.0; d <= cycle; d += cycle / pointCount) {
                var x = (float) (r * Math.Cos(d * (Math.PI / 180)));
                var y = (float) (r * Math.Sin(d * (Math.PI / 180)));
                mapPoints.Add(new Vector3(x, 0, y));
            }

            return mapPoints;
        }

        public static void Locate(Minimap.PinType pinType, List<Tuple<string, string>> names) {
            if (!StatusUtils.isPlayerLoaded()) return;
            foreach (var name in names)
                MapPoints.ForEach(p => Game.instance.DiscoverClosestLocation(name.Item1, p, name.Item2, (int) pinType));
        }

        public static void ListLocations(string[] parameters) {
            if (!StatusUtils.isPlayerLoaded() || !StatusUtils.isPlayerOffline()) return;
            var locations = MapLocations;
            if (parameters != null && parameters.Length > 0)
                locations = locations.FindAll(location => parameters.Contains(location.m_location.m_prefabName));

            locations.ForEach(location =>
                ConsoleUtils.WriteToConsole($"{location.m_location.m_prefabName}\t\t{location.m_position}"));
        }

        public static void ListPins(string[] parameters) {
            if (!StatusUtils.isPlayerLoaded()) return;
            var pins = MapPins;
            if (parameters != null && parameters.Length > 0)
                pins = pins.FindAll(pin => parameters.Contains(pin.m_name));

            pins.ForEach(pin =>
                ConsoleUtils.WriteToConsole($"name:{pin.m_name}\t\tpos:{pin.m_pos}\t\ticon:{pin.m_icon.name}"));
        }
    }
}