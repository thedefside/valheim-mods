// GuckLantern
// a Valheim mod skeleton using Jötunn
// 
// File:    GuckLantern.cs
// Project: GuckLantern

using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GuckLantern
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("RockerKitten.BuildIt")]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class GuckLantern : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.GuckLantern";
        public const string PluginName = "GuckLantern";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            PrefabManager.OnPrefabsRegistered += AddClonedItems;
        }

        private void AddClonedItems()
        {
            try
            {
                var guckLantern = new CustomPiece("GuckLantern", "rk_lamp",
                    new PieceConfig 
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Guck Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Guck",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("GuckLamp")
                    });

                ChangeColor(guckLantern, Color.green);
                PieceManager.Instance.AddPiece(guckLantern);

                var guckHangingLantern = new CustomPiece("GuckHangingLantern", "rk_lamphanging",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Guck Hanging Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Guck",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("GuckLampHanging")
                    });

                ChangeColor(guckHangingLantern, Color.green);
                PieceManager.Instance.AddPiece(guckHangingLantern);

                var guckLamppost = new CustomPiece("GuckLampPost", "rk_lamppost",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Guck Lamp Post",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Guck",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("GuckLampPost")
                    });

                ChangeColor(guckLamppost, Color.green);
                PieceManager.Instance.AddPiece(guckLamppost);

                
                var bloodLantern = new CustomPiece("BloodLantern", "rk_lamp",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Blood Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Bloodbag",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("BloodLamp")
                    });
                ChangeColor(bloodLantern, Color.red);
                PieceManager.Instance.AddPiece(bloodLantern);

                var bloodHangingLantern = new CustomPiece("BloodHangingLantern", "rk_lamphanging",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Blood Hanging Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Bloodbag",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("BloodLampHanging")
                    });
                ChangeColor(bloodHangingLantern, Color.red);
                PieceManager.Instance.AddPiece(bloodHangingLantern);

                var bloodLampPost = new CustomPiece("BloodLampPost", "rk_lamppost",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Blood Lamp Post",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Bloodbag",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("BloodLampPost")
                    });
                ChangeColor(bloodLampPost, Color.red);
                PieceManager.Instance.AddPiece(bloodLampPost);

                var frostLantern = new CustomPiece("FrostLantern", "rk_lamp",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Frost Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "FreezeGland",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("FrostLamp")
                    });
                ChangeColor(frostLantern, Color.blue);
                PieceManager.Instance.AddPiece(frostLantern);

                var frostHangingLantern = new CustomPiece("FrostHangingLantern", "rk_lamphanging",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Frost Hanging Lantern",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "FreezeGland",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("FrostLampHanging")
                    });
                ChangeColor(frostHangingLantern, Color.blue);
                PieceManager.Instance.AddPiece(frostHangingLantern);

                var frostLampPost = new CustomPiece("FrostLampPost", "rk_lamppost",
                    new PieceConfig
                    {
                        PieceTable = "_RKCustomTable",
                        Name = "Frost Lamp Post",
                        Category = "Furniture",
                        Requirements = new RequirementConfig[]
                        {
                            new RequirementConfig
                            {
                                Item = "Tin",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "FreezeGland",
                                Amount = 2,
                                Recover = true
                            }
                        },
                        Icon = LoadEmbeddedSprite("FrostLampPost")
                    });
                ChangeColor(frostLampPost, Color.blue);
                PieceManager.Instance.AddPiece(frostLampPost);

            }
            catch (Exception e)
            {
                Jotunn.Logger.LogError(e.Message + e.StackTrace);
            }
            finally
            {
                PrefabManager.OnPrefabsRegistered -= AddClonedItems;
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

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void ChangeColor(CustomPiece go, Color color)
        {
            go.PiecePrefab.GetComponentInChildren<Light>().color = color;
            foreach (var item in go.PiecePrefab.GetComponentsInChildren<MeshRenderer>())
            {
                item.material = new Material(item.material) { color = color };
            }
        }

    }
}