// <copyright file="ExtendedTextBox.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Extended TextBox (notified by CustomAction when text was changed)
	/// </summary>
	public class ExtendedTextBox : TextBox
	{
		/// <summary>
		/// CustomAction
		/// </summary>
		public static readonly DependencyProperty CustomActionProperty = DependencyProperty.Register("CustomAction", typeof(Action<string>), typeof(ExtendedTextBox), new PropertyMetadata(null, OnPropertyChanged));

		/// <summary>
		/// CustomAction
		/// </summary>
		public Action<string> CustomAction
		{
			get
			{
				return (Action<string>)GetValue(CustomActionProperty);
			}
			set
			{
				SetValue(CustomActionProperty, value);
			}
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
				(d as ExtendedTextBox).TextChanged += ExtendedTextBox_TextChanged;
			else
				(d as ExtendedTextBox).TextChanged -= ExtendedTextBox_TextChanged;
		}

		private async static void ExtendedTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var extendedTextBox = sender as ExtendedTextBox;
			await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.High, () => extendedTextBox.CustomAction(extendedTextBox.Text));
		}
	}
}
