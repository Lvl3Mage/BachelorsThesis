using UnityEngine;

namespace Project
{
	public static class EnigineExtensions
	{
		public static bool ContainsLayer(this LayerMask mask, int layer)
		{
			return (mask & (1 << layer)) != 0;
		}
	}
}