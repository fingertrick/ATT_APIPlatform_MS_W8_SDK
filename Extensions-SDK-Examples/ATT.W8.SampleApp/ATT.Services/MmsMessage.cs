// <copyright file="Mms.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

using ATT.Utility;

namespace ATT.Services
{
	/// <summary>
	/// This object represents an MMS message service.
	/// </summary>
	public class MmsMessage : OutboundMessage
	{
		private StorageFile[] _attachments;

		/// <summary>
		/// Creates new instance of <see cref="MmsMessage"/>
		/// </summary>
		/// <param name="phoneNumbers">Collection of phone numbers to send MMS to.</param>
		/// <param name="body">MMS text message.</param>
		public MmsMessage(IEnumerable<PhoneNumber> phoneNumbers, string body)
			: base(phoneNumbers, body)
		{
			_attachments = new StorageFile[0];
		}

		/// <summary>
		/// Creates new instance of <see cref="MmsMessage"/>.
		/// </summary>
		/// <param name="phoneNumbers">Collection of phone numbers to send MMS to.</param>
		/// <param name="body">MMS text message.</param>
		/// <param name="attachments">Collection of attached files.</param>
		/// <exception cref="System.ArgumentNullException">attachments is null</exception>
		public MmsMessage(IEnumerable<PhoneNumber> phoneNumbers, string body, IEnumerable<StorageFile> attachments)
			: base(phoneNumbers, body)
		{
			Argument.ExpectNotNull(() => attachments);

			_attachments = attachments.ToArray();		  
		}

		/// <summary>
		/// add new StorageFile attachment to Attachments collection
		/// </summary>
		/// <param name="attachment"></param>
		public void AddAttachment(StorageFile attachment)
		{
			_attachments = Enumerable.Concat<StorageFile>(_attachments, new[] { attachment }).ToArray();
		}

		/// <summary>
		/// Gets MMS attachments.
		/// </summary>
		public IEnumerable<StorageFile> Attachments { get { return _attachments; } }
	}
}
