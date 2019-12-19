using System;
using System.Collections.Generic;
using System.Linq;
using UnityModManagerNet;
using Harmony12;

namespace PortiaHelper.Core
{
	public class PatchController
	{
		private static PatchController _instance;

		public static PatchController Instance
		{
			get {
				return _instance ?? (_instance = new PatchController());
			}
		}
		
		private Dictionary<string, string> _takenPatches;

		public PatchController() {
			_takenPatches = new Dictionary<string, string>();
		}

		public bool CanApplyPatches() {
			var conflict = new List<string>(new string[] {
				"Actor_cp",
				"Actor_hp",
				"Actor_Vp",
				"ActorDeadDropModule_Handler",
				"ItemBag_ChangeMoney",
				"Player_AddExp"
			});

			foreach (var ass in UnityModManager.modEntries) {
				if (ass.Info.DisplayName == "Portia Helper") {
					continue;
				}

				string origin = ass.Info.DisplayName;

				foreach (var tp in ass.Assembly.GetTypes()) {
					if (!Attribute.IsDefined(tp, typeof(HarmonyPatch))) {
						continue;
					}

					object[] attrs = tp.GetCustomAttributes(true);
					string methodName = "";
					string typeName = "";

					foreach (var attr in attrs) {
						HarmonyPatch hpAttr = attr as HarmonyPatch;

						if (hpAttr is null) {
							continue;
						}

						if (methodName == "" && hpAttr.info.methodName != "") {
							methodName = hpAttr.info.methodName;
						}

						if (typeName == "" && hpAttr.info.declaringType.Name != "") {
							typeName = hpAttr.info.declaringType.Name;
						}

						if (methodName != "" && typeName != "") {
							break;
						}
					}

					if (methodName != "" && typeName != "") {
						string key = $"{typeName}_{methodName}";

						if (conflict.IndexOf(key) < 0) {
							continue;
						}

						if (_takenPatches.ContainsKey(key)) {
							continue;
						}

						_takenPatches.Add(key, origin);
					}
				}
			}

			return _takenPatches.Count == 0;
		}

		public string[] ConflictedMods() {
			return _takenPatches.GroupBy(kv => kv.Value).Select(g => g.Key).ToArray();
		}
	}
}
