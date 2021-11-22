using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackMetalBuildPieces
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class JotunnModStub : BaseUnityPlugin
    {
        public const string PluginGUID = "thedefside.BlackMetalBuildPieces";
        public const string PluginName = "BlackMetalBuildPieces";
        public const string PluginVersion = "0.0.3";
                
        private void Awake()
        {
            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;            
        }

        private void AddClonedItems()
        {
            try
            {
                var pieces = new List<CustomPiece>();

                var woodblackmetal_beam = new CustomPiece("woodblackmetal_beam", "woodiron_beam",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_woodblackmetal_beam_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Wood",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });

                pieces.Add(woodblackmetal_beam);


                CustomPiece woodblackmetal_pole = new CustomPiece("woodblackmetal_pole", "woodiron_pole",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_woodblackmetal_pole_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Wood",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });
                pieces.Add(woodblackmetal_pole);


                var greenblackmetal_beam = new CustomPiece("greenblackmetal_beam", "woodiron_beam",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_woodgreen_beam_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "ElderBark",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });

                greenblackmetal_beam.MakeItGreen();
                pieces.Add(greenblackmetal_beam);


                CustomPiece greenblackmetal_pole = new CustomPiece("greenblackmetal_pole", "woodiron_pole",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_woodgreen_pole_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "ElderBark",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });
                greenblackmetal_pole.MakeItGreen();
                pieces.Add(greenblackmetal_pole);

                //iron_floor_1x1
                CustomPiece blackmetal_floor_1x1 = new CustomPiece("blackmetal_floor_1x1", "iron_floor_1x1",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalfloor_1x1_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Recover = true
                            }
                        }
                    });
                blackmetal_floor_1x1.MakeItGreen();
                pieces.Add(blackmetal_floor_1x1);

                //iron_floor_2x2
                CustomPiece blackmetal_floor_2x2 = new CustomPiece("blackmetal_floor_2x2", "iron_floor_2x2",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalfloor_2x2_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });
                blackmetal_floor_2x2.MakeItGreen();
                pieces.Add(blackmetal_floor_2x2);

                //iron_grate
                CustomPiece blackmetal_gate = new CustomPiece("blackmetal_gate", "iron_grate",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalgate_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 4,
                                Recover = true
                            }
                        }
                    });
                
                var door = blackmetal_gate.PiecePrefab.GetComponent<Door>();
                door.m_name = "$piece_blackmetalgate_name";
                foreach (var renderer in door.GetComponentsInChildren<Renderer>())
                {
                    renderer.materials = renderer.materials.Select(m => new Material(m) { color = Color.green }).ToArray();
                }

                blackmetal_gate.MakeItGreen();
                pieces.Add(blackmetal_gate);

                //iron_wall_1x1
                CustomPiece blackmetal_wall_1x1 = new CustomPiece("blackmetal_wall_1x1", "iron_wall_1x1",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalwall_1x1_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 1,
                                Recover = true
                            }
                        }
                    });
                blackmetal_wall_1x1.MakeItGreen();
                pieces.Add(blackmetal_wall_1x1);

                //iron_wall_2x2
                CustomPiece blackmetal_wall_2x2 = new CustomPiece("blackmetal_wall_2x2", "iron_wall_2x2",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalwall_2x2_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });
                blackmetal_wall_2x2.MakeItGreen();
                pieces.Add(blackmetal_wall_2x2);

                //piece_cookingstation_iron
                CustomPiece piece_cookingstation_blackmetal = new CustomPiece("piece_cookingstation_blackmetal", "piece_cookingstation_iron",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalcookingrack_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 3,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Chain",
                                Amount = 3,
                                Recover = true
                            }
                        }

                    });
                var cookingStation = piece_cookingstation_blackmetal.PiecePrefab.GetComponent<CookingStation>();
                cookingStation.m_name = "$piece_blackmetalcookingrack_name";

                piece_cookingstation_blackmetal.MakeItGreen();
                pieces.Add(piece_cookingstation_blackmetal);

                // piece_cauldron
                CustomPiece piece_cauldron_blackmetal = new CustomPiece("piece_cauldron_blackmetal", "piece_cauldron",
                    new PieceConfig
                    {
                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetalcauldron_name",
                        Description = "$piece_blackmetalcauldron_name",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 10,
                                Recover = true
                            }
                        }

                    });
                
                piece_cauldron_blackmetal.MakeItGreen();
                pieces.Add(piece_cauldron_blackmetal);

                // piece_groundtorch_green
                CustomPiece piece_blackmetal_groundtorch_green = new CustomPiece("piece_blackmetal_groundtorch_green", "piece_groundtorch_green",
                    new PieceConfig
                    {

                        PieceTable = "Hammer",
                        Category = "Black Metal",
                        Name = "$piece_blackmetal_groundtorch_green",
                        Requirements = new[]
                        {
                            new RequirementConfig
                            {
                                Item = "BlackMetal",
                                Amount = 2,
                                Recover = true
                            },
                            new RequirementConfig
                            {
                                Item = "Guck",
                                Amount = 2,
                                Recover = true
                            }
                        }
                    });
                piece_blackmetal_groundtorch_green.MakeItGreen();
                Fireplace fireplace = piece_blackmetal_groundtorch_green.PiecePrefab.GetComponent<Fireplace>();
                fireplace.m_name = "$piece_blackmetal_groundtorch_green";
                pieces.Add(piece_blackmetal_groundtorch_green);

                foreach (var piece in pieces)
                {
                    piece.Piece.GetComponent<WearNTear>().m_health *= 1.2f;
                    PieceManager.Instance.AddPiece(piece);
                }

            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while adding cloned item: {ex.Message}");
                Jotunn.Logger.LogError(ex.StackTrace);
            }
            finally
            {
                // You want that to run only once, Jotunn has the item cached for the game session
                PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
            }
        }

    }



    internal static class PieceExtentions
    {
        public static CustomPiece MakeItGreen(this CustomPiece piece)
        {
            foreach (var renderer in piece.PiecePrefab.GetComponentsInChildren<Renderer>())
            {
                renderer.materials = renderer.materials.Select(m => new Material(m) { color = new Color(0, 0.5f, 0, 1) }).ToArray();
            }

            return piece;

        }
    }
}