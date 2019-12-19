using System.Collections.Generic;
using System.IO;
using System.Linq;
using Harmony12;
using Pathea.ItemSystem;
using PortiaHelper.Models;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(ItemDataMgr), "OnLoad")]
	static class ItemDataPatcher
	{
		[HarmonyPostfix]
		public static void Postfix(List<ItemBaseConfData> ___itemBaseList) {
			// Load sprites offsets.
			var offsets = File.ReadAllLines($"{Central.Instance.HomePath}/offsets.txt");

			Central.Instance.ItemOffsets = new Dictionary<string, int[]>(offsets.Length);

			foreach (var off in offsets) {
				var offSplit = off.Split('=');

				Central.Instance.ItemOffsets.Add(offSplit[0], offSplit[1].Split(',').Select(s => int.Parse(s)).ToArray());
			}

			// Load items.
			Central.Instance.ItemDB = new List<DbItem>();

			var validItems = ___itemBaseList.Where(i => Central.Instance.ItemOffsets.ContainsKey(i.IconPath.Replace("Sprites/Package/", "")))
				.GroupBy(i => i.NameID);

			foreach (var kv in validItems) {
				if (kv.Count() == 1) {
					var ib = kv.First();
					var name = TextMgr.GetStr(ib.NameID).ToLower();

					if (name.Trim() == "") {
						continue;
					}

					Central.Instance.ItemDB.Add(new DbItem {
						ID = ib.ID,
						Name = TextMgr.GetStr(ib.NameID).ToLower(),
						NameID = ib.NameID,
						IconName = ib.IconPath.Replace("Sprites/Package/", "")
					});

					continue;
				}

				var first = kv.ElementAt(0);
				var firstName = TextMgr.GetStr(first.NameID).ToLower();

				if (firstName.Trim() == "") {
					continue;
				}

				Central.Instance.ItemDB.Add(new DbItem(kv.Skip(1).Select(i => new int[] { i.UseLevel, i.ID })) {
					ID = first.ID,
					Name = firstName,
					NameID = first.NameID,
					IconName = first.IconPath.Replace("Sprites/Package/", "")
				});
			}
		}
	}
}
