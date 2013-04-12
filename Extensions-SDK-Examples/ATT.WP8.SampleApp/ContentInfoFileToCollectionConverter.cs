// <copyright file="ContentInfoFileToCollectionConverter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using ATT.WP8.SDK;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// ContentInfo to ObservableCollection ContentInfo converter.
	/// </summary>
	public class ContentInfoFileToCollectionConverter : IValueConverter
	{
		/// <summary>
		/// Converts ContentInfo file to ObservableCollection ContentInfo value. 
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>ContentInfo ObservableCollection.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var val = value as ContentInfo;
			return val != null ? new ObservableCollection<ContentInfo> { val } : null;
		}

		/// <summary>
		/// Converts back. Not supported.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>Always null.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}