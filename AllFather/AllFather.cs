// AllFather
// a Valheim mod skeleton using Jötunn
// 
// File:    AllFather.cs
// Project: AllFather

using BepInEx;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace AllFather
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class AllFather : BaseUnityPlugin
    {
        public const string PluginGUID = "AllFather";
        public const string PluginName = "AllFather";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(AllFather).Assembly);
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
        }

        private void AddClonedItems()
        {
            try
            {
                var odinCape = new CustomItem("OdinCape", "CapeOdin",
                    new ItemConfig
                    {
                        Name = "Cape of Odin",
                        Description = "Odin's finest warriors deserve the finest cloth.",
                        CraftingStation = "piece_workbench",
                        RepairStation = "piece_workbench",
                        MinStationLevel = 1,
                        Enabled = true,
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig { Item = "Coal", Amount = 4 },
                            new RequirementConfig { Item = "LeatherScraps", Amount = 10 }
                        }
                    });
                odinCape.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_dlc = "";
                ItemManager.Instance.AddItem(odinCape);

                var odinHelmet = new CustomItem("OdinHelmet", "HelmetOdin",
                    new ItemConfig
                    {
                        Name = "Hood of Odin",
                        Description = "Odin's finest warriors deserve the finest cloth.",
                        Enabled = true,
                        CraftingStation = "piece_workbench",
                        RepairStation = "piece_workbench",
                        MinStationLevel = 1,
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig { Item = "Coal", Amount = 4 },
                            new RequirementConfig { Item = "LeatherScraps", Amount = 10 }
                        }
                    });
                odinHelmet.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_dlc = "";
                ItemManager.Instance.AddItem(odinHelmet);

                var odinTankard = new CustomItem("OdinTankard", "TankardOdin",
                    new ItemConfig
                    {
                        Name = "Mead horn of Odin",
                        Description = "Odin's finest warriors deserve the finest drinks.",
                        Enabled = true,
                        CraftingStation = "piece_workbench",
                        RepairStation = "piece_workbench",
                        MinStationLevel = 1,
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig { Item = "Resin", Amount = 2 },
                            new RequirementConfig { Item = "FineWood", Amount = 5 }

                        }
                    });
                odinTankard.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_dlc = "";
                ItemManager.Instance.AddItem(odinTankard);
            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
            }
        }

        [HarmonyPatch(typeof(TeleportWorld), "Interact")]
        private class Interact_Patch
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = new List<CodeInstruction>(instructions);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Ldc_I4_S)
                    {
                        list[i].operand = 127;
                    }
                }
                return Enumerable.AsEnumerable<CodeInstruction>(list);
            }
        }
    }

    
}