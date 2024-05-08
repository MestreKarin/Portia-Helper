using System;
using Harmony12;
using Pathea;
using Pathea.ActorNs;
using Pathea.ActorDeadDropNs;
using Pathea.ItemSystem;
using Pathea.ModuleNs;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(Actor), "cp", MethodType.Setter)]
	internal static class PlayerInfiniteStamina
	{
		[HarmonyPrefix]
		static void Prefix(ref float value, Actor __instance) {
			if (Module<Player>.Self is null || Module<Player>.Self.actor is null) {
				return;
			}

			if (Central.Instance.PlayerOptions.InfiniteStamina && Module<Player>.Self.actor.ActorName == __instance.ActorName) {
				value = __instance.ConstAttrCpMax;
			}
		}
	}

	[HarmonyPatch(typeof(Actor), "hp", MethodType.Setter)]
	internal static class PlayerGodMode
	{
		[HarmonyPrefix]
		static void Prefix(ref float value, Actor __instance) {
			if (Module<Player>.Self is null || Module<Player>.Self.actor is null) {
				return;
			}

			if (Central.Instance.PlayerOptions.GodMode && Module<Player>.Self.actor.ActorName == __instance.ActorName) {
				value = __instance.ConstAttrHpMax;
			}
		}
	}

	[HarmonyPatch(typeof(Actor), "Vp", MethodType.Setter)]
	internal static class PlayerInfiniteVp
	{
		[HarmonyPrefix]
		static void Prefix(ref float value, Actor __instance) {
			if (Central.Instance.PlayerOptions.InfiniteVP) {
				value = __instance.ConstAttrVpMax;
			}
		}
	}

	[HarmonyPatch(typeof(Player), "AddExp")]
	internal static class PlayerExpMultiplier
	{
		[HarmonyPrefix]
		static void Prefix(ref int exp) {
            try {
                exp = exp > 0 ? Convert.ToInt32(exp * Central.Instance.PlayerOptions.ExpRatio) : exp;
            } catch (OverflowException) {
                Main.Logger.Log("Capping exp increase to max Int32");
                exp = Int32.MaxValue;
            }
		}
	}

	[HarmonyPatch(typeof(ItemBag), "ChangeMoney")]
	internal static class PlayerGoldMultiplier
	{
		[HarmonyPrefix]
		static void Prefix(ref int baseValue) {
            try {
                baseValue = baseValue > 0 ? Convert.ToInt32(baseValue * Central.Instance.PlayerOptions.GoldRatio) : baseValue;
            } catch (OverflowException) {
                Main.Logger.Log("Capping money increase to max Int32");
                baseValue = Int32.MaxValue;
            }
		}
	}

	[HarmonyPatch(typeof(ActorDeadDropModule), "Handler")]
	internal static class NoDrop
	{
		[HarmonyPrefix]
		static bool Prefix() {
			return !Central.Instance.PlayerOptions.NoDrop;
		}
	}
}
