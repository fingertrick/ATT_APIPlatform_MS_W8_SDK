using System;
using System.Collections.ObjectModel;
using ATT.WP8.Controls.Messages;
using ATT.WP8.Controls.Utils;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Base view model for SMS/MMS senders page
	/// </summary>
	public class SenderPageViewModelBase : NotifyPropertyChangedBase
	{
		private string _message;
		private readonly ObservableCollection<string> _phoneNumbers = new ObservableCollection<string>();
		private bool _isEditableEnabled = true;
		private bool _isNewMessageEnabled = true;
		private string _messageStatusName;
		private MessageDeliveryStatus _messageStatus = MessageDeliveryStatus.Initial;
		private string _errorMessage;
		private string _textPhoneNumber;

		/// <summary>
		/// Creates instance of SenderPageViewModelBase
		/// </summary>
		public SenderPageViewModelBase()
		{
			NewCommand = new RelayCommand( o => ClearFields(), () => IsNewMessageEnabled);
		}

		/// <summary>
		/// Gets phone numbers as observable collection of strings
		/// </summary>
		public ObservableCollection<string> PhoneNumbers
		{
			get { return _phoneNumbers; }
		}

		/// <summary>
		/// Gets pr set Text phone numbers.
		/// </summary>
		public string TextPhoneNumber
		{
			get { return _textPhoneNumber; }
			set
			{
				if (_textPhoneNumber != value)
				{
					_textPhoneNumber = value;
					OnPropertyChanged(() => TextPhoneNumber);
				}
			}
		}

		/// <summary>
		/// Gets or sets text of message
		/// </summary>
		public string Message
		{
			get { return _message; }
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
		/// Gets or sets message status
		/// </summary>
		public MessageDeliveryStatus MessageStatus
		{
			get { return _messageStatus; }
			set
			{
				if (_messageStatus != value)
				{
					_messageStatus = value;
					OnPropertyChanged(() => MessageStatus);
					UpdateByDeliveryStatus();
				}
			}
		}

		/// <summary>
		/// Gets or sets message status name
		/// </summary>
		public string MessageStatusName
		{
			get { return _messageStatusName; }
			set
			{
				if (_messageStatusName != value)
				{
					_messageStatusName = value;
					OnPropertyChanged(() => MessageStatusName);
				}
			}
		}

		/// <summary>
		/// Gets or sets error message
		/// </summary>
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				if (_errorMessage != value)
				{
					_errorMessage = value;
					OnPropertyChanged(() => ErrorMessage);
				}
			}
		}
		
		/// <summary>
		/// Gets or sets property defining if editable is enabled
		/// </summary>
		public bool IsEditableEnabled
		{
			get { return _isEditableEnabled; }
			set
			{
				if (_isEditableEnabled != value)
				{
					_isEditableEnabled = value;
					OnPropertyChanged(() => IsEditableEnabled);
				}
			}
		}

		/// <summary>
		/// Gets or sets property defining if new message is enabled
		/// </summary>
		public bool IsNewMessageEnabled
		{
			get { return _isNewMessageEnabled; }
			set
			{
				if (_isNewMessageEnabled != value)
				{
					_isNewMessageEnabled = value;
					OnPropertyChanged(() => IsNewMessageEnabled);
				}
			}
		}

		/// <summary>
		/// Gets command for new message
		/// </summary>
		public RelayCommand NewCommand
		{
			get;
			private set;
		}

		protected virtual void ClearFields()
		{
			PhoneNumbers.Clear();
			TextPhoneNumber = String.Empty;
			Message = "";
			MessageStatusName = "";
			ErrorMessage = "";
			EnableNewMessageSending(true);
		}

		private void EnableNewMessageSending(bool enabled)
		{
			IsNewMessageEnabled = enabled;
			IsEditableEnabled = enabled;
		}

		private void UpdateByDeliveryStatus()
		{
			if (MessageStatus==MessageDeliveryStatus.Initial)
			{
				ClearFields();
			}
			else if (MessageStatus == MessageDeliveryStatus.Sending)
			{
				EnableNewMessageSending(false);
			}
			else if (MessageStatus == MessageDeliveryStatus.DeliveredToNetwork)
			{
				IsNewMessageEnabled = true;
			}
			else if (MessageStatus == MessageDeliveryStatus.DeliveredToTerminal
				|| MessageStatus == MessageDeliveryStatus.DeliveryImpossible)
			{
				IsNewMessageEnabled = true;
			}
			else if (MessageStatus == MessageDeliveryStatus.Error)
			{
				EnableNewMessageSending(true);
			}
		}
	}
}