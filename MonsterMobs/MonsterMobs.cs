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

namespace MonsterMobs
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("DYBAssets", "1.7.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MonsterMobs : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.MonsterMobs";
        public const string PluginName = "MonsterMobs";
        public const string PluginVersion = "1.0.1";

        public static bool Debug = false;

        private AssetBundle bundle;
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            bundle = AssetUtils.LoadAssetBundleFromResources("mmbundle", typeof(MonsterMobs).Assembly);
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
        }

        private void AddClonedItems()
        {
            // add a CharacterDrop to the TentaRoot
            PrefabManager.Cache.GetPrefab<GameObject>("TentaRoot").AddComponent<CharacterDrop>();

            var mobs = new List<Clone>
            {
                new Clone("Deathsquito", "AshMosquito", "AshMosquito"),
                new Clone("Fenring", "DevourerFenring", "DevourerFenring"),
                new Clone("Fenring_ragdoll", "DevourerFenring_ragdoll", "DevourerFenring"),
                new Clone("Troll", "Jotunn_mm", "Jotunn"),
                new Clone("Troll_ragdoll", "Jotunn_ragdoll_mm", "Jotunn"),
                new Clone("Fenring", "PolarFenring", "PolarFenring"),
                new Clone("Fenring_ragdoll", "PolarFenring_ragdoll", "PolarFenring"),
                new Clone("Lox", "PolarLox", "PolarLox"),
                new Clone("lox_ragdoll", "PolarLox_ragdoll", "PolarLox"),
                new Clone("Fenring", "StormFenring", "StormFenring"),
                new Clone("Fenring_ragdoll", "StormFenring_ragdoll", "StormFenring"),
                new Clone("Wolf", "StormWolf", "StormWolf"),
                new Clone("Wolf_Ragdoll", "StormWolf_ragdoll", "StormWolf"),
                new Clone("Draugr", "SwollenBody", "SwollenBody"),
                new Clone("Draugr_ragdoll", "SwollenBody_ragdoll", "SwollenBody"),                
                new Clone("SkeletonWarrior", "BurnedBones", "BurnedBones"),
                new Clone("TreeSpider", "DarkSpider", "DarkSpider"),
                new Clone("TreeSpider_ragdoll", "DarkSpider_ragdoll", "DarkSpider"),
                new Clone("TreeSpider", "MotherDarkSpider", "MotherDarkSpider"),
                new Clone("Wolf", "ArcticWolf", "ArcticWolf"),
                new Clone("Wolf_Ragdoll", "ArcticWolf_ragdoll", "ArcticWolf"),
                new Clone("TentaRoot", "Froot", "Froot")

            };

            foreach (var mob in mobs)
            {
                try
                {
                    Jotunn.Logger.LogDebug($"Creating new Prefab: {mob.NewPrefab} from {mob.BasePrefab}");
                    var prefab = PrefabManager.Instance.CreateClonedPrefab(mob.NewPrefab, mob.BasePrefab);

                    Jotunn.Logger.LogDebug($"Setting texture: {mob.Texture}.png");
                    var newTexture = LoadEmbeddedTexture(mob.Texture);
                    prefab.ChangeTexture(newTexture);

                    Jotunn.Logger.LogDebug($"Add Prefab: {mob.NewPrefab}");
                    PrefabManager.Instance.AddPrefab(prefab);
                }
                catch (System.Exception e)
                {
                    Jotunn.Logger.LogError($"Adding prefab failed for {mob.NewPrefab}");
                    Jotunn.Logger.LogError(e.Message);
                    Jotunn.Logger.LogError(e.StackTrace);
                }

            }

            var ashHatchling = bundle.LoadAsset<GameObject>("AshHatchling");
            PrefabManager.Instance.AddPrefab(ashHatchling);

            var ashHatchlingRagdoll = PrefabManager.Instance.CreateClonedPrefab("AshHatchling_ragdoll", "Hatchling_ragdoll");
            var hatchling_Mountain = ashHatchlingRagdoll.transform.Find("Hatchling_mountain");
            var eyes = hatchling_Mountain.transform.Find("eyes").GetComponent<SkinnedMeshRenderer>();
            var ashHatchling_mat = new Material(eyes.material);
            ashHatchling_mat.SetTexture("_MainTex", LoadEmbeddedTexture("AshHatchling"));
            ashHatchling_mat.SetTexture("_BumpMap", LoadEmbeddedTexture("Hatchling_n"));
            ashHatchling_mat.SetTexture("_EmissionMap", LoadEmbeddedTexture("Hatchling_E"));
            eyes.material = ashHatchling_mat;
            hatchling_Mountain.transform.Find("Hatchling").GetComponent<SkinnedMeshRenderer>().material = ashHatchling_mat;
            hatchling_Mountain.transform.Find("Hatchling.001").GetComponent<SkinnedMeshRenderer>().material = ashHatchling_mat;
            hatchling_Mountain.transform.Find("Horns").GetComponent<SkinnedMeshRenderer>().material = ashHatchling_mat;
            PrefabManager.Instance.AddPrefab(ashHatchlingRagdoll);

            ashHatchling.GetComponent<Humanoid>().m_deathEffects.m_effectPrefabs = new EffectList.EffectData[]
            {
                new EffectList.EffectData { m_prefab = ashHatchlingRagdoll},
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("vfx_hatchling_death")},
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("sfx_hatchling_death")}
            };

            var stormHatchling = bundle.LoadAsset<GameObject>("StormHatchling");
            PrefabManager.Instance.AddPrefab(stormHatchling);

            var stormHatchlingRagdoll = PrefabManager.Instance.CreateClonedPrefab("StormHatchling_ragdoll", "Hatchling_ragdoll");
            var stormhatchling_Mountain = stormHatchlingRagdoll.transform.Find("Hatchling_mountain");
            var stormeyes = hatchling_Mountain.transform.Find("eyes").GetComponent<SkinnedMeshRenderer>();
            var stormHatchling_mat = new Material(stormeyes.material);
            stormHatchling_mat.SetTexture("_MainTex", LoadEmbeddedTexture("StormHatchling"));
            stormHatchling_mat.SetTexture("_BumpMap", LoadEmbeddedTexture("Hatchling_n"));
            stormHatchling_mat.SetTexture("_Emission", LoadEmbeddedTexture("Hatchling_E"));
            stormeyes.material = stormHatchling_mat;
            stormhatchling_Mountain.transform.Find("Hatchling").GetComponent<SkinnedMeshRenderer>().material = stormHatchling_mat;
            stormhatchling_Mountain.transform.Find("Hatchling.001").GetComponent<SkinnedMeshRenderer>().material = stormHatchling_mat;
            stormhatchling_Mountain.transform.Find("Horns").GetComponent<SkinnedMeshRenderer>().material = stormHatchling_mat;
            PrefabManager.Instance.AddPrefab(stormHatchlingRagdoll);

            stormHatchling.GetComponent<Humanoid>().m_deathEffects.m_effectPrefabs = new EffectList.EffectData[]
            {
                new EffectList.EffectData { m_prefab = stormHatchlingRagdoll },
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("vfx_hatchling_death") },
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("sfx_hatchling_death") }
            };

            var ashNeck = bundle.LoadAsset<GameObject>("AshNeck");
            PrefabManager.Instance.AddPrefab(ashNeck);
            
            var ashNeckRagdoll = PrefabManager.Instance.CreateClonedPrefab("AshNeck_ragdoll1", "Neck_Ragdoll");
            var bodyRenderer = ashNeckRagdoll.transform.Find("Body").GetComponent<SkinnedMeshRenderer>();
            var newMat = new Material(bodyRenderer.material);
            newMat.SetTexture("_MainTex", LoadEmbeddedTexture("AshNeck"));
            bodyRenderer.material = newMat;
            var lilliesRenderer = ashNeckRagdoll.transform.Find("Lillies").GetComponent<SkinnedMeshRenderer>();
            lilliesRenderer.material = newMat;
            PrefabManager.Instance.AddPrefab(ashNeckRagdoll);

            ashNeck.GetComponent<Humanoid>().m_deathEffects.m_effectPrefabs = new EffectList.EffectData[]
            {
                new EffectList.EffectData { m_prefab = ashNeckRagdoll },
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("vfx_neck_death")},
                new EffectList.EffectData { m_prefab = PrefabManager.Instance.GetPrefab("sfx_dragon_death")}
            };

            var ashNeckTrophy = bundle.LoadAsset<GameObject>("AshNeckTrophy");
            ashNeckTrophy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(0, 8);
            ItemManager.Instance.AddItem(new CustomItem(ashNeckTrophy, false));

            var fx_polarlox_death = PrefabManager.Instance.CreateClonedPrefab("fx_polarlox_death", "fx_lox_death");
            fx_polarlox_death.transform.Find("skinflakes").GetComponent<ParticleSystem>().startColor = Color.white;
            PrefabManager.Instance.AddPrefab(fx_polarlox_death);

            var polarLox = PrefabManager.Instance.GetPrefab("PolarLox");
            var polarLox_ragdoll = PrefabManager.Instance.GetPrefab("PolarLox_ragdoll");
            polarLox.GetComponent<Humanoid>().m_deathEffects.m_effectPrefabs = new EffectList.EffectData[]
            {
                new EffectList.EffectData { m_prefab = polarLox_ragdoll },
                new EffectList.EffectData { m_prefab = fx_polarlox_death }
            };

            //wolf pelt
            var arcticWolfPeltIcon = LoadEmbeddedSprite("ArcticWolfPeltIcon");
            var arcticWolfPelt = new CustomItem("ArcticWolfPelt", "WolfPelt", 
                new ItemConfig 
                { 
                    Name = "Arctic Wolf Pelt", 
                    Description = "The fur of an Arctic Wolf", 
                    Icons = new Sprite[] { arcticWolfPeltIcon } 
                });
            var arcticWolfTexture = LoadEmbeddedTexture("ArcticWolfPelt");
            arcticWolfPelt.ItemPrefab.ChangeMeshTexture(arcticWolfTexture);
            ItemManager.Instance.AddItem(arcticWolfPelt);

            // wolf trophy
            var arcticWolfTrophyIcon = LoadEmbeddedSprite("ArcticWolfTrophy");
            var arcticWolfTrophy = new CustomItem("ArcticWolfTrophy", "TrophyWolf", 
                new ItemConfig 
                { 
                    Name = "Arctic Wolf Trophy", 
                    Description = "The head of the beast", 
                    Icons = new Sprite[] { arcticWolfTrophyIcon } 
                });
            var arcticWolfTrophyTexture = LoadEmbeddedTexture("ArcticWolf");
            arcticWolfTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(0, 7);
            arcticWolfTrophy.ItemPrefab.ChangeMeshTexture(arcticWolfTrophyTexture);

            ItemManager.Instance.AddItem(arcticWolfTrophy);

            //wolf rug
            var arcticWolfRugIcon = LoadEmbeddedSprite("ArcticWolfRugIcon");
            var arcticWolfRug = new CustomPiece("ArcticWolfRug", "rug_wolf", 
                new PieceConfig 
                { 
                    Name = "Arctic Wolf Rug", 
                    Description = "A rug made from the fur of the frozen beast.", 
                    PieceTable = "Hammer", 
                    Requirements = new RequirementConfig[]
                    {
                        new RequirementConfig
                        {
                            Amount = 4,
                            Item = "ArcticWolfPelt",
                            Recover = true
                        }
                    },
                    Icon = arcticWolfRugIcon
                    
                });
            
            foreach (var renderer in arcticWolfRug.PiecePrefab.GetComponentsInChildren<MeshRenderer>())
            {
                List<Material> mats = new List<Material>();
                foreach (var mat in renderer.materials)
                {
                    mats.Add(new Material(mat) { color = new Color(1.5f, 1.5f, 1.5f) });
                }
                renderer.materials = mats.ToArray();
            }
            PieceManager.Instance.AddPiece(arcticWolfRug);


            var stormWolfTrophyIcon = LoadEmbeddedSprite("StormWolfTrophy");
            var stormWolfTrophy = new CustomItem("StormWolfTrophy", "TrophyWolf",
                new ItemConfig
                {
                    Name = "Storm Wolf Trophy",
                    Description = "The head of a Storm Wolf",
                    Icons = new Sprite[] { stormWolfTrophyIcon }
                });
            var stormWolfTrophyTexture = LoadEmbeddedTexture("StormWolf");
            stormWolfTrophy.ItemPrefab.ChangeMeshTexture(stormWolfTrophyTexture);
            stormWolfTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(2, 5);
            ItemManager.Instance.AddItem(stormWolfTrophy);

            var stormHatchlingTrophy = bundle.LoadAsset<GameObject>("StormHatchlingTrophy");
            stormHatchlingTrophy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(3, 5);
            ItemManager.Instance.AddItem(new CustomItem(stormHatchlingTrophy, false));

            var ashHatchlingTrophy = bundle.LoadAsset<GameObject>("AshHatchlingTrophy");
            ashHatchlingTrophy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(1, 8);
            ItemManager.Instance.AddItem(new CustomItem(ashHatchlingTrophy, false));

            var stormWolfPelt = bundle.LoadAsset<GameObject>("StormWolfPelt");
            ItemManager.Instance.AddItem(new CustomItem(stormWolfPelt, false, new ItemConfig { Name = "Storm Wolf Pelt", Description = "The pelt of a Storm Wolf" }));

            var stormWolfRug = bundle.LoadAsset<GameObject>("StormWolfRug");
            var piece_StormWolfRug = new CustomPiece(stormWolfRug, false, new PieceConfig
            {
                CraftingStation = "piece_workbench",
                PieceTable = "Hammer",
                Category = "Furniture",
                Name = "Storm Wolf Rug",
                Description = "It's soft and warm and makes your hair stand on end",
                Requirements = new RequirementConfig[]
                {
                        new RequirementConfig
                        {
                            Amount = 4,
                            Item = "StormWolfPelt",
                            Recover = true
                        }
                }
            });
            PieceManager.Instance.AddPiece(piece_StormWolfRug);

            var stormWolfCape = bundle.LoadAsset<GameObject>("StormWolfCape");
            ItemManager.Instance.AddItem(new CustomItem(stormWolfCape, false, new ItemConfig
            {
                Amount = 1,
                CraftingStation = "piece_workbench",
                MinStationLevel = 1,
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig
                    {
                        Amount = 8,
                        Item = "StormWolfPelt",
                        AmountPerLevel = 2
                    },
                    new RequirementConfig
                    {
                        Amount = 1,
                        Item = "StormWolfTrophy"
                    },
                    new RequirementConfig
                    {
                        Amount = 1,
                        Item = "Thunderstone",
                        AmountPerLevel = 1
                    }
                }
                
            }));

            var ArcticWolfCape = bundle.LoadAsset<GameObject>("ArcticWolfCape");
            ItemManager.Instance.AddItem(new CustomItem(ArcticWolfCape, false, new ItemConfig
            {
                Amount = 1,
                CraftingStation = "reforger",
                MinStationLevel = 1,
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig
                    {
                        Amount = 8,
                        Item = "ArcticWolfPelt",
                        AmountPerLevel = 2
                    },
                    new RequirementConfig
                    {
                        Amount = 1,
                        Item = "ArcticWolfTrophy"
                    },
                    new RequirementConfig
                    {
                        Amount = 1,
                        Item = "FrometalBar",
                        AmountPerLevel = 1
                    }
                }

            }));

            var stormFenringTrophyIcon = LoadEmbeddedSprite("StormFenringTrophy");
            var stormFenringTrophy = new CustomItem("StormFenringTrophy", "TrophyFenring", new ItemConfig
            {
                Amount = 1,
                Name = "Storm Fenring Trophy",
                Description = "All that remains from the mighty beast",
                Icons = new Sprite[] { stormFenringTrophyIcon }
            });
            
            stormFenringTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("StormFenring"));
            stormFenringTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(4, 5);
            ItemManager.Instance.AddItem(stormFenringTrophy);

            var devourerFenringTrophyIcon = LoadEmbeddedSprite("DevourerFenringTrophy");
            var devourerFenringTrophy = new CustomItem("DevourerFenringTrophy", "TrophyFenring", new ItemConfig
            {
                Amount = 1,
                Name = "Blood-thirsty Fenring Trophy",
                Description = "A blood-soaked arm of a Fenring",
                Icons = new Sprite[] { devourerFenringTrophyIcon }
            });

            devourerFenringTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("DevourerFenring"));
            devourerFenringTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(1, 7);
            ItemManager.Instance.AddItem(devourerFenringTrophy);

            var polarLoxTrophyIcon = LoadEmbeddedSprite("PolarLoxTrophy");
            var polarLoxTrophy = new CustomItem("PolarLoxTrophy", "TrophyLox", new ItemConfig
            {
                Name = "Ice-breaker Lox Trophy",
                Description = "The head of a Polar Lox",
                Icons = new Sprite[] { polarLoxTrophyIcon }
            });
            polarLoxTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("PolarLox"));
            polarLoxTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(2, 7);
            ItemManager.Instance.AddItem(polarLoxTrophy);

            var burnedBonesTrophyIcon = LoadEmbeddedSprite("BurnedBonesTrophy");
            var burnedBonesTrophy = new CustomItem("BurnedBonesTrophy", "TrophySkeleton", new ItemConfig
            {
                Name = "Burned Bones Trophy",
                Description = "The head of a Burned Bones",
                Icons = new Sprite[] { burnedBonesTrophyIcon }
            });
            burnedBonesTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("BurnedBonesTex"));
            burnedBonesTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(2, 8);
            ItemManager.Instance.AddItem(burnedBonesTrophy);

            var frozenCorpseTrophyIcon = LoadEmbeddedSprite("FrozenCorpseTrophy");
            var frozenCorpseTrophy = new CustomItem("FrozenCorpseTrophy", "TrophySkeleton", new ItemConfig
            {
                Name = "Frozen Corpse Trophy",
                Description = "The head of a Frozen Corpse",
                Icons = new Sprite[] { frozenCorpseTrophyIcon }
            });
            frozenCorpseTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("FrozenCorpse"));
            frozenCorpseTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(5, 7);
            ItemManager.Instance.AddItem(frozenCorpseTrophy);

            var swollenBodyTrophyIcon = LoadEmbeddedSprite("SwollenBodyTrophy");
            var swollenBodyTrophy = new CustomItem("SwollenBodyTrophy", "TrophyDraugr", new ItemConfig
            {
                Name = "Swollen Body Trophy",
                Description = "A foul piece of rotting flesh",
                Icons = new Sprite[] { swollenBodyTrophyIcon }
            });
            swollenBodyTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("SwollenBody"));
            swollenBodyTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(4, 8);
            ItemManager.Instance.AddItem(swollenBodyTrophy);

            var polarFenringTrophyIcon = LoadEmbeddedSprite("PolarFenringTrophy");
            var polarFenringTrophy = new CustomItem("PolarFenringTrophy", "TrophyFenring", new ItemConfig
            {
                Name = "Pale-coat Fenring Trophy",
                Description = "The arm of a Polar Fenring",
                Icons = new Sprite[] { polarFenringTrophyIcon }
            });
            polarFenringTrophy.ItemPrefab.ChangeMeshTexture(LoadEmbeddedTexture("PolarFenring"));
            polarFenringTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(6, 7);
            ItemManager.Instance.AddItem(polarFenringTrophy);


            var jotunnTrophy = bundle.LoadAsset<GameObject>("JotunnTrophy");
            jotunnTrophy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(3, 7);
            ItemManager.Instance.AddItem(new CustomItem(jotunnTrophy, false));

            var smallPolarSerpentTrophy = bundle.LoadAsset<GameObject>("SmallPolarSerpentTrophy");
            smallPolarSerpentTrophy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(4, 7);
            ItemManager.Instance.AddItem(new CustomItem(smallPolarSerpentTrophy, false));

            var silverGolemTrophyIcon = LoadEmbeddedSprite("SilverGolemTrophyIcon");
            var silverGolemTrophy = new CustomItem("SilverGolemTrophy", "TrophySGolem", new ItemConfig
            {
                Name = "Silver Golem Trophy",
                Description = "The massive head of a defeated Silver Golem",
                Icons = new Sprite[] { silverGolemTrophyIcon }
            });
            DestroyImmediate(silverGolemTrophy.ItemPrefab.transform.Find("attach").transform.Find("Particle System").gameObject);
            silverGolemTrophy.ItemPrefab.GetComponentInChildren<Light>().color = Color.gray;
            silverGolemTrophy.ItemPrefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(4, 6);
            foreach (var renderer in silverGolemTrophy.ItemPrefab.GetComponentsInChildren<MeshRenderer>())
            {
                List<Material> mats = new List<Material>();
                foreach (var mat in renderer.materials)
                {
                    mats.Add(new Material(mat) { color = Color.gray });
                }
                renderer.materials = mats.ToArray();
            }
            ItemManager.Instance.AddItem(silverGolemTrophy);

            var TrophySvartalfrQueen = PrefabManager.Instance.GetPrefab("TrophySvartalfrQueen");
            TrophySvartalfrQueen.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(3, 6);

            var TrophyBlazingDamnedOne = PrefabManager.Instance.GetPrefab("TrophyBlazingDamnedOne");
            TrophyBlazingDamnedOne.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(3, 8);

            var CursedEffigy = PrefabManager.Instance.GetPrefab("CursedEffigy");
            CursedEffigy.GetComponent<ItemDrop>().m_itemData.m_shared.m_trophyPos = new Vector2Int(2, 6);

            PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
            bundle.Unload(false);
        }

        private Texture2D LoadEmbeddedTexture(string imageName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var resourceName = myAssembly.GetManifestResourceNames().Single(str => str == $"MonsterMobs.Assets.{imageName}.png");
            Jotunn.Logger.LogDebug($"Resource name : {resourceName}");
            
            Stream stream = myAssembly.GetManifestResourceStream(resourceName);
            var byteArray = ReadFully(stream);
            Jotunn.Logger.LogDebug($"Byte array length : {byteArray.Length}")
                ;
            var tex = new Texture2D(2, 2);
            tex.LoadImage(byteArray);
            return tex;
        }

        private Sprite LoadEmbeddedSprite(string imageName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var resourceName = myAssembly.GetManifestResourceNames().SingleOrDefault(str => str == $"MonsterMobs.Assets.Sprites.{imageName}.png");
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

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static void WriteChildren(Transform go )
        {
            if (go.childCount == 0)
            {
                DebugLog(go.name);
            }
            else
            {
                for (int i = 0; i < go.childCount; i++)
                {
                    var child = go.GetChild(i);
                    DebugLog($"{go.name} {i} {child.name}");
                    WriteChildren(child.transform);
                }
            }
            
        }

        public static void DebugLog(string message)
        {
            if (Debug) Jotunn.Logger.LogInfo($"DEBUG | {message}");
        }

    }

    public static class GameObjectExtensions
    {

        public static GameObject ChangeTexture(this GameObject go, Texture2D texture)
        {
            MonsterMobs.DebugLog($"Changing texture for {go.name}");
            if (texture == null) MonsterMobs.DebugLog($"Texture is null for {go.name}");
            foreach (var renderer in go.GetComponentsInChildren<Renderer>())
            {
                MonsterMobs.DebugLog($"Changing texture for material {renderer.name}");
                renderer.materials = renderer.materials.Select(m => new Material(m) { mainTexture = texture }).ToArray();
            }
            return go;
        }

        public static GameObject ChangeMeshTexture(this GameObject go, Texture2D texture)
        {
            
            foreach (var renderer in go.GetComponentsInChildren<MeshRenderer>())
            {
                List<Material> mats = new List<Material>();
                foreach (var mat in renderer.materials)
                {
                    mats.Add(new Material(mat) { mainTexture = texture });
                }
                renderer.materials = mats.ToArray();
            }

            return go;
        }

    }

    public struct Clone
    {
        public Clone(string basePrefab, string newPrefab, string texture)
        {
            BasePrefab = basePrefab;
            NewPrefab = newPrefab;
            Texture = texture;
        }

        public string BasePrefab { get; set; }
        public string NewPrefab { get; set; }
        public string Texture { get; set; }
    }
}