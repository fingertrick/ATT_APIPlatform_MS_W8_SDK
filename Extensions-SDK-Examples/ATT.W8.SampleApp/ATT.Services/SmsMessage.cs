// <copyright file="Sms.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.Generic;

namespace ATT.Services
{
	/// <summary>
	/// Represents SMS message.
	/// </summary>
	public class SmsMessage : OutboundMessage
	{
		/// <summary>
		/// Creates instance of <see cref="SmsMessage"/>.
		/// </summary>
		/// <param name="phoneNumbers">Collection of phone numbers to send SMS to.</param>
		/// <param name="body">SMS message text.</param>
		public SmsMessage(IEnumerable<PhoneNumber> phoneNumbers, string body)
			: base(phoneNumbers, body)
		{
		}
	}
}
