// <copyright file="SenderControlBase.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.Controls.Presenters;
using System;
using Windows.UI.Xaml;

namespace ATT.Controls
{
	/// <summary>
	/// Base class for sender controls like SmsControl, MmsControl, MmsCouponControl.
	/// </summary>
	public abstract class SenderControlBase : ControlWithPresenter
	{
		/// <summary>
		/// add and remove subscribers to MessagePrepared event
		/// </summary>
		public event EventHandler<MessagePreparedEventArgs> MessagePrepared
		{
			add
			{
				Presenter.MessagePrepared += value;
			}

			remove
			{
				Presenter.MessagePrepared -= value;
			}
		}

		/// <summary>
		/// Gets current instance of control presenter.
		/// </summary>
		protected new SenderPresenterBase Presenter
		{
			get { return DataContext as SenderPresenterBase; }
		}

		/// <summary>
		/// Style for textBox with phone numbers.
		/// </summary>
		public static readonly DependencyProperty PhoneNumberStyleProperty = DependencyProperty.Register("PhoneNumberStyle", typeof(Style), typeof(SenderControlBase), new PropertyMetadata(null));

		/// <summary>
		/// Style for textBox with MMS.
		/// </summary>
		public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register("TextStyle", typeof(Style), typeof(SenderControlBase), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets style for textBox with phone numbers.  The control will accept .NET standard styling options for a textBox.
		/// </summary>
		public Style PhoneNumberStyle
		{
			get { return (Style)GetValue(PhoneNumberStyleProperty); }
			set { SetValue(PhoneNumberStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for textBox with MMS.   The control will accept .NET standard styling options for a textBox.
		/// </summary>
		public Style TextStyle
		{
			get { return (Style)GetValue(TextStyleProperty); }
			set { SetValue(TextStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets comma-or-semicolon-separated list of phone numbers specified by the user.
		/// </summary>
		public string PhoneNumbers
		{
			get
			{
				return Presenter == null ? null : Presenter.PhoneNumbers;
			}
			set
			{
				if (Presenter != null)
				{
					Presenter.PhoneNumbers = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the actual text-to-send for the message.  This is separate and distinct from the multimedia image for MMS messages.
		/// </summary>
		public string Message
		{
			get
			{
				return Presenter == null ? null : Presenter.Message;
			}
			set
			{
				if (Presenter != null)
				{
					Presenter.Message = value;
				}
			}
		}
	}
}
