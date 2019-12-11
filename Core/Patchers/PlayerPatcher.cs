using Harmony;
using Pathea;
using PortiaHelper.Core.Loaders;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(Player))]
	[HarmonyPatch("OnLoad")]
	public static class PlayerPatcher
	{
		[HarmonyPostfix]
		public static void Postfix() {
			PlayerAddonsLoader.Load();
		}
	}
}
