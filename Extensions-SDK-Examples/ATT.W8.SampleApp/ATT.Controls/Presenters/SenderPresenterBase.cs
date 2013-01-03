// <copyright file="SenderPresenterBase.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ATT.Controls.Utility;
using ATT.Services;
using ATT.Services.Impl;
using ATT.Services.Impl.Delivery;
using ATT.Services.Impl.WeakEvent;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Base class for all sender presenters
	/// </summary>
	public abstract class SenderPresenterBase : PresenterBase
	{
		/// <summary>
		/// Delegate for SendMessage command which is used by message listeners.
		/// </summary>
		/// <returns></returns>
		public delegate MessageDeliveryListener MessageDeliveryListenerHandler();

		/// <summary>
		/// EventHandler for MessagePrepared
		/// </summary>
		public event EventHandler<MessagePreparedEventArgs> MessagePrepared;

		/// <summary>
		/// Timeout for message delivery listeners (sec).
		/// </summary>
		protected const int GetStatusTimeoutMinutes = 30;

		/// <summary>
		/// Poll time for message delivery listeners (ms).
		/// </summary>
		protected const int GetStatusPollMilliseconds = 10000;

		private MessageDeliveryStatus _messageStatus = MessageDeliveryStatus.Initial;
		private string _messageStatusName = String.Empty;
		private string _phoneNumbers = String.Empty;
		private string _message = String.Empty;
		private bool _isValidPhoneNumbers = true;

		/// <summary>
		/// Currently sent message.
		/// </summary>
		protected OutboundMessage CurrentMessage { get; set; }

		/// <summary>
		/// Creates instance of <see cref="SenderPresenterBase"/>.
		/// </summary>
		protected SenderPresenterBase()
		{
			SendCommand = new RelayCommand(obj => SendMessage(new MessageDeliveryListenerHandler(CreateMessageDeliveryListener)), () => CanSendMessage);
			NewCommand = new RelayCommand(ClearAll);
			MessageStatus = MessageDeliveryStatus.Initial;
		}

		/// <summary>
		/// Send message to network.
		/// </summary>
		/// <param name="messageDeliveryListenerCreator"></param>
		protected async void SendMessage(MessageDeliveryListenerHandler messageDeliveryListenerCreator)
		{
			try
			{
				IsNewMessageEnabled = false;
				IsEditableEnabled = false;
				MessageStatus = MessageDeliveryStatus.Initial;

				await Send();

				IsNewMessageEnabled = true;
				MessageStatus = MessageDeliveryStatus.DeliveredToNetwork;
				MessageDeliveryListener listener = messageDeliveryListenerCreator.Invoke();
				if (listener != null)
				{
					NotificationManager.Instance.AddListener(listener);
					listener.StatusChangedEvent += StatusChangedEvent;
					listener.ListenForMessageDelivered();
				}
			}
			catch (Exception ex)
			{
				MessageStatus = MessageDeliveryStatus.Error;
				HandleException(ex);
				IsNewMessageEnabled = true;
				IsEditableEnabled = true;
			}
		}

		private void StatusChangedEvent(object sender, MessageStatusEventArgs e)
		{
			if (CurrentMessage != null && CurrentMessage.MessageId == e.Message.MessageId)
			{
				MessageStatus = e.MessageStatus;
				IsNewMessageEnabled = true;
				IsEditableEnabled = true;
			}
		}

		/// <summary>
		/// Create new <see cref="MessageDeliveryListener"/>
		/// </summary>
		/// <returns><see cref="MessageDeliveryListener"/></returns>
		protected abstract MessageDeliveryListener CreateMessageDeliveryListener();

		/// <summary>
		/// Send message
		/// </summary>
		protected abstract Task Send();

		/// <summary>
		/// Clear all fields on control and reset some properties
		/// </summary>
		protected virtual void ClearControl()
		{
			CurrentMessage = null;
			MessageStatus = MessageDeliveryStatus.Initial;
			ErrorMessage = String.Empty;

			Message = String.Empty;
			PhoneNumbers = String.Empty;

			IsNewMessageEnabled = true;
			IsEditableEnabled = true;
		}
		/// <summary>
		/// Get array of <see cref="PhoneNumber"/> from PhoneNumbers property
		/// </summary>
		/// <returns>Array of <see cref="PhoneNumber"/></returns>
		protected IEnumerable<PhoneNumber> GetPhoneNumbers()
		{
			return PhoneNumbersInputParser.Parse(PhoneNumbers).Select(x => PhoneNumberIsdnFormatValidator.ConvertToIsdn(x)).Distinct().Select(x => new PhoneNumber(x));
		}

		/// <summary>
		/// Gets value can we send message
		/// </summary>
		public bool CanSendMessage
		{
			get
			{
				return PhoneNumbers.Length > 0 && IsValidPhoneNumbers && Message != null && Message.Trim().Length > 0;
			}
		}

		/// <summary>
		/// Gets or sets status of message
		/// </summary>
		public MessageDeliveryStatus MessageStatus
		{
			get
			{
				return _messageStatus;
			}
			protected set
			{
				if (_messageStatus != value)
				{
					_messageStatus = value;

					if (MessageStatus == MessageDeliveryStatus.Initial)
					{
						ErrorMessage = String.Empty;
						MessageStatusName = String.Empty;
					}
					else if (MessageStatus == MessageDeliveryStatus.DeliveredToNetwork)
					{
						ErrorMessage = String.Empty;
						MessageStatusName = ResourcesHelper.GetString("SuccessfullySent");
					}
					else if (MessageStatus == MessageDeliveryStatus.DeliveredToTerminal)
					{
						ErrorMessage = String.Empty;
						MessageStatusName = ResourcesHelper.GetString("SuccessfullyDelivered");
					}
					else if (MessageStatus == MessageDeliveryStatus.DeliveryImpossible)
					{
						MessageStatusName = ResourcesHelper.GetString("NotDelivered");
					}
					else if (MessageStatus == MessageDeliveryStatus.Error)
					{
						MessageStatusName = String.Empty;
					}

					OnPropertyChanged(() => MessageStatus);
				}
			}
		}

		/// <summary>
		/// Gets text for field which show message status
		/// </summary>
		public string MessageStatusName
		{
			get
			{
				return _messageStatusName;
			}
			private set
			{
				if (_messageStatusName != value)
				{
					_messageStatusName = value;
					OnPropertyChanged(() => MessageStatusName);
				}
			}
		}

		/// <summary>
		/// Gets or sets comma-separated list of phone numbers specified by user
		/// </summary>
		public string PhoneNumbers
		{
			get
			{
				return _phoneNumbers;
			}
			set
			{
				if (_phoneNumbers != value)
				{
					_phoneNumbers = value;
					OnPropertyChanged(() => PhoneNumbers);
				}
			}
		}

		/// <summary>
		/// Gets or sets text of message
		/// </summary>
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				if (_message != value)
				{
					_message = value;
					OnPropertyChanged(() => Message);
				}
			}
		}

		/// <summary>
		/// Gets or sets valid status for phone numbers field
		/// </summary>
		public bool IsValidPhoneNumbers
		{
			get
			{
				return _isValidPhoneNumbers;
			}
			set
			{
				if (_isValidPhoneNumbers != value)
				{
					_isValidPhoneNumbers = value;
					OnPropertyChanged(() => IsValidPhoneNumbers);
				}
			}
		}

		/// <summary>
		/// Gets or sets command for sending message
		/// </summary>
		public RelayCommand SendCommand { get; protected set; }

		/// <summary>
		/// Gets or sets command for prepare control for making new message
		/// </summary>
		public RelayCommand NewCommand { get; protected set; }
		
		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public override void Unload()
		{
			SendCommand.Deactivate();
			NewCommand.Deactivate();
		}
	  
		private void ClearAll(object parameter)
		{
			try
			{
				ClearControl();
			}
			catch (Exception ex)
			{
				MessageStatus = MessageDeliveryStatus.Error;
				HandleException(ex);
			}
		}		

		protected void OnMessagePrepared(MmsMessage message)
		{
			if (MessagePrepared != null)
			{
				MessagePrepared(this, new MessagePreparedEventArgs(message));
			}
		}
	}
}
