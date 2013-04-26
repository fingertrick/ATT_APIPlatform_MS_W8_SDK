using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Sms page class
	/// </summary>
	public partial class SmsPage : PhoneApplicationPage
	{
		private SmsPageViewModel _smsPageViewModel = new SmsPageViewModel();

		/// <summary>
		/// Creates instance of SmsPage
		/// </summary>
		public SmsPage()
		{
			InitializeComponent();
			Loaded += (sender, args) => DataContext = _smsPageViewModel;
		}
	}
}