// <copyright file="FileOpenPickerPreviewControlViewModel.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.WP8.Controls.Utils;
using ATT.WP8.SDK;

namespace ATT.WP8.SampleApp.Controls
{
	/// <summary>
	/// View Model for <see cref="ATT.WP8.SampleApp.Controls.FileOpenPickerPreviewControl"/>
	/// </summary>
	public class FileOpenPickerPreviewViewModel : NotifyPropertyChangedBase
	{
		private ContentInfo _file;
		private long _maxNewFilesSize;

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public ContentInfo File
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
				return _maxNewFilesSize;
			}
			set
			{
				if (_maxNewFilesSize != value)
				{
					_maxNewFilesSize = value;
					OnPropertyChanged(() => MaxNewFilesSize);
				}
			}
		}
	}
}
