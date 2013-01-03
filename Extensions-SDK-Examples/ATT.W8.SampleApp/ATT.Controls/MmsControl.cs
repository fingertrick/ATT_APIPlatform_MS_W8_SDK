// <copyright file="MmsControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml;

using ATT.Controls.Presenters;
using ATT.Controls.SubControls;
using ATT.Controls.Utility;
using ATT.Services.Impl;

namespace ATT.Controls
{
	/// <summary>
	/// Control for sending MMS Messages.
	/// </summary>
	public sealed class MmsControl : SenderControlBase
	{		
		/// <summary>
		/// Style for open file button.
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonStyleProperty = DependencyProperty.Register("FileOpenButtonStyle", typeof(Style), typeof(MmsControl), new PropertyMetadata(null));

		/// <summary>
		/// Creates and initializes presenter instance for MMS control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			var mmsSrv = new AttMmsService(Endpoint, ApiKeyConfigured, SecretKeyConfigured);
			
			return new MmsControlPresenter(mmsSrv)
			{
				NeedClearCommand = new MmsCouponControlPresenter.RelayCommandHandler(() => ClearCommand),
				NeedClearErrorFileCommand = new MmsCouponControlPresenter.RelayCommandHandler(() => ClearErrorFileCommand),
				MaxNewFilesSize = MmsControlPresenter.MaxSize,			
			};
		}
	   
		/// <summary>
		/// Gets or sets style for open file button.
		/// </summary>
		public Style FileOpenButtonStyle
		{
			get { return (Style)GetValue(FileOpenButtonStyleProperty); }
			set { SetValue(FileOpenButtonStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets attachment file.
		/// </summary>
		public ObservableCollection<StorageFile> AttachmentFiles
		{
			get
			{
				return Presenter == null ? null : ((MmsControlPresenter)Presenter).AttachmentFiles;
			}
			set
			{
				if (Presenter != null)
				{
					((MmsControlPresenter)Presenter).AttachmentFiles = value;
				}
			}
		}		

		private RelayCommand ClearCommand
		{
			get
			{
				var ctrl = GetTemplateChild("fileOpenPicker") as FileOpenPickerControl;

				if (ctrl != null)
				{
					return ctrl.ClearCommand;
				}
				return null;
			}
		}

		private RelayCommand ClearErrorFileCommand
		{
			get
			{
				var ctrl = GetTemplateChild("fileOpenPicker") as FileOpenPickerControl;

				if (ctrl != null)
				{
					return ctrl.ClearErrorFileCommand;
				}
				return null;
			}
		}
	}
}
