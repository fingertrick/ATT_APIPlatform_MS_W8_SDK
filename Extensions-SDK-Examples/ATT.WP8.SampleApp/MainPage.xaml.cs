using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace ATT.WP8.SampleApp
{
	public partial class MainPage : PhoneApplicationPage
	{
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
	}
}