// <copyright file="MmsGalleryControlPage.xaml.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ATT.SampleApp
{
    /// <summary>
    /// Page that contains MMS Gallery control.
    /// </summary>
    public sealed partial class MmsGalleryControlPage : Common.LayoutAwarePage
    {
        /// <summary>
        /// Creates instance of <see cref="MmsGalleryControlPage"/>
        /// </summary>
        public MmsGalleryControlPage()
        {
            InitializeComponent();

            controlMmsGallery.MmsShortCode = "555";
        }
    }
}
