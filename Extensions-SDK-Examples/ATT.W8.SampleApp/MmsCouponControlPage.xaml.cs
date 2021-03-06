﻿// <copyright file="MmsCouponControlPage.xaml.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Linq;
using Windows.Storage;

using ATT.Controls;
using ATT.Services;

namespace ATT.SampleApp
{
	/// <summary>
	/// Page that contains MMS Coupon control.
	/// </summary>
	public sealed partial class MmsCouponControlPage : ATT.SampleApp.Common.LayoutAwarePage
	{
		/// <summary>
		/// Creates instance of <see cref="MmsCouponControlPage"/>
		/// </summary>
		public MmsCouponControlPage()
		{
			InitializeComponent();
		}

		private void mmsControl_MMSMessagePrepared(object sender, MessagePreparedEventArgs e)
		{
			var message = e.Message as MmsMessage;

			if (message != null && message.Attachments.Count() == 1)
			{
				var fileTask = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/SmallLogo.png")).AsTask();
				fileTask.Wait();
				message.AddAttachment(fileTask.Result);
			}
		}

		private void mmsCouponControlPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			mmsCouponControl.MessagePrepared += mmsControl_MMSMessagePrepared;
		}
	}
}
