// <copyright file="PhoneNumbersInputParser.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using ATT.Utility;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Handles parsing of phone numbers input text
	/// </summary>
	public static class PhoneNumbersInputParser
	{
		/// <summary>
		/// Parses phone numbers comma-or-semicolon-separated input text to return list of entered phone numbers
		/// </summary>
		/// <param name="input">string with comma-or-semicolon-separated phone numbers</param>
		/// <returns></returns>
		public static IEnumerable<string> Parse(string input)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => input);

			return input.Replace(" ", String.Empty)
							   .Replace("\r\n", String.Empty)
							   .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
