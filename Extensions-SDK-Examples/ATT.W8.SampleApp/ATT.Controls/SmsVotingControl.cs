// <copyright file="SmsVotingControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.Controls.Presenters;
using ATT.Services;
using ATT.Services.Impl;

namespace ATT.Controls
{
	/// <summary>
	/// This control visualizes voting statistics based on a short code.
	/// </summary>
	public sealed class SmsVotingControl : ControlWithPresenter
	{
		/// <summary>
		/// Gets or sets SMS voting storage from the AT&amp;T SDK.
		/// </summary>
		public ISmsVotingStorage SmsVotingStorage { get; set; }

		/// <summary>
		/// Gets or sets SMS voting short code - the number votes will "call" into in order to vote.
		/// Learn more about short codes, and how to get them, at developer.att.com.
		/// </summary>
		public string SmsShortCode { get; set; }

		/// <summary>
		/// Creates instance of <see cref="SmsVotingControl"/>.
		/// </summary>
		public SmsVotingControl()
		{
			Loaded += (o, e) => ((SmsVotingControlPresenter)Presenter).Listen();
		}

		/// <summary>
		/// Creates and initializes presenter instance for SMS Voting control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			var smsSrv = new AttSmsService(Endpoint, ApiKeyConfigured, SecretKeyConfigured);
			return new SmsVotingControlPresenter(SmsVotingStorage, smsSrv, SmsShortCode);
		}
	}
}
