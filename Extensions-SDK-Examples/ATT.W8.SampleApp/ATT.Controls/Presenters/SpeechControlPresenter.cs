// <copyright file="SpeechControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.Storage;

using ATT.Controls.Utility;
using ATT.Services;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Presenter for <see cref="SpeechControl"/>
	/// </summary>
	public class SpeechControlPresenter : PresenterBase
	{
		private readonly ISpeechService _speechService;
		private string _transcriptMessage = String.Empty;
		private StorageFile _file;

		/// <summary>
		/// Creates instance of <see cref="SpeechControlPresenter"/>
		/// </summary>
		/// <param name="srv">Speech service</param>
		public SpeechControlPresenter(ISpeechService srv)
		{
			_speechService = srv;
			SendSpeech = new RelayCommand(Transcript, () => CanSendSpeech);
		}


		/// <summary>
		/// Gets value can we send speech
		/// </summary>
		public bool CanSendSpeech
		{
			get
			{
				return File != null;
			}
		}

		/// <summary>
		/// Gets or sets command for sending speech
		/// </summary>
		public RelayCommand SendSpeech { get; private set; }

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
		/// Gets transcript text
		/// </summary>
		public string TranscriptMessage
		{
			get
			{
				return _transcriptMessage;
			}
			private set
			{
				if (_transcriptMessage != value)
				{
					_transcriptMessage = value;
					OnPropertyChanged(() => TranscriptMessage);
				}
			}
		}

		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public override void Unload()
		{
			SendSpeech.Deactivate();
		}

		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private async void Transcript(object parameter)
		{
			try
			{
				IsEditableEnabled = false;
				TranscriptMessage = String.Empty;
				ErrorMessage = String.Empty;
				SpeechResponse response = null;
				
				if (File != null)
				{
					response = await _speechService.Send(File);
				}

				TranscriptMessage = response != null ? response.TranscriptText : String.Empty;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				IsEditableEnabled = true;
			}
		}
	}
}
