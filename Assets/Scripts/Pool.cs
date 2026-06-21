using System;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
	public class Pool<T> where T : class
	{
		readonly List<T> pool = new();
		readonly Func<T> create;
		readonly Func<T, bool> isReleased;

		public Pool(Func<T> create, Func<T, bool> isReleased)
		{
			this.create = create;
			this.isReleased = isReleased;
		}

		public T GetReleased()
		{
			T val = pool.FirstOrDefault(isReleased);
			if (val != null) return val;

			val = create();
			pool.Add(val);
			return val;
		}
	}
}