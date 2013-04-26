using Microsoft.Phone.Controls;
using System.Windows;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Mms Coupon Page Class.
	/// </summary>
	public partial class MmsCouponPage : PhoneApplicationPage
	{
		private readonly MmsCouponPageViewModel _viewModel = new MmsCouponPageViewModel();

		/// <summary>
		/// Creates new instance of <see cref="MmsCouponPage"/>
		/// </summary>
		public MmsCouponPage()
		{
			InitializeComponent();
			Loaded += delegate { DataContext = _viewModel; };
		}

        private void MmsCouponButton_MessageStatusChanged(object sender, WP8.Controls.Messages.MessageStatusEventArgs e)
        {
            
        }
	}
}