// <copyright file="ResourcesHelper.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml;
using ATT.Utility;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Class for working with resources
	/// </summary>
	public static class ResourcesHelper
	{
		private const string DefaultAssembly = "ATT.Controls";
		private const string DefaultPath = "Themes/Generic.xaml";
		private static readonly Dictionary<string, ResourceDictionary> _resourcesDictionaries = new Dictionary<string, ResourceDictionary>();

		/// <summary>
		/// Gets string resource from resource dictionary
		/// </summary>
		/// <param name="assembly">Name of resource assembly</param>
		/// <param name="path">Path to resource</param>
		/// <param name="name">resource name</param>
		/// <returns>resource value</returns>
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static string GetString(string assembly, string path, string name)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => assembly);
			Argument.ExpectNotNullOrWhiteSpace(() => path);
			Argument.ExpectNotNullOrWhiteSpace(() => name);

			object result = null;
			try
			{
				var key = String.Format(CultureInfo.InvariantCulture, "{0};{1}", assembly, path);
				if (!_resourcesDictionaries.ContainsKey(key))
				{
					var rs = new ResourceDictionary();
					var uri = new Uri(String.Format(CultureInfo.InvariantCulture, "ms-appx:///{0}/{1}", assembly, path));
					rs.Source = uri;
					_resourcesDictionaries.Add(key, rs);
				}

				result = _resourcesDictionaries[key][name];
			}
			catch (Exception)
			{ }

			return result != null ? result.ToString() : String.Empty;
		}

		/// <summary>
		/// Gets string resource from default resource dictionary
		/// </summary>
		/// <param name="name">Resource name</param>
		/// <returns>Resource value</returns>
		/// <exception cref="System.ArgumentNullException">name is null.</exception>
		public static string GetString(string name)
		{
			Argument.ExpectNotNullOrWhiteSpace(() => name);

			return GetString(DefaultAssembly, DefaultPath, name);
		}
	}
}
