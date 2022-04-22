using UnityEngine;

namespace jtv
{

    [HarmonyLib.HarmonyPatch(typeof(OfferingBowl), "Awake")]
    public static class AlterOfferBowlAwake
    {
        public static void Prefix(OfferingBowl __instance)
        {
            if (__instance == null) return;
            Jotunn.Logger.LogDebug($"Calling OfferingBowl.Awake for {__instance.name}");
            
            var mistlandsAltar = GameObject.Find("SvartalfrQueenAltar_New(Clone)");
            if (mistlandsAltar != null )
            {
                var mistlandsBoss = ZNetScene.instance.GetPrefab("RRRN_SvartalfrQueen");

                if (mistlandsBoss != null)
                {
                    var mistlandsOfferingBowl = mistlandsAltar.gameObject.GetComponentInChildren<OfferingBowl>();
                    mistlandsOfferingBowl.m_name = mistlandsBoss.gameObject.GetComponent<Humanoid>().m_name;
                    mistlandsOfferingBowl.m_bossPrefab = mistlandsBoss.gameObject;
                }
                else
                {
                    Jotunn.Logger.LogDebug("Did not find boss <RRRN_SvartalfrQueen>");
                }
            }
            else
            {
                Jotunn.Logger.LogDebug("Did not find altar <SvartalfrQueenAltar_New>");
            }
            var deepNorthAltar = GameObject.Find("JotunnAltar(Clone)");
            if (deepNorthAltar != null)
            {
                var deepNorthBoss = ZNetScene.instance.GetPrefab("RRRM_Jotunn");
                if (deepNorthBoss != null)
                {
                    var deepNorthOfferingBowl = deepNorthAltar.gameObject.GetComponentInChildren<OfferingBowl>();
                    deepNorthOfferingBowl.m_name = deepNorthBoss.gameObject.GetComponent<Humanoid>().m_name;
                    deepNorthOfferingBowl.m_bossPrefab = deepNorthBoss.gameObject;
                }
                else
                {
                    Jotunn.Logger.LogDebug("Did not find boss <RRRM_Jotunn>");
                }
            }
            else
            {
                Jotunn.Logger.LogDebug("Did not find altar <JotunnAltar>");
            }

            var ashlandsAltar = GameObject.Find("BlazingDamnedOneAltar(Clone)");
            if (ashlandsAltar != null)
            {
                var ashlandsBoss = ZNetScene.instance.GetPrefab("RRRM_DamnedOne");
                if (ashlandsBoss != null)
                {
                    var ashlandsOfferingBowl = ashlandsAltar.gameObject.GetComponentInChildren<OfferingBowl>();
                    ashlandsOfferingBowl.m_name = ashlandsBoss.gameObject.GetComponent<Humanoid>().m_name;
                    ashlandsOfferingBowl.m_bossPrefab = ashlandsBoss.gameObject;
                }
                else
                {
                    Jotunn.Logger.LogDebug("Did not find boss <RRRM_DamnedOne>");
                }
            }
            else
            {
                Jotunn.Logger.LogDebug("Did not find altar <BlazingDamnedOneAltar>");
            }
        }
    }
}
