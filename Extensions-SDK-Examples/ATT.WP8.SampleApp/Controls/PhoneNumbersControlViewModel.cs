// <copyright file="PhoneNumbersControlViewModel.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;
using ATT.WP8.Controls;

namespace ATT.WP8.SampleApp.Controls
{
	/// <summary>
	/// PhoneNumbersControlViewModel class.
	/// </summary>
	public class PhoneNumbersControlViewModel : ViewModelBase
	{
		private readonly PhoneNumberModel _phoneNumberModel;

		/// <summary>
		/// Inisializez new instance of <see cref="PhoneNumbersControlViewModel"/> class.
		/// </summary>
		public PhoneNumbersControlViewModel()
		{
			_phoneNumberModel = new PhoneNumberModel();
			_phoneNumberModel.PropertyChanged+=PhoneNumberModelOnPropertyChanged;
		}

		private void PhoneNumberModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			OnPropertyChanged(propertyChangedEventArgs.PropertyName);
		}

		/// <summary>
		/// Gets or sets tex phone number
		/// </summary>
		public string TextPhoneNumber
		{
			get { return _phoneNumberModel.TextPhoneNumber; }
			set { _phoneNumberModel.TextPhoneNumber = value; }
		}

		/// <summary>
		/// Gets or set Phone Numbers.
		/// </summary>
		public ObservableCollection<string> PhoneNumbers
		{
			get { return _phoneNumberModel.PhoneNumbers; }
			set { _phoneNumberModel.PhoneNumbers = value; }
		}

		/// <summary>
		/// Gets IsValidPhoneNumbers
		/// </summary>
		public bool IsValidPhoneNumbers { get { return _phoneNumberModel.IsValidPhoneNumbers; } }
	}
}