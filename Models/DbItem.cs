using System.Collections.Generic;
using System.Linq;

namespace PortiaHelper.Models
{
	public class DbItem
	{
		private string _similarCountStr = "";

		public int ID;

		public string IconName;

		public string Name;

		public int NameID;

		public string SimilarCountStr
		{
			get {
				if (_similarCountStr == "") {
					_similarCountStr = Similar?.Count.ToString() ?? "0";
				}

				return _similarCountStr;
			}
		}

		/// <summary>
		/// Will hold other instances of the item.
		/// Example: The beginner sword has 17 variations, there's no
		/// need to show the same item multiple times on Item Spawner UI.
		/// </summary>
		public Dictionary<int, int> Similar;

		public DbItem(IEnumerable<int[]> similar = null) {
			if (similar != null) {
				Similar = new Dictionary<int, int>(similar.Count());

				foreach (var s in similar) {
					if (Similar.ContainsKey(s[0])) {
						continue;
					}

					Similar.Add(s[0], s[1]);
				}
			}
		}
	}
}
