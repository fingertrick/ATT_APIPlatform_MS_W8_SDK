// <copyright file="PhoneNumbersControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Phone numbers control
	/// </summary>
	public sealed class PhoneNumbersControl : Control
	{
		#region Dependency properties

		/// <summary>
		/// Style for textBox with phone numbers
		/// </summary>
		public static readonly DependencyProperty PhoneNumberStyleProperty = DependencyProperty.Register("PhoneNumberStyle", typeof(Style), typeof(PhoneNumbersControl), new PropertyMetadata(null));

		/// <summary>
		/// Text phone number field
		/// </summary>
		public static readonly DependencyProperty TextPhoneNumberProperty = DependencyProperty.Register("TextPhoneNumber", typeof(string), typeof(PhoneNumbersControl), new PropertyMetadata(""));

		/// <summary>
		/// Text header field
		/// </summary>
		public static readonly DependencyProperty PhoneNumberLabelProperty = DependencyProperty.Register("PhoneNumberLabel", typeof(string), typeof(PhoneNumbersControl), new PropertyMetadata("Phone Number(s)"));
		
		#endregion

		/// <summary>
		/// Creates instance of <see cref="PhoneNumbersControl"/>
		/// </summary>
		public PhoneNumbersControl()
		{
			DefaultStyleKey = typeof(PhoneNumbersControl);
		}

		/// <summary>
		/// Gets or sets style for textBox with phone numbers
		/// </summary>
		public Style PhoneNumberStyle
		{
			get { return (Style)GetValue(PhoneNumberStyleProperty); }
			set { SetValue(PhoneNumberStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets text header field
		/// </summary>
		public string PhoneNumberLabel
		{
			get { return GetValue(PhoneNumberLabelProperty).ToString(); }
			set { SetValue(PhoneNumberLabelProperty, value); }
		}

		/// <summary>
		/// Gets or sets text phone number field
		/// </summary>
		public string TextPhoneNumber
		{
			get { return GetValue(TextPhoneNumberProperty).ToString(); }
			set { SetValue(TextPhoneNumberProperty, value); }
		}

		/// <summary>
		/// Update binded ViewModel property
		/// </summary>
		public Action<string> UpdateBindedViewModelProperty
		{
			get { return value => TextPhoneNumber = value; }
		}
	}
}
