// <copyright file="FileOpenPickerPreviewControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.WP8.Controls.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ATT.WP8.SDK;

namespace ATT.WP8.SampleApp.Controls
{
	/// <summary>
	/// Control for select and preview files
	/// </summary>
	public class FileOpenPickerPreviewControl : Control
	{
		/// <summary>
		/// Presenter for <see cref="FileOpenPickerPreviewControl"/>
		/// </summary>
		public FileOpenPickerPreviewViewModel ViewModel
		{
			get { return DataContext as FileOpenPickerPreviewViewModel; }
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
		public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(ContentInfo), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCurrentFileSelectionChanged)));

		/// <summary>
		/// Image
		/// </summary>
		public static readonly DependencyProperty ImagePreviewProperty = DependencyProperty.Register("ImagePreview", typeof(BitmapImage), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(null));

		/// <summary>
		/// Max new files size
		/// </summary>
		public static readonly DependencyProperty MaxNewFilesSizeProperty = DependencyProperty.Register("MaxNewFilesSize", typeof(long), typeof(FileOpenPickerPreviewControl), new PropertyMetadata(long.MinValue));

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
			get { return GetValue(FileOpenButtonTextProperty) as String; }
			set { SetValue(FileOpenButtonTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public ContentInfo File
		{
			get
			{
				return ViewModel == null ? null : ViewModel.File;
			}
			set
			{
				if (ViewModel != null)
				{
					ViewModel.File = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets max new files size
		/// </summary>
		public long MaxNewFilesSize
		{
			get
			{
				return ViewModel == null ? 0 : ViewModel.MaxNewFilesSize;
			}
			set
			{
				if (ViewModel != null)
				{
					ViewModel.MaxNewFilesSize = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets image
		/// </summary>
		public BitmapImage ImagePreview
		{
			get { return GetValue(ImagePreviewProperty) as BitmapImage; }
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
				if (ctrl != null && ctrl.ClearCommand != null)
				{
					return ctrl.ClearCommand;
				}

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
				if (ctrl != null && ctrl.ClearErrorFileCommand != null)
				{
					return ctrl.ClearErrorFileCommand;
				}

				return null;
			}
		}

		private async static void OnCurrentFileSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var picker = (FileOpenPickerPreviewControl)d;
			var contentInfo = e.NewValue as ContentInfo;
			if (contentInfo != null)
			{
				var bitmapImage = new BitmapImage();
				
				using (var memoryStream = new MemoryStream())
				{
					await memoryStream.WriteAsync(contentInfo.Content, 0, contentInfo.Content.Length);
					bitmapImage.SetSource(memoryStream);
				}
				
				picker.ImagePreview = bitmapImage;
			}
			else
			{
				picker.ImagePreview = null;
			}
		}
	}
}
