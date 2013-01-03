// <copyright file="MessageStatusEventArgs.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;

namespace ATT.Services.Impl.WeakEvent
{
	/// <summary>
	/// EventArgs for MessageStatus
	/// </summary>
	public class MessageStatusEventArgs : EventArgs
	{
		/// <summary>
		/// Creates instance of <see cref="MessageStatusEventArgs"/>
		/// </summary>
		/// <param name="status"></param>
		/// <param name="message"></param>
		public MessageStatusEventArgs(MessageDeliveryStatus status, OutboundMessage message)
		{
			MessageStatus = status;
			Message = message;
		}

		/// <summary>
		/// Gets or sets message status
		/// </summary>
		public MessageDeliveryStatus MessageStatus { get; private set; }

		/// <summary>
		/// Gets or sets original message
		/// </summary>
		public OutboundMessage Message { get; private set; }
	}
}
