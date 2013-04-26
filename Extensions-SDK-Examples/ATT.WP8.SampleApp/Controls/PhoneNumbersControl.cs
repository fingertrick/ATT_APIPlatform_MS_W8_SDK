// <copyright file="PhoneNumbersControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Windows;
using ATT.WP8.Controls;

namespace ATT.WP8.SampleApp.Controls
{
	/// <summary>
	/// Phone numbers control
	/// </summary>
	public sealed class PhoneNumbersControl : ControlBase
	{
		/// <summary>
		/// Creates instance of <see cref="PhoneNumbersControl"/>
		/// </summary>
		public PhoneNumbersControl()
		{
			DefaultStyleKey = typeof(PhoneNumbersControl);
			ViewModel = CreateViewModel();
		}

		/// <summary>
		/// Style for textBox with phone numbers
		/// </summary>
		public static readonly DependencyProperty PhoneNumberStyleProperty = DependencyProperty.Register("PhoneNumberStyle", typeof(Style), typeof(PhoneNumbersControl), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets style for textBox with phone numbers
		/// </summary>
		public Style PhoneNumberStyle
		{
			get { return GetValue(PhoneNumberStyleProperty) as Style; }
			set { SetValue(PhoneNumberStyleProperty, value); }
		}

		/// <summary>
		/// Text phone number field
		/// </summary>
		public static readonly DependencyProperty TextPhoneNumberProperty =
			DependencyProperty.Register("TextPhoneNumber", typeof(string), typeof(PhoneNumbersControl),
				new PropertyMetadata(String.Empty, TextPhoneNumberPropertyChangedCallback));

		private static void TextPhoneNumberPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as PhoneNumbersControl;
			if (control != null)
			{
				control.GetViewModel().TextPhoneNumber = dependencyPropertyChangedEventArgs.NewValue as string;
			}
		}

		/// <summary>
		/// Gets or sets text phone number field
		/// </summary>
		public string TextPhoneNumber
		{
			get { return GetValue(TextPhoneNumberProperty) as String; }
			set { SetValue(TextPhoneNumberProperty, value); }
		}

		/// <summary>
		/// Text header field
		/// </summary>
		public static readonly DependencyProperty PhoneNumberLabelProperty = DependencyProperty.Register("PhoneNumberLabel", typeof(string), typeof(PhoneNumbersControl), new PropertyMetadata("Phone Number(s)"));

		/// <summary>
		/// Gets or sets text header field
		/// </summary>
		public string PhoneNumberLabel
		{
			get { return GetValue(PhoneNumberLabelProperty) as String; }
			set { SetValue(PhoneNumberLabelProperty, value); }
		}

		/// <summary>
		/// Gets collection of phone numbers
		/// </summary>
		public static readonly DependencyProperty PhoneNumbersProperty =
			DependencyProperty.Register("PhoneNumbers", typeof(ObservableCollection<string>), typeof(PhoneNumbersControl),
			new PropertyMetadata(default(ObservableCollection<string>), PhoneNumbersPropertyChangedCallback));

		private static void PhoneNumbersPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as PhoneNumbersControl;
			if (control != null)
			{
				control.GetViewModel().PhoneNumbers = dependencyPropertyChangedEventArgs.NewValue as ObservableCollection<string>;
			}
		}

		/// <summary>
		/// Gets collection of phone numbers
		/// </summary>
		public ObservableCollection<string> PhoneNumbers
		{
			get { return (ObservableCollection<string>)GetValue(PhoneNumbersProperty); }
			set { SetValue(PhoneNumbersProperty, value); }
		}

		/// <summary>
		/// Gets or set is valid phone numbers.
		/// </summary>
		public static readonly DependencyProperty IsValidPhoneNumbersProperty =
			DependencyProperty.Register("IsValidPhoneNumbers", typeof(bool), typeof(PhoneNumbersControl), new PropertyMetadata(default(bool)));

		/// <summary>
		/// Gets or sets is valid phone numbers.
		/// </summary>
		public bool IsValidPhoneNumbers
		{
			get { return (bool) GetValue(IsValidPhoneNumbersProperty); }
			set { SetValue(IsValidPhoneNumbersProperty, value); }
		}

		/// <summary>
		/// Update binded ViewModel property
		/// </summary>
		public Action<string> UpdateBindedViewModelProperty
		{
			get { return value => TextPhoneNumber = value; }
		}

		private PhoneNumbersControlViewModel GetViewModel()
		{
			return ViewModel as PhoneNumbersControlViewModel;
		}

		private PhoneNumbersControlViewModel CreateViewModel()
		{
			return new PhoneNumbersControlViewModel();
		}
	}
}
