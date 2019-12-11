using System;
using System.Reflection;

namespace PortiaHelper.Core.Extensions
{
	public static class ObjectExtensions
	{
		public static T ReadProperty<T>(this object obj, string name) {
			Type tp = obj.GetType();

			var pi = tp.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

			try {
				var pv = pi.GetValue(obj, null);

				if (pv is T) {
					return (T)pv;
				}

				Main.Logger.Log($"Value is not T. {pv.GetType().Name}");

				return (T)Convert.ChangeType(pv, typeof(T));
			} catch (Exception) {
				return default(T);
			}
		}
	}
}
