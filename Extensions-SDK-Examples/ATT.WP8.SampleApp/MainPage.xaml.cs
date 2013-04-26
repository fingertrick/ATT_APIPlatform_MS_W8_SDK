using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Main page
	/// </summary>
	public partial class MainPage : PhoneApplicationPage
	{
		/// <summary>
		/// Creates instance for MainPage
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}

		private void btnSpeechControl_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("SpeechControlPage", UriKind.RelativeOrAbsolute));
		}

		private void btnSpeech_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("SpeechButtonPage", UriKind.RelativeOrAbsolute));
		}

		private void btnMms_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("MmsPage", UriKind.RelativeOrAbsolute));
		}

		private void btnMmsCoupon_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("MmsCouponPage", UriKind.RelativeOrAbsolute));
		}

		private void btnSms_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("SmsPage", UriKind.RelativeOrAbsolute));
		}
	}
}