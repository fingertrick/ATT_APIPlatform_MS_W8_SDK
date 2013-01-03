// <copyright file="ErrorToolTip.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Control which shows error in tool tip.
	/// </summary>
	public sealed class ErrorToolTip : Control
	{
		#region Dependency properties

		/// <summary>
		/// Error message
		/// </summary>
		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ErrorToolTip), new PropertyMetadata(""));

		#endregion

		/// <summary>
		/// Creates instance of <see cref="ErrorToolTip"/>
		/// </summary>
		public ErrorToolTip()
		{
			DefaultStyleKey = typeof(ErrorToolTip);
		}

		/// <summary>
		/// Gets or sets error message
		/// </summary>
		public string ErrorMessage
		{
			get { return GetValue(ErrorMessageProperty).ToString(); }
			set { SetValue(ErrorMessageProperty, value); }
		}
	}
}
