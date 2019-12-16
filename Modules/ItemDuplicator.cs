using System;
using UnityEngine;
using Pathea;
using Pathea.ItemSystem;
using Pathea.UISystemNs;
using PortiaHelper.Core;
using PortiaHelper.Core.Extensions;

namespace PortiaHelper.Modules
{
	public class ItemDuplicator : MonoBehaviour
	{
		private PackageUIBase _storage;

		private GameObject _dupWindow;

		private bool _inDupMode = false;

		void Start() {
			_storage = gameObject.GetComponent<PackageUIBase>();

			if (_storage is null) {
				Main.Logger.Log("PackageUIBase component not found!");
			}

			Central.Instance.IsInsideInventory = true;
		}

		void Update() {
			if (Input.GetKeyUp(KeyCode.KeypadMinus) && !_inDupMode) {
				_storage.DeleteItem();
			}

			if (Input.GetKey(KeyCode.KeypadMultiply) && !_inDupMode) {
				if (_storage is null) {
					Main.Logger.Log("Storage is null, can't duplicate!");
					return;
				}

				ItemObject curSelected = _storage.ReadProperty<ItemObject>("curSelectItem");

				if (curSelected != null) {
					_inDupMode = true;

					Action<int> confirm = delegate (int num) {
						if (num > curSelected.Number) {
							curSelected.ChangeNumber(num - curSelected.Number);
						} else {
							curSelected.ChangeNumber(-(curSelected.Number - num));
						}

						_storage.FreshCurpageItem();
						_storage.playerItemBar.FreshItem();
					};

					ShowDupWindow(curSelected, confirm);
				}
			}
		}

		void OnEnable() {
			Central.Instance.IsInsideInventory = true;
		}

		void OnDisable() {
			if (_dupWindow != null && _dupWindow.activeSelf) {
				_dupWindow.GetComponent<PackageItemDelete>().Exit(false);
			}

			//Central.Instance.IsInsideInventory = false;
		}

		void ShowDupWindow(ItemObject item, Action<int> confirm) {
			try {
				if (_dupWindow is null) {
					_dupWindow = GameUtils.AddChild(gameObject, Singleton<ResMgr>.Instance.LoadSyncByType<GameObject>(AssetType.UiSystem, "Prefabs/ItemSplit"), false, true);
				}

				_dupWindow.GetComponent<PackageItemDelete>().Show(confirm, null, 999, item, () => {
					_inDupMode = false;
				});
			} catch (Exception ex) {
				Main.Logger.LogException(ex);
			}
		}
	}
}
