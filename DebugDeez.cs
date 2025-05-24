using MelonLoader;
using HarmonyLib;
using UnityEngine;         // for KeyCode & Input
using System;
using System.Reflection;
using System.Collections;
using Il2Cpp;

namespace IndustrialHorizons
{
    [HarmonyPatch(typeof(MenuWindowR), nameof(MenuWindowR.Update))]
    public static class DebugProgressPatch_OnHash
    {
        // change this if you want a different key combo
        private static bool IsHashPressed()
        {
            return Input.GetKeyDown(KeyCode.Alpha3);
        }

        static void Prefix()
        {
            if (!IsHashPressed())
                return;

            DumpProgressInfo();
        }

        private static void DumpProgressInfo()
        {
            var gameAsm = typeof(MenuWindowR).Assembly;
            MelonLogger.Msg($"[Debug] Game assembly: {gameAsm.GetName().Name}");

            var progressType = gameAsm.GetType("Progress")
                               ?? gameAsm.GetType("YourGameNamespace.Progress");
            MelonLogger.Msg($"[Debug] Progress type: {progressType}");

            if (progressType == null) return;

            var discoverMethod = progressType.GetMethod(
                "DiscoverSubstance",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MelonLogger.Msg($"[Debug] DiscoverSubstance: {discoverMethod}");

            FieldInfo listField = null;
            foreach (var fi in progressType.GetFields(
                         BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(IList).IsAssignableFrom(fi.FieldType))
                {
                    MelonLogger.Msg($"[Debug] Found list field: {fi.Name} ({fi.FieldType})");
                    listField = fi;
                    break;
                }
            }
            if (listField == null) return;

            object progInstance = null;
            var prop = progressType.GetProperty("Instance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
                progInstance = prop.GetValue(null);
            else
            {
                var instField = progressType.GetField("Instance",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                progInstance = instField?.GetValue(null);
            }
            MelonLogger.Msg($"[Debug] Progress.Instance => {progInstance}");
            if (progInstance == null) return;

            var listObj = listField.GetValue(progInstance) as IList;
            MelonLogger.Msg($"[Debug] {listField.Name}.Count = {listObj?.Count}");
            if (listObj != null)
            {
                for (int i = 0; i < listObj.Count; i++)
                    MelonLogger.Msg($"[Debug]   [{i}] = {listObj[i]}");
            }
        }
    }
}