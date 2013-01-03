// <copyright file="MmsCouponControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.Storage;
using Windows.UI.Xaml;
using ATT.Controls.Presenters;
using ATT.Controls.SubControls;
using ATT.Controls.Utility;
using ATT.Services.Impl;
using System;

namespace ATT.Controls
{
	/// <summary>
	/// This control sends an MMS image with preview.  Its most common use is to send coupons to a group of customers. 
	/// </summary>
	public sealed class MmsCouponControl : SenderControlBase
	{		
		/// <summary>
		/// Style for open file button.
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonStyleProperty = DependencyProperty.Register("FileOpenButtonStyle", typeof(Style), typeof(MmsCouponControl), new PropertyMetadata(null));

		/// <summary>
		/// Creates and initializes presenter instance for MMS Coupon control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			var mmsSrv = new AttMmsService(Endpoint, ApiKeyConfigured, SecretKeyConfigured);

			return new MmsCouponControlPresenter(mmsSrv)
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
		public StorageFile File
		{
			get
			{
				return Presenter == null ? null : ((MmsCouponControlPresenter)Presenter).File;
			}
			set
			{
				if (Presenter != null)
				{
					((MmsCouponControlPresenter)Presenter).File = value;
				}
			}
		}

		private RelayCommand ClearCommand
		{
			get
			{
				var ctrl = GetTemplateChild("previewControl") as FileOpenPickerPreviewControl;
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
				var ctrl = GetTemplateChild("previewControl") as FileOpenPickerPreviewControl;

				if (ctrl != null)
				{
					return ctrl.ClearErrorFileCommand;
				}
				return null;
			}
		}
	}
}
