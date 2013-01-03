// <copyright file="MockMmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading.Tasks;

namespace ATT.Services.Mock
{
	/// <summary>
	/// Mock for mms service
	/// </summary>
	public class MockMmsService : IMmsService
	{
		private int _mmsStatusRequestCount;
		private const int MaxMmsStatusRequestCount = 20;

		/// <summary>
		/// Get status of mms by id
		/// </summary>
		/// <param name="mmsId">message id</param>
		/// <returns>current message delivery status</returns>
		static public MessageDeliveryStatus GetMmsStatusById(string mmsId)
		{
			bool fail = mmsId.IndexOf("fail", StringComparison.CurrentCulture) != -1;
			return fail ? MessageDeliveryStatus.DeliveryImpossible : MessageDeliveryStatus.DeliveredToTerminal;
		}

		/// <summary>
		/// Sends MMS message to multiple recipients
		/// </summary>
		/// <param name="mms">instance of <see cref="MmsMessage"/> to send</param>
		/// <returns>Returns Task as a result of asynchronous operation. Task result is generated message identifier.</returns>
		public Task<MmsMessage> Send(MmsMessage mms)
		{
			_mmsStatusRequestCount = 0;
			var msgId = new Guid().ToString();
			bool failMsg = mms.Body.IndexOf("fail", StringComparison.CurrentCulture) != -1;
			if (failMsg)
			{
				msgId += "fail";
			}
			mms.MessageId = msgId;

			return Task.FromResult(mms);
		}	   

		/// <summary>
		/// Gets sent MMS message delivery status.
		/// </summary>
		/// <param name="mmsId">message identifier.</param>
		/// <returns>Current message delivery status.</returns>
		public Task<MessageDeliveryStatus> GetMmsStatus(string mmsId)
		{
			if (_mmsStatusRequestCount == MaxMmsStatusRequestCount)
			{
				return Task.FromResult(GetMmsStatusById(mmsId));
			}
			_mmsStatusRequestCount++;
			return Task.FromResult(MessageDeliveryStatus.DeliveredToNetwork);
		}
	}
}
