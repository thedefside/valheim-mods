using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
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
        public const string PluginVersion = "0.0.1";
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;
        }

        private void AddClonedItems()
        {
            
            var mobs = new Dictionary<string, string> 
            {
                { "AshHatchling", "Hatchling" },
                { "AshMosquito", "Deathsquito" },
                { "AshNeck", "Neck" },
                { "DevourerFenring", "Fenring" },
                { "Jotunn", "Troll" },
                { "PolarFenring", "Fenring" },
                { "PolarLox", "Lox" },
                { "StormFenring", "Fenring" },
                { "StormHatchling", "Hatchling" },
                { "StormWolf", "Wolf" },
                { "SwollenBody", "Draugr" },
                { "BurnedBones", "SkeletonWarrior" },
                { "DarkSpider", "TreeSpider" },
                { "MotherDarkSpider", "TreeSpider" }

            };

            foreach (var mob in mobs)
            {
                try
                {
                    Jotunn.Logger.LogDebug($"Creating new Prefab: {mob.Key} from {mob.Value}");
                    var prefab = PrefabManager.Instance.CreateClonedPrefab(mob.Key, mob.Value);

                    Jotunn.Logger.LogDebug($"Setting texture: {mob.Key}.png");
                    var newTexture = LoadEmbeddedTexture(mob.Key);
                    prefab.ChangeTexture(newTexture);

                    Jotunn.Logger.LogInfo($"Add Prefab: {mob.Key}");
                    PrefabManager.Instance.AddPrefab(prefab);
                }
                catch (System.Exception e)
                {
                    Jotunn.Logger.LogError($"Adding prefab failed for {mob}");
                    Jotunn.Logger.LogError(e.Message);
                    Jotunn.Logger.LogError(e.StackTrace);
                }
            }

            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;

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

    public static class GemObjectExtensions
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
    }
}