using UnityEngine;
using PortiaHelper.Modules;

namespace PortiaHelper.Core.Loaders
{
	public static class PlayerAddonsLoader
	{
		public static void Load() {
			var obj = Camera.main.gameObject;

			// Infinite VP.
			if (obj.GetComponent<InfiniteVp>() is null) {
				obj.AddComponent<InfiniteVp>();
			}
		}
	}
}
