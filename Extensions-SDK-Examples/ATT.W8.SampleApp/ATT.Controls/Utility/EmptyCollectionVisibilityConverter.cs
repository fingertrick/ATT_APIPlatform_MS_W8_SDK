// <copyright file="EmptyCollectionVisibilityConverter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Converts empty collection to collapse visibility.
	/// </summary>
	public class EmptyCollectionVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Convert empty collection to Visibility.Collapsed
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns><see cref="Visibility"/></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value != null && value is ICollection && (value as ICollection).Count > 0)
			{
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		/// <summary>
		/// Convert back
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return null;
		}
	}
}
