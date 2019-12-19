## v2.1.0 (2019-12-19)

* Added compatibility check with other mods.
* Removed `IsInsideInventory` property.

## v2.0.0 (2019-12-16)

### Changed
* Item spawner now uses a single texture to draw icons, improving performance.
* Duplicated items are grouped and the one with highest level wich player can use will be spawned.
* Item name and duplicate count (if any) are shown in front of it's icon.
* `GuiActivator` will manage any GUI component state to prevent `OnGUI` from being called when not using, thus, not affecting performance during gameplay.

### Added
* Brute remover (any item can be removed).
* God mode.
* Infinite stamina.
* Infinite vp.
* No drops.
* Exp multiplier.
* Gold multiplier.
