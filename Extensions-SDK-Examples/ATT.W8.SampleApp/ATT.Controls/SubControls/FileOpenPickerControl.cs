// <copyright file="FileOpenPickerControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ATT.Controls.Utility;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Control for select file
	/// </summary>
	public class FileOpenPickerControl : Control
	{
		private readonly string[] _pictureExtensions = new[] { ".jpg", ".bmp", ".gif", ".xwd", ".png", ".tiff", ".tif", ".ief" };
		private readonly string[] _audioExtensions = new[] { ".amr", ".wav", ".mp3", ".aifc", ".aiff", ".au", ".imy", ".m4a", ".awb", ".spx" };
		private readonly string[] _videoExtensions = new[] { ".3gp", ".3g2", ".wmv", ".m4v", ".mp4", ".avi", ".mov", ".mpeg" };
		private readonly string[] _allExtensions = new[] {".jpg", ".bmp", ".gif", ".xwd", ".png", ".tiff", ".tif", ".ief", ".amr", ".wav", ".3gp", ".3g2", ".wmv", ".m4a",
															 ".m4v", ".mp4", ".avi", ".mov", ".mpeg", ".midi", ".txt", ".html", ".vcf", ".vcs", ".mid", ".mp3", ".aifc", ".aiff", ".au" , ".imy" };

		#region Dependency properties

		/// <summary>
		/// Command for showing openFileDialog
		/// </summary>
		public static readonly DependencyProperty FileOpenCommandProperty = DependencyProperty.Register("FileOpenCommand", typeof(RelayCommand), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Command for clear fields
		/// </summary>
		public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register("ClearCommand", typeof(RelayCommand), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Command for remove current file
		/// </summary>
		public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register("RemoveCommand", typeof(RelayCommand), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Command for clear error of files
		/// </summary>
		public static readonly DependencyProperty ClearErrorFileCommandProperty = DependencyProperty.Register("ClearErrorFileCommand", typeof(RelayCommand), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Style for textBox with open file path
		/// </summary>
		public static readonly DependencyProperty FileOpenPathStyleProperty = DependencyProperty.Register("FileOpenPathStyle", typeof(Style), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Style for open file button
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonStyleProperty = DependencyProperty.Register("FileOpenButtonStyle", typeof(Style), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Open file button text.
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonTextProperty = DependencyProperty.Register("FileOpenButtonText", typeof(string), typeof(FileOpenPickerControl), new PropertyMetadata("..."));

		/// <summary>
		/// File type
		/// </summary>
		public static readonly DependencyProperty FileTypeProperty = DependencyProperty.Register("FileType", typeof(FileType), typeof(FileOpenPickerControl), new PropertyMetadata(FileType.Any));

		/// <summary>
		/// Attachment file
		/// </summary>
		public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(StorageFile), typeof(FileOpenPickerControl), new PropertyMetadata(null));

		/// <summary>
		/// Attachment files
		/// </summary>
		public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("Files", typeof(ObservableCollection<StorageFile>), typeof(FileOpenPickerControl), new PropertyMetadata(new ObservableCollection<StorageFile>()));

		/// <summary>
		/// Select multiple files mode
		/// </summary>
		public static readonly DependencyProperty SelectMultipleFilesProperty = DependencyProperty.Register("SelectMultipleFiles", typeof(bool), typeof(FileOpenPickerControl), new PropertyMetadata(false));

		/// <summary>
		/// Current selected Attachment file
		/// </summary>
		public static readonly DependencyProperty CurrentFileProperty = DependencyProperty.Register("CurrentFile", typeof(FilePreview), typeof(FileOpenPickerControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCurrentFileSelectionChanged)));

		/// <summary>
		/// Error text
		/// </summary>
		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(FileOpenPickerControl), new PropertyMetadata(""));

		/// <summary>
		/// Max size for new files
		/// </summary>
		public static readonly DependencyProperty MaxNewFilesSizeProperty = DependencyProperty.Register("MaxNewFilesSize", typeof(ulong), typeof(FileOpenPickerControl), new PropertyMetadata(ulong.MaxValue));

		#endregion

		private static void OnCurrentFileSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FileOpenPickerControl picker = (FileOpenPickerControl)d;
			picker.CurrentFile = (FilePreview)e.NewValue;
		}

		private ObservableCollection<FilePreview> _filesExt;

		/// <summary>
		/// Creates instance of <see cref="FileOpenPickerControl"/>
		/// </summary>
		public FileOpenPickerControl()
		{
			DefaultStyleKey = typeof(FileOpenPickerControl);
			ErrorMessage = "";
			FilesExt = new ObservableCollection<FilePreview>();
			FileOpenCommand = new RelayCommand(FileOpenCommandAction);
			ClearCommand = new RelayCommand(ClearCommandAction);
			RemoveCommand = new RelayCommand(RemoveCommandAction);
			ClearErrorFileCommand = new RelayCommand((o) => ErrorMessage = "");
		}

		/// <summary>
		/// Gets command for showing openFileDialog
		/// </summary>
		public RelayCommand FileOpenCommand
		{
			get { return (RelayCommand)GetValue(FileOpenCommandProperty); }
			private set { SetValue(FileOpenCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for clear fields
		/// </summary>
		public RelayCommand ClearCommand
		{
			get { return (RelayCommand)GetValue(ClearCommandProperty); }
			private set { SetValue(ClearCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for remove selected file
		/// </summary>
		public RelayCommand RemoveCommand
		{
			get { return (RelayCommand)GetValue(RemoveCommandProperty); }
			private set { SetValue(RemoveCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for clear error of files
		/// </summary>
		public RelayCommand ClearErrorFileCommand
		{
			get { return (RelayCommand)GetValue(ClearErrorFileCommandProperty); }
			private set { SetValue(ClearErrorFileCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for open file path textBox
		/// </summary>
		public Style FileOpenPathStyle
		{
			get { return (Style)GetValue(FileOpenPathStyleProperty); }
			set { SetValue(FileOpenPathStyleProperty, value); }
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
		/// Gets or sets text for open button
		/// </summary>
		public string FileOpenButtonText
		{
			get { return GetValue(FileOpenButtonTextProperty).ToString(); }
			set { SetValue(FileOpenButtonTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets type of files for openFileDialog
		/// </summary>
		public FileType FileType
		{
			get { return (FileType)GetValue(FileTypeProperty); }
			set { SetValue(FileTypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public StorageFile File
		{
			get { return GetValue(FileProperty) as StorageFile; }
			set { SetValue(FileProperty, value); }
		}

		/// <summary>
		/// Gets or sets attachment files
		/// </summary>
		public ObservableCollection<StorageFile> Files
		{
			get { return GetValue(FilesProperty) as ObservableCollection<StorageFile>; }
			set { SetValue(FilesProperty, value); }
		}

		/// <summary>
		/// Gets attachment files
		/// </summary>
		public ObservableCollection<FilePreview> FilesExt
		{
			get
			{
				return _filesExt;
			}
			private set
			{
				_filesExt = value;
			}
		}

		/// <summary>
		/// Gets or sets select multiple files mode
		/// </summary>
		public bool SelectMultipleFiles
		{
			get { return (bool)GetValue(SelectMultipleFilesProperty); }
			set { SetValue(SelectMultipleFilesProperty, value); }
		}

		/// <summary>
		/// Gets or sets current attachment file
		/// </summary>
		internal FilePreview CurrentFile
		{
			get { return GetValue(CurrentFileProperty) as FilePreview; }
			set { SetValue(CurrentFileProperty, value); }
		}

		/// <summary>
		/// Gets or sets error text
		/// </summary>
		public string ErrorMessage
		{
			get { return GetValue(ErrorMessageProperty).ToString(); }
			set { SetValue(ErrorMessageProperty, value); }
		}

		/// <summary>
		/// Gets or sets max size for new files
		/// </summary>
		public ulong MaxNewFilesSize
		{
			get { return (ulong)GetValue(MaxNewFilesSizeProperty); }
			set { SetValue(MaxNewFilesSizeProperty, value); }
		}

		private async void FileOpenCommandAction(object parameter)
		{
			ErrorMessage = "";
			var openPicker = new FileOpenPicker { ViewMode = PickerViewMode.Thumbnail };
			switch (FileType)
			{
				case FileType.Audio:
					openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
					foreach (string extension in _audioExtensions)
					{
						openPicker.FileTypeFilter.Add(extension);
					}
					break;
				case FileType.Video:
					openPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
					foreach (string extension in _videoExtensions)
					{
						openPicker.FileTypeFilter.Add(extension);
					}
					break;
				case FileType.Picture:
					openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

					foreach (string extension in _pictureExtensions)
					{
						openPicker.FileTypeFilter.Add(extension);
					}

					break;
				default:
					openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
					foreach (string extension in _allExtensions)
					{
						openPicker.FileTypeFilter.Add(extension);
					}

					break;
			}

			if (SelectMultipleFiles)
			{
				IEnumerable<StorageFile> files = await openPicker.PickMultipleFilesAsync();
				if (await CheckSize(null, files))
				{
					foreach (StorageFile file in files)
					{
						Files.Add(file);
						FilesExt.Add(new FilePreview(file, RemoveCommand));
					}
					if (Files.Count > 0)
					{
						CurrentFile = FilesExt[FilesExt.Count - 1];
					}
				}
				else
				{
					ErrorMessage = ResourcesHelper.GetString("MmsLimit");
				}
			}
			else
			{
				StorageFile file = await openPicker.PickSingleFileAsync();
				if (file != null)
				{
					if (await CheckSize(file, null))
					{
						Files.Clear();
						FilesExt.Clear();
						Files.Add(file);
						FilesExt.Add(new FilePreview(file, RemoveCommand));
						CurrentFile = FilesExt[FilesExt.Count - 1];
						File = file;
					}
					else
					{
						ErrorMessage = ResourcesHelper.GetString("MmsLimit");
					}
				}
			}
		}

		private void ClearCommandAction(object parameter)
		{
			ErrorMessage = "";
			File = null;
			CurrentFile = null;
			Files.Clear();
			FilesExt.Clear();
		}

		private void RemoveCommandAction(object parameter)
		{
			var deletedFile = parameter as FilePreview;
			if (deletedFile != null)
			{
				ErrorMessage = "";
				File = null;
				Files.Remove(deletedFile.File);
				FilesExt.Remove(deletedFile);

				if (null == CurrentFile && FilesExt.Count > 0)
					CurrentFile = FilesExt[FilesExt.Count - 1];
			}
		}

		private async Task<bool> CheckSize(StorageFile file, IEnumerable<StorageFile> files)
		{
			ulong currentSize = 0;
			if (file != null)
			{
				BasicProperties properties = await file.GetBasicPropertiesAsync();
				currentSize += properties.Size;
			}
			if (files != null)
			{
				foreach (StorageFile f in files)
				{
					BasicProperties properties = await f.GetBasicPropertiesAsync();
					currentSize += properties.Size;
				}
			}
			return MaxNewFilesSize == ulong.MaxValue || currentSize <= MaxNewFilesSize;
		}
	}
}
