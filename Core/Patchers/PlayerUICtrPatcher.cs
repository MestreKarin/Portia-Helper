using UnityEngine;
using Harmony;
using Pathea.UISystemNs.PlayUI;
using PortiaHelper.Modules;

namespace PortiaHelper.Core.Patchers
{
	[HarmonyPatch(typeof(PlayUICtr))]
	[HarmonyPatch("OnEnable")]
	static class PlayerUICtrPatcher
	{
		[HarmonyPostfix]
		static void Postfix() {
			var uiCtr = GameObject.FindObjectOfType<PlayUICtr>();

			if (uiCtr.gameObject.GetComponent<GuiActivator>() is null) {
				uiCtr.gameObject.AddComponent<GuiActivator>();
			}

			if (uiCtr.gameObject.GetComponent<JetPackToggler>() is null) {
				uiCtr.gameObject.AddComponent<JetPackToggler>();
			}
		}
	}
}
