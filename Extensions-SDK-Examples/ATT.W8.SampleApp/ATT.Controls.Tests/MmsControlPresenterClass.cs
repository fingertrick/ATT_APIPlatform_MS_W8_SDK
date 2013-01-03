// <copyright file="MMSControlPresenterClass.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ATT.Services;
using ATT.Controls.Presenters;

namespace ATT.Controls.Tests
{
	[TestClass]
	public class MmsControlPresenterClass
	{
		[TestClass]

		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class CanSendMmsProperty
		{
			[TestMethod]
			public void ShouldBeFalseWhenControlInitialized()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				Assert.IsFalse(presenter.CanSendMessage);
			}

			[TestMethod]
			public void ShouldBeTrueWhenPhoneNumbersAndMessageAreSpecified()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test";
				Assert.IsTrue(presenter.CanSendMessage);
			}

			[TestMethod]
			public void ShouldBeFalseAfterNewMessageCommandIsFired()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.NewCommand.Execute(null);
				Assert.IsFalse(presenter.CanSendMessage);
			}
		}

		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class SendMmsCommand
		{
			[TestMethod]
			public void ShouldNotBeExecutableBeforePhoneNumbersAreSpecified()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				Assert.IsFalse(presenter.SendCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldBeExecutableAfterPhoneNumbersAndMessageAreSpecified()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test";
				Assert.IsTrue(presenter.SendCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldSendMmsToSingleNumber()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.SendCommand.Execute(null);
			   
				Assert.AreEqual(1, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
			}

			[TestMethod]
			public void ShouldSendMmsToMultipleNumbers()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890,1098765432";
				presenter.SendCommand.Execute(null);

				Assert.IsTrue(srv.SendMmsToMultipleNumberMethodWasCalled);
				Assert.AreEqual(2, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
				Assert.AreEqual("tel:1098765432", srv.NumbersSentTo[1].Number);
			}

			[TestMethod]
			public void ShouldIgnoreEmptyPhoneNumbers()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
				presenter.PhoneNumbers = ",,,1234567890,,,,1098765432,,,,,,,";
				presenter.SendCommand.Execute(null);

				Assert.IsTrue(srv.SendMmsToMultipleNumberMethodWasCalled);
				Assert.AreEqual(2, srv.NumbersSentTo.Length);
				Assert.AreEqual("tel:1234567890", srv.NumbersSentTo[0].Number);
				Assert.AreEqual("tel:1098765432", srv.NumbersSentTo[1].Number);
			}

			[TestMethod]
			public void ShouldSendEmptySms()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = String.Empty;
				presenter.SendCommand.Execute(null);

				Assert.IsNotNull(srv.MmsSent);
				Assert.IsTrue(String.IsNullOrWhiteSpace(srv.MmsSent.Body));
			}

			[TestMethod]
			public void ShouldSendMmsWithText()
			{
				var smsText = "some text " + Guid.NewGuid().ToString();

				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = smsText;
				presenter.SendCommand.Execute(null);

				Assert.IsNotNull(srv.MmsSent);
				Assert.AreEqual(smsText, srv.MmsSent.Body);
			}

			[TestMethod]
			public void ShouldUpdateMessageStatus()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
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
		public class NewMmsCommand
		{
			[TestMethod]
			public void ShouldBeExecutableBeforeFieldsAreSpecified()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				Assert.IsTrue(presenter.NewCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldBeExecutableAfterFieldsAreSpecified()
			{
				var presenter = new MmsControlPresenter(new StubMmsService());
				presenter.PhoneNumbers = "1234567890";
				presenter.Message = "test text";
				Assert.IsTrue(presenter.NewCommand.CanExecute(null));
			}

			[TestMethod]
			public void ShouldClearFields()
			{
				var srv = new StubMmsService();
				var presenter = new MmsControlPresenter(srv);
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

	internal class StubMmsService : IMmsService
	{
		public MmsMessage MmsSent = null;
		public PhoneNumber[] NumbersSentTo;

		public bool SendMmsToSingleNumberMethodWasCalled{get;set;}
		public bool SendMmsToMultipleNumberMethodWasCalled { get; set; }
	   

		public Task<MmsMessage> Send(MmsMessage mms)
		{
			mms.MessageId = "123";
			SendMmsToMultipleNumberMethodWasCalled = true;
			NumbersSentTo = mms.PhoneNumbers.ToArray();
			MmsSent = mms;
			return Task.FromResult(mms);
		}		

		public Task<MessageDeliveryStatus> GetMmsStatus(string mmsId)
		{
			return Task.FromResult(MessageDeliveryStatus.DeliveredToTerminal);
		}
	}
}
