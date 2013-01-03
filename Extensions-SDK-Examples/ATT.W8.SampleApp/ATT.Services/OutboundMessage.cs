// <copyright file="PhoneMessage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.Generic;
using System.Linq;
using ATT.Utility;

namespace ATT.Services
{
	/// <summary>
	/// Represents a message which can be sent to phone devices.
	/// </summary>
	public abstract class OutboundMessage
	{
		private readonly PhoneNumber[] _phoneNumbers;

		/// <summary>
		/// Creates instance of <see cref="PhoneNumber"/>.
		/// </summary>
		/// <param name="phoneNumbers">Collection of phone numbers to send message to.</param>
		/// <param name="body">message text.</param>
		protected OutboundMessage(IEnumerable<PhoneNumber> phoneNumbers, string body)
		{
			Argument.ExpectNotNull(() => phoneNumbers);

			_phoneNumbers = phoneNumbers.ToArray();
			Body = body != null ? body.Replace("\r\n", "\n") : null;
		}

		/// <summary>
		/// Gets message body.
		/// </summary>
		public string Body { get; private set; }

		/// <summary>
		/// Gets receiver phone numbers.
		/// </summary>
		public IEnumerable<PhoneNumber> PhoneNumbers { get { return _phoneNumbers; } }

		/// <summary>
		/// Gets or sets message identifier.
		/// </summary>
		public string MessageId { get; set; }
	}
}
