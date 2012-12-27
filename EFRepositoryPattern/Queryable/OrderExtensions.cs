using System;
using System.Collections.Generic;
using System.Linq;

namespace EFRepository.Queryable
{
	public static class OrderExtensions
	{
		public static String ToOrderByString<T>(this IEnumerable<Order<T>> orderSpecs)
		{
			return orderSpecs.Aggregate(String.Empty,
			                            (s, o) =>
			                            s.Length == 0
			                            	? s + o
			                            	: s + ',' + o);
		}
	}
}