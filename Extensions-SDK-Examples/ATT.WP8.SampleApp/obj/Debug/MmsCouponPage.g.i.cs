﻿#pragma checksum "D:\Git_SampleAppsDev\ATT_APIPlatform_DemoApps\MSVSExtensions\ATT_APIPlatform_MS_W8_SDK\Extensions-SDK-Examples\ATT.WP8.SampleApp\MmsCouponPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D24A0113E0FD9C6D4038169884D1798B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ATT.WP8.Controls;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace ATT.WP8.SampleApp {
    
    
    public partial class MmsCouponPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal ATT.WP8.Controls.MmsButton MmsCouponButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/ATT.WP8.SampleApp;component/MmsCouponPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MmsCouponButton = ((ATT.WP8.Controls.MmsButton)(this.FindName("MmsCouponButton")));
        }
    }
}

