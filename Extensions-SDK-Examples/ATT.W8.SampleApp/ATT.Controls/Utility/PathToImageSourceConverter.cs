// <copyright file="PathToImageSourceConverter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Convert path to image to <see cref="ImageSource"/>
	/// </summary>
	public class PathToImageSourceConverter : IValueConverter
	{
		/// <summary>
		/// Convert path to image to <see cref="ImageSource"/> 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value != null && !String.IsNullOrEmpty(value.ToString()))
			{
				var bitmapImage = new BitmapImage(new Uri(value.ToString()))
									  {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
				return bitmapImage;
			}

			return null;
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
