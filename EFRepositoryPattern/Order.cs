using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EFRepository
{
	public class Order<T>
	{
		public static Order<T> By(Expression<Func<T, object>> orderBy)
		{
			return new Order<T>(orderBy, false);
		}

		public static Order<T> ByDescending(Expression<Func<T, object>> orderBy)
		{
			return new Order<T>(orderBy, true);
		}

		public Order(Expression<Func<T, object>> orderBy, bool descending)
		{
			Descending = descending;

			GetPropertyInfo(orderBy);
		}

		public LambdaExpression OrderByExpression { get; private set; }

		public string PropertyName { get; private set; }
		public PropertyInfo PropertyInfo { get; private set; }
		public bool Descending { get; set; }

		public override string ToString()
		{
			return PropertyName + (Descending ? " DESC" : " ASC");
		}

		private void GetPropertyInfo<TProperty>(

			Expression<Func<T, TProperty>> propertyLambda)
		{
			Type type = typeof(T);

			MemberExpression member = null;

			if (propertyLambda.Body.NodeType == ExpressionType.Convert)
			{
				member = ((UnaryExpression)propertyLambda.Body).Operand as MemberExpression;
			}
			else
			{
				member = propertyLambda.Body as MemberExpression;
			}

			if (member == null)
			{
				throw new ArgumentException(string.Format(
					"Expression '{0}' refers to a method, not a property.",
					propertyLambda));
			}

			var propInfo = member.Member as PropertyInfo;
			if (propInfo == null)
			{
				throw new ArgumentException(string.Format(
					"Expression '{0}' refers to a field, not a property.",
					propertyLambda));
			}

			PropertyInfo = propInfo;

			if (type != propInfo.ReflectedType &&
			    !type.IsSubclassOf(propInfo.ReflectedType))
			{
				PropertyName = member.ToString().Substring(member.ToString().IndexOf(".") + 1);
			}
			else
			{
				PropertyName = propInfo.Name;
			}

			Type delegateType =
				typeof(Func<,>).MakeGenericType(typeof(T), propInfo.PropertyType);

			var param = Expression.Parameter(type);

			var props = PropertyName.Split(new[] { '.' });

			Expression propId = Expression.Property(param, props[0]);

			for (int n = 1; n < props.Length; n++)
			{
				propId = Expression.Property(propId, props[n]);
			}

			LambdaExpression lambda = Expression.Lambda(delegateType, propId, param);

			OrderByExpression = lambda;
		}
	}
}