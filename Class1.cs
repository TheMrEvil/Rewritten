using System;
using MelonLoader;
using HarmonyLib;

namespace Rewritten
{
    public class Class1 : MelonMod
    {
        static int rerollsUsed = 0;
        
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initializing Harmony patches...");
            
            // Initialize Harmony and apply patches
            var harmony = new HarmonyLib.Harmony("com.rewritten.mod");
            
            try
            {
                // Apply specific patch for TryReroll
                harmony.Patch(
                    AccessTools.Method("PlayerChoicePanel:TryReroll"),
                    postfix: new HarmonyMethod(typeof(Class1), nameof(TryRerollPostfix))
                );
                
                MelonLogger.Msg("Harmony patches applied successfully");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to apply Harmony patches: {ex.Message}");
            }
        }

        // Postfix for the TryReroll method
        public static void TryRerollPostfix(PlayerChoicePanel __instance)
        {
            MelonLogger.Msg("TryRerollPostfix called!");
            
            // Get the current passive mod value for rerolls
            if (PlayerControl.myInstance != null)
            {
                int maxRerolls = (int)PlayerControl.myInstance.GetPassiveMod(Passive.EntityValue.P_PageRerolls, 0f);
                // Set RerollsUsed to ensure RerollsRemaining equals 69
                // RerollsRemaining = maxRerolls - RerollsUsed
                // So: RerollsUsed = maxRerolls - 69
                PlayerChoicePanel.RerollsUsed = Math.Max(0, maxRerolls - 69);
                rerollsUsed++;
                // Log for debugging
                MelonLogger.Msg($"Rerolls Used: " + rerollsUsed);
            }
        }
    }
}
