using ATT.WP8.SampleApp.Resources;

namespace ATT.WP8.SampleApp
{
	/// <summary>
	/// Provides access to string resources.
	/// </summary>
	public class LocalizedStrings
	{
		private static AppResources _localizedResources = new AppResources();

		/// <summary>
		/// Get localized resources
		/// </summary>
		public AppResources LocalizedResources { get { return _localizedResources; } }
	}
}