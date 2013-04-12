using ATT.WP8.SDK;
namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// MmsCouponPageViewModel class.
	/// </summary>
	public class MmsCouponPageViewModel : SenderPageViewModelBase
	{
		private ContentInfo _file;

		/// <summary>
		/// Gets or sets selected coupon picture.
		/// </summary>
		public ContentInfo File
		{
			get { return _file; }
			set
			{
				if (_file != value)
				{
					_file = value;
					OnPropertyChanged(() => File);
				}
			}
		}

		protected override void ClearFields()
		{
			base.ClearFields();
			File = null;
		}
	}
}