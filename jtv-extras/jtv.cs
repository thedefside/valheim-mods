﻿// jtv
// a Valheim mod skeleton using Jötunn
// 
// File:    jtv.cs
// Project: jtv

using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace jtv
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("swolewizard.HuntardsEpicValheimsAdditions")]
    [BepInDependency("DasSauerkraut.Terraheim")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Jtv : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.jtv";
        public const string PluginName = "jtv";
        public const string PluginVersion = "0.0.6";
        private AssetBundle bundle;
        private GameObject MineRock_Salt;
        private GameObject Salt;

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            bundle = AssetUtils.LoadAssetBundleFromResources("jtvbundle", typeof(Jtv).Assembly);
            LoadPrefabs();
            AddVegetation();
            PieceManager.OnPiecesRegistered += AddForges;
            PrefabManager.OnVanillaPrefabsAvailable += AddItems;
        }

        private void LoadPrefabs()
        {
            MineRock_Salt = bundle.LoadAsset<GameObject>("MineRock_Salt");
            Salt = bundle.LoadAsset<GameObject>("SaltCube");
                                    
        }

        private void AddVegetation()
        {
            Jotunn.Logger.LogInfo("Adding Vegetation");
            try
            {
                
                ZoneManager.Instance.AddCustomVegetation(new CustomVegetation(MineRock_Salt, new VegetationConfig 
                {
                    Max = 3f,
                    GroupSizeMin = 1,
                    GroupSizeMax = 2,
                    GroupRadius = 15f,
                    BlockCheck = true,
                    Biome = Heightmap.Biome.DeepNorth,
                    MinAltitude = 0f,
                    MaxTilt = 30f
                }));
                
                
            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }

        }

        private void AddForges()
        {
            try
            {
                Jotunn.Logger.LogInfo("Repurposing Crafting Stations");
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
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("ElderBark")
                    },
                };

                var armorsBench = PrefabManager.Instance.GetPrefab("reforger");
                foreach (var renderer in armorsBench.GetComponentsInChildren<Renderer>())
                {
                    renderer.materials = renderer.materials.Select(m => new Material(m) { color = new Color(0.20f, 0.80f, 1.3f) }).ToArray();
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
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("WorldTreeFragment")
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
                        m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("BurningWorldTreeFragment")
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

        private void AddItems()
        {
            Jotunn.Logger.LogInfo("Adding Food");
            try
            {
                ItemManager.Instance.AddItem(new CustomItem(Salt, false));

                var newItems = new List<string>
                {
                    "Amethyst",
                    "JotunnBone",
                    "FrozenRemains",
                    "BurnedRemains",
                    "SpiderSilk"
                };

                foreach (var item in newItems)
                {
                    try
                    {
                        var prefab = bundle.LoadAsset<GameObject>(item);
                        ItemManager.Instance.AddItem(new CustomItem(prefab, false));
                    }
                    catch (Exception e)
                    {
                        Jotunn.Logger.LogError($"Error Adding item: {item} " + e.Message + e.StackTrace);
                    }
                }

                // make bananas
                var banana = new CustomItem("Banana", "WolfFang", new ItemConfig { Name = "Banana", Description = "A sweet fruit packed with nutrients that increase stamina." });
                foreach (var renderer in banana.ItemPrefab.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.materials = renderer.materials.Select(m => new Material(m) { color = Color.yellow }).ToArray();
                }
                banana.ItemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
                banana.ItemDrop.m_itemData.m_shared.m_food = 15;
                banana.ItemDrop.m_itemData.m_shared.m_foodStamina = 50;
                banana.ItemDrop.m_itemData.m_shared.m_foodBurnTime = 900;
                banana.ItemDrop.m_itemData.m_shared.m_foodRegen = 2f;
                banana.ItemDrop.m_autoPickup = true;
                var bananaIcon = LoadEmbeddedSprite("bananas_icon");
                banana.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { bananaIcon };

                ItemManager.Instance.AddItem(banana);

                // create the dough
                var bananaBreadDough = new CustomItem("BananaBreadDough", "BreadDough",
                    new ItemConfig 
                    {
                        Amount = 1,
                        CraftingStation = "piece_cauldron",
                        MinStationLevel = 4,
                        Name = "Banana Bread Dough",
                        Description = "Dough that when baked in the oven will produce Banana Bread",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Amount = 5,
                                Item = "BarleyFlour"
                            },
                            new RequirementConfig
                            {
                                Amount = 3,
                                Item = "Banana"
                            }
                        }
                    });
                var bananaBreadDoughIcon = LoadEmbeddedSprite("BananaBreadDough");
                bananaBreadDough.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { bananaBreadDoughIcon };
                ItemManager.Instance.AddItem(bananaBreadDough);

                var recipe_bananabread10 = new CustomRecipe(new RecipeConfig
                {
                    Amount = 20,
                    Item = "BananaBreadDough",
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Name = "Banana Bread Dough x20",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                            {
                                Amount = 100,
                                Item = "BarleyFlour"
                            },
                            new RequirementConfig
                            {
                                Amount = 60,
                                Item = "Banana"
                            }
                    }
                });
                ItemManager.Instance.AddRecipe(recipe_bananabread10);

                // create the bread
                var bananaBread = new CustomItem("BananaBread", "Bread",
                    new ItemConfig
                    {
                        Name = "Banana Bread",
                        Description = "A powerful stamina booster containing nutrients from bananas combined with the energy from the bread carbs."
                    });
                bananaBread.ItemDrop.m_itemData.m_shared.m_food = 30f;
                bananaBread.ItemDrop.m_itemData.m_shared.m_foodStamina = 90f;
                bananaBread.ItemDrop.m_itemData.m_shared.m_foodBurnTime = 1800f;
                bananaBread.ItemDrop.m_itemData.m_shared.m_foodRegen = 2f;
                var bananabreadIcon = LoadEmbeddedSprite("BananaBread");
                bananaBread.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { bananabreadIcon };
                
                ItemManager.Instance.AddItem(bananaBread);

                // add conversion to the oven
                var bananaBreadConversion = new CustomItemConversion(
                    new CookingConversionConfig
                    {
                        Station = "piece_oven",
                        FromItem = "BananaBreadDough",
                        ToItem = "BananaBread",
                        CookTime = 60f
                    });
                ItemManager.Instance.AddItemConversion(bananaBreadConversion);


                // liver and onions
                var liver = new CustomItem("Liver", "Entrails", new ItemConfig { Name = "Liver", Description = "The liver of an elf. Full of iron and other nutrients." });
                var liverIcon = LoadEmbeddedSprite("Liver");
                liver.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { liverIcon };
                ItemManager.Instance.AddItem(liver);

                var liverAndOnions = new CustomItem("LiverAndOnions", "OnionSoup", new ItemConfig 
                { 
                    Name = "Liver and Onions",
                    Description = "The liver of an elf sauteed with onions. An acquired taste.",
                    Amount = 1,
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Amount = 2,
                                Item = "Liver"
                            },
                            new RequirementConfig
                            {
                                Amount = 3,
                                Item = "Onion"
                            }
                        }

                });
                liverAndOnions.ItemDrop.m_itemData.m_shared.m_food = 90f;
                liverAndOnions.ItemDrop.m_itemData.m_shared.m_foodStamina = 30f;
                liverAndOnions.ItemDrop.m_itemData.m_shared.m_foodBurnTime = 1800f;
                liverAndOnions.ItemDrop.m_itemData.m_shared.m_foodRegen = 2f;
                var liverAndOnionsIcon = LoadEmbeddedSprite("LiverAndOnions");
                liverAndOnions.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { liverAndOnionsIcon };
                ItemManager.Instance.AddItem(liverAndOnions);

                var recipe_LiverAndOnions10 = new CustomRecipe(new RecipeConfig 
                {
                    Amount = 10,
                    Item = "LiverAndOnions",
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Name = "Liver and Onions x10",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                            {
                                Amount = 20,
                                Item = "Liver"
                            },
                            new RequirementConfig
                            {
                                Amount = 30,
                                Item = "Onion"
                            }
                    }
                });
                ItemManager.Instance.AddRecipe(recipe_LiverAndOnions10);
                
                var chickenNoodleSoup = new CustomItem("ChickenNoodleSoup", "TurnipStew", new ItemConfig
                {
                    Name = "$chicken_noodle_soup",
                    Description = "$chicken_noodle_description",
                    Amount = 1,
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Amount = 2,
                                Item = "BarleyFlour"
                            },
                            new RequirementConfig
                            {
                                Amount = 3,
                                Item = "raw_egg"
                            },
                            new RequirementConfig
                            {
                                Amount = 1,
                                Item = "cooked_chicken"
                            },
                            new RequirementConfig
                            {
                                Amount = 1,
                                Item = "SaltCube"
                            }
                        }

                });
                chickenNoodleSoup.ItemDrop.m_itemData.m_shared.m_food = 35f;
                chickenNoodleSoup.ItemDrop.m_itemData.m_shared.m_foodStamina = 100f;
                chickenNoodleSoup.ItemDrop.m_itemData.m_shared.m_foodBurnTime = 1800f;
                chickenNoodleSoup.ItemDrop.m_itemData.m_shared.m_foodRegen = 2f;
                var chickenNoodleIcon = LoadEmbeddedSprite("ChickenNoodleIcon");
                var chickenNoodleTexture = LoadEmbeddedTexture("ChickenNoodleSoup");
                chickenNoodleSoup.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { chickenNoodleIcon };
                foreach (var renderer in chickenNoodleSoup.ItemPrefab.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.materials = renderer.materials.Select(m => new Material(m) { mainTexture = chickenNoodleTexture }).ToArray();
                }
                ItemManager.Instance.AddItem(chickenNoodleSoup);

                var recipe_ChickenNoodleSoup10 = new CustomRecipe(new RecipeConfig
                {
                    Amount = 10,
                    Item = "ChickenNoodleSoup",
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Name = "Chicken Noodle Soup x10",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                        {
                            Amount = 20,
                            Item = "BarleyFlour"
                        },
                        new RequirementConfig
                        {
                            Amount = 30,
                            Item = "raw_egg"
                        },
                        new RequirementConfig
                        {
                            Amount = 10,
                            Item = "cooked_chicken"
                        },
                        new RequirementConfig
                        {
                            Amount = 10,
                            Item = "SaltCube"
                        }
                    }
                });
                ItemManager.Instance.AddRecipe(recipe_ChickenNoodleSoup10);

                var saltedMeat = new CustomItem("SaltedMeat", "CookedLoxMeat", new ItemConfig
                {
                    Name = "$salted_meat",
                    Description = "$salted_meat_description",
                    Amount = 1,
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Amount = 2,
                                Item = "CookedLoxMeat"
                            },
                            new RequirementConfig
                            {
                                Amount = 1,
                                Item = "CookedWolfMeat"
                            },
                            new RequirementConfig
                            {
                                Amount = 3,
                                Item = "SaltCube"
                            },
                        }

                });
                saltedMeat.ItemDrop.m_itemData.m_shared.m_food = 100f;
                saltedMeat.ItemDrop.m_itemData.m_shared.m_foodStamina = 35f;
                saltedMeat.ItemDrop.m_itemData.m_shared.m_foodBurnTime = 1800f;
                saltedMeat.ItemDrop.m_itemData.m_shared.m_foodRegen = 2f;
                var saltedMeatIcon = LoadEmbeddedSprite("SaltedMeat");
                saltedMeat.ItemDrop.m_itemData.m_shared.m_icons = new Sprite[] { saltedMeatIcon };
                ItemManager.Instance.AddItem(saltedMeat);

                var recipe_SaltedMeat10 = new CustomRecipe(new RecipeConfig
                {
                    Amount = 20,
                    Item = "SaltedMeat",
                    CraftingStation = "piece_cauldron",
                    MinStationLevel = 4,
                    Name = "Salted Meat x20",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                        {
                            Amount = 40,
                            Item = "CookedLoxMeat"
                        },
                        new RequirementConfig
                        {
                            Amount = 20,
                            Item = "CookedWolfMeat"
                        },
                        new RequirementConfig
                        {
                            Amount = 60,
                            Item = "SaltCube"
                        },
                    }
                });
                ItemManager.Instance.AddRecipe(recipe_SaltedMeat10);
                
                

                var obsidianGolemTrophy = bundle.LoadAsset<GameObject>("ObsidianGolemTrophy");
                ItemManager.Instance.AddItem(new CustomItem(obsidianGolemTrophy, true));

                var frometalWarhammer = new CustomItem(bundle.LoadAsset<GameObject>("FrometalWarhammer"), false, new ItemConfig
                {
                    Name = "$frometal_warhammer",
                    Description = "$frometal_warhammer_description",
                    CraftingStation = "reforger",
                    Requirements = new RequirementConfig[]
                {
                    new RequirementConfig
                    {
                        Item = "FrometalBar",
                        Amount = 15,
                        AmountPerLevel = 7
                    },
                    new RequirementConfig
                    {
                        Item = "Silver",
                        Amount = 10,
                        AmountPerLevel = 3
                    },
                    new RequirementConfig
                    {
                        Item = "ArcticWolfPelt",
                        Amount = 4,
                        AmountPerLevel = 2
                    },
                    new RequirementConfig
                    {
                        Item = "WorldTreeFragment",
                        Amount = 4,
                        AmountPerLevel = 2
                    },
                }
                });

                ItemManager.Instance.AddItem(frometalWarhammer);

                var frometalBuckler = new CustomItem(bundle.LoadAsset<GameObject>("ShieldFrometalBuckler"), false, new ItemConfig
                {
                    Name = "$frometal_buckler",
                    Description = "$frometal_buckler_description",
                    CraftingStation = "reforger",
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                        {
                            Item = "FrometalBar",
                            Amount = 8,
                            AmountPerLevel = 2
                        },
                        new RequirementConfig
                        {
                            Item = "WorldTreeFragment",
                            Amount = 4,
                            AmountPerLevel = 2
                        },
                    }
                });

                ItemManager.Instance.AddItem(frometalBuckler);

                var frometalShield = ItemManager.Instance.GetItem("ShieldFrometal");
                foreach (var renderer in frometalShield.ItemPrefab.GetComponentsInChildren<MeshRenderer>())
                {
                    List<Material> mats = new List<Material>();
                    foreach (var mat in renderer.materials)
                    {
                        var newMat = new Material(mat);
                        newMat.SetTexture("_BumpMap", LoadEmbeddedTexture("bam2"));
                        mats.Add(newMat);
                    }
                    renderer.materials = mats.ToArray();
                }

                var frometalShieldTower = ItemManager.Instance.GetItem("ShieldFrometalTower");
                foreach (var renderer in frometalShieldTower.ItemPrefab.GetComponentsInChildren<MeshRenderer>())
                {
                    List<Material> mats = new List<Material>();
                    foreach (var mat in renderer.materials)
                    {
                        var newMat = new Material(mat);
                        newMat.SetTexture("_BumpMap", LoadEmbeddedTexture("bam2"));
                        mats.Add(newMat);
                    }
                    renderer.materials = mats.ToArray();
                }



            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                PrefabManager.OnVanillaPrefabsAvailable -= AddItems;
                bundle.Unload(false);
            }
                        
        }

        
        private Sprite LoadEmbeddedSprite(string imageName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var resourceName = myAssembly.GetManifestResourceNames().SingleOrDefault(str => str.EndsWith($".{imageName}.png"));
            if (resourceName is null) throw new Exception($"{imageName}.png not found.");
            Jotunn.Logger.LogDebug($"Resource name : {resourceName}");

            Stream stream = myAssembly.GetManifestResourceStream(resourceName);
            var byteArray = ReadFully(stream);
            Jotunn.Logger.LogDebug($"Byte array length : {byteArray.Length}")
                ;
            var tex = new Texture2D(2, 2);
            tex.LoadImage(byteArray);
            var sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
            return sprite;
        }

        private Texture2D LoadEmbeddedTexture(string imageName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var resourceName = myAssembly.GetManifestResourceNames().Single(str => str.EndsWith($".{imageName}.png"));
            Jotunn.Logger.LogDebug($"Resource name : {resourceName}");

            Stream stream = myAssembly.GetManifestResourceStream(resourceName);
            var byteArray = ReadFully(stream);
            Jotunn.Logger.LogDebug($"Byte array length : {byteArray.Length}")
                ;
            var tex = new Texture2D(2, 2);
            tex.LoadImage(byteArray);
            return tex;
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}