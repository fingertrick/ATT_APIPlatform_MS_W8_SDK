// <copyright file="FileOpenPickerControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Specialized;
using ATT.WP8.Controls.Utils;
using ATT.WP8.SDK;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ATT.WP8.SampleApp.Controls
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

		private long _filesSize;

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
		public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(ContentInfo), typeof(FileOpenPickerControl), new PropertyMetadata(null, FileChanged));

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
		public static readonly DependencyProperty MaxNewFilesSizeProperty = DependencyProperty.Register("MaxNewFilesSize", typeof(long), typeof(FileOpenPickerControl), new PropertyMetadata(long.MaxValue));

		/// <summary>
		/// Gets file as collection of ContentInfo instances
		/// </summary>
		public static readonly DependencyProperty FilesContentProperty = 
			DependencyProperty.Register("FilesContent", typeof(ObservableCollection<ContentInfo>),
			typeof(FileOpenPickerControl), new PropertyMetadata(new ObservableCollection<ContentInfo>(), FilesContentChanged));

		#endregion

		private static void FilesContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as FileOpenPickerControl;
			if (dependencyPropertyChangedEventArgs.NewValue != null && control != null)
			{
				var oldValue = dependencyPropertyChangedEventArgs.OldValue as ObservableCollection<ContentInfo>;
				if (oldValue!= null)
				{
					oldValue.CollectionChanged -= control.NewValueOnCollectionChanged;
				}
				var newValue = dependencyPropertyChangedEventArgs.NewValue as ObservableCollection<ContentInfo>;
				if (newValue != null)
				{
					newValue.CollectionChanged += control.NewValueOnCollectionChanged;
				}
			}
		}

		private void NewValueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
			{
				ClearCommandAction(null);
			}
		}

		private static void FileChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var control = dependencyObject as FileOpenPickerControl;
			if (dependencyPropertyChangedEventArgs.NewValue == null && control != null)
			{
				control.ClearCommandAction(null);
			}
		}

		private static void OnCurrentFileSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FileOpenPickerControl picker = (FileOpenPickerControl)d;
			picker.CurrentFile = (FilePreview)e.NewValue;
		}

		/// <summary>
		/// Creates instance of <see cref="FileOpenPickerControl"/>
		/// </summary>
		public FileOpenPickerControl()
		{
			DefaultStyleKey = typeof(FileOpenPickerControl);
			ErrorMessage = "";
			Files = new ObservableCollection<FilePreview>();
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
			get { return GetValue(FileOpenCommandProperty) as RelayCommand; }
			private set { SetValue(FileOpenCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for clear fields
		/// </summary>
		public RelayCommand ClearCommand
		{
			get { return GetValue(ClearCommandProperty) as RelayCommand; }
			private set { SetValue(ClearCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for remove selected file
		/// </summary>
		public RelayCommand RemoveCommand
		{
			get { return GetValue(RemoveCommandProperty) as RelayCommand; }
			private set { SetValue(RemoveCommandProperty, value); }
		}

		/// <summary>
		/// Gets command for clear error of files
		/// </summary>
		public RelayCommand ClearErrorFileCommand
		{
			get { return GetValue(ClearErrorFileCommandProperty) as RelayCommand; }
			private set { SetValue(ClearErrorFileCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for open file path textBox
		/// </summary>
		public Style FileOpenPathStyle
		{
			get { return GetValue(FileOpenPathStyleProperty) as Style; }
			set { SetValue(FileOpenPathStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for open file button.
		/// </summary>
		public Style FileOpenButtonStyle
		{
			get { return GetValue(FileOpenButtonStyleProperty) as Style; }
			set { SetValue(FileOpenButtonStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets text for open button
		/// </summary>
		public string FileOpenButtonText
		{
			get { return GetValue(FileOpenButtonTextProperty) as String; }
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
		public ContentInfo File
		{
			get { return GetValue(FileProperty) as ContentInfo; }
			set { SetValue(FileProperty, value); }
		}

		/// <summary>
		/// Gets files collection suitable for ListBox binding with DeleteFileCommand
		/// </summary>
		public ObservableCollection<FilePreview> Files { get; private set; }
		
		/// <summary>
		/// Gets file as collection of ContentInfo instances
		/// </summary>
		public ObservableCollection<ContentInfo> FilesContent
		{
			get { return (ObservableCollection<ContentInfo>)GetValue(FilesContentProperty); }
			set { SetValue(FilesContentProperty, value); }
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
			get { return GetValue(ErrorMessageProperty) as String; }
			set { SetValue(ErrorMessageProperty, value); }
		}

		/// <summary>
		/// Gets or sets max size for new files
		/// </summary>
		public long MaxNewFilesSize
		{
			get { return (long)GetValue(MaxNewFilesSizeProperty); }
			set { SetValue(MaxNewFilesSizeProperty, value); }
		}

		private void FileOpenCommandAction(object parameter)
		{
			ErrorMessage = "";
			var openPicker = new PhotoChooserTask();

			openPicker.Completed += async (s, e) => { await AddNewFile(e); };
			openPicker.Show();
		}

		private async Task AddNewFile(PhotoResult e)
		{
			if (e.TaskResult != TaskResult.OK)
			{
				return;
			}

			long fileSize = e.ChosenPhoto.Length;
			if (!UpdateFilesSize(fileSize))
			{
				return;
			}

			if (RemoveCommand == null)
			{
				return;
			}
			Files.Add(new FilePreview(e.OriginalFileName, RemoveCommand));
			await AddFileContent(e, fileSize);
		}

		private async Task AddFileContent(PhotoResult e, long fileSize)
		{
			byte[] fileContent = new byte[fileSize];
			await e.ChosenPhoto.ReadAsync(fileContent, 0, fileContent.Length);
			ContentInfo fileContentInfo = new ContentInfo { Content = fileContent, Name = e.OriginalFileName };
			if (SelectMultipleFiles)
			{
				FilesContent.Add(fileContentInfo);
			}
			else
			{
				ClearCommand.Execute(null);
				FilesContent.Add(fileContentInfo);
			}
			
			File = fileContentInfo;
		}

		private void ClearCommandAction(object parameter)
		{
			ErrorMessage = "";
			CurrentFile = null;
			Files.Clear();
			_filesSize = 0;
		}

		private void RemoveCommandAction(object parameter)
		{
			var deletedFile = parameter as FilePreview;
			if (deletedFile == null)
				return;

			ErrorMessage = "";

			int deletedFileIndex = Files.IndexOf(deletedFile);
			Files.RemoveAt(deletedFileIndex);
			FilesContent.RemoveAt(deletedFileIndex);
		}


		private bool UpdateFilesSize(long newFileSize)
		{
			long newSumSize = _filesSize + newFileSize;
			if (newSumSize >= MaxNewFilesSize)
			{
				ErrorMessage = ResourcesHelper.GetString("MmsLimit");
				return false;
			}
			_filesSize = newSumSize;
			return true;
		}
	}
}
