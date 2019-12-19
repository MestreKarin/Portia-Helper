using UnityEngine;
using Harmony12;
using Pathea.UISystemNs;
using PortiaHelper.Modules;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(PackageUIBase), "OnEnable")]
	static class ItemDuplicatorLoader
	{
		[HarmonyPostfix]
		static void Postfix() {
			var storages = GameObject.FindObjectsOfType<PackageUIBase>();

			if (storages is null || storages.Length < 1) {
				Main.Logger.Log("No storages!");
			}

			foreach (var st in storages) {
				if (st.gameObject.GetComponent<ItemDuplicator>() != null) {
					continue;
				}

				st.gameObject.AddComponent<ItemDuplicator>();
			}
		}
	}
}
