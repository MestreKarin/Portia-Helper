using UnityEngine;
using Pathea;
using Pathea.ModuleNs;
using PortiaHelper.Gui;

namespace PortiaHelper.Core.Loaders
{
	public static class ItemSpawnerLoader
	{
		public static void Load(GameObject playUiObject) {
			if (Module<Player>.Self == null) {
				return;
			}

			if (!Central.Instance.ItemSpawnerLoaded) {
				Central.Instance.ItemSpawnerLoaded = true;

				playUiObject.AddComponent<ItemSpawnerGui>();
			}
		}
	}
}
