// <copyright file="SmsVotingControlPage.xaml.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.UI.Xaml;
using ATT.Services;

namespace ATT.SampleApp
{
	/// <summary>
	/// Page presents SMS Voting control
	/// </summary>
	public sealed partial class SmsVotingControlPage : Common.LayoutAwarePage
	{
		/// <summary>
		/// Voting storage filename
		/// </summary>
		private const string FileName = "SmsVotingStorage.xml";

		/// <summary>
		/// SMS short code
		/// </summary>
        private const string ShortCode = "your_shortcode";

		/// <summary>
		/// Voting storage instance
		/// </summary>
		private readonly ISmsVotingStorage _votingStorage = null;

		/// <summary>
		/// Default constructor.
		/// Creates voting storage instance.
		/// </summary>
		public SmsVotingControlPage()
		{
			InitializeComponent();

			_votingStorage = new ATT.Controls.SmsVotingFileStorage(FileName, ShortCode);

			controlVoting.SmsVotingStorage = _votingStorage;
			controlVoting.SmsShortCode = ShortCode;

			shortCodeValue.Text = ShortCode;
		}

		/// <summary>
		/// Handler occurs when user clicks on Clear Voting button.
		/// All voting statistics is cleared.
		/// </summary>
		private async void btnClearVoting_Click(object sender, RoutedEventArgs e)
		{
			await _votingStorage.ClearStatisticsAsync();
		}
	}
}
