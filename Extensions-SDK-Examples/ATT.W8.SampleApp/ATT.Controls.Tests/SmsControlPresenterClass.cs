// <copyright file="SMSControlPresenterClass.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATT.Controls.Presenters;
using ATT.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace ATT.Controls.Tests
{
	[TestClass]
	public class SmsControlPresenterClass
	{
		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class CanSendSmsProperty
		{
			[TestMethod]
			public void ShouldBeFalseWhenControlInitialized()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				Assert.IsFalse(presenter.CanSendMessage);
			}

			[TestMethod]
			public void ShouldBeTrueWhenPhoneNumbersAndMessageAreSpecified()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test";
				Assert.IsTrue(presenter.CanSendMessage);
			}

			[TestMethod]
			public void ShouldBeFalseAfterNewMessageCommandIsFired()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.NewCommand.Execute(null);

				Assert.IsFalse(presenter.CanSendMessage);
			}
		}

		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class SendSmsCommand
		{
			[TestMethod]
			public void ShouldNotBeExecutableBeforePhoneNumbersAreSpecified()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				Assert.IsFalse(presenter.SendCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldBeExecutableAfterPhoneNumbersAndMessageAreSpecified()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test";
				Assert.IsTrue(presenter.SendCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldSendSmsToSingleNumber()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.SendCommand.Execute(null);
				
				Assert.AreEqual(1, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
			}

			[TestMethod]
			public void ShouldSendSmsToMultipleNumbers()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890,1098765432";
				presenter.SendCommand.Execute(null);

				Assert.IsTrue(srv.SendSmsToMultipleNumberMethodWasCalled);
				Assert.AreEqual(2, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
				Assert.AreEqual("tel:1098765432", srv.NumbersSentTo[1].Number);
			}

			[TestMethod]
			public void ShouldIgnoreEmptyPhoneNumbers()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = ",,,1234567890,,,,1098765432,,,,,,,";
				presenter.SendCommand.Execute(null);

				Assert.IsTrue(srv.SendSmsToMultipleNumberMethodWasCalled);
				Assert.AreEqual(2, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
				Assert.AreEqual("tel:1098765432", srv.NumbersSentTo[1].Number);
			}

			[TestMethod]
			public void ShouldSendEmptySms()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = String.Empty;
				presenter.SendCommand.Execute(null);

				Assert.IsNotNull(srv.SmsSent);
				Assert.IsTrue(String.IsNullOrWhiteSpace(srv.SmsSent.Body));
			}

			[TestMethod]
			public void ShouldSendSmsWithText()
			{
				var smsText = "some text " + Guid.NewGuid().ToString();

				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = smsText;
				presenter.SendCommand.Execute(null);

				Assert.IsNotNull(srv.SmsSent);
				Assert.AreEqual(smsText, srv.SmsSent.Body);
			}

			[TestMethod]
			public void ShouldUpdateMessageStatus()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				
				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName.Equals("MessageStatus", StringComparison.CurrentCulture))
					{
						messageStatusUpdated.Set();
					}
				};

				presenter.SendCommand.Execute(null);

				Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
				Assert.AreEqual(MessageDeliveryStatus.DeliveredToTerminal, presenter.MessageStatus);
			}
		}

		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class NewSmsCommand
		{
			[TestMethod]
			public void ShouldBeExecutableBeforeFieldsAreSpecified()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				Assert.IsTrue(presenter.NewCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldBeExecutableAfterFieldsAreSpecified()
			{
				var presenter = new SmsControlPresenter(new StubSmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test text";
				Assert.IsTrue(presenter.NewCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldClearFields()
			{
				var srv = new StubSmsService();
				var presenter = new SmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test text";
				presenter.NewCommand.Execute(null);

				Assert.IsTrue(String.IsNullOrEmpty(presenter.PhoneNumbers));
				Assert.IsTrue(String.IsNullOrEmpty(presenter.Message));
				Assert.IsTrue(String.IsNullOrEmpty(presenter.MessageStatusName));
				Assert.AreEqual(MessageDeliveryStatus.Initial, presenter.MessageStatus);
			}
		}
	}

	internal class StubSmsService : ISmsService
	{
		public SmsMessage SmsSent = null;
		public PhoneNumber[] NumbersSentTo;

		public bool SendSmsToSingleNumberMethodWasCalled { get; set; }
		public bool SendSmsToMultipleNumberMethodWasCalled { get; set; }

		public Task<SmsMessage> Send(SmsMessage sms)
		{
			sms.MessageId = "123";
			SendSmsToMultipleNumberMethodWasCalled = true;
			NumbersSentTo = sms.PhoneNumbers.ToArray();
			SmsSent = sms;
			return Task.FromResult(sms);
		}	 

		public Task<MessageDeliveryStatus> GetSmsStatus(string smsId)
		{
			return Task.FromResult(MessageDeliveryStatus.DeliveredToTerminal);
		}

		public async Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode)
		{
			await Task.Delay(10);
			return Enumerable.Empty<InboundSms>();
		}
	}
}
