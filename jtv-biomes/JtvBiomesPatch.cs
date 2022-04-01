using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jtv_biomes
{
    [HarmonyPatch(typeof(ZoneSystem))]
    [HarmonyPatch(nameof(ZoneSystem.ValidateVegetation))]
    public class ZoneSystem_ValidateVegetation_Patch
    {

        public static bool Prefix(ref ZoneSystem __instance)
        {

            if (__instance is null || __instance.m_vegetation is null) return true;

            var AshLands_Vegetation = new List<string>
            {
                "Waystone",
                "Skull1",
                "Skull2",
                "Rock_3",
                "Rock_4",
                "Rock_4_plains",
                "rock1_mountain",
                "rock2_heath",
                "rock2_mountain",
                "rock3_mountain",
                "rock4_coast",
                "rock4_copper",
                "rock4_heath",
                "MineRock_Obsidian",
                "Pickable_Stone",
                "Pickable_Flint",
                "HeathRockPillar",
                "vfx_edge_clouds",
                "vfx_ocean_clouds",
                "vfx_swamp_mist"
            };

            var DeepNorth_Vegetation = new List<string>
            {
                "Waystone",
                "silvervein",
                "Rock_3",
                "Rock_4",
                "Rock_4_plains",
                "rock1_mountain",
                "rock2_heath",
                "rock2_mountain",
                "rock3_mountain",
                "rock3_silver",
                "rock4_coast",
                "rock4_copper",
                "rock4_heath",
                "rockformation1",
                "Pickable_Stone",
                "Pickable_Flint",
                "HugeStone1",
                "ice1"
            };

            
            foreach (ZoneSystem.ZoneVegetation veg in __instance.m_vegetation.ToList())
            {
                try
                {
                    if (veg.m_biome == Heightmap.Biome.Mistlands && jtv_biomes.MistlandsVegetation.Value)
                    {
                        veg.m_enable = true;
                        continue;
                    }

                    if (AshLands_Vegetation.Contains(veg.m_prefab.name) && jtv_biomes.AshlandsVegetation.Value)
                    {
                        // clone and add to ashlands
                        var newVeg = CloneVegetation(veg, Heightmap.Biome.AshLands);
                        if (newVeg is null)
                        {
                            Jotunn.Logger.LogError("CloneVegetation returned null");
                        }

                        if (newVeg.m_prefab.GetComponent<ZNetView>() == null) newVeg.m_prefab.AddComponent<ZNetView>();
                        
                        ZoneSystem.instance.m_vegetation.Add(newVeg);
                    }

                    if (DeepNorth_Vegetation.Contains(veg.m_prefab.name) && jtv_biomes.DeepNorthVegetation.Value)
                    {
                        // clone and add to deep north
                        var newVeg = CloneVegetation(veg, Heightmap.Biome.DeepNorth);

                        if (newVeg.m_prefab.GetComponent<ZNetView>() == null) newVeg.m_prefab.AddComponent<ZNetView>();

                        ZoneSystem.instance.m_vegetation.Add(newVeg);
                    }

                }
                catch (Exception)
                {
                    Jotunn.Logger.LogError($"Error loading vegetation <{veg.m_name}>");
                }

            }
            
            return true;
        }

        public static ZoneSystem.ZoneVegetation CloneVegetation(ZoneSystem.ZoneVegetation veg, Heightmap.Biome biome)
        {
            return new ZoneSystem.ZoneVegetation
            {
                m_name = $"{biome:G}_{veg.m_prefab.name}",
                m_prefab = veg.m_prefab,
                m_enable = true,
                m_min = veg.m_min,
                m_max = veg.m_max,
                m_forcePlacement = veg.m_forcePlacement,
                m_scaleMin = veg.m_scaleMin,
                m_scaleMax = veg.m_scaleMax,
                m_randTilt = veg.m_randTilt,
                m_chanceToUseGroundTilt = veg.m_chanceToUseGroundTilt,
                m_biome = biome,
                m_biomeArea = veg.m_biomeArea,
                m_blockCheck = veg.m_blockCheck,
                m_minAltitude = veg.m_minAltitude,
                m_maxAltitude = veg.m_maxAltitude,
                m_minOceanDepth = veg.m_minOceanDepth,
                m_maxOceanDepth = veg.m_maxOceanDepth,
                m_minTilt = veg.m_minTilt,
                m_maxTilt = veg.m_maxTilt,
                m_terrainDeltaRadius = veg.m_terrainDeltaRadius,
                m_maxTerrainDelta = veg.m_maxTerrainDelta,
                m_minTerrainDelta = veg.m_minTerrainDelta,
                m_snapToWater = veg.m_snapToWater,
                m_groundOffset = veg.m_groundOffset,
                m_groupSizeMin = veg.m_groupSizeMin,
                m_groupSizeMax = veg.m_groupSizeMax,
                m_groupRadius = veg.m_groupRadius,
                m_inForest = veg.m_inForest,
                m_forestTresholdMin = veg.m_forestTresholdMin,
                m_forestTresholdMax = veg.m_forestTresholdMax,
                m_foldout = veg.m_foldout
            };
        }
    }
}
