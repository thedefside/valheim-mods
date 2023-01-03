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

            var deepNorthAltar = GameObject.Find("JotunnAltar(Clone)");
            if (deepNorthAltar != null)
            {
                var deepNorthBoss = ZNetScene.instance.GetPrefab("RRRM_Jotunn");
                if (deepNorthBoss != null)
                {
                    // make it so you can't stagger the boss
                    deepNorthBoss.GetComponent<Humanoid>().m_staggerWhenBlocked = false;
                    deepNorthBoss.GetComponent<Humanoid>().m_staggerDamageFactor = 0;

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
                var ashlandsBoss2 = ZNetScene.instance.GetPrefab("RRRM_BlazingDamnedOne");

                if (ashlandsBoss != null && ashlandsBoss2 != null)
                {
                    // make it so you can't stagger the boss
                    ashlandsBoss.GetComponent<Humanoid>().m_staggerWhenBlocked = false;
                    ashlandsBoss.GetComponent<Humanoid>().m_staggerDamageFactor = 0;
                    ashlandsBoss2.GetComponent<Humanoid>().m_staggerWhenBlocked = false;
                    ashlandsBoss2.GetComponent<Humanoid>().m_staggerDamageFactor = 0;

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
