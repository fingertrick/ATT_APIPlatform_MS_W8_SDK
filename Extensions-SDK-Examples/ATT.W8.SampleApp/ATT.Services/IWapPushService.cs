// <copyright file="IWapPushService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Threading.Tasks;

namespace ATT.Services
{
	/// <summary>
	/// Interface for WAP Push service.
	/// </summary>
	public interface IWapPushService
	{
		/// <summary>
		/// Sends a WAP push notification.
		/// </summary>
		/// <param name="phoneNumber">Phone number where send notification</param>
		/// <param name="url">notification's url</param>
		/// <param name="alertText">notification's alert text</param>
		/// <returns>Id of notification returned by REST service</returns>
		Task<string> Send(string phoneNumber, string url, string alertText);
	}
}
