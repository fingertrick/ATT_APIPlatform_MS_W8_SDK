// <copyright file="MmsGalleryControl.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml;

using ATT.Controls.Presenters;
using ATT.Services.Impl;

namespace ATT.Controls
{
    /// <summary>
    /// Control with gallery of MMS.
    /// </summary>
    public sealed class MmsGalleryControl : ControlWithPresenter
    {
		/// <summary>
		/// Creates and initializes presenter instance for MMS Gallery control.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected override PresenterBase InitializePresenter()
		{
			return new MmsGalleryControlPresenter(MmsShortCode, new LocalFileMmsStorage());
		}

        /// <summary>
        /// File name of mms gallery.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(MmsGalleryControl), new PropertyMetadata("fileName.png"));
      
        /// <summary>
        /// Gets or sets file name of mms gallery.
        /// </summary>
        public string FileName
        {
            get { return GetValue(MmsShortCodeProperty).ToString(); }
            set { SetValue(MmsShortCodeProperty, value); }
        }

        /// <summary>
        /// MMS voting short code.
        /// </summary>
        public static readonly DependencyProperty MmsShortCodeProperty = DependencyProperty.Register("MmsShortCode", typeof(string), typeof(MmsGalleryControl), new PropertyMetadata("89654123"));

        /// <summary>
        /// Gets or sets MMS voting short code.
        /// </summary>
        public string MmsShortCode
        {
            get { return GetValue(MmsShortCodeProperty).ToString(); }
            set { SetValue(MmsShortCodeProperty, value); }
        }      
    }
}
