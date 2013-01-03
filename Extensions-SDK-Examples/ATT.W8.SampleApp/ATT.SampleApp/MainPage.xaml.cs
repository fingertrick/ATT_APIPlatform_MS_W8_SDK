// <copyright file="MainPage.xaml.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ATT.SampleApp
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		/// <summary>
		/// Creates instance of <see cref="MainPage"/>
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void btnSMS_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SmsControlPage));
		}

		private void btnSMSVoting_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SmsVotingControlPage));
		}

		private void btnMMS_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MmsControlPage));
		}

		private void btnMMSCoupon_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MmsCouponControlPage));
		}

		private void btnSpeech_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SpeechControlPage));
		}


		private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
		{
			ApplicationDataContainer settings = ApplicationData.Current.RoamingSettings;

			if (settings.Values.ContainsKey("currentTheme"))
			{
				settings.Values.Remove("currentTheme");
			}

			settings.Values.Add("currentTheme", ((ToggleSwitch)(e.OriginalSource)).IsOn ? "dark" : "light");
		}
	}
}
