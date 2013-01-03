// <copyright file="FileOpenPickerPreviewControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ATT.Controls.Presenters;
using ATT.Controls.Utility;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Control for select and preview files
	/// </summary>
	public class FileOpenPickerPreviewControl : Control
	{
		/// <summary>
		/// Presenter for <see cref="FileOpenPickerPreviewControl"/>
		/// </summary>
		public FileOpenPickerPreviewControlPresenter Presenter
		{
			get { return DataContext as FileOpenPickerPreviewControlPresenter; }
		}

		#region Dependency properties		

		/// <summary>
		/// Style for open file button
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonStyleProperty = DependencyProperty.Register("FileOpenButtonStyle", typeof(Style), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(null));

		/// <summary>
		/// Open file button text.
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonTextProperty = DependencyProperty.Register("FileOpenButtonText", typeof(string), typeof(FileOpenPickerPreviewControl), new PropertyMetadata("..."));

		/// <summary>
		/// Attachment file
		/// </summary>
		public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(StorageFile), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCurrentFileSelectionChanged)));

		/// <summary>
		/// Image
		/// </summary>
		public static readonly DependencyProperty ImagePreviewProperty = DependencyProperty.Register("ImagePreview", typeof(BitmapImage), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(null));

		/// <summary>
		/// Max new files size
		/// </summary>
		public static readonly DependencyProperty MaxNewFilesSizeProperty = DependencyProperty.Register("MaxNewFilesSize", typeof(ulong), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(0));

		#endregion


		/// <summary>
		/// Creates instance of <see cref="FileOpenPickerPreviewControl"/>
		/// </summary>
		public FileOpenPickerPreviewControl()
		{
			DefaultStyleKey = typeof(FileOpenPickerPreviewControl);
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
		/// Gets or sets open button text
		/// </summary>
		public string FileOpenButtonText
		{
			get { return GetValue(FileOpenButtonTextProperty).ToString(); }
			set { SetValue(FileOpenButtonTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public StorageFile File
		{
			get
			{
				return Presenter == null ? null : Presenter.File;
			}
			set
			{
				if (Presenter != null)
				{
					Presenter.File = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets max new files size
		/// </summary>
		public ulong MaxNewFilesSize
		{
			get
			{
				return Presenter == null ? 0 : Presenter.MaxNewFilesSize;
			}
			set
			{
				if (Presenter != null)
				{
					Presenter.MaxNewFilesSize = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets image
		/// </summary>
		public BitmapImage ImagePreview
		{
			get { return (BitmapImage)GetValue(ImagePreviewProperty); }
			set { SetValue(ImagePreviewProperty, value); }
		}

		/// <summary>
		/// Gets command for clear fields
		/// </summary>
		public RelayCommand ClearCommand
		{
			get 
			{
				var ctrl = GetTemplateChild("fileOpenPicker") as FileOpenPickerControl;
				if (ctrl != null)
					return ctrl.ClearCommand;
				return null;
			}
		}

		/// <summary>
		/// Gets command for clear error of file
		/// </summary>
		public RelayCommand ClearErrorFileCommand
		{
			get
			{
				var ctrl = GetTemplateChild("fileOpenPicker") as FileOpenPickerControl;
				if (ctrl != null)
					return ctrl.ClearErrorFileCommand;
				return null;
			}
		}

		private static async void OnCurrentFileSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FileOpenPickerPreviewControl picker = (FileOpenPickerPreviewControl)d;
			StorageFile file = e.NewValue as StorageFile;
			if (file != null)
			{
				var bitmapImage = new BitmapImage();
				await bitmapImage.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));

				picker.ImagePreview = bitmapImage;
			}
			else
			{
				picker.ImagePreview = null;
			}
		}
	}
}
