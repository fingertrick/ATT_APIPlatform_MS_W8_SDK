// <copyright file="MockSmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATT.Services.Mock
{
	/// <summary>
	/// Mock for sms service
	/// </summary>
	public class MockSmsService : ISmsService
	{
		private int _smsStatusRequestCount;
		private const int MaxSmsStatusRequestCount = 20;

		private List<InboundSms> _inboundSms = new List<InboundSms>();
		private Random _rand = new Random();

		/// <summary>
		/// Gets status of sms by identifier.
		/// </summary>
		/// <param name="smsId">Id of SMS sent.</param>
		/// <returns>SMS delivery status.</returns>
		static public MessageDeliveryStatus GetSmsStatusById(string smsId)
		{
			bool fail = smsId.IndexOf("fail", StringComparison.CurrentCulture) >= 0;
			return fail ? MessageDeliveryStatus.DeliveryImpossible : MessageDeliveryStatus.DeliveredToTerminal;
		}

		/// <summary>
		/// Sends SMS message to multiple recipients.
		/// </summary>
		/// <param name="sms">Instance of <see cref="SmsMessage"/> to send</param>
		/// <returns>Generated message identifier.</returns>
		public Task<SmsMessage> Send(SmsMessage sms)
		{
			_smsStatusRequestCount = 0;
			var msgId = new Guid().ToString();
			bool failMsg = sms.Body.IndexOf("fail", StringComparison.CurrentCulture) >= 0;
			if (failMsg)
			{
				msgId += "fail";
			}
			else
			{
				foreach (var phoneNumber in sms.PhoneNumbers)
				{
					_inboundSms.Add(new InboundSms(msgId, phoneNumber, sms.Body));
				}
			}
			sms.MessageId = msgId;

			return Task.FromResult(sms);
		}
	   

		/// <summary>
		/// Gets SMS delivery status
		/// </summary>
		/// <param name="smsId">Id of SMS sent</param>
		public Task<MessageDeliveryStatus> GetSmsStatus(string smsId)
		{
			if (_smsStatusRequestCount == MaxSmsStatusRequestCount)
			{
				return Task.FromResult(GetSmsStatusById(smsId));
			}
			_smsStatusRequestCount++;
			return Task.FromResult(MessageDeliveryStatus.DeliveredToNetwork);
		}

		private static IList<string> _msgKeys = new List<string>
		{
			"orange",
			"yellow",
			"brown",
			"green"
		};
		
		/// <summary>
		/// Gets collection of inbound SMS messages sent to some short code
		/// </summary>
		/// <param name="shortCode">short code</param>
		public async Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode)
		{
			var result = new List<InboundSms>();

			int count = _rand.Next(5); // Number of sms
			for (int i = 0; i < count; i++)
			{
				var sms = new InboundSms(new Guid().ToString(), new PhoneNumber(shortCode), _msgKeys[_rand.Next(100) % 4]);
				result.Add(sms);
			}

			_inboundSms.Clear();

			await Task.Delay(100);
			return result;
		}
	}
}
