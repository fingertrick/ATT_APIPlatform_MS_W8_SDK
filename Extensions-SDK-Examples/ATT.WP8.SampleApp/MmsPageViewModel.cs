using System.Collections.ObjectModel;
using ATT.WP8.SDK;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// MmsPageViewModel class.
	/// </summary>
	public class MmsPageViewModel : SenderPageViewModelBase
	{
		private readonly ObservableCollection<ContentInfo> _attachments = new ObservableCollection<ContentInfo>();

		/// <summary>
		/// Gets or sets picture attachments.
		/// </summary>
		public ObservableCollection<ContentInfo> Attachments
		{
			get { return _attachments; }
		}

		protected override void ClearFields()
		{
			base.ClearFields();
			Attachments.Clear();
		}
	}
}