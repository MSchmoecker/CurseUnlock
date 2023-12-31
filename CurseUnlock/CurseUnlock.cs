﻿using HarmonyLib;

namespace CurseUnlock {
    [HarmonyPatch]
    public class CurseUnlock : Mod {
        public const string Name = "Curse Unlock";
        public const string GUID = "com.maxsch.stacklands.curseunlock";
        public const string Version = "0.1.1";

        private static Harmony harmony;
        private static ConfigEntry<bool> unlockIdeas;

        private void Awake() {
            unlockIdeas = Config.GetEntry<bool>("Unlock Ideas", true, new ConfigUI {
                Tooltip = "Unlocks all ideas, currently only the sidebar is affected",
            });

            harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        private void OnDestroy() {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(RunOptionsScreen), nameof(RunOptionsScreen.SetCurseButton)), HarmonyPrefix]
        private static void SetCurseActive(ref bool curseUnlocked) {
            curseUnlocked = true;
        }

        [HarmonyPatch(typeof(GameScreen), nameof(GameScreen.KnowledgeWasFound)), HarmonyPostfix]
        private static void SetKnowledgeWasFound(ref bool __result) {
            if (unlockIdeas.Value) {
                __result = true;
            }
        }
    }
}
