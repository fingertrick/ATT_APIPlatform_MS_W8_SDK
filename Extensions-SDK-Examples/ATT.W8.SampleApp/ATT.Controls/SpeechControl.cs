// <copyright file="SpeechControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.Storage;
using Windows.UI.Xaml;
using ATT.Controls.Presenters;
using ATT.Services.Impl;

namespace ATT.Controls
{
	/// <summary>
	/// The speech control takes in an audio file and transcribes it into text which you can copy/paste, save, and so on.
	/// </summary>
	public sealed class SpeechControl : ControlWithPresenter
	{
		#region Dependency properties

		/// <summary>
		/// Style for textBox with open file path
		/// </summary>
		public static readonly DependencyProperty FileOpenPathStyleProperty = DependencyProperty.Register("FileOpenPathStyle", typeof(Style), typeof(SpeechControl), new PropertyMetadata(null));

		/// <summary>
		/// Style for open file button
		/// </summary>
		public static readonly DependencyProperty FileOpenButtonStyleProperty = DependencyProperty.Register("FileOpenButtonStyle", typeof(Style), typeof(SpeechControl), new PropertyMetadata(null));

		/// <summary>
		/// Style for transcript textBox
		/// </summary>
		public static readonly DependencyProperty TranscriptTextStyleProperty = DependencyProperty.Register("TranscriptTextStyle", typeof(Style), typeof(SpeechControl), new PropertyMetadata(null));   

		/// <summary>
		/// Transcript text
		/// </summary>
		public static readonly DependencyProperty TranscriptTextProperty = DependencyProperty.Register("TranscriptText", typeof(string), typeof(SpeechControl), new PropertyMetadata(""));

		#endregion

		/// <summary>
		/// Creates and initializes presenter instance for Speech control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			var speechSrv = new AttSpeechService(Endpoint, ApiKeyConfigured, SecretKeyConfigured);
			return new SpeechControlPresenter(speechSrv);
		}

		/// <summary>
		/// Gets or sets style for open file path textBox.
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
		/// Gets or sets style for the text received from the server.
		/// </summary>
		public Style TranscriptTextStyle
		{
			get { return (Style)GetValue(TranscriptTextStyleProperty); }
			set { SetValue(TranscriptTextStyleProperty, value); }
		}

		/// <summary>
		/// The actual file to attach, upload to server, and interpret.
		/// </summary>
		public StorageFile File
		{
			get
			{
				return Presenter == null ? null : ((SpeechControlPresenter)Presenter).File;
			}
			set
			{
				if (Presenter != null)
				{
					((SpeechControlPresenter)Presenter).File = value;
				}
			}
		}

		/// <summary>
		/// The actual text message that comes back from the server - AT&amp;T's translation of your audio file into English text.
		/// </summary>
		public string TranscriptMessage
		{
			get
			{
				return Presenter == null ? null : ((SpeechControlPresenter)Presenter).TranscriptMessage;
			}
		}

		/// <summary>
		/// If the service encounters and error (no internet connection, file too large, service is not available, etc) it will be displayed here. 
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return Presenter == null ? null : Presenter.ErrorMessage;
			}
		}

		/// <summary>
		/// Gets true if waiting input, otherwise returns false.
		/// </summary>
		public bool IsNewMessageEnabled
		{
			get
			{
				return Presenter == null || Presenter.IsNewMessageEnabled;
			}
		}
	}
}
