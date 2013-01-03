// <copyright file="InboundMms.cs" company="AT&amp;T">
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
	/// Represents received MMS message.
	/// </summary>
	public class InboundMms : InboundMessage
	{
		private string[] _attachments;

		/// <summary>
		/// Creates new instance of <see cref="InboundMms"/>.
		/// </summary>
		/// <param name="id">Id of MMS message</param>
		/// <param name="senderNumber">phone number of a sender who sent this MMS message</param>
		/// <param name="body">MMS body</param>
		public InboundMms(string id, PhoneNumber senderNumber, string body)
			: this(id, senderNumber, body, Enumerable.Empty<string>())
		{
		}

		/// <summary>
		/// Creates new instance of <see cref="InboundMms"/>.
		/// </summary>
		/// <param name="id">Id of MMS message</param>
		/// <param name="senderNumber">phone number of a sender who sent this MMS message</param>
		/// <param name="body">MMS body</param>
		/// <param name="attachments">MMS attachments</param>
		public InboundMms(string id, PhoneNumber senderNumber, string body, IEnumerable<string> attachments)
			: base(id, senderNumber, body)
		{
			Argument.ExpectNotNull(() => attachments);

			_attachments = attachments.ToArray();
		}

		/// <summary>
		/// Gets MMS attachments.
		/// </summary>
		public IEnumerable<string> Attachments { get { return _attachments; } }
	}
}
