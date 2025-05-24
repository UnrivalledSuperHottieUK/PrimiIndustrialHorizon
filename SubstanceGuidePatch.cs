using MelonLoader;
using HarmonyLib;
using Il2Cpp;               // for MenuWindowR
using Il2CppTMPro;

namespace IndustrialHorizons

{
    // Wrap the entire Update() in try/catch and skip original on error:
    [HarmonyPatch(typeof(MenuWindowR), nameof(MenuWindowR.Update))]
    public static class MenuWindowR_Update_Patch
    {
        // A Prefix that returns false on error to skip the original Update()
        static bool Prefix(MenuWindowR __instance)
        {
            try
            {
                // Let the original Update() execute
                return true;
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                MelonLogger.Warning($"Caught OOB in MenuWindowR.Update(): {e.Message}");
                // Skip the original Update body entirely
                return false;
            }
        }

        // Your guide-text Postfix still runs regardless of whether the original did
        static void Postfix(MenuWindowR __instance)
        {
            // … your existing guide-text logic here …
            {
            var nameText  = __instance.substanceNameText;
            var guideText = __instance.substanceGuideText;
            if (nameText == null || guideText == null)
                return;

            // compare the displayed string
            string subName = nameText.text?.ToLower();
            MelonLogger.Msg("subName" + subName);
            if (subName == "chalcocite (cu2s)")
            {
                //MelonLogger.Msg("subName" + subName);
                guideText.text = $"Here's a quick guide for {subName}!";
            }
            }
    }
        }
}
