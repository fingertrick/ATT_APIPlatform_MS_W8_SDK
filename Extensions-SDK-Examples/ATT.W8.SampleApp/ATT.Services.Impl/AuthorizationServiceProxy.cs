// <copyright file="AuthorizationServiceProxy.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using ATT.WP8.SDK;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Proxy for WinRTSDK AuthorizationService event AuthorizationFailed
	/// </summary>
	public class AuthorizationServiceProxy
	{
		/// <summary>
		/// raise when authorization failed
		/// </summary>
		public static event EventHandler<EventArgs> AuthorizationFailed;

		static AuthorizationServiceProxy()
		{
			AuthorizationService.AuthorizationFailed += ServiceAuthorizationFailed;
		}

		private static void ServiceAuthorizationFailed(object sender, EventArgs e)
		{
			OnAuthorizationFailed();
		}

		private static void OnAuthorizationFailed()
		{
			if (AuthorizationFailed != null)
			{
				AuthorizationFailed.Invoke(null, new EventArgs());
			}
		}
	}
}
