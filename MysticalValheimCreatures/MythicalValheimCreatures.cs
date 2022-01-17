// MysticalValheimCreatures
// a Valheim mod skeleton using Jötunn
// 
// File:    MysticalValheimCreatures.cs
// Project: MysticalValheimCreatures

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

namespace MysticalValheimCreatures
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MysticalValheimCreatures : BaseUnityPlugin
    {
        public const string PluginGUID = "Fjalgard.MythicalValheimCreatures";
        public const string PluginName = "MysticalValheimCreatures";
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
            var mobs = new List<Clone>
            {
                new Clone("Serpent", "Nidhoeggr", "Nidhoeggr")
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

            // wolf trophy
            var nidhoeggrTrophyIcon = LoadEmbeddedSprite("TrophyNidhoeggr");
            var trophyNidhoeggr = new CustomItem("TrophyNidhoeggr", "TrophySerpent",
                new ItemConfig
                {
                    Name = "Nidhoeggr Trophy",
                    Description = "The head of Nidhoeggr",
                    Icons = new Sprite[] { nidhoeggrTrophyIcon }
                });
            var nidhoeggrTrophyTexture = LoadEmbeddedTexture("Nidhoeggr");
            trophyNidhoeggr.ItemPrefab.ChangeMeshTexture(nidhoeggrTrophyTexture);

            ItemManager.Instance.AddItem(trophyNidhoeggr);


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