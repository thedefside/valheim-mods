// jtv_biomes
// a Valheim mod skeleton using Jötunn
// 
// File:    jtv_biomes.cs
// Project: jtv_biomes

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jtv_biomes
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class jtv_biomes : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.jtv_biomes";
        public const string PluginName = "jtv_biomes";
        public const string PluginVersion = "0.0.1";
        readonly Harmony harmony = new Harmony(PluginGUID);

        public static ConfigEntry<bool> MistlandsVegetation;
        public static ConfigEntry<bool> MistlandsLocations;
        public static ConfigEntry<bool> DeepNorthVegetation;
        public static ConfigEntry<bool> DeepNorthLocations;
        public static ConfigEntry<bool> AshlandsVegetation;
        public static ConfigEntry<bool> AshlandsLocations;
        public static ConfigEntry<bool> MeadowsLocations;
        public static ConfigEntry<bool> BlackForestLocations;
        public static ConfigEntry<bool> SwampLocations;
        public static ConfigEntry<bool> MountainsLocations;
        public static ConfigEntry<bool> PlainsLocations;

        private void Awake()
        {

            SetupConfig();
            ZoneManager.OnVanillaLocationsAvailable += AddLocations;
            //ZoneManager.OnVanillaLocationsAvailable += AddCustomVegetation;
            
            harmony.PatchAll();
        }

        private void SetupConfig()
        {
            MeadowsLocations = Config.Bind("Meadows", "Meadows Locations", true, "Add Locations to the Meadows");
            BlackForestLocations = Config.Bind("Black Forest", "Black Forest Locations", true, "Add Locations to the Black Forest");
            SwampLocations = Config.Bind("Swamp", "Swamp Locations", true, "Add Locations to the Swamp");
            MountainsLocations = Config.Bind("Mountains", "Mountains Locations", true, "Add Locations to the Mountains");
            PlainsLocations = Config.Bind("Plains", "Plains Locations", true, "Add Locations to the Plains");
            MistlandsVegetation = Config.Bind("Mistlands", "Mistlands Vegetation", true, "Add Rocks and plants to the Mistlands");
            MistlandsLocations = Config.Bind("Mistlands", "Mistlands Locations", true, "Add Locations to the Mistlands");
            DeepNorthVegetation = Config.Bind("Deep North", "Deep North Vegetation", true, "Add Rocks and plants to the Deep North");
            DeepNorthLocations = Config.Bind("Deep North", "Deep North Locations", true, "Add Locations to the Deep North");
            AshlandsVegetation = Config.Bind("Ashlands", "Ashlands Vegetation", true, "Add Rocks and plants to the Ashlands");
            AshlandsLocations = Config.Bind("Ashlands", "Ashlands Locations", true, "Add Locations to the Ashlands");
        }

        private void AddLocations()
        {
            Jotunn.Logger.LogInfo("Adding Locations");
            try
            {
                var scene = SceneManager.GetSceneByName("locations");
                var root = scene.GetRootGameObjects()[0];

                
                if (MeadowsLocations.Value)
                {
                    var StaminaGreydwarf = root.transform.Find("Misc/StaminaGreydwarf");
                    var staminaGreydwarfConfig = GetDefaultConfig(Heightmap.Biome.Meadows, quantity: 10);
                    staminaGreydwarfConfig.MinDistanceFromSimilar = 150;
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StaminaGreydwarf.gameObject, false, staminaGreydwarfConfig));

                    var WoodFarm1_Old = root.transform.Find("Meadows/WoodFarm1_Old");
                    var woodfarm1OldConfig = GetDefaultConfig(Heightmap.Biome.Meadows);
                    woodfarm1OldConfig.ExteriorRadius = 30;
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(WoodFarm1_Old.gameObject, false, woodfarm1OldConfig)); 
                }

                
                if (BlackForestLocations.Value)
                {
                    var StaminaTroll = root.transform.Find("Misc/StaminaTroll");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StaminaTroll.gameObject, false, GetDefaultConfig(Heightmap.Biome.BlackForest, 8500, quantity: 10))); 
                }

                
                if (SwampLocations.Value)
                {
                    var SwampRuinX = root.transform.Find("Swamp/SwampRuinX");
                    var SwampRuinXConfig = GetDefaultConfig(Heightmap.Biome.Swamp, 8500, quantity: 50);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(SwampRuinX.gameObject, false, SwampRuinXConfig));

                    var SwampRuinY = root.transform.Find("Swamp/SwampRuinY");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(SwampRuinY.gameObject, false, SwampRuinXConfig)); 
                }

                // mountains
                if (MountainsLocations.Value)
                {
                    var StoneTowerRuins06 = root.transform.Find("Mountains/StoneTowerRuins06");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerRuins06.gameObject, false, GetDefaultConfig(Heightmap.Biome.Mountain, 10500, 40))); 
                }

                // plains
                if (PlainsLocations.Value)
                {
                    var StoneVillage1 = root.transform.Find("Heath/StoneVillage1");
                    var plainsConfig = GetDefaultConfig(Heightmap.Biome.Plains, 8500);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneVillage1.gameObject, false, plainsConfig));

                    var StoneVillage2 = root.transform.Find("Heath/StoneVillage2");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneVillage2.gameObject, false, plainsConfig)); 
                }

                
                if (MistlandsLocations.Value)
                {
                    var StaminaWraith = root.transform.Find("Misc/StaminaWraith");
                    var mistlandsConfig = GetDefaultConfig(Heightmap.Biome.Mistlands, 8500);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StaminaWraith.gameObject, false, mistlandsConfig));

                    var StoneKeepX = root.transform.Find("Misc/StoneKeepX");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneKeepX.gameObject, false, mistlandsConfig));

                    var Oktanc1 = root.transform.Find("Swamp/Oktanc1");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(Oktanc1.gameObject, false, mistlandsConfig));

                    var StoneTowerU = root.transform.Find("Misc/StoneTowerU");
                    var StoneTowerUConfig = GetDefaultConfig(Heightmap.Biome.Mistlands, 8500, 40);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerU.gameObject, false, StoneTowerUConfig));

                    var StoneTowerV = root.transform.Find("Misc/StoneTowerV");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerV.gameObject, false, StoneTowerUConfig)); 
                }

                
                if (DeepNorthLocations.Value)
                {
                    var StoneKeepY = root.transform.Find("Misc/StoneKeepY");
                    var stoneKeepYConfig = GetDefaultConfig(Heightmap.Biome.DeepNorth, 10500);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneKeepY.gameObject, false, stoneKeepYConfig));

                    var Oktanc2 = root.transform.Find("Swamp/Oktanc2");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(Oktanc2.gameObject, false, stoneKeepYConfig));

                    var StoneTowerW = root.transform.Find("Misc/StoneTowerW");
                    var StoneTowerWConfig = GetDefaultConfig(Heightmap.Biome.DeepNorth, 10500, 40);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerW.gameObject, false, StoneTowerWConfig));

                    var StoneTowerX = root.transform.Find("Misc/StoneTowerX");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerX.gameObject, false, StoneTowerWConfig));

                    var XMasTree = root.transform.Find("Misc/XMasTree");
                    var XMasTreeConfig = GetDefaultConfig(Heightmap.Biome.DeepNorth, 10500, quantity: 5);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(XMasTree.gameObject, false, XMasTreeConfig));

                    var AbandonedLogCabin01 = root.transform.Find("Mountains/AbandonedLogCabin01");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(AbandonedLogCabin01.gameObject, false, stoneKeepYConfig));

                    var ShipWreckOcean03 = root.transform.Find("Misc/ShipWreckOcean03");
                    var ShipWreckOcean03Config = GetDefaultConfig(Heightmap.Biome.DeepNorth, 10500);
                    ShipWreckOcean03Config.Quantity = 50;
                    ShipWreckOcean03Config.MinAltitude = -1;
                    ShipWreckOcean03Config.MaxAltitude = 0;
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(ShipWreckOcean03.gameObject, false, ShipWreckOcean03Config));

                    var ShipWreckOcean04 = root.transform.Find("Misc/ShipWreckOcean04");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(ShipWreckOcean04.gameObject, false, ShipWreckOcean03Config)); 
                }

                
                if (AshlandsLocations.Value)
                {
                    var StoneKeepZ = root.transform.Find("Misc/StoneKeepZ");
                    var StoneKeepZConfig = GetDefaultConfig(Heightmap.Biome.AshLands, 10500);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneKeepZ.gameObject, false, StoneKeepZConfig));

                    var StoneTowerY = root.transform.Find("Misc/StoneTowerY");
                    var StoneTowerYConfig = GetDefaultConfig(Heightmap.Biome.AshLands, 10500, 40);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerY.gameObject, false, StoneTowerYConfig));

                    var StoneTowerZ = root.transform.Find("Misc/StoneTowerZ");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneTowerZ.gameObject, false, StoneTowerYConfig));

                    var Runestone_Surtlings = root.transform.Find("Misc/Runestone_Surtlings");
                    var Runestone_SurtlingsConfig = GetDefaultConfig(Heightmap.Biome.AshLands, 10500, quantity: 100);
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(Runestone_Surtlings.gameObject, false, Runestone_SurtlingsConfig));

                    var ShipWreckOcean01 = root.transform.Find("Misc/ShipWreckOcean01");
                    var ShipWreckOcean01Config = GetDefaultConfig(Heightmap.Biome.AshLands, 10500);
                    ShipWreckOcean01Config.Quantity = 50;
                    ShipWreckOcean01Config.MinAltitude = -1;
                    ShipWreckOcean01Config.MaxAltitude = 0;
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(ShipWreckOcean01.gameObject, false, ShipWreckOcean01Config));

                    var ShipWreckOcean02 = root.transform.Find("Misc/ShipWreckOcean02");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(ShipWreckOcean02.gameObject, false, ShipWreckOcean01Config));

                    var StoneWall1Config = GetDefaultConfig(Heightmap.Biome.AshLands, 10500, quantity: 100);
                    var StoneWall1 = root.transform.Find("Heath/StoneWall1");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneWall1.gameObject, false, StoneWall1Config));

                    var StoneWall2 = root.transform.Find("Heath/StoneWall2");
                    ZoneManager.Instance.AddCustomLocation(new CustomLocation(StoneWall2.gameObject, false, StoneWall1Config));

                    var ashHole = ZoneManager.Instance.CreateClonedLocation("AshHole", "FireHole");
                    ashHole.ZoneLocation.m_quantity = 50;
                    ashHole.ZoneLocation.m_biome = Heightmap.Biome.AshLands;
                    ashHole.ZoneLocation.m_maxDistance = 10500;
                }


            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                ZoneManager.OnVanillaLocationsAvailable -= AddLocations;
            }

        }

        private static LocationConfig GetDefaultConfig(Heightmap.Biome biome, int maxDistance = 5000, int maxTerrainDelta = 3, int quantity = 25)
        {
            return new LocationConfig
            {
                Biome = biome,
                BiomeArea = Heightmap.BiomeArea.Everything,
                CenterFirst = false,
                InteriorRadius = 10,
                ExteriorRadius = 10,
                ForestTrasholdMax = 1,
                ForestTresholdMin = 0,
                Group = "Jtv_Biomes",
                IconAlways = false,
                IconPlaced = false,
                InForest = false,
                MinDistance = 200,
                MaxDistance = maxDistance,
                MinAltitude = 1,
                MaxAltitude = 1000,
                MinDistanceFromSimilar = 100,
                MinTerrainDelta = 0,
                MaxTerrainDelta = maxTerrainDelta,
                Priotized = false,
                Quantity = quantity,
                RandomRotation = true,
                SlopeRotation = false,
                SnapToWater = false,
                Unique = false
            };
        }

        private void AddCustomVegetation()
        {
            try
            {
                if (AshlandsVegetation.Value)
                {
                    var pillar = PrefabManager.Instance.CreateClonedPrefab("AshlandsPillar", "HeathRockPillar");
                    ChangeColor(pillar, Color.black);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(pillar, Heightmap.Biome.AshLands, "HeathRockPillar"));

                    var rock4_ashlands = PrefabManager.Instance.CreateClonedPrefab("rock4_ashlands", "rock4_coast");
                    ChangeColor(rock4_ashlands, Color.red);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(rock4_ashlands, Heightmap.Biome.AshLands, "rock4_coast"));

                    var rock3_ashlands = PrefabManager.Instance.CreateClonedPrefab("rock3_ashlands", "rock3_mountain");
                    ChangeColor(rock3_ashlands, Color.red);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(rock3_ashlands, Heightmap.Biome.AshLands, "rock3_mountain"));

                    var rock2_ashlands = PrefabManager.Instance.CreateClonedPrefab("rock2_ashlands", "rock2_mountain");
                    ChangeColor(rock2_ashlands, Color.red);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(rock2_ashlands, Heightmap.Biome.AshLands, "rock2_mountain"));

                    var rock1_ashlands = PrefabManager.Instance.CreateClonedPrefab("rock1_ashlands", "rock1_mountain");
                    ChangeColor(rock1_ashlands, Color.black);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(rock1_ashlands, Heightmap.Biome.AshLands, "rock1_mountain"));

                    var Skull1_ashlands = PrefabManager.Instance.CreateClonedPrefab("Skull1_ashlands", "Skull1");
                    ChangeColor(Skull1_ashlands, Color.red);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(Skull1_ashlands, Heightmap.Biome.AshLands, "Skull1"));

                    var Skull2_ashlands = PrefabManager.Instance.CreateClonedPrefab("Skull2_ashlands", "Skull2");
                    ChangeColor(Skull2_ashlands, Color.red);
                    ZoneManager.Instance.AddCustomVegetation(GetCustomVegetation(Skull2_ashlands, Heightmap.Biome.AshLands, "Skull2"));
                }
            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                ZoneManager.OnVanillaLocationsAvailable -= AddCustomVegetation;
            }
        }

        private CustomVegetation GetCustomVegetation(GameObject go, Heightmap.Biome biome, string originalVegName)
        {
            var originalVeg = ZoneManager.Instance.GetZoneVegetation(originalVegName);
            if (originalVeg is null)
            {
                Jotunn.Logger.LogError($"Could not find original vegetation {originalVegName}");
                return new CustomVegetation(go, false, new VegetationConfig { Biome = biome });
            }

            return new CustomVegetation(go, true, new VegetationConfig 
            { 
                Biome = biome,
                BiomeArea = Heightmap.BiomeArea.Everything,
                BlockCheck = originalVeg.m_blockCheck,
                ForcePlacement = originalVeg.m_forcePlacement,
                ForestThresholdMax = originalVeg.m_forestTresholdMax,
                ForestThresholdMin = originalVeg.m_forestTresholdMin,
                GroundOffset = originalVeg.m_groundOffset,
                GroupRadius = originalVeg.m_groupRadius,
                GroupSizeMax = originalVeg.m_groupSizeMax,
                GroupSizeMin = originalVeg.m_groupSizeMin,
                InForest = originalVeg.m_inForest,
                Max = originalVeg.m_max,
                Min = originalVeg.m_min,
                MaxAltitude = originalVeg.m_maxAltitude,
                MinAltitude = originalVeg.m_minAltitude,
                MaxOceanDepth = originalVeg.m_maxOceanDepth,
                MinOceanDepth = originalVeg.m_minOceanDepth,
                MaxTerrainDelta = originalVeg.m_maxTerrainDelta,
                MinTerrainDelta = originalVeg.m_minTerrainDelta,
                MaxTilt = originalVeg.m_maxTilt,
                MinTilt = originalVeg.m_minTilt,
                ScaleMax = originalVeg.m_scaleMax,
                ScaleMin = originalVeg.m_scaleMin,
                TerrainDeltaRadius = originalVeg.m_terrainDeltaRadius
            });
        }

        public static void ChangeColor(GameObject go, Color color)
        {
            foreach (var renderer in go.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.materials = renderer.materials.Select(m => new Material(m) { color = color }).ToArray();
            }

        }
    }
}