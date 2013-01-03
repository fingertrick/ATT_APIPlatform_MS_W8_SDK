// <copyright file="MessageControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Message control
	/// </summary>
	public sealed class MessageControl : Control
	{
		#region Dependency properties

		/// <summary>
		/// Style for textBox with message
		/// </summary>
		public static readonly DependencyProperty MessageTextStyleProperty = DependencyProperty.Register("MessageTextStyle", typeof(Style), typeof(MessageControl), new PropertyMetadata(null));

		/// <summary>
		/// Max message length
		/// </summary>
		public static readonly DependencyProperty MaxMessageLengthProperty = DependencyProperty.Register("MaxMessageLength", typeof(int), typeof(MessageControl), new PropertyMetadata(255, OnMaxMessageLengthChanged));

		/// <summary>
		/// Current message length
		/// </summary>
		public static readonly DependencyProperty CurrentAttachSizeProperty = DependencyProperty.Register("CurrentAttachSize", typeof(int), typeof(MessageControl), new PropertyMetadata(0, OnCurrentMessageLengthChanged));

		/// <summary>
		/// Total message length
		/// </summary>
		public static readonly DependencyProperty TotalMessageLengthProperty = DependencyProperty.Register("TotalMessageLength", typeof(int), typeof(MessageControl), new PropertyMetadata(0));

		/// <summary>
		/// Max TextBox length
		/// </summary>
		public static readonly DependencyProperty MaxTextBoxLengthProperty = DependencyProperty.Register("MaxTextBoxLength", typeof(int), typeof(MessageControl), new PropertyMetadata(255));

		/// <summary>
		/// Text header field
		/// </summary>
		public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register("TextHeader", typeof(string), typeof(MessageControl), new PropertyMetadata(String.Empty));

		/// <summary>
		/// Text message field
		/// </summary>
		public static readonly DependencyProperty TextMessageProperty = DependencyProperty.Register("TextMessage", typeof(string), typeof(MessageControl), new PropertyMetadata(String.Empty, OnCurrentMessageLengthChanged));

		/// <summary>
		/// Text status field
		/// </summary>
		public static readonly DependencyProperty MessageStatusProperty = DependencyProperty.Register("MessageStatus", typeof(string), typeof(MessageControl), new PropertyMetadata("New"));

		/// <summary>
		/// Error message
		/// </summary>
		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(MessageControl), new PropertyMetadata(String.Empty));

		#endregion

		private static void OnCurrentMessageLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (MessageControl)d;
			control.TotalMessageLength = control.CurrentAttachSize + control.TextMessage.Replace("\r\n", "\n").Length;
			control.MaxTextBoxLength = control.MaxMessageLength - control.CurrentAttachSize;
		}

		private static void OnMaxMessageLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = (MessageControl)d;
			control.MaxTextBoxLength = control.MaxMessageLength;
		}

		/// <summary>
		/// Creates instance of <see cref="MessageControl"/>
		/// </summary>
		public MessageControl()
		{
			DefaultStyleKey = typeof(MessageControl);
		}

		/// <summary>
		/// Gets or sets style for textBox with message
		/// </summary>
		public Style MessageTextStyle
		{
			get { return (Style)GetValue(MessageTextStyleProperty); }
			set { SetValue(MessageTextStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets max message length
		/// </summary>
		public int MaxMessageLength
		{
			get { return (int)GetValue(MaxMessageLengthProperty); }
			set { SetValue(MaxMessageLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets current message length
		/// </summary>
		public int CurrentAttachSize
		{
			get { return (int)GetValue(CurrentAttachSizeProperty); }
			set { SetValue(CurrentAttachSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets total message length
		/// </summary>
		public int TotalMessageLength
		{
			get { return (int)GetValue(TotalMessageLengthProperty); }
			set { SetValue(TotalMessageLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets max TextBox length
		/// </summary>
		public int MaxTextBoxLength
		{
			get { return (int)GetValue(MaxTextBoxLengthProperty); }
			set { SetValue(MaxTextBoxLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets text header field
		/// </summary>
		public string TextHeader
		{
			get { return GetValue(TextHeaderProperty).ToString(); }
			set { SetValue(TextHeaderProperty, value); }
		}

		/// <summary>
		/// Gets or sets text message field
		/// </summary>
		public string TextMessage
		{
			get { return GetValue(TextMessageProperty).ToString(); }
			set { SetValue(TextMessageProperty, value); }
		}

		/// <summary>
		/// Gets or sets text status field
		/// </summary>
		public string MessageStatus
		{
			get { return GetValue(MessageStatusProperty).ToString(); }
			set { SetValue(MessageStatusProperty, value); }
		}

		/// <summary>
		/// Gets or sets error message
		/// </summary>
		public string ErrorMessage
		{
			get { return GetValue(ErrorMessageProperty).ToString(); }
			set { SetValue(ErrorMessageProperty, value); }
		}

		/// <summary>
		/// Update binded ViewModel property
		/// </summary>
		public Action<string> UpdateBindedViewModelProperty
		{
			get { return value => TextMessage = value; }
		}
	}
}
