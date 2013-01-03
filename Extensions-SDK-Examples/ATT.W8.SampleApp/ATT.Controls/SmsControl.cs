// <copyright file="SmsControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.UI.Xaml;
using ATT.Controls.Presenters;
using ATT.Services.Impl;

namespace ATT.Controls
{
	/// <summary>
	/// Use this control for sending SMS messages up to four kilobytes in length.
	/// </summary>
	public sealed class SmsControl : SenderControlBase
	{
		/// <summary>
		/// The max SMS length property, if not set explicitly, will be defined as 4096 bytes (4K).
		/// </summary>
		public static readonly DependencyProperty MaxSmsLengthProperty = DependencyProperty.Register("MaxSmsLength", typeof(int), typeof(SmsControl), new PropertyMetadata(4096));
	 
		/// <summary>
		/// Creates and initializes presenter instance for SMS control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			var smsSrv = new AttSmsService(Endpoint, ApiKeyConfigured, SecretKeyConfigured);
			return new SmsControlPresenter(smsSrv);
		}
		
		/// <summary>
		/// The Max length, in bytes, of an SMS to be sent.  You can use this to limit the message length for user interface reasons.  Defaults to 4K, which is the AT&amp;T network limit as of August 2012.  If, in the future, that limit changes, you can explicitly set a higher length with this variable.
		/// </summary>
		public int MaxSmsLength
		{
			get { return (int)GetValue(MaxSmsLengthProperty); }
			set { SetValue(MaxSmsLengthProperty, value); }
		}
	}
}
