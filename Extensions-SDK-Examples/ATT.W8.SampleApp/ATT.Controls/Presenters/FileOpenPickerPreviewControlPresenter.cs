// <copyright file="FileOpenPickerPreviewControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.Storage;
using ATT.Controls.Utility;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Presenter for <see cref="ATT.Controls.SubControls.FileOpenPickerPreviewControl"/>
	/// </summary>
	public class FileOpenPickerPreviewControlPresenter : NotifyPropertyChangedBase
	{
		private StorageFile _file;
		private ulong _maxNewFilesSize;

		/// <summary>
		/// Gets or sets attachment file
		/// </summary>
		public StorageFile File
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
		public ulong MaxNewFilesSize
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
