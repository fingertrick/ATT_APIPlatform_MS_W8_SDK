// <copyright file="AttControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATT.Controls
{
	/// <summary>
	/// Abstract Base class for all AT&amp;T API controls.  
	/// </summary>
	public abstract class AttControl : Control
	{
		/// <summary>
		/// Identifies the Endpoint dependency property - the base URL for the REST API to access.  Defaults to https://api.att.com which is working as of September 2012.
		/// </summary>
		public static readonly DependencyProperty EndpointProperty = DependencyProperty.Register("Endpoint", typeof(string), typeof(AttControl), new PropertyMetadata("https://api.att.com"));

		/// <summary>
		/// Gets or sets Endpoint for ATT services - the base URL for the REST API to access.  Defaults to https://api.att.com which is working as of September 2012.
		/// </summary>
		public string Endpoint
		{
			get { return GetValue(EndpointProperty).ToString(); }
			set { SetValue(EndpointProperty, value); }
		}

		/// <summary>
		/// Identifies the ApiKey dependency property.
		/// </summary>
		public static readonly DependencyProperty ApiKeyProperty = DependencyProperty.Register("ApiKey", typeof(string), typeof(AttControl), new PropertyMetadata(String.Empty));

		/// <summary>
		/// Gets or sets ApiKey for access to ATT services.  If you do not set an ApiKey, the software will default to the value set with the global static ATTSettings.ApiKey.
		/// </summary>
		public string ApiKey
		{
			get { return GetValue(ApiKeyProperty).ToString(); }
			set { SetValue(ApiKeyProperty, value); }
		}

		/// <summary>
		/// Identifies the SecretKey dependency property.
		/// </summary>
		public static readonly DependencyProperty SecretKeyProperty = DependencyProperty.Register("SecretKey", typeof(string), typeof(AttControl), new PropertyMetadata(String.Empty));

		/// <summary>
		/// Gets or sets SecretKey for access to ATT services. If you do not set an ApiKey, the software will default to the value set with the global static ATTSettings.SecretKey.
		/// </summary>
		public string SecretKey
		{
			get { return GetValue(SecretKeyProperty).ToString(); }
			set { SetValue(SecretKeyProperty, value); }
		}

		/// <summary>
		/// Returns true if API key is configured; false else.
		/// </summary>
		protected string ApiKeyConfigured
		{
			get { return String.IsNullOrEmpty(ApiKey) ? AttSettings.ApiKey : ApiKey; }
		}

		/// <summary>
		/// Returns true if secret key is configured; false else.
		/// </summary>
		protected string SecretKeyConfigured
		{
			get { return String.IsNullOrEmpty(SecretKey) ? AttSettings.SecretKey : SecretKey; }
		}
	}
}
