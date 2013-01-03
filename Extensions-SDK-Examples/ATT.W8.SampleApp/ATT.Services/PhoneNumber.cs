// <copyright file="PhoneNumber.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.Utility;

namespace ATT.Services
{
	/// <summary>
	/// A phone number data object.
	/// </summary>
	public class PhoneNumber
	{
		/// <summary>
		/// Creates instance of <see cref="PhoneNumber"/>
		/// </summary>
		/// <param name="number">Phone number</param>
		public PhoneNumber(string number)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => number);

			Number = number;
		}

		/// <summary>
		/// Gets phone number.
		/// </summary>
		public string Number { get; private set; }
	}
}
