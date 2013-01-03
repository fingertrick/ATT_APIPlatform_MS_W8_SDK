// <copyright file="Program.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using ATT.Services.Impl;

namespace ATT.Services.Console
{
	class Program
	{
		//test phone number  4258028620

		private const string SendSmsCommand = "send-sms";
		private const string GetSmsStatusCommand = "get-sms-status";

		private const string SendMmsCommand = "send-mms";
		private const string GetMmsStatusCommand = "get-mms-status";

		private const string HelpCommand = "help";
		private const string ExitCommand = "exit";

		private static string _endPoint = ConfigurationManager.AppSettings["endPoint"];
		private static string _apiKey = ConfigurationManager.AppSettings["apiKey"];
		private static string _secretKey = ConfigurationManager.AppSettings["secretKey"];

		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		static void Main(string[] args)
		{
			bool continueLooping = true;
			while (continueLooping)
			{
				System.Console.Write("Enter command: ");

				try
				{
					string command = System.Console.ReadLine();

					continueLooping = ProcessCommand(command);
				}
				catch (Exception ex)
				{
					System.Console.WriteLine("Something happened:");
					System.Console.WriteLine(ex.ToString());
				}
			}
		}

		private static bool ProcessCommand(string command)
		{
			bool continueLooping = true;

			if (command.Equals(HelpCommand, StringComparison.OrdinalIgnoreCase))
			{
				System.Console.WriteLine("Possible commands:");

				System.Console.WriteLine(SendSmsCommand + " <phone number> <message>");
				System.Console.WriteLine(GetSmsStatusCommand + " <sms id>");

				System.Console.WriteLine(SendMmsCommand + " <phone number> <message>");
				System.Console.WriteLine(GetMmsStatusCommand + " <mmsId id>");

				System.Console.WriteLine(ExitCommand);
			}
			else if (command.StartsWith(SendSmsCommand, StringComparison.OrdinalIgnoreCase))
			{
				string number = GetArgument(command, 1);
				string message = GetArgument(command, 2);

				var phoneNumber = new PhoneNumber(number);
				var sms = new SmsMessage(new List<PhoneNumber>() { phoneNumber }, message);

				System.Console.WriteLine("Sending sms...");

				var service = new AttSmsService(_endPoint, _apiKey, _secretKey);
				SmsMessage sentSms = service.Send(sms).Result;

				System.Console.WriteLine("SMS sent");
				System.Console.WriteLine("SMS id is {0}", sentSms.MessageId);
			}
			else if (command.StartsWith(GetSmsStatusCommand, StringComparison.OrdinalIgnoreCase))
			{
				string id = GetArgument(command, 1);
				var smsId = id;

				System.Console.WriteLine("Retrieving sms delivery status...");

				ISmsService service = new AttSmsService(_endPoint, _apiKey, _secretKey);
				var status = service.GetSmsStatus(smsId);

				System.Console.WriteLine("Status is {0}", status);
			}
			else if (command.StartsWith(SendMmsCommand, StringComparison.OrdinalIgnoreCase))
			{
				string number = GetArgument(command, 1);
				string message = GetArgument(command, 2);

				var phoneNumber = new PhoneNumber(number);
				var mms = new MmsMessage(new List<PhoneNumber>() { phoneNumber }, message);

				System.Console.WriteLine("Sending mms...");

				var service = new AttMmsService(_endPoint, _apiKey, _secretKey);
				MmsMessage sentMms = service.Send(mms).Result;

				System.Console.WriteLine("MMS sent");
				System.Console.WriteLine("MMS id is {0}", sentMms.MessageId);
			}
			else if (command.StartsWith(GetMmsStatusCommand, StringComparison.OrdinalIgnoreCase))
			{
				string id = GetArgument(command, 1);
				var mmsId = id;

				System.Console.WriteLine("Retrieving mms delivery status...");

				var service = new AttMmsService(_endPoint, _apiKey, _secretKey);
				var status = service.GetMmsStatus(mmsId);

				System.Console.WriteLine("Status is {0}", status);
			}
			else if (command.Equals(ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				continueLooping = false;

				System.Console.WriteLine("Exiting. Please any key to close...");
				System.Console.Read();
			}
			else
			{
				System.Console.WriteLine("Command is not recognized. Type '{0}' for the list of allowed commands.", HelpCommand);
			}

			return continueLooping;
		}

		private static string GetArgument(string command, int position)
		{
			string value = command.Trim().Split(' ')[position].Trim();

			if (String.Equals(value, "_", StringComparison.OrdinalIgnoreCase))
			{
				return String.Empty;
			}
			return value;
		}
	}
}
