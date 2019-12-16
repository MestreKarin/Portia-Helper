using System.Collections.Generic;
using Pathea;
using Pathea.GameFlagNs;
using Pathea.UISystemNs;
using PortiaHelper.Models;

namespace PortiaHelper.Core
{
	public class Central {
		private static Central _instance;

		public static Central Instance
		{
			get {
				return _instance ?? (_instance = new Central());
			}
		}

		public BoolTrue MouseLocker { get; set; }

		/// <summary>
		/// Mod path.
		/// </summary>
		public string HomePath { get; set; }

		/// <summary>
		/// Item Database.
		/// </summary>
		public IList<DbItem> ItemDB { get; set; }

		/// <summary>
		/// Item icons offsets.
		/// </summary>
		public Dictionary<string, int[]> ItemOffsets { get; set; }

		public PlayerOptions PlayerOptions { get; private set; }

		public bool IsInsideInventory { get; set; }

		public bool ItemDbLoaded
		{
			get {
				return ItemDB != null && ItemDB.Count > 0;
			}
		}

		public Central() {
			MouseLocker = new BoolTrue();
			IsInsideInventory = false;
			PlayerOptions = new PlayerOptions() {
				ExpRatio = 1f,
				GoldRatio = 1f
			};
		}

		public void PauseGame() {
			UIStateComm.Instance.SetCursor(true);
			Singleton<GameFlag>.Instance.Add(Flag.Pause, MouseLocker);
		}

		public void ResumeGame() {
			UIStateComm.Instance.SetCursor(false);
			Singleton<GameFlag>.Instance.Remove(Flag.Pause, MouseLocker);
		}
	}
}
