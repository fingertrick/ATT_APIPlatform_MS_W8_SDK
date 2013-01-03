// <copyright file="InboundSms.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

namespace ATT.Services
{
	/// <summary>
	/// Represents received SMS message.
	/// </summary>
	public class InboundSms : InboundMessage
	{
		/// <summary>
		/// Creates new instance of <see cref="InboundSms"/>.
		/// </summary>
		/// <param name="id">Id of SMS message</param>
		/// <param name="senderNumber">phone number of a sender who sent this SMS message</param>
		/// <param name="body">SMS body</param>
		/// <exception cref="System.ArgumentNullException">id is null or senderNumber is null</exception>
		public InboundSms(string id, PhoneNumber senderNumber, string body)
			: base(id, senderNumber, body)
		{
		}
	}
}
