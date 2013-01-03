// <copyright file="MmsCouponControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

using ATT.Controls.Utility;
using ATT.Services;
using ATT.Services.Impl.Delivery;
using ATT.Utility;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Presenter for <see cref="MmsCouponControl"/>
	/// </summary>
	public class MmsCouponControlPresenter : SenderPresenterBase
	{
		private ulong _mmsAttachSize;
		private ulong _maxNewFilesSize = MmsControlPresenter.MaxSize;
		private ulong _currentTotalLength;
		private readonly IMmsService _mmsService;
		private StorageFile _file;
		private RelayCommand _clearCommand;
		private RelayCommand _clearErrorFileCommand;

		/// <summary>
		/// Creates instance of <see cref="MmsCouponControlPresenter"/>
		/// </summary>
		/// <param name="srv">MMS service</param>
		/// <exception cref="System.ArgumentNullException">srv is null</exception>
		public MmsCouponControlPresenter(IMmsService srv)
		{
			Argument.ExpectNotNull(() => srv);

			_mmsService = srv;
		}

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public StorageFile File
		{
			get
			{
				return _file;
			}
			set
			{
				if (_file != value)
				{
					_file = value;
					OnPropertyChanged(() => File);
					UpdateMmsAttachSize();
				}
			}
		}

		/// <summary>
		///  Gets or sets mms attach size in bytes
		/// </summary>
		public ulong MmsAttachSize
		{
			get
			{
				return _mmsAttachSize;
			}
			set
			{
				_mmsAttachSize = value;
				OnPropertyChanged(() => MmsAttachSize);
			}
		}

		/// <summary>
		///  Gets or sets max new files size
		/// </summary>
		public ulong MaxNewFilesSize
		{
			get
			{
				return _maxNewFilesSize;
			}
			set
			{
				_maxNewFilesSize = value;
				OnPropertyChanged(() => MaxNewFilesSize);
			}
		}

		/// <summary>
		///  Gets max mms size in bytes
		/// </summary>
		public ulong MaxMmsSize
		{
			get
			{
				return MmsControlPresenter.MaxSize;
			}
		}

		/// <summary>
		///  Gets or sets length of message
		/// </summary>
		public ulong CurrentTotalLength
		{
			get
			{
				return _currentTotalLength;
			}
			set
			{
				_currentTotalLength = value;
				OnPropertyChanged(() => CurrentTotalLength);
				MaxNewFilesSize = MaxMmsSize - (CurrentTotalLength - MmsAttachSize);
			}
		}

		/// <summary>
		/// Gets or sets command for cleaning name and image
		/// </summary>
		public RelayCommandHandler NeedClearCommand 
		{ 
			get; 
			set; 
		}

		/// <summary>
		/// Gets or sets command for clear error of files
		/// </summary>
		public RelayCommandHandler NeedClearErrorFileCommand
		{
			get;
			set;
		}		

		/// <summary>
		/// Clear all fields on control and reset some properties
		/// </summary>
		protected override void ClearControl()
		{
			base.ClearControl();

			if (_clearCommand == null && NeedClearCommand != null)
			{
				_clearCommand = NeedClearCommand();
			}

			if (_clearCommand != null)
			{
				_clearCommand.Execute(null);
			}
		}

		/// <summary>
		/// Send message
		/// </summary>
		protected override async Task Send()
		{
			ClearErrorFile();
			var attach = new List<StorageFile>();
			if (File != null)
			{
				attach.Add(File);
			}

			IEnumerable<PhoneNumber> numbers = GetPhoneNumbers();
			var mms = new MmsMessage(numbers, Message, attach);

			OnMessagePrepared(mms);

			CurrentMessage = await _mmsService.Send(mms);
		}

		/// <summary>
		/// Create new <see cref="MessageDeliveryListener"/>
		/// </summary>
		/// <returns><see cref="MessageDeliveryListener"/></returns>
		protected override MessageDeliveryListener CreateMessageDeliveryListener()
		{
			return new MmsDeliveryListener(_mmsService, (MmsMessage)CurrentMessage, TimeSpan.FromMilliseconds(GetStatusPollMilliseconds), TimeSpan.FromMinutes(GetStatusTimeoutMinutes));
		}

		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public override void Unload()
		{
			base.Unload();
			ClearControl();
			_clearCommand.Deactivate();
		}

		private void ClearErrorFile()
		{
			if (_clearErrorFileCommand == null && NeedClearErrorFileCommand != null)
			{
				_clearErrorFileCommand = NeedClearErrorFileCommand();
			}

			if (_clearErrorFileCommand != null)
			{
				_clearErrorFileCommand.Execute(null);
			}
		}

		private async void UpdateMmsAttachSize()
		{
			if (File != null)
			{
				BasicProperties properties = await File.GetBasicPropertiesAsync();
				MmsAttachSize = properties.Size;
			}
			else
			{
				MmsAttachSize = 0;
			}
		}
	}
}
