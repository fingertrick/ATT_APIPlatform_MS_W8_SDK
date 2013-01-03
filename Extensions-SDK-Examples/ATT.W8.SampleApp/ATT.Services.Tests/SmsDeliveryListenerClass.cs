// <copyright file="SmsDeliveryListenerClass.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATT.Services;
using ATT.Services.Impl.Delivery;
using System.Threading;

namespace ATT.Utility.Tests
{
	[TestClass]
	public class SmsDeliveryListenerClass
	{
		[TestMethod]
		public void ShouldCancelOperationOnTimeout()
		{
			var serviceMock = new AttSmsServiceMock(MessageDeliveryStatus.DeliveredToTerminal, 999);
			var sms = new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("343434") }, "body");
			sms.MessageId = "test";

			var messageStatusUpdated = new ManualResetEvent(false);
			var status = MessageDeliveryStatus.Initial;

			var listener = new SmsDeliveryListener(serviceMock, sms, TimeSpan.FromMilliseconds(20), TimeSpan.FromMilliseconds(100));
			listener.StatusChangedEvent += (s, e) =>
			{
				status = e.MessageStatus;
				messageStatusUpdated.Set();
			};
			listener.ListenForMessageDelivered();

			Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
			Assert.AreEqual(MessageDeliveryStatus.Error, status);
		}

		[TestMethod]
		public void ShouldReturnDeliveredToTerminalStatus()
		{
			var serviceMock = new AttSmsServiceMock(MessageDeliveryStatus.DeliveredToTerminal, 0);
			var sms = new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("343434") }, "body");
			sms.MessageId = "test";

			var messageStatusUpdated = new ManualResetEvent(false);
			var status = MessageDeliveryStatus.Initial;

			var listener = new SmsDeliveryListener(serviceMock, sms, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2));
			listener.StatusChangedEvent += (s, e) =>
			{
				status = e.MessageStatus;
				messageStatusUpdated.Set();
			};
			listener.ListenForMessageDelivered();

			Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
			Assert.AreEqual(MessageDeliveryStatus.DeliveredToTerminal, status);
		}

		[TestMethod]
		public void ShouldReturnDeliveryImpossibleStatus()
		{
			var serviceMock = new AttSmsServiceMock(MessageDeliveryStatus.DeliveryImpossible, 0);
			var sms = new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("343434") }, "body");
			sms.MessageId = "test";

			var messageStatusUpdated = new ManualResetEvent(false);
			var status = MessageDeliveryStatus.Initial;

			var listener = new SmsDeliveryListener(serviceMock, sms, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2));
			listener.StatusChangedEvent += (s, e) =>
			{
				status = e.MessageStatus;
				messageStatusUpdated.Set();
				
			};
			listener.ListenForMessageDelivered();		   

			Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
			Assert.AreEqual(MessageDeliveryStatus.DeliveryImpossible, status);
		}

		[TestMethod]
		public void ShouldCallGetMessageStatusUntilItIsChanged()
		{
			var serviceMock = new AttSmsServiceMock(MessageDeliveryStatus.DeliveredToTerminal, 3);
			var sms = new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("343434") }, "body");
			sms.MessageId = "test";

			var messageStatusUpdated = new ManualResetEvent(false);
			var listener = new SmsDeliveryListener(serviceMock, sms, TimeSpan.FromMilliseconds(20), TimeSpan.FromSeconds(2));
			listener.StatusChangedEvent += (s, e) =>
			{
				messageStatusUpdated.Set();
			};

			listener.ListenForMessageDelivered();

			Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
			Assert.AreEqual(4, serviceMock.TimesCalled);
		}
	}

	internal class AttSmsServiceMock : ISmsService
	{
		private readonly MessageDeliveryStatus _statusToReturn;
		private int _timesToReturnDefaultStatus;

		public AttSmsServiceMock(MessageDeliveryStatus statusToReturn, int timesToReturnDefaultStatus)
		{
			_statusToReturn = statusToReturn;
			_timesToReturnDefaultStatus = timesToReturnDefaultStatus;
		}

		public int TimesCalled
		{
			get;
			private set;
		}

		public Task<SmsMessage> Send(SmsMessage sms)
		{
			throw new NotImplementedException();
		}

		public Task<MessageDeliveryStatus> GetSmsStatus(string smsId)
		{
			TimesCalled++;

			if (_timesToReturnDefaultStatus > 0)
			{
				_timesToReturnDefaultStatus--;
				return Task.FromResult(MessageDeliveryStatus.DeliveredToNetwork);
			}

			return Task.FromResult(_statusToReturn);
		}

		public Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode)
		{
			throw new NotImplementedException();
		}

		public Task<string> Send(SmsMessage sms, IEnumerable<PhoneNumber> phoneNumbers)
		{
			throw new NotImplementedException();
		}
	}
}
