// <copyright file="AttApiService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using ATT.Utility;
using ATT.WP8.SDK.Entities;

namespace ATT.Services.Impl
{
	/// <summary>
	/// Base class for AT&amp;T services
	/// </summary>
	public abstract class AttApiService
	{
		private readonly AttServiceSettings _settings = null;

		/// <summary>
		/// Gets AT&amp;T service settings <see cref="AttServiceSettings"/>
		/// </summary>
		public AttServiceSettings Settings
		{
			get { return _settings; }
		}

		/// <summary>
		/// Creates instance of <see cref="AttApiService"/>
		/// </summary>
		/// <param name="endPoint">AT&amp;T service endpoint</param>
		/// <param name="apiKey">API key</param>
		/// <param name="secretKey">Secret key</param>
		/// <exception cref="System.ArgumentNullException">endPoint is null or apiKey is null or secretKey is null.</exception>
		protected AttApiService(string endPoint, string apiKey, string secretKey)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => endPoint);
			Argument.ExpectNotNullOrWhiteSpace(() => apiKey);
			Argument.ExpectNotNullOrWhiteSpace(() => secretKey);

			_settings = new AttServiceSettings(apiKey, secretKey, new Uri(endPoint));
		}
	}
}
