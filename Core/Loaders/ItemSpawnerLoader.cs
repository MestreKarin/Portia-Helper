using UnityEngine;
using PortiaHelper.Gui;

namespace PortiaHelper.Core.Loaders
{
	public static class ItemSpawnerLoader
	{
		public static void Load() {
			var obj = Camera.main.gameObject;

			if (Camera.main.GetComponent<ItemSpawnerGui>() is null) {
				Camera.main.gameObject.AddComponent<ItemSpawnerGui>();
			}
		}
	}
}
