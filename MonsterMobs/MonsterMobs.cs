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
        public const string PluginVersion = "0.0.4";
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
        }

        private void AddClonedItems()
        {
            // add a CharacterDrop to the TentaRoot
            PrefabManager.Cache.GetPrefab<GameObject>("TentaRoot").AddComponent<CharacterDrop>();

            var mobs = new List<Clone>
            {
                new Clone("Deathsquito", "AshMosquito", "AshMosquito"),
                new Clone("Neck", "AshNeck", "AshNeck"),
                new Clone("Fenring", "DevourerFenring", "DevourerFenring"),
                new Clone("Fenring_ragdoll", "DevourerFenring_ragdoll", "DevourerFenring"),
                new Clone("Troll", "Jotunn", "Jotunn"),
                new Clone("Troll_ragdoll", "Jotunn_ragdoll", "Jotunn"),
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

            //wolf cape
            //var arcticWolfCapeIcon = LoadEmbeddedSprite("ArcticWolfCapeIcon");
            //var arcticWolfCape = new CustomItem("ArcticWolfCape", "CapeWolf",
            //    new ItemConfig
            //    {
            //        Name = "Arctic Wolf Cape",
            //        Description = "The fur of an Arctic Wolf turned into a stylish cape.",
            //        CraftingStation = "reforger",
            //        Icons = new Sprite[] { arcticWolfCapeIcon },
            //        Requirements = new RequirementConfig[]
            //        {
            //            new RequirementConfig
            //            {
            //                Amount = 6,
            //                AmountPerLevel = 4,
            //                Item = "ArcticWolfPelt"
            //            },
            //            new RequirementConfig
            //            {
            //                Amount = 4,
            //                AmountPerLevel = 2,
            //                Item = "FrometalBar"
            //            },
            //            new RequirementConfig
            //            {
            //                Amount = 1,
            //                AmountPerLevel = 0,
            //                Item = "ArcticWolfTrophy"
            //            },
            //        }
            //    });
            //var arcticWolfCapeTexture = LoadEmbeddedTexture("ArcticWolfCape");
            ////arcticWolfCape.ItemPrefab.ChangeTexture(arcticWolfTexture);
            //foreach (var renderer in arcticWolfCape.ItemPrefab.GetComponentsInChildren<SkinnedMeshRenderer>().Where(s => s.name == "WolfCape_cloth"))
            //{
            //    List<Material> mats = new List<Material>();
            //    foreach (var mat in renderer.materials)
            //    {
            //        mats.Add(new Material(mat) { mainTexture = arcticWolfCapeTexture });
            //    }
            //    renderer.materials = mats.ToArray();
            //}

            //ItemManager.Instance.AddItem(arcticWolfCape);

            PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
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

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }

    public static class GameObjectExtensions
    {

        public static GameObject ChangeTexture(this GameObject go, Texture2D texture)
        {
            if (texture == null) Jotunn.Logger.LogInfo($"Texture is null for {go.name}");
            foreach (var renderer in go.GetComponentsInChildren<Renderer>())
            {
                renderer.materials = renderer.materials.Select(m => new Material(m) { mainTexture = texture }).ToArray();
            }
            return go;
        }

        public static GameObject ChangeMeshTexture(this GameObject go, Texture2D texture)
        {
            if (texture == null) Jotunn.Logger.LogInfo($"Texture is null for {go.name}");
            foreach (var renderer in go.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.materials = renderer.materials.Select(m => new Material(m) { mainTexture = texture }).ToArray();
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