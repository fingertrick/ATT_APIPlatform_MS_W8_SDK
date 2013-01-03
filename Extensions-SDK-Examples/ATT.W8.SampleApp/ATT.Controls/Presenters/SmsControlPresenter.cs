// <copyright file="SmsControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ATT.Services;
using ATT.Services.Impl.Delivery;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Presenter for <see cref="SmsControl"/>
	/// </summary>
	public class SmsControlPresenter : SenderPresenterBase
	{
		private readonly ISmsService _smsService;

		/// <summary>
		/// Creates instance of <see cref="SmsControlPresenter"/>
		/// </summary>
		/// <param name="srv">SMS service</param>
		public SmsControlPresenter(ISmsService srv)
		{
			_smsService = srv;
		}

		/// <summary>
		/// Send message
		/// </summary>
		protected override async Task Send()
		{
			IEnumerable<PhoneNumber> numbers = GetPhoneNumbers();
			var sms = new SmsMessage(numbers, Message);

			CurrentMessage = await _smsService.Send(sms);
		}

		/// <summary>
		/// Create new <see cref="MessageDeliveryListener"/>
		/// </summary>
		/// <returns><see cref="MessageDeliveryListener"/></returns>
		protected override MessageDeliveryListener CreateMessageDeliveryListener()
		{
			return new SmsDeliveryListener(_smsService, (SmsMessage)CurrentMessage, TimeSpan.FromMilliseconds(GetStatusPollMilliseconds), TimeSpan.FromMinutes(GetStatusTimeoutMinutes));
		}

		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public override void Unload()
		{
			base.Unload();
			ClearControl();			
		}
	}
}
