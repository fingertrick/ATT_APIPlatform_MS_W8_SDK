using ATT.WP8.Controls;
using ATT.WP8.Controls.Utils;
using ATT.WP8.SampleApp.Resources;
using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Speech button demo page
	/// </summary>
	public partial class SpeechButtonPage : PhoneApplicationPage
	{
		private SoundRecorder _soundRecorder;
		private bool _isRecording;

		/// <summary>
		/// Creates instance of SpeechButtonPage
		/// </summary>
		public SpeechButtonPage()
		{
			InitializeComponent();
			btnRecord.Content = AppResources.txtStartRecord;
			CreateSoundRecorder();

			Unloaded += (sender, args) => _soundRecorder.Dispose();
		}

		private void EnableTranscription(bool isEnabled)
		{
			btnRecord.IsEnabled = isEnabled;
			tbxTranscriptedText.IsEnabled = isEnabled;
		}

		private void btnSpeech_Click(object sender, RoutedEventArgs e)
		{
			EnableTranscription(false);
			tbErrorMessage.ErrorMessage = String.Empty;
			tbxTranscriptedText.Text = String.Empty;
		}

		private void btnSpeech_MessageTranscripted(object sender, TranscriptedMessageEventArgs e)
		{
			if (!String.IsNullOrEmpty(btnSpeech.TranscriptedText))
			{
				tbxTranscriptedText.Text = btnSpeech.TranscriptedText;
			}
			
			EnableTranscription(true);
		}

		private void btnSpeech_Error(object sender, UnhandledExceptionEventArgs e)
		{
			var exception = e.ExceptionObject as Exception;
			if (exception != null)
			{
				tbErrorMessage.ErrorMessage = exception.Message;
			}
			EnableTranscription(true);
		}

		private async void btnRecord_Click(object sender, RoutedEventArgs e)
		{
			if (!_isRecording)
			{
				_isRecording = true;
				btnRecord.Content = AppResources.txtStopRecord;
				_soundRecorder.StartRecording();
				btnSpeech.AudioContent = null;
				tbErrorMessage.ErrorMessage = String.Empty;
			}
			else
			{
				_isRecording = false;
				btnRecord.Content = AppResources.txtStartRecord;
				_soundRecorder.StopRecording();
				btnSpeech.AudioContent = await _soundRecorder.GetBytes();
				btnSpeech.AudioContentName = _soundRecorder.FilePath;
			}
		}

		private void RecodingTimerStoped(object sender, EventArgs eventArgs)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() => btnRecord_Click(this, new RoutedEventArgs()));
		}

		private void CreateSoundRecorder()
		{
			try
			{
				_soundRecorder = new SoundRecorder();
				_soundRecorder.RecodingTimerStoped+=RecodingTimerStoped;
			}
			catch (MicrophoneNotSupportedException ex)
			{
				tbErrorMessage.ErrorMessage = ex.Message;
			}
		}
	}
}