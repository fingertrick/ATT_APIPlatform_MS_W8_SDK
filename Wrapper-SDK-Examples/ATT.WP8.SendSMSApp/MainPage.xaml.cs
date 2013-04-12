// <copyright file="ATT.WP8.SendSMSApp.MainPage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ATT.WP8.SendSMSApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();            
            txtResult.Visibility = Visibility.Collapsed;
            statusProgress.Visibility = Visibility.Collapsed;
        }

        private async void btnSendSMS_Click(object sender, RoutedEventArgs e)
        {
            statusProgress.Visibility = Visibility.Visible;
            txtResult.Visibility = Visibility.Collapsed;
            btnSendSMS.IsEnabled = false;

            try
            {
                clientId = "your_att_app_key";
                clientSecret = "your_att_secret_key";
                uriString = "https://api.att.com";

                SmsService smsService = new SmsService(new AttServiceSettings(clientId, clientSecret, new Uri(uriString)));
                List<string> phoneNumbers = txtPhone.Text.Split(';').ToList<string>();
                SmsResponse response = await smsService.SendSms(phoneNumbers, txtMessage.Text);
                if (null != response)
                {
                    txtResult.Visibility = Visibility.Visible;
                    txtResult.Text = "Message has been sent";
                }
            }
            catch (Exception ex)
            {
                txtResult.Visibility = Visibility.Visible;
                txtResult.Text = ex.Message;
            }

            statusProgress.Visibility = Visibility.Collapsed;
            btnSendSMS.IsEnabled = true;
        }

        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string uriString { get; set; }

        private void btnNewMessage_Click(object sender, RoutedEventArgs e)
        {
            txtPhone.Text = string.Empty;
            txtMessage.Text = string.Empty;
            txtResult.Visibility = Visibility.Collapsed;
            statusProgress.Visibility = Visibility.Collapsed;
        }

     
    }
}