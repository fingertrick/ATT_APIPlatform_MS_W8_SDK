using Microsoft.Phone.Controls;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// MMS Page class
	/// </summary>
	public partial class MmsPage : PhoneApplicationPage
	{
		private readonly MmsPageViewModel _viewModel = new MmsPageViewModel();

		/// <summary>
		/// Creates instance of MmsPage
		/// </summary>
		public MmsPage()
		{
			InitializeComponent();
			Loaded += delegate { DataContext = _viewModel; };
		}
	}
}