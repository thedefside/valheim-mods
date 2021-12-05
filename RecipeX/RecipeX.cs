// RecipeX
// a Valheim mod skeleton using Jötunn
// 
// File:    RecipeX.cs
// Project: RecipeX

using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecipeX
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    internal class RecipeX : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.RecipeX";
        public const string PluginName = "RecipeX";
        public const string PluginVersion = "0.0.2";


        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();
        public static string configPath = Path.Combine(BepInEx.Paths.ConfigPath, "thedefside.RecipeX.json");

        private void Awake()
        {
            ItemManager.OnItemsRegisteredFejd += AddRecipes;
        }

        private void AddRecipes()
        {
            if (!File.Exists(configPath))
            {
                GenerateJsonFile();
                return;
            }

            var recipeMultipliers = GetJson();
            Jotunn.Logger.LogInfo($"Loaded {recipeMultipliers.Count} recipe multiplier(s)");

            foreach (var recipe in recipeMultipliers)
            {
                try
                {
                    if (recipe.Enabled)
                    {
                        Jotunn.Logger.LogDebug($"Loading new recipe based on {recipe.RecipeName} with a multiplier of {recipe.Multiplier}");
                        var originalRecipe = ObjectDB.instance.m_recipes.Single(r => r.name == recipe.RecipeName);
                        if (originalRecipe == null) throw new System.Exception($"The original recipe {recipe.RecipeName} was not found in the ObjectDB.");

                        var amount = originalRecipe.m_amount * recipe.Multiplier;
                        var recipeConfig = new RecipeConfig
                        {
                            Item = originalRecipe.m_item.name,
                            Name = originalRecipe.m_item.name + $"x{amount}",
                            CraftingStation = originalRecipe.m_craftingStation.name,
                            Amount = amount,
                            MinStationLevel = originalRecipe.m_minStationLevel,
                            Requirements = originalRecipe.m_resources.ToList().Select(r => new RequirementConfig { Amount = r.m_amount * recipe.Multiplier, Item = r.m_resItem.name }).ToArray()
                        };

                        var newRecipe = new CustomRecipe(recipeConfig);
                        ItemManager.Instance.AddRecipe(newRecipe); 
                    }
                }
                catch (System.Exception e)
                {
                    Jotunn.Logger.LogError($"Recipe could not be loaded for {recipe.RecipeName}: {e.Message}\n{e.StackTrace}");                    
                }
            }

        }

        internal static void GenerateJsonFile()
        {
            Jotunn.Logger.LogInfo($"Generating new config file with default recipes at {configPath}.");
            var defaultRecipes = ObjectDB.instance.m_recipes.Select(r => new RecipeMultiplier { Enabled = false, Multiplier = 1, RecipeName = r.name });
            var jsonText = SimpleJson.SimpleJson.SerializeObject(defaultRecipes);
            File.WriteAllText(configPath, jsonText);
        }

        internal static List<RecipeMultiplier> GetJson()
        {
            Jotunn.Logger.LogDebug($"Attempting to load config file from path {configPath}");
            var jsonText = AssetUtils.LoadText(configPath);
            Jotunn.Logger.LogDebug("File found. Attempting to deserialize...");
            return (List<RecipeMultiplier>)SimpleJson.SimpleJson.DeserializeObject(jsonText, typeof(List<RecipeMultiplier>));            
        }
    }
    internal class RecipeMultiplier
    {
        public string RecipeName { get; set; }
        public int Multiplier { get; set; }
        public bool Enabled { get; set; }
    }
}