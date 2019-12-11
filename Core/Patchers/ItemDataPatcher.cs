using System.Collections.Generic;
using System.Linq;
using Harmony;
using Pathea.ItemSystem;
using PortiaHelper.Core.Loaders;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(ItemDataMgr))]
	[HarmonyPatch("OnLoad")]
	static class ItemDataPatcher
	{
		[HarmonyPostfix]
		public static void Postfix(List<ItemBaseConfData> ___itemBaseList) {
			Central.Instance.ItemDB = ___itemBaseList.Where(ib => ib.CanPickup && ib.HaveIcon).Select(ib => new DbItem {
				RawItem = ib,
				Name = TextMgr.GetStr(ib.NameID)
			});

			ItemSpawnerLoader.Load();
		}
	}
}
