using UnityEngine;
using PortiaHelper.Gui;

namespace PortiaHelper.Modules
{
	class GuiActivator : MonoBehaviour
	{
		protected ItemSpawnerGui _itemSpawner;

		protected PlayerOptionsGui _playerOptions;

		void Start() {
			_itemSpawner = gameObject.AddComponent<ItemSpawnerGui>();
			_playerOptions = gameObject.AddComponent<PlayerOptionsGui>();

			_itemSpawner.enabled = false;
			_playerOptions.enabled = false;
		}

		void Update() {
			if (Input.GetKeyUp(KeyCode.Insert) && !_playerOptions.enabled) {
				_itemSpawner.enabled = !_itemSpawner.enabled;
			}

			if (Input.GetKeyUp(KeyCode.Home) && !_itemSpawner.enabled) {
				_playerOptions.enabled = !_playerOptions.enabled;
			}
		}
	}
}
