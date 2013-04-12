// <copyright file="FilePreview.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.IO;
using System.Windows.Input;

namespace ATT.WP8.SampleApp.Controls
{
	/// <summary>
	/// Class describes file entity for user selected file from file picker.
	/// </summary>
	public class FilePreview
	{
		private string _filePath = String.Empty;
		private string _fileName = String.Empty;
		

		/// <summary>
		/// Gets or sets file path
		/// </summary>
		public string FilePath 
		{
			get 
			{ 
				return _filePath; 
			}
			private set 
			{ 
				_filePath = value;
				_fileName = Path.GetFileName(_filePath);
			}
		}

		/// <summary>
		/// Gets file name
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
		}


		/// <summary>
		/// Delete file command.
		/// </summary>
		public ICommand DeleteFileCommand { get; private set; }

		/// <summary>
		/// Creates instance of <see cref="FilePreview"/>
		/// </summary>
		/// <param name="filePath">File Path</param>
		/// <param name="deleteFileCommand">Delete File command.</param>
		public FilePreview(string filePath, ICommand deleteFileCommand)
		{
			FilePath = filePath;
			DeleteFileCommand = deleteFileCommand;
		}
	}
}
