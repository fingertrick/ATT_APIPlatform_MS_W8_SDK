// <copyright file="MmsControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
	/// Presenter for <see cref="MmsControl"/>
	/// </summary>
	public class MmsControlPresenter : SenderPresenterBase
	{
		/// <summary>
		/// Max size for MMS message (in bytes)
		/// </summary>
		public const ulong MaxSize = 614400;
	   
		private ulong _mmsAttachSize;
		private ulong _maxNewFilesSize = MaxSize;
		private ulong _currentTotalLength;
		private readonly IMmsService _mmsService;		
		private ObservableCollection<StorageFile> _attachmentFiles;
	   
		private RelayCommand _clearCommand;
		private RelayCommand _clearErrorFileCommand;

		/// <summary>
		/// Creates instance of <see cref="MmsControlPresenter"/>
		/// </summary>
		/// <param name="srv">MMS service</param>
		/// <exception cref="System.ArgumentNullException">srv is null.</exception>
		public MmsControlPresenter(IMmsService srv)
		{
			Argument.ExpectNotNull(() => srv);

			_mmsService = srv;
			_attachmentFiles = new ObservableCollection<StorageFile>();

			_attachmentFiles.CollectionChanged += (s, e) => 
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					IncrementAttachmentSize(e.NewItems);
				}

				if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					DecrementAttachmentSize(e.OldItems);
				}			  
			};
		}	   

		/// <summary>
		/// Gets or sets attachment files
		/// </summary>
		public ObservableCollection<StorageFile> AttachmentFiles
		{
			get 
			{ 
				return _attachmentFiles; 
			}
			set 
			{ 
				_attachmentFiles = value;
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
				return MaxSize;
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
				MaxNewFilesSize = MaxMmsSize - CurrentTotalLength;
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

			MmsAttachSize = 0;
		}

		/// <summary>
		/// Send message
		/// </summary>
		protected override async Task Send()
		{
			ClearErrorFile();
			IEnumerable<PhoneNumber> numbers = GetPhoneNumbers();
			var mms = new MmsMessage(numbers, Message, AttachmentFiles);

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

		private async void IncrementAttachmentSize(IList storageFileCollection)
		{
			foreach (StorageFile file in storageFileCollection)
			{
				BasicProperties properties = await file.GetBasicPropertiesAsync();
				MmsAttachSize += properties.Size;
			}			
		}

		private async void DecrementAttachmentSize(IList storageFileCollection)
		{
			foreach (StorageFile file in storageFileCollection)
			{
				BasicProperties properties = await file.GetBasicPropertiesAsync();
				MmsAttachSize -= properties.Size;
			}
		}		
	}
}
