// <copyright file="IMmsService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Threading.Tasks;

namespace ATT.Services
{
	/// <summary>
	/// This object abstracts the service used to send and receive MMS messages.
	/// </summary>
	public interface IMmsService
	{
		/// <summary>
		///  Sends MMS message to multiple recipients.
		/// </summary>
		/// <param name="mms"></param>
		/// <returns></returns>
		Task<MmsMessage> Send(MmsMessage mms);	   

		/// <summary>
		/// Gets sent MMS message delivery status.
		/// </summary>
		/// <param name="mmsId">message id</param>
		/// <returns>current message delivery status</returns>
		Task<MessageDeliveryStatus> GetMmsStatus(string mmsId);
	}
}
