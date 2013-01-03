// <copyright file="MessagePreparedEventArgs.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;

using ATT.Services;

namespace ATT.Controls
{
	/// <summary>
	/// EventArgs for Prepared mms message
	/// </summary>
	public class MessagePreparedEventArgs : EventArgs
	{
		/// <summary>
		/// Creates instance of <see cref="MessagePreparedEventArgs"/>
		/// </summary>
		/// <param name="message"></param>
		public MessagePreparedEventArgs(OutboundMessage message)
		{			
			Message = message;
		}
	
		/// <summary>
		/// Gets or sets original message
		/// </summary>
		public OutboundMessage Message { get; private set; }
	}
}
