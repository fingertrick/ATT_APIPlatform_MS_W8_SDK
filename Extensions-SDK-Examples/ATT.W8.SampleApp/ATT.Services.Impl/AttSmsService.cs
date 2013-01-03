// <copyright file="AttSmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATT.Utility;
using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Implementation of AT&amp;T SMS service 
	/// </summary>
	public class AttSmsService : AttApiService, ISmsService
	{
		private const int GetSmsPollPeriodMilliseconds = 250;

		private readonly SmsService _smsServiceWrapper;

		/// <summary>
		/// Creates instance of <see cref="AttSmsService"/>
		/// </summary>
		/// <param name="endPoint">AT&amp;T Service endpoint.</param>
		/// <param name="apiKey">API key.</param>
		/// <param name="secretKey">Secret key.</param>
		public AttSmsService(string endPoint, string apiKey, string secretKey)
			: base(endPoint, apiKey, secretKey)
		{
			_smsServiceWrapper = new SmsService(Settings);
		}

		/// <summary>
		/// Sends SMS message to multiple recipients.
		/// </summary>
		/// <param name="sms">instance of <see cref="SmsMessage"/> to send</param>
		/// <returns>SMS message sent</returns>
		/// <exception cref="System.ArgumentNullException">sms is null</exception>
		public async Task<SmsMessage> Send(SmsMessage sms)
		{
			Argument.ExpectNotNull(() => sms);

			SmsResponse resp = await _smsServiceWrapper.SendSms(sms.PhoneNumbers.Select(p => p.Number).ToList(), sms.Body);
			sms.MessageId = resp.Id;

			return sms;
		}

		/// <summary>
		/// Gets SMS delivery status.
		/// </summary>
		/// <param name="smsId">Id of SMS sent.</param>
		/// <exception cref="System.ArgumentNullException">smsId is null.</exception>
		public async Task<MessageDeliveryStatus> GetSmsStatus(string smsId)
		{
			Argument.ExpectNotNull(() => smsId);

			DeliveryInfoList resp = await _smsServiceWrapper.GetSmsStatus(smsId);

            // AT&T service returns message delivery status as string - cast it to enum with all possible delivery statuses
			return (MessageDeliveryStatus)Enum.Parse(typeof(MessageDeliveryStatus), resp.DeliveryStatus.DeliveryInfoList.First().DeliveryStatus);
		}

		/// <summary>
		/// Gets collection of inbound SMS messages sent to some short code.
		/// </summary>
		/// <param name="shortCode">Short code.</param>
		/// <exception cref="System.ArgumentNullException">shortCode is null.</exception>
		public async Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => shortCode);

			var result = new List<InboundSms>();
			int messagesLeft = 1;

			// Call get SMS messages until there are no messages left on the server.
			while (messagesLeft > 0)
			{
				InboundSmsMessageList messages = await _smsServiceWrapper.GetInboundSmsMessages(shortCode);
				result.AddRange( 
					messages.InboundSmsMessage
						   .Select(m => new InboundSms(
												m.MessageId,
												new PhoneNumber(m.SenderAddress),
												m.Message)).ToList<InboundSms>()
				);
				messagesLeft = messages.TotalNumberOfPendingMessages;
			}

			await Task.Delay(GetSmsPollPeriodMilliseconds);
			return result;
		}
	}
}
