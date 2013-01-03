// <copyright file="AttSettings.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections;
using Windows.UI.Popups;

using ATT.Controls.Utility;
using ATT.Services.Impl;
using Windows.Storage;

namespace ATT.Controls
{
	/// <summary>
	/// Connection setting for AT&amp;T SDK. If you would like to use multiple controls and set the values in one place, you may use the ATTSettings static class.
	/// </summary>
	public class AttSettings
	{
		private const string CurrentKeyPositionName = "currentKeyPosition";

		private static string _apiKey;		
		private static string _secretKey;
		
		private static IList _staticSettings;		
		private static ApplicationDataContainer _localSettings;

		static AttSettings()
		{
			_staticSettings = new[] { new { ApiKey = "86b5fcd92027c7f80aeaec367a726313", SecretKey = "1f6c971dca8ce879" }, 
									 new { ApiKey = "c6afc25343ad3f62205e51aa91650737", SecretKey = "5d29a08e6aeb6133" },
									 new { ApiKey = "beab2c59b9a7034e888e663d0b7d7d8d", SecretKey = "2b9824801e1543aa" },
									 new { ApiKey = "5ab47be3dab3bbbfc3b0ddd512b50c08", SecretKey = "dea80b11f5fc3ff9" },
									 new { ApiKey = "418c1abe0b8c9ad3713d1b484fafa267", SecretKey = "a1dcea238e606e92" }};

			_localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			if (!_localSettings.Values.ContainsKey(CurrentKeyPositionName))
			{
				CurrentKeyPosition = 0;
			}			

			AuthorizationServiceProxy.AuthorizationFailed += ServiceAuthorizationFailed;			
		}


		/// <summary>
		/// Gets or sets API key.
		/// </summary>
		public static string ApiKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_apiKey))
				{
					dynamic defaultSettings = _staticSettings[CurrentKeyPosition];

					return defaultSettings.ApiKey;
				}

				return _apiKey;
			}
			set
			{
				_apiKey = value;
			}
		}

		/// <summary>
		/// Gets or sets Secret key.
		/// </summary>
		public static string SecretKey
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_secretKey))
				{
					dynamic defaultSettings = _staticSettings[CurrentKeyPosition];

					return defaultSettings.SecretKey;
				}

				return _secretKey;
			}
			set
			{
				_secretKey = value;
			}
		}

		private static async void ServiceAuthorizationFailed(object sender, EventArgs e)
		{
			if (CurrentKeyPosition + 1 == _staticSettings.Count)
			{
				var messageDialog = new MessageDialog(ResourcesHelper.GetString("KeysExpired"));
				await messageDialog.ShowAsync();
			}
			else
			{
				CurrentKeyPosition++;				
			}
		}

		private static int CurrentKeyPosition
		{
			get
			{
				return (int)_localSettings.Values[CurrentKeyPositionName];
			}
			set
			{
				_localSettings.Values[CurrentKeyPositionName] = value;
			}
		}
	}
}
