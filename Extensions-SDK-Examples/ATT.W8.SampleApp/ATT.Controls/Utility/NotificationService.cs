// <copyright file="NotificationService.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ATT.Services;
using ATT.Utility;

//in Package.appxmanifest

//<Applications>
//	<Application Id="ToastsSampleCS.App" Executable="ToastsSampleCS.exe" EntryPoint="ToastsSampleCS.App">
//	  <VisualElements DisplayName="ToastsSample CS" Logo="Images\squareTile-sdk.png" SmallLogo="Images\smallTile-sdk.png" Description="ToastsSample CS" ForegroundText="light" BackgroundColor="#00b2f0" ToastCapable="true">
//		<DefaultTile ShortName="Toasts CS" ShowName="allLogos" />
//		<SplashScreen Image="Images\splash-sdk.png" BackgroundColor="#00b2f0" />
//	  </VisualElements>
//	</Application>
//  </Applications>

//where 
//BackgroundColor="#00b2f0" - color for application and for Toast
//SmallLogo="Images\smallTile-sdk.png" - logo which show on Toast

namespace ATT.Controls.Utility
{
	/// <summary>
	/// show ToastNotification message status
	/// </summary>
	internal static class NotificationService
	{
		/// <summary>
		/// Shows message status notification.
		/// </summary>
		public static void ShowNotification(OutboundMessage message, MessageDeliveryStatus status)
		{
			Argument.ExpectNotNull(() => message);

			string notificationHeader;
			if (message is SmsMessage)
			{
				notificationHeader = ResourcesHelper.GetString("SMS");
			}
			else if (message is MmsMessage)
			{
				notificationHeader = ResourcesHelper.GetString("MMS");
			}
			else
			{
				throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "Message type {0} is not supported", message.GetType().Name));
			}

			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
			XmlNodeList textElements = toastXml.GetElementsByTagName("text");

			var builder = new StringBuilder();

			builder.Append(notificationHeader);

			builder.Append(" ");
			builder.Append(ResourcesHelper.GetString("MessageSentTo"));
			builder.Append(" ");

			string phones = String.Join(", ", message.PhoneNumbers.Select(n => n.Number));
			builder.Append(phones);
			builder.Append(" ");

			switch (status)
			{
				case MessageDeliveryStatus.DeliveredToNetwork:
					builder.Append(ResourcesHelper.GetString("SuccessfullySent"));
					break;
				case MessageDeliveryStatus.DeliveredToTerminal:
					builder.Append(ResourcesHelper.GetString("SuccessfullyDelivered"));
					break;
				case MessageDeliveryStatus.DeliveryImpossible:
					builder.Append(ResourcesHelper.GetString("NotDelivered"));
					break;
				case MessageDeliveryStatus.Error:
					builder.Append(ResourcesHelper.GetString("Error"));
					break;
			}

			textElements[0].InnerText = builder.ToString();

			var toast = new ToastNotification(toastXml);
			ToastNotificationManager.CreateToastNotifier().Show(toast);
		}
	}
}
