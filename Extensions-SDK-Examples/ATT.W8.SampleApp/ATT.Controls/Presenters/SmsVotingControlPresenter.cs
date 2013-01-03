// <copyright file="SmsVotingControlPresenter.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ATT.Controls.SubControls.SmsVotingSubControls;
using ATT.Controls.Utility;
using ATT.Services;
using ATT.Utility;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Presenter for <see cref="SmsVotingControl"/>
	/// </summary>
	public class SmsVotingControlPresenter : PresenterBase
	{
		private readonly ISmsVotingStorage _smsVotingStorage;
		private readonly ISmsService _smsService;

		private readonly string _shortCode;
		private readonly int _smsPollInterval;
		private ObservableCollection<Bar> _votingResults = new ObservableCollection<Bar>();

		private volatile bool _stopListen/* = false*/;

		/// <summary>
		/// Creates instance of <see cref="SmsVotingControlPresenter"/>
		/// </summary>
		/// <param name="smsVotingStorage">storage used to store incoming SMS messages</param>
		/// <param name="smsService">SMS service implementation</param>
		/// <param name="shortCode">short code to be used for getting incoming SMS messages</param>
		/// <param name="smsPollInterval">interval between SMS messages polling</param>
		public SmsVotingControlPresenter(ISmsVotingStorage smsVotingStorage, ISmsService smsService, string shortCode, int smsPollInterval = 500)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => shortCode);
			Argument.ExpectNotNull(() => smsVotingStorage);
			Argument.ExpectNotNull(() => smsService);
			Argument.Expect(() => smsPollInterval > 0, "smsPollInterval", "smsPollInterval should be positive");

			_shortCode = shortCode;
			_smsVotingStorage = smsVotingStorage;
			_smsService = smsService;
			_smsPollInterval = smsPollInterval;

			UpdateCommand = new RelayCommand(UpdateVotingStatistics);
		}

		/// <summary>
		/// Gets or sets status of message
		/// </summary>
		public ObservableCollection<Bar> VotingResults
		{
			get
			{
				return _votingResults;
			}
			protected set
			{
				_votingResults = value;
			}
		}

		/// <summary>
		/// Gets or sets command for sending message
		/// </summary>
		public RelayCommand UpdateCommand { get; protected set; }

		/// <summary>
		/// Constantly gets inbound SMS messages from server stopping for configured interval of time
		/// </summary>
		public async void Listen()
		{
			try
			{
				await UpdateStatistics();

				if (!_stopListen)
				{
					await Task.Delay(_smsPollInterval);
					Listen();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private async Task UpdateStatistics()
		{
			ErrorMessage = String.Empty;
			var messages = await _smsService.GetInboundSmsMessages(_shortCode);

			await _smsVotingStorage.AddVotesAsync(messages);

			IEnumerable<KeyValuePair<string, int>> statistics = await _smsVotingStorage.GetStatisticsAsync();
			if (VotingResults.Count > 0)
			{
				VotingResults.Clear();
			}
			foreach (var vote in statistics)
			{
				VotingResults.Add(new Bar(vote.Key, vote.Value));
			}
		}

		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public override void Unload()
		{
			_stopListen = true;
		}

		private async void UpdateVotingStatistics(object parameter)
		{
			try
			{
				await UpdateStatistics();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	}
}
