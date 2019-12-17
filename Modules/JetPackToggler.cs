using UnityEngine;
using Pathea;
using Pathea.ModuleNs;

namespace PortiaHelper.Modules
{
	class JetPackToggler : MonoBehaviour
	{
		void Update() {
			if (Input.GetKeyUp(KeyCode.Keypad8)) {
				Module<Player>.Self.ToggleJetPack();
			}
		}
	}
}
