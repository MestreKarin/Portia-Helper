using System;
using System.Collections.Generic;
using System.Linq;
using Pathea;
using Pathea.AimSystemNs;
using Pathea.ModuleNs;
using Pathea.UISystemNs;
using UnityEngine;
using PortiaHelper.Core;

namespace PortiaHelper.Gui
{
	public class ItemSpawnerGui : MonoBehaviour
	{
		#region Main Window Variables
		private bool _toShow = false;
		private int _width = 744;
		private int _height = 507;

		private Rect _windowRect;

		private int _currentPage;
		private int _itemsPerPage = 50;
		private double _itemsPerRow = 10;
		private int _totalPages;

		private string _searchText;
		private string _paginationText;

		private List<Rect> _coordsCache;
		private IEnumerable<DbItem> _items;
		private IEnumerable<DbItem> _filteredItems;

		private DebounceDispatcher _debouncer = new DebounceDispatcher();
		#endregion

		public void Awake() {
			var x = Screen.width / 2 - _width / 2;
			var y = Screen.height / 2 - _height / 2;

			_windowRect = new Rect(x, y, _width, _height);

			_currentPage = 1;
			_totalPages = (int)Math.Ceiling(Central.Instance.ItemDB.Count() / 50d);
			_searchText = "";
			_paginationText = $"1 ~ 50 of {Central.Instance.ItemDB.Count()} — Page 1 of {_totalPages}";

			_items = Central.Instance.ItemDB;
			_filteredItems = _items;

			BuildCoordsCache();
		}

		public void OnGUI() {
			GUI.depth = 0;

			var e = Event.current;

			if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Insert && Central.Instance.IsInsideInventory) {
				_toShow = !_toShow;				
			}

			if (Central.Instance.ItemDbLoaded && Central.Instance.IsInsideInventory && _toShow) {
				_windowRect = GUI.Window(1, _windowRect, RenderWindow, "Item Spawner");
			}

			GUI.depth++;
		}

		void RenderWindow(int windowId) {
			var newText = GUI.TextField(new Rect(16, 16, 712, 32), _searchText);

			if (newText != _searchText) {
				_searchText = newText;

				_debouncer.Debounce(400, (obj) => {
					_currentPage = 1;

					if (ValidSearchQuery()) {
						_filteredItems = Central.Instance.ItemDB.Where(dbi => dbi.Name.IndexOf(_searchText) >= 0);

						if (_filteredItems is null || _filteredItems.Count() <= 0) {
							_filteredItems = Central.Instance.ItemDB;
						}
					} else {
						_filteredItems = Central.Instance.ItemDB;
					}

					_totalPages = (int)Math.Ceiling(_filteredItems.Count() / 50d);

					if (_filteredItems.Count() >= 50) {
						_paginationText = $"1 ~ 50 of {_filteredItems.Count()} — Page 1 of {_totalPages}";
					} else {
						_paginationText = $"1 ~ {_filteredItems.Count()} of {_filteredItems.Count()} — Page 1 of {_totalPages}";
					}

					_items = _filteredItems.Take(_itemsPerPage);
				});
			}

			for (var i = 0; i < _items.Count(); i++) {
				if (i >= _itemsPerPage) {
					break;
				}

				GUI.DrawTexture(_coordsCache[i], UIUtils.GetSpriteByPath(_items.ElementAt(i).RawItem.IconPath).texture);
			}

			// Pagination label.
			GUI.Label(new Rect(16, 471, 400, 20), _paginationText);

			// Pagination back button.
			if (_currentPage > 1) {
				if (GUI.Button(new Rect(656, 463, 28, 28), "<")) {
					_currentPage -= 1;
					_items = _filteredItems.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage);
					_paginationText = $"{1 + (_currentPage - 1) * _itemsPerPage} ~ {_currentPage * _items.Count()} of {_filteredItems.Count()} — Page {_currentPage} of {_totalPages}";
				}
			}

			// Pagination forward button.
			if (_currentPage < _totalPages) {
				if (GUI.Button(new Rect(700, 463, 28, 28), ">")) {
					_currentPage += 1;
					_items = _filteredItems.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage);
					_paginationText = $"{1 + (_currentPage - 1) * _itemsPerPage} ~ {_currentPage * _items.Count()} of {_filteredItems.Count()} — Page {_currentPage} of {_totalPages}";
				}
			}

			if (Input.GetMouseButton(0)) {
				if (Event.current.isMouse && Event.current.mousePosition != null) {
					for (int i = 0; i < _coordsCache.Count; i++) {
						if (_coordsCache[i].Contains(Event.current.mousePosition)) {
							var item = _items.ElementAt(i).RawItem;

							Main.Logger.Log($"Attempted to deploy item {_items.ElementAt(i).Name}; ID: {item.ID}");
							
							Module<Player>.Self.bag.TryAddItem(item.ID, 1, true);
							Central.Instance.CurrentStorage.FreshCurpageItem();

							break;
						}
					}
				}
			}
		}

		void BuildCoordsCache() {
			_coordsCache = new List<Rect>(_itemsPerPage);

			for (var i = 0; i < _itemsPerPage; i++) {
				var slotX = (i + 1) % _itemsPerRow == 0 ? (float)_itemsPerRow : (float)((i + 1) % _itemsPerRow);
				var slotY = (float)Math.Floor(i / _itemsPerRow);

				var x = slotX > 1 ? 16 + ((slotX - 1) * 64) + ((slotX - 1) * 8) : 16;
				var y = slotY > 0 ? 95 + (slotY * 64) + (slotY * 8) : 95;

				_coordsCache.Add(new Rect(x, y, 64, 64));
			}
		}

		bool ValidSearchQuery() {
			return !string.IsNullOrEmpty(_searchText.Trim()) && _searchText.Length >= 2;
		}
	}
}
