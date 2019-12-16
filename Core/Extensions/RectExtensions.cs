using UnityEngine;

namespace PortiaHelper.Core.Extensions
{
	public static class RectExtensions
	{
		public static Rect Normalize(this Rect input, float textureSize) {
			float b = 1f / textureSize;

			return new Rect(b * input.x, b * input.y, b * input.width, b * input.height);
		}
	}
}
