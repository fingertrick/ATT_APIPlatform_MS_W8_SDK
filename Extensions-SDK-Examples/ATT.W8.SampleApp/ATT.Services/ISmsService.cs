// <copyright file="ISmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATT.Services
{
	/// <summary>
	/// Represents a service used to send, receive and manage SMS messages.
	/// </summary>
	public interface ISmsService
	{	  
		/// <summary>
		/// Sends SMS message.
		/// </summary>
		/// <param name="sms">Instance of <see cref="SmsMessage"/> to send.</param>
		/// <returns>SMS message sent.</returns>
		Task<SmsMessage> Send(SmsMessage sms);

		/// <summary>
		/// Gets SMS delivery status.
		/// </summary>
		/// <param name="smsId">Id of SMS sent.</param>
		Task<MessageDeliveryStatus> GetSmsStatus(string smsId);

		/// <summary>
		/// Gets collection of inbound SMS messages sent to some short code.
		/// </summary>
		/// <param name="shortCode">Short code.</param>
		Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode);
	}
}
