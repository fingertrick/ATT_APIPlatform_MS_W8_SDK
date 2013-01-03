// <copyright file="AttMmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATT.Utility;
using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Implementation of AT&amp;T MMS service.
	/// </summary>
	public class AttMmsService : AttApiService, IMmsService
	{
		private readonly MmsService _mmsServiceWrapper;

		/// <summary>
		/// Creates instance of <see cref="AttMmsService"/>.
		/// </summary>
		/// <param name="endPoint">Service endpoint.</param>
		/// <param name="apiKey">API key.</param>
		/// <param name="secretKey">Secret key.</param>
		public AttMmsService(string endPoint, string apiKey, string secretKey)
			: base(endPoint, apiKey, secretKey)
		{
			_mmsServiceWrapper = new MmsService(Settings);
		}	   

	   /// <summary>
		/// Sends MMS message to multiple recipients
	   /// </summary>
	   /// <param name="mms">MMS message to be sent.</param>
		/// <returns>Returns Task as a result of asynchronous operation. task result is <see cref="MmsMessage"/> sent MMS.</returns>
		/// <exception cref="System.ArgumentNullException">mms is null.</exception>
		public async Task<MmsMessage> Send(MmsMessage mms)
		{
			Argument.ExpectNotNull(() => mms);

			var contentInfoList = new List<ContentInfo>();
			foreach(var attach in mms.Attachments)
			{
				var contentInfo = new ContentInfo
					{
						Content = await FileUtils.ReadAllBytes(attach), 
						Name = attach.Name
					};
				contentInfoList.Add(contentInfo);
			}
			
			MmsResponse taskResp = await _mmsServiceWrapper.SendMms(mms.PhoneNumbers.Select(p => p.Number).ToList(), mms.Body, contentInfoList);
			mms.MessageId = taskResp.Id;

			return mms;
		}			

		/// <summary>
		/// Gets sent MMS message delivery status.
		/// </summary>
		/// <param name="mmsId">Message identifier.</param>
		/// <returns>Current message delivery status.</returns>
		/// <exception cref="System.ArgumentNullException">mmsId is null.</exception>
		public async Task<MessageDeliveryStatus> GetMmsStatus(string mmsId)
		{
			Argument.ExpectNotNull(() => mmsId);

			DeliveryInfoList resp = await _mmsServiceWrapper.GetMmsStatus(mmsId);

			// AT&T service returns message delivery status as string - cast it to enum with all possible delivery statuses
			return (MessageDeliveryStatus)Enum.Parse(typeof(MessageDeliveryStatus), resp.DeliveryStatus.DeliveryInfoList.First().DeliveryStatus);
		}
	}
}
