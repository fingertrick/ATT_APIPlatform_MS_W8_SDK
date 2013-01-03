// <copyright file="SpeechResponse.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

namespace ATT.Services
{
	/// <summary>
	/// Represents response from speech service.
	/// </summary>
	public class SpeechResponse
	{
		/// <summary>
		/// Creates instance of <see cref="SpeechResponse"/>.
		/// </summary>
		/// <param name="transcriptText">Transcribed text.</param>
		public SpeechResponse(string transcriptText)
		{
			TranscriptText = transcriptText;
		}

		/// <summary>
		/// Gets or sets transcribed text.
		/// </summary>
		public string TranscriptText
		{
			get;
			private set;
		}

	}
}
