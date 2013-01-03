// <copyright file="FilePreview.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Windows.Input;
using Windows.Storage;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Class describes file entity for user selected file from file picker.
	/// </summary>
	public class FilePreview
	{
		/// <summary>
		/// Selected storage file.
		/// </summary>
		public StorageFile File { get; private set; }

		/// <summary>
		/// Delete file command.
		/// </summary>
		public ICommand DeleteFileCommand { get; private set; }

		/// <summary>
		/// Creates instance of <see cref="FilePreview"/>
		/// </summary>
		/// <param name="file">Storage file.</param>
		/// <param name="deleteFileCommand">Delete File command.</param>
		public FilePreview(StorageFile file, ICommand deleteFileCommand)
		{
			File = file;
			DeleteFileCommand = deleteFileCommand;
		}
	}
}
