using Harmony;
using UnityEngine;
using UnityEngine.UI;
using Pathea.UISystemNs.PlayUI;
using PortiaHelper.Core.Extensions;
using PortiaHelper.Core.Loaders;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(PlayUICtr))]
	[HarmonyPatch("OnEnable")]
	public static class PlayUiPatcher
	{
		[HarmonyPostfix]
		public static void Postfix() {
			var playUi = GameObject.FindObjectOfType<PlayUICtr>();

			if (playUi != null) {
				//Central.Instance.Crosshair = playUi.ReadProperty<Image>("crossHair");
				//ItemSpawnerLoader.Load(playUi.gameObject);
				ItemSpawnerLoader.Load(Camera.main.gameObject);
			}
		}
	}
}
