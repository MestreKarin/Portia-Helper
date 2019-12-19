using Harmony12;
using Pathea.EquipmentNs;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(JetPack), "OnEnable")]
	public static class JetPackPatcher
	{
		[HarmonyPostfix]
		public static void Postfix(ref float ___durationValue) {
			___durationValue = 999_999.0f;
		}
	}
}
