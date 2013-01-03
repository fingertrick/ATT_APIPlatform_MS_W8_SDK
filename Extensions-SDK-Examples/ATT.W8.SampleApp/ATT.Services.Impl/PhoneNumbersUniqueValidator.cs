// <copyright file="PhoneNumbersUniqueValidator.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using ATT.Utility;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Used to check collections of phone numbers for duplicates
	/// </summary>
	public static class PhoneNumbersUniqueValidator
	{
		/// <summary>
		/// Validates phone numbers collection to check if there are duplicated numbers
		/// </summary>
		/// <param name="phoneNumbers">phone numbers collection to validate</param>
		/// <returns>false if the are at least two duplicated phone number in the collection</returns>
		public static bool Validate(IEnumerable<string> phoneNumbers)
		{
			Argument.ExpectNotNull(() => phoneNumbers);

			var uniqueNumbers = new HashSet<string>(phoneNumbers, StringComparer.OrdinalIgnoreCase);

			return uniqueNumbers.Count == phoneNumbers.Count();
		}
	}
}
