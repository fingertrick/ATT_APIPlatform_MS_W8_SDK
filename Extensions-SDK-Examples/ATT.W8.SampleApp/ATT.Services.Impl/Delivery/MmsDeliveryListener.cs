// <copyright file="MmsDeliveryListener.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading.Tasks;
using ATT.Utility;

namespace ATT.Services.Impl.Delivery
{
	/// <summary>
	/// Handles MMS delivery listening.
	/// </summary>
	public class MmsDeliveryListener : MessageDeliveryListener
	{
		private readonly IMmsService _mmsService;	   

		/// <summary>
		/// Creates new instance of <see cref="MmsDeliveryListener"/>
		/// </summary>
		/// <param name="mmsService">Instance of <see cref="IMmsService"/></param>
		/// <param name="message">MMS message listen should check delivery status for.</param>
		/// <param name="pollPeriod">Time interval between delivery status polls.</param>
		/// <param name="timeout">If delivery status is not changed after this time interval then listener will stop polling.</param>
		public MmsDeliveryListener(
			IMmsService mmsService,
			MmsMessage message,
			TimeSpan pollPeriod,
			TimeSpan timeout)
			: base(message, pollPeriod, timeout)
		{
			Argument.ExpectNotNull(() => mmsService);		  

			_mmsService = mmsService;		   
		}

		/// <summary>
		/// Gets poll message status.
		/// </summary>
		/// <returns>Poll message status.</returns>
		protected async override Task<MessageDeliveryStatus> PollMessageStatus()
		{
			return await _mmsService.GetMmsStatus(Message.MessageId);
		}
	}
}
