// <copyright file="Argument.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Linq.Expressions;
using System.Globalization;

namespace ATT.Utility
{
	/// <summary>
	/// Utility class used to encapsulate and simplify argument checks
	/// </summary>
	public static class Argument
	{
		/// <summary>
		/// Checks the argument value and throws <see cref="ArgumentNullException"/> if it is null
		/// </summary>
		/// <typeparam name="T">tye of the argument to check</typeparam>
		/// <param name="f">expression pointing to the argument</param>
		/// <param name="message">message to include in exception</param>
		public static void ExpectNotNull<T>(Expression<Func<T>> f, string message = null)
		{
			string argumentName = (f.Body as MemberExpression).Member.Name;
			Func<T> func = f.Compile();

			if (func() == null)
			{
				throw new ArgumentNullException(argumentName, message);
			}
		}

		/// <summary>
		/// Checks string argument and throws <see cref="ArgumentNullException"/> if it is null, empty or consists only of white-space characters.
		/// </summary>
		/// <param name="f">Expression pointing to the argument</param>
		/// <param name="message">message to include in exception</param>
		public static void ExpectNotNullOrWhiteSpace(Expression<Func<string>> f, string message = null)
		{
			string argumentName = (f.Body as MemberExpression).Member.Name;
			Func<string> func = f.Compile();

			if (String.IsNullOrWhiteSpace(func()))
			{
				throw new ArgumentNullException(argumentName, message);
			}
		}

		/// <summary>
		/// Checks Guid argument and throws <see cref="ArgumentException"/> if it equals to empty Guid
		/// </summary>
		/// <param name="f">Expression pointing to the argument</param>
		public static void ExpectNotEmptyGuid(Expression<Func<Guid>> f)
		{
			string argumentName = (f.Body as MemberExpression).Member.Name;
			Func<Guid> func = f.Compile();

			if (func() == Guid.Empty)
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "{0} cannot be an empty Guid", argumentName), argumentName);
			}
		}

		/// <summary>
		/// Checks custom argument condition and throws <see cref="ArgumentException"/> if its result is false
		/// </summary>
		/// <param name="condition">Condition expression to check</param>
		/// <param name="argumentName">name of the argument checked by condition</param>
		/// <param name="errorMessage">error message to include in exception</param>
		public static void Expect(Func<bool> condition, string argumentName, string errorMessage)
		{
			if (!condition())
			{
				throw new ArgumentException(errorMessage, argumentName);
			}
		}
	}
}
