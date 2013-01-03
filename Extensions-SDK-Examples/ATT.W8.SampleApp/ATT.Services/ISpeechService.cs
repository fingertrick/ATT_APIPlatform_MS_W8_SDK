// <copyright file="ISpeechService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Threading.Tasks;
using Windows.Storage;

namespace ATT.Services
{
	/// <summary>
	/// Service for working with speech.
	/// </summary>
	public interface ISpeechService
	{
		/// <summary>
		/// Sends speech message.
		/// </summary>
		/// <param name="attachment">Instance of <see cref="StorageFile"/>File to send.</param>
		/// <returns>Text transcript (instance of <see cref="SpeechResponse"/>)</returns>
		Task<SpeechResponse> Send(StorageFile attachment);
	}
}
