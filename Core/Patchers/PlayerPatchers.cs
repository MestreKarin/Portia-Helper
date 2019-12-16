using System;
using UnityEngine;
using Harmony;
using Pathea;
using Pathea.ActorNs;
using Pathea.ActorDeadDropNs;
using Pathea.ItemDropNs;
using Pathea.ItemSystem;
using Pathea.ModuleNs;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(Actor))]
	[HarmonyPatch("cp", MethodType.Setter)]
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

	[HarmonyPatch(typeof(Actor))]
	[HarmonyPatch("hp", MethodType.Setter)]
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

	[HarmonyPatch(typeof(Actor))]
	[HarmonyPatch("Vp", MethodType.Setter)]
	internal static class PlayerInfiniteVp
	{
		[HarmonyPrefix]
		static void Prefix(ref float value, Actor __instance) {
			if (Central.Instance.PlayerOptions.InfiniteVP) {
				value = __instance.ConstAttrVpMax;
			}
		}
	}

	[HarmonyPatch(typeof(Player))]
	[HarmonyPatch("AddExp")]
	internal static class PlayerExpMultiplier
	{
		[HarmonyPrefix]
		static void Prefix(ref int exp) {
			exp = exp > 0 ? Convert.ToInt32(exp * Central.Instance.PlayerOptions.ExpRatio) : exp;
		}
	}

	[HarmonyPatch(typeof(ItemBag))]
	[HarmonyPatch("ChangeMoney")]
	internal static class PlayerGoldMultiplier
	{
		[HarmonyPrefix]
		static void Prefix(ref int baseValue) {
			baseValue = baseValue > 0 ? Convert.ToInt32(baseValue * Central.Instance.PlayerOptions.GoldRatio) : baseValue;
		}
	}

	[HarmonyPatch(typeof(ActorDeadDropModule))]
	[HarmonyPatch("Handler")]
	internal static class NoDrop
	{
		[HarmonyPrefix]
		static bool Prefix(ref KillEventArg arg) {
			return !Central.Instance.PlayerOptions.NoDrop;
		}
	}
}
