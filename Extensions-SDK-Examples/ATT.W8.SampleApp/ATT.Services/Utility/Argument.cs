using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ATT.Services.Utility
{
    public static class Argument
    {
		public static void ExpectNotNull<T>(Expression<Func<T>> f, string message = null)
        {
            var argumentName = (f.Body as MemberExpression).Member.Name;
            var func = f.Compile();
			if (func() == null)
			{
				throw new ArgumentNullException(argumentName, message);
			}
        }

		public static void ExpectNotNullOrWhiteSpace(Expression<Func<string>> f, string message = null)
		{
			var argumentName = (f.Body as MemberExpression).Member.Name;
			var func = f.Compile();
			if (String.IsNullOrWhiteSpace(func()))
			{
				throw new ArgumentNullException(argumentName, message);
			}
		}

		public static void ExpectNotEmptyGuid(Expression<Func<Guid>> f)
        {
            var argumentName = (f.Body as MemberExpression).Member.Name;
            var func = f.Compile();
			if (func() == Guid.Empty)
			{
				throw new ArgumentException(string.Format("{0} cannot be an empty Guid", argumentName), argumentName);
			}
        }

        public static void Expect(Func<bool> condition, string argumentName, string errorMessage)
        {
            if (!condition())
            {
                throw new ArgumentException(errorMessage, argumentName);
            }
        }
    }
}
