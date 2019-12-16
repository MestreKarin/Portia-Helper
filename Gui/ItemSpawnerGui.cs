using System;
using System.Collections.Generic;
using System.Linq;
using Pathea;
using Pathea.GameFlagNs;
using Pathea.ModuleNs;
using Pathea.UISystemNs;
using UnityEngine;
using PortiaHelper.Core;
using PortiaHelper.Core.Extensions;
using PortiaHelper.Models;

namespace PortiaHelper.Gui
{

	public class ItemSpawnerGui : MonoBehaviour
	{
		#region Main Window Variables
		private Texture2D _atlas;

		private int _width = 744;
		private int _height = 507;

		private Rect _windowRect;

		private int _currentPage = 1;
		private int _itemsPerPage = 50;
		private int _itemsPerRow = 10;
		private int _totalPages;

		private string _searchText;
		private string _paginationText;

		private GUIStyle _itemNameStyle;
		private GUIStyle _itemSimilarStyle;

		/// <summary>
		/// Cache of items coordinates.
		/// </summary>
		private List<Rect> _coordsCache;

		/// <summary>
		/// Cache of icons coordinates.
		/// </summary>
		private List<Rect> _spritesCache;

		/// <summary>
		/// Indicate if <see cref="_items"/> is updating.
		/// </summary>
		private bool _updating = false;

		/// <summary>
		/// Indicate <see cref="_spritesCache"/> need to be updated.
		/// </summary>
		private bool _toUpdateSprites = false;

		/// <summary>
		/// Will hold no more than  <see cref="_itemsPerPage"/>
		/// </summary>
		private IList<DbItem> _items;

		/// <summary>
		/// This will hold all filtered items, it can have
		/// hundreds/thousands of items.
		/// </summary>
		private IList<DbItem> _filteredItems;

		/// <summary>
		/// Debouncer for item search.
		/// </summary>
		private DebounceDispatcher _debouncer = new DebounceDispatcher();
		#endregion

		public void Awake() {
			var x = Screen.width / 2 - _width / 2;
			var y = Screen.height / 2 - _height / 2;

			_atlas = TGALoader.LoadTGA($"{Central.Instance.HomePath}/resources/ui.tga", false);
			_windowRect = new Rect(x, y, _width, _height);

			_coordsCache = new List<Rect>();
			_spritesCache = new List<Rect>();

			_itemNameStyle = new GUIStyle() {
				fixedHeight = 64,
				fontSize = 12,
				fontStyle = FontStyle.BoldAndItalic,
				wordWrap = true
			};
			
			_itemNameStyle.normal.textColor = Color.yellow;
			_itemNameStyle.alignment = TextAnchor.MiddleCenter;

			_itemSimilarStyle = new GUIStyle(_itemNameStyle) {
				fontSize = 10,
				fontStyle = FontStyle.Bold,
				fixedHeight = 20,
				fixedWidth = 20
			};
			_itemSimilarStyle.normal.textColor = Color.white;
		}

		void OnEnable() {
			_currentPage = 1;
			_filteredItems = Central.Instance.ItemDB;
			_items = _filteredItems.Take(_itemsPerPage).ToList();

			_searchText = "";
			_totalPages = (int)Math.Ceiling(Central.Instance.ItemDB.Count / (double)_itemsPerPage);
			_paginationText = $"1 ~ {_itemsPerPage} of {Central.Instance.ItemDB.Count} — Page 1 of {_totalPages}";

			BuildCoordsCache();
			BuildSpritesCache();

			Central.Instance.PauseGame();
		}

		void OnDisable() {
			Central.Instance.ResumeGame();
		}

		void OnGUI() {
			_windowRect = GUI.Window(1, _windowRect, RenderWindow, "Item Spawner");
		}

		void RenderWindow(int windowId) {
			var newText = GUI.TextField(new Rect(16, 32, 712, 24), _searchText);
			
			if (newText != _searchText) {
				_searchText = newText;
				_debouncer.Debounce(350, OnSearchTextUpdate);
			}
			
			if (_toUpdateSprites) {
				BuildSpritesCache();

				_toUpdateSprites = false;
				_updating = false;
			}

			if (!_updating) {
				for (var i = 0; i < _items.Count; i++) {
					if (i >= _coordsCache.Count || i >= _spritesCache.Count) {
						break;
					}

					GUI.DrawTextureWithTexCoords(_coordsCache[i], _atlas, _spritesCache[i]);
					GUI.Label(new Rect(_coordsCache[i].x, _coordsCache[i].y, 64, 64), _items[i].Name, _itemNameStyle);

					if (_items[i].Similar?.Count > 0) {
						GUI.Label(new Rect(_coordsCache[i].x + 44, _coordsCache[i].y, 20, 20), _items[i].SimilarCountStr, _itemSimilarStyle);
					}
				}
			}

			// Pagination label.
			GUI.Label(new Rect(16, 471, 250, 20), _paginationText);
			
			// Pagination back button.
			if (_currentPage > 1) {
				if (GUI.Button(new Rect(656, 463, 28, 28), "<")) {
					ChangePage(PaginationNav.Back);
				}
			}

			// Pagination forward button.
			if (_currentPage < _totalPages) {
				if (GUI.Button(new Rect(700, 463, 28, 28), ">")) {
					ChangePage(PaginationNav.Next);
				}
			}

			if (Input.GetMouseButton(0)) {
				if (Event.current.isMouse && Event.current.mousePosition != null) {
					for (int i = 0; i < _coordsCache.Count; i++) {
						if (_coordsCache[i].Contains(Event.current.mousePosition)) {
							if (i >= _items.Count) {
								break;
							}

							DeployItem(_items[i]);
							break;
						}
					}
				}
			}
		}

		void OnSearchTextUpdate(object obj) {
			_currentPage = 1;
			_updating = true;

			if (ValidSearchQuery()) {
				var lwText = _searchText.ToLower();
				_filteredItems = Central.Instance.ItemDB.Where(dbi => dbi.Name.IndexOf(lwText) >= 0).ToList();

				if (_filteredItems is null || _filteredItems.Count <= 0) {
					_filteredItems = Central.Instance.ItemDB;
				}
			} else {
				_filteredItems = Central.Instance.ItemDB;
			}

			_totalPages = (int)Math.Ceiling(_filteredItems.Count / (double)_itemsPerPage);

			if (_filteredItems.Count >= _itemsPerPage) {
				_paginationText = $"1 ~ {_itemsPerPage} of {_filteredItems.Count} — Page 1 of {_totalPages}";
			} else {
				_paginationText = $"1 ~ {_filteredItems.Count} of {_filteredItems.Count} — Page 1 of {_totalPages}";
			}

			_items = _filteredItems.Take(_itemsPerPage).ToList();

			_toUpdateSprites = true;
		}

		void BuildCoordsCache() {
			_coordsCache.Clear();

			for (var i = 0; i < _itemsPerPage; i++) {
				float slotX = (i + 1) % _itemsPerRow == 0 ? _itemsPerRow : (i + 1) % _itemsPerRow;
				float slotY = (float)Math.Floor(i / (double)_itemsPerRow);

				var x = slotX > 1 ? 16 + ((slotX - 1) * 64) + ((slotX - 1) * 8) : 16;
				var y = slotY > 0 ? 95 + (slotY * 64) + (slotY * 8) : 95;

				_coordsCache.Add(new Rect(x, y, 64, 64));
			}
		}

		void BuildSpritesCache() {
			_spritesCache.Clear();
			
			foreach (var it in _items) {
				var off = Central.Instance.ItemOffsets[it.IconName];
				_spritesCache.Add(new Rect(off[0], off[1], 64, 64).Normalize(4096));
			}
		}

		void ChangePage(PaginationNav navType) {
			_currentPage = navType == PaginationNav.Back ? _currentPage - 1 : _currentPage + 1;

			_items = _filteredItems.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
			_paginationText = $"{1 + (_currentPage - 1) * _itemsPerPage} ~ {_currentPage * _items.Count} of {_filteredItems.Count} — Page {_currentPage} of {_totalPages}";

			BuildSpritesCache();
		}

		void DeployItem(DbItem item) {
			Main.Logger.Log($"Attempted to deploy item {item.Name}; ID: {item.ID}");

			if (item.Similar?.Count > 0) {
				int playerLevel = Module<Player>.Self.ActorLevel;
				int maxLevel = 0;
				int itemId = item.ID;

				foreach (var i in item.Similar) {
					if (i.Key > maxLevel && i.Key < playerLevel) {
						maxLevel = i.Key;
						itemId = i.Value;
					}
				}

				Module<Player>.Self.bag.TryAddItem(itemId, 1, true);
				return;
			}

			Module<Player>.Self.bag.TryAddItem(item.ID, 1, true);
		}

		bool ValidSearchQuery() {
			var str = _searchText.Trim();

			return str != "" && str.Length >= 2;
		}

		internal enum PaginationNav
		{
			Next,
			Back
		}
	}
}
