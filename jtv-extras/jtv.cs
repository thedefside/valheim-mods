// jtv_extras
// a Valheim mod skeleton using Jötunn
// 
// File:    jtv_extras.cs
// Project: jtv_extras

using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.Linq;
using UnityEngine;

namespace jtv
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("swolewizard.HuntardsEpicValheimsAdditions")]
    [BepInDependency("DasSauerkraut.Terraheim")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class jtv_extras : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.jtv";
        public const string PluginName = "jtv";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            PieceManager.OnPiecesRegistered += AddForges;
        }

        private void AddForges()
        {
            try
            {
                Jotunn.Logger.LogInfo("Updating Crafting Stations");
                var inscriptionTable = PrefabManager.Instance.GetPrefab("piece_alchemystation");
                inscriptionTable.GetComponent<CraftingStation>().m_name = "Dark Forge";
                var darkForgePiece = inscriptionTable.GetComponent<Piece>();
                darkForgePiece.m_name = "Dark Forge";
                darkForgePiece.m_description = "A table for crafting Heavy metal weapons and armor.";
                darkForgePiece.m_resources = new Piece.Requirement[]
                {
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("YagluthDrop")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 8,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("BlackMetal")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 6,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("OakWood_DoD")
                    },
                };

                var armorsBench = PrefabManager.Instance.GetPrefab("reforger");
                foreach (var renderer in armorsBench.GetComponentsInChildren<Renderer>())
                {
                    renderer.materials = renderer.materials.Select(m => new Material(m) { color = new Color(0.20f, 0.80f, 1) }).ToArray();
                }
                armorsBench.GetComponent<CraftingStation>().m_name = "Cold Forge";
                var coldForgePiece = armorsBench.GetComponent<Piece>();
                coldForgePiece.m_name = "Cold Forge";
                coldForgePiece.m_description = "A table for crafting Frometal weapons and armor.";
                coldForgePiece.m_resources = new Piece.Requirement[]
                {
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("InfusedGemstone_DoD")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 8,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("HeavymetalBar")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 6,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("OakWood_DoD")
                    }
                };
                
                var thorsForge = PrefabManager.Instance.GetPrefab("piece_thorsforge").GetComponent<Piece>();
                thorsForge.m_resources = new Piece.Requirement[]
                {
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("GreyPearl_DoD")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 4,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Thunderstone")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 6,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("OakWood_DoD")
                    },
                    new Piece.Requirement
                    {
                        m_amount = 6,
                        m_amountPerLevel = 0,
                        m_recover = true,
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("StormlingCore_DoD")
                    }
                };

                
            }
            finally
            {
                PieceManager.OnPiecesRegistered -= AddForges;
            }
        }
    }
}