using System.Collections.Generic;
using System.Linq;
using Pathea.UISystemNs;

namespace PortiaHelper.Core
{
	public class Central
	{
		public static Central Instance = new Central();

		public IEnumerable<DbItem> ItemDB { get; set; }

		public bool IsInsideInventory { get; set; }

		public PackageUIBase CurrentStorage { get; set; }

		public bool ItemDbLoaded
		{
			get {
				return ItemDB != null && ItemDB.Count() > 0;
			}
		}

		public Central() {
			IsInsideInventory = false;
		}
	}
}
