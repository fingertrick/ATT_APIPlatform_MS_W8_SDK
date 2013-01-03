// <copyright file="NotifyPropertyChangedBase.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ATT.Utility;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Base class that raises the property changed.
	/// </summary>
	public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
	{
		private static readonly Dictionary<string, PropertyChangedEventArgs> _eventArgsCache =
			new Dictionary<string, PropertyChangedEventArgs>();

		/// <summary>
		/// PropertyChanged event, occurs when some property is modified.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property changed event using a string
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property that changed
		/// </param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => propertyName);

			VerifyProperty(propertyName);
			OnPropertyChangedCore(propertyName);
		}

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="propertyExpression">
		/// Expression pointing to the property.
		/// </param>
		/// <typeparam name="T">
		/// The type of the property (usually implicit)
		/// </typeparam>
		/// <exception cref="ArgumentNullException">
		/// Raised if the property name is null
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Raised if the property does not point to a valid member
		/// </exception>
		protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			Argument.ExpectNotNull(() => propertyExpression);

			var memberExpression = propertyExpression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("Property Expression body should be a MemberExpression", "propertyExpression");
			}

			var propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("Property Expression member should be a Property Info", "propertyExpression");
			}

			OnPropertyChanged(propertyInfo.Name);
		}

		private static PropertyChangedEventArgs LookupEventArgs(string propertyName)
		{
			PropertyChangedEventArgs e;
			if (!_eventArgsCache.TryGetValue(propertyName, out e))
			{
				e = new PropertyChangedEventArgs(propertyName);
				_eventArgsCache.Add(propertyName, e);
			}

			return e;
		}

		private void OnPropertyChangedCore(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChangedEventArgs eventArgs = LookupEventArgs(propertyName);
				PropertyChanged(this, eventArgs);
			}

			CommandManager.InvalidateRequerySuggested();
		}

		private void VerifyProperty(string propertyName)
		{		   
			var allProperties = new List<PropertyInfo>(this.GetType().GetRuntimeProperties());
			if (!allProperties.Any(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)))
			{
				throw new ArgumentException(
							String.Format(CultureInfo.InvariantCulture, "The type '{0}' does not contain a property with the name '{1}'", this.GetType().Name, propertyName),
							"propertyName");
			}
		}
	}
}
