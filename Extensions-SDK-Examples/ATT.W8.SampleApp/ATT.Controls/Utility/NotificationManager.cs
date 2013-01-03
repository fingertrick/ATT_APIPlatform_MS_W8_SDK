// <copyright file="NotificationManager.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using ATT.Services.Impl;
using ATT.Services.Impl.Delivery;
using ATT.Services.Impl.WeakEvent;
using ATT.Utility;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Manager that manage listeners and wait return status
	/// </summary>
	public class NotificationManager
	{		
		private static readonly Lazy<NotificationManager> _instance = new Lazy<NotificationManager>(() => new NotificationManager());

		private List<MessageDeliveryListener> _listeners;

		private NotificationManager()
		{
			_listeners = new List<MessageDeliveryListener>();
		}

		/// <summary>
		/// Single instance of NotificationManager
		/// </summary>
		public static NotificationManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		/// <summary>
		/// add listener for wait return status
		/// </summary>
		/// <param name="listener"></param>
		public void AddListener(MessageDeliveryListener listener)
		{
			Argument.ExpectNotNull(() => listener);

			listener.StatusChangedEvent += StatusChangedEvent;

			_listeners.Add(listener);
		}	  

		private void StatusChangedEvent(object sender, MessageStatusEventArgs e)
		{
			NotificationService.ShowNotification(e.Message, e.MessageStatus);

			_listeners.Remove(sender as MessageDeliveryListener);
		}
	}
}
