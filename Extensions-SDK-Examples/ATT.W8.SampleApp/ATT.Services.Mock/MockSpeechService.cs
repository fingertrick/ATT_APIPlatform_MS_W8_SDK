// <copyright file="MockSpeechService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace ATT.Services.Mock
{
	/// <summary>
	/// Mock for speech service
	/// </summary>
	public sealed class MockSpeechService : ISpeechService
	{
		private static bool _returnError;

		/// <summary>
		/// Sends speech message.
		/// </summary>
		/// <param name="speech">Instance of <see cref="StorageFile"/> to be sent.</param>
		/// <returns>Transcript text (instance of <see cref="SpeechResponse"/>)</returns>
		public async Task<SpeechResponse> Send(StorageFile speech)
		{
			await Task.Delay(3000);
			_returnError = !_returnError;
			return new SpeechResponse(String.Format("Example of transcript text. File: '{0}'", speech != null ? speech.Name : "unknown"));
		}
	}
}
