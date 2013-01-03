// <copyright file="SmsVotingControlPresenterClass.cs" company="AT&amp;T">
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
	public class SmsVotingControlPresenterClass
	{
		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class SaveVotingToMockStorage
		{
			[TestMethod]
			public void ShouldUpdateVotesWhenSmsSent()
			{
				string code = "555";
				string wordKey = "something";
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber(code) }, wordKey));

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				Assert.IsTrue(messageStatusUpdated.WaitOne(TimeSpan.FromSeconds(1)));
				Assert.AreEqual(1, presenter.VotingResults.Count);
			}

			[TestMethod]
			public void ShouldUpdateVotingStorageWhenSmsSent()
			{
				string code = "555";
				string voteKey = "something";
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber(code) }, voteKey));

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				messageStatusUpdated.WaitOne(TimeSpan.FromMilliseconds(100));

				IEnumerable<KeyValuePair<string, int>> result = stubStorage.GetStatistics();
				Assert.IsNotNull(result);
				Assert.AreEqual(1, result.Count());
				Assert.AreEqual(voteKey, result.First().Key);
				Assert.AreEqual(1, result.First().Value);
			}

			[TestMethod]
			public void ShouldUpdateVotingStorageByAllSms()
			{
				string code = "555";
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				var wordKeys = new List<string> { "orange", "yellow", "brown" };
				foreach (var key in wordKeys)
				{
					smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber(code) }, key));
				}

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				messageStatusUpdated.WaitOne(TimeSpan.FromMilliseconds(100));

				IEnumerable<KeyValuePair<string, int>> result = stubStorage.GetStatistics();
				Assert.IsNotNull(result);
				Assert.AreEqual(3, result.Count());
				for (int i = 0; i < 3; i++)
				{
					var element = result.FirstOrDefault(item => String.Equals(item.Key, wordKeys[i], StringComparison.CurrentCulture));
					Assert.IsNotNull(element);
					Assert.AreEqual(1, element.Value);
				}
			}

			[TestMethod]
			public void ShouldUpdateVotesByOnlyCodedMessages()
			{
				string code = "555";
				string wordKey = "something";
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("666") }, wordKey));

				Assert.IsFalse(messageStatusUpdated.WaitOne(TimeSpan.FromMilliseconds(200)));
			}

			[TestMethod]
			public void ShouldUpdateVotingStorageByOnlyCodedMessages()
			{
				string code = "555";
				string wordKey = "something";
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber("666") }, wordKey));

				messageStatusUpdated.WaitOne(TimeSpan.FromMilliseconds(200));

				IEnumerable<KeyValuePair<string, int>> result = stubStorage.GetStatistics();
				Assert.AreEqual<int>(0, result.Count());
			}

			[TestMethod]
			public void ShouldAllowEmptyMessages()
			{
				string code = "555";
				string wordKey = String.Empty;
				var smsService = new StubSmsVotingService();
				var stubStorage = new StubSmsVotingStorage();
				var presenter = new SmsVotingControlPresenter(stubStorage, smsService, code, 50);

				var messageStatusUpdated = new ManualResetEvent(false);
				presenter.VotingResults.CollectionChanged += (sender, e) =>
				{
					messageStatusUpdated.Set();
					presenter.Unload();
				};
				presenter.Listen();

				smsService.Send(new SmsMessage(new List<PhoneNumber>() { new PhoneNumber(code) }, wordKey));

				messageStatusUpdated.WaitOne(TimeSpan.FromMilliseconds(200));

				IEnumerable<KeyValuePair<string, int>> result = stubStorage.GetStatistics();
				Assert.AreEqual<int>(1, result.Count());
			}
		}
	}

	internal class StubSmsVotingService : ISmsService
	{
		private List<InboundSms> _inboundSms = new List<InboundSms>();

		public Task<SmsMessage> Send(SmsMessage sms)
		{
			var msgId = new Guid().ToString();
			foreach (var phoneNumber in sms.PhoneNumbers)
			{
				_inboundSms.Add(new InboundSms(msgId, phoneNumber, sms.Body));
			}
			sms.MessageId = msgId;

			return Task.FromResult(sms);
		}	  

		public Task<MessageDeliveryStatus> GetSmsStatus(string smsId)
		{
			return Task.FromResult(MessageDeliveryStatus.DeliveredToTerminal);
		}

		public async Task<IEnumerable<InboundSms>> GetInboundSmsMessages(string shortCode)
		{
			var result = new List<InboundSms>();
			for (int i = _inboundSms.Count - 1; i >= 0; i--)
			{
				InboundSms sms = _inboundSms[i];
				if (sms.SenderNumber.Number == shortCode)
				{
					result.Add(sms);
					_inboundSms.RemoveAt(i);
				}
			}

			await Task.Delay(10);
			return result;
		}
	}

	internal class StubSmsVotingStorage : ISmsVotingStorage
	{
		private Dictionary<string, int> _stat = new Dictionary<string, int>();

		public IEnumerable<KeyValuePair<string, int>> GetStatistics()
		{
			return _stat;
		}

		public void AddVote(string msg, int count)
		{
			if (_stat.ContainsKey(msg))
			{
				_stat[msg] = _stat[msg] + count;
			}
			else
			{
				_stat[msg] = count;
			}
		}

		public void ClearStatistics()
		{
			_stat.Clear();
		}

		public Task<IEnumerable<KeyValuePair<string, int>>> GetStatisticsAsync()
		{
			
			return Task.FromResult(GetStatistics());
		}

		public Task AddVoteAsync(string msg, int count)
		{
			return Task.Run(() => AddVote(msg, count));
		}

		public Task ClearStatisticsAsync()
		{
			return Task.Run(() => ClearStatistics());
		}

		public void AddVotes(IEnumerable<InboundSms> messages)
		{
			foreach (var msg in messages)
			{
				AddVote(msg.Body, 1);
			}
		}

		public Task AddVotesAsync(IEnumerable<InboundSms> messages)
		{
			return Task.Run(() => AddVotes(messages));
		}
	}
}
