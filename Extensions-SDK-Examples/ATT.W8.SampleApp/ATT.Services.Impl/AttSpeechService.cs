// <copyright file="AttSpeechService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

using ATT.Utility;
using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;

namespace ATT.Services.Impl
{
	/// <summary>
	/// ATT speech service implementation.
	/// </summary>
	public class AttSpeechService: AttApiService, ISpeechService
	{
		// Speech service instance.
		private readonly SpeechService _service;
		
		/// <summary>
		/// Create instance of <see cref="AttSpeechService"/>
		/// </summary>
		/// <param name="endPoint">Service endpoint (service address)</param>
		/// <param name="apiKey">Api key for ATT REST services.</param>
		/// <param name="secretKey">User secret key.</param>
		public AttSpeechService(string endPoint, string apiKey, string secretKey)
			: base(endPoint, apiKey, secretKey)
		{
			_service = new SpeechService(Settings);
		}

		/// <summary>
		/// Sends audio file to Speech service API and retrieves the recognition of call. 
		/// </summary>
		/// <param name="attachment">Audio file.</param>
		/// <returns>Recognition of call.</returns>
		public async Task<SpeechResponse> Send(StorageFile attachment)
		{
			Argument.ExpectNotNull(() => attachment);


			var contentInfo = new ContentInfo
				{
					Content = await FileUtils.ReadAllBytes(attachment),
					Name = attachment.Name
				};
			WP8.SDK.Entities.SpeechResponse taskResp = await _service.SpeechToText(contentInfo);

			var recognition = new StringBuilder();
			foreach (NBest nBest in taskResp.Recognition.NBest)
			{
				foreach (var text in nBest.ResultText)
				{
					recognition.Append(text);
				}
			}
			
			var response = new SpeechResponse(recognition.ToString());
			return response;
		}
	}
}
