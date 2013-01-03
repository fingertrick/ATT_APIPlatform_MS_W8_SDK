// <copyright file="ATT.WP8.SendMMSApp.MainPage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ATT.WP8.SendMMSApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoChooserTask _photoChooserTask;
        ContentInfo _mmsContent;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            txtResult.Visibility = Visibility.Collapsed;
            statusProgress.Visibility = Visibility.Collapsed;
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            _mmsContent = null;
            if (e.TaskResult == TaskResult.OK)
            {                
                _mmsContent = new ContentInfo();
                _mmsContent.Name = e.OriginalFileName;
                _mmsContent.Content = ToByteArray(e.ChosenPhoto);

                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                selectedImage.Source = bmp;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _photoChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred." + ex.Message );
            }
            
        }

        private async void btnSendMMS_Click(object sender, RoutedEventArgs e)
        {
            statusProgress.Visibility = Visibility.Visible;
            txtResult.Visibility = Visibility.Collapsed;
            btnSendMMS.IsEnabled = false;

            clientId = "your_att_app_key";
            clientSecret = "your_att_secret_key";
            uriString = "https://api.att.com";

            List<string> phoneNumbers = txtPhone.Text.Split(';').ToList<string>();
            MmsService mmsService = new MmsService(new AttServiceSettings(clientId, clientSecret, new Uri(uriString)));

            List<ContentInfo> attachments = new List<ContentInfo>();
            attachments.Add(_mmsContent);

            MmsResponse mmsResponse = await mmsService.SendMms(phoneNumbers, txtMessage.Text, attachments);

            if (null != mmsResponse)
            {
                txtResult.Visibility = Visibility.Visible;
                txtResult.Text = "Message has been sent";
            }

            statusProgress.Visibility = Visibility.Collapsed;
            btnSendMMS.IsEnabled = true;
        }

        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string uriString { get; set; }

        public byte[] ToByteArray(Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
            {
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            }

            return buffer;
        }

        private void btnNewMessage_Click(object sender, RoutedEventArgs e)
        {
            _mmsContent = null;
            selectedImage.Source = null;
            txtMessage.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtResult.Visibility = Visibility.Collapsed;
            statusProgress.Visibility = Visibility.Collapsed;
        }        
    } 
}