using System;
using System.Reflection;
using Harmony12;
using UnityModManagerNet;
using PortiaHelper.Core;

namespace PortiaHelper
{
	static class Main
    {
		public static HarmonyInstance harmonyInstance;
		public static UnityModManager.ModEntry.ModLogger Logger;

		public static bool Load(UnityModManager.ModEntry modEntry) {
			Logger = modEntry.Logger;
			
			Central.Instance.HomePath = modEntry.Path;

			try {
				if (!PatchController.Instance.CanApplyPatches()) {
					Logger.Log($"========== PORTIA HELPER - CONFLICTS FOUND ==========");
					Logger.Log("In order to use Portia Helper, disable these mods (and restart the game):");

					foreach (var cm in PatchController.Instance.ConflictedMods()) {
						Logger.Log($"-> {cm}");
					}
					
					return false;
				}

				harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
				harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
			} catch (Exception ex) {
				modEntry.Logger.LogException(ex);
			}

			return true;
		}
	}
}
