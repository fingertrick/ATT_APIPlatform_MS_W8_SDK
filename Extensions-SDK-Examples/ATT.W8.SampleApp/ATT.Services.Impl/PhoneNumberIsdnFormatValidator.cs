// <copyright file="PhoneNumberIsdnFormatValidator.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using ATT.WP8.SDK.Utils;
using Argument = ATT.Utility.Argument;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Used to validate phone numbers against ISDN format
	/// </summary>
	public static class PhoneNumberIsdnFormatValidator
	{
		/// <summary>
		/// Validates phone number on ISDN format
		/// </summary>
		/// <param name="phoneNumber">phone number to validate</param>
		/// <returns>false if the phone number cannot be converted to ISDN format</returns>
		public static bool Validate(string phoneNumber)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => phoneNumber);

			try
			{
				PhoneNumberConverter.ConvertToIsdn(phoneNumber);
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}			
		}

		/// <summary>
		/// Convert phone number to ISDN format.  Removes dashes, removes "+" from country code, etc.
		/// </summary>
		/// <param name="numberToConvert">Phone number to convert.</param>
		/// <returns>Converted phone number.</returns>
		/// <exception cref="System.ArgumentNullException">numberToConvert is null.</exception>
		/// <exception cref="System.ArgumentException">numberToConvert is in incorrect format</exception>
		public static string ConvertToIsdn(string numberToConvert)
		{
			return PhoneNumberConverter.ConvertToIsdn(numberToConvert);
		}
	}
}
