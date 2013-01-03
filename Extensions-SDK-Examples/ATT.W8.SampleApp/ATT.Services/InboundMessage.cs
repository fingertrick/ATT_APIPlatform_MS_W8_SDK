// <copyright file="InboundMessage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using ATT.Utility;

namespace ATT.Services
{
	/// <summary>
	/// Represents an interface for inbound messages.
	/// </summary>
	public abstract class InboundMessage
	{
		 /// <summary>
		/// Creates new instance of <see cref="InboundMessage"/>.
		/// </summary>
		/// <param name="id">Id of the message</param>
		/// <param name="senderNumber">phone number of a sender who sent this message</param>
		/// <param name="body">message body</param>
		/// <exception cref="System.ArgumentNullException">id is null or senderNumber is null</exception>
		protected InboundMessage(string id, PhoneNumber senderNumber, string body)
		{
			Argument.ExpectNotNull(() => id);
			Argument.ExpectNotNull(() => senderNumber);

			Id = id;
			SenderNumber = senderNumber;
			Body = body ?? String.Empty;
		}

		/// <summary>
		/// Gets message Id.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Gets phone number of a sender who sent this message.
		/// </summary>
		public PhoneNumber SenderNumber { get; private set; }

		/// <summary>
		/// Gets message body.
		/// </summary>
		public string Body { get; private set; }
	}
}
