using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ATT.WP8.SpeechToTextApp.Resources;
using ATT.WP8.Controls.Utils;
using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;

namespace ATT.WP8.SpeechToTextApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SoundRecorder _soundRecorder;
        private bool _isRecording;
        
        public MainPage()
        {
            InitializeComponent();

            statusProgress.Visibility = Visibility.Collapsed;
            btnRecord.Content = "Start";
            CreateSoundRecorder();

            Unloaded += (sender, args) => _soundRecorder.Dispose();
        }

        private async void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRecording)
            {
                _isRecording = true;
                btnRecord.Content = "Stop";
                _soundRecorder.StartRecording();
            }
            else
            {
                statusProgress.Visibility = Visibility.Visible;
                _isRecording = false;
                btnRecord.Content = "Converting...";
                btnRecord.IsEnabled = false;
                _soundRecorder.StopRecording();

                clientId = "your_att_app_key";
                clientSecret = "your_att_secret_key";
                uriString = "https://api.att.com";
                try
                {
                    ContentInfo speechContentInfo = new ContentInfo();
                    speechContentInfo.Content = await _soundRecorder.GetBytes();
                    speechContentInfo.Name = _soundRecorder.FilePath;

                    SpeechService speechService = new SpeechService(new SDK.Entities.AttServiceSettings(clientId, clientSecret, new Uri(uriString)));
                    SpeechResponse speechResponse = await speechService.SpeechToText(speechContentInfo);
                    if (null != speechResponse)
                    {
                        txtSpeechOutput.Text = speechResponse.GetTranscription();
                    }
                }
                catch (Exception ex)
                {
                    txtSpeechOutput.Text = ex.Message;
                }

                statusProgress.Visibility = Visibility.Collapsed;
                btnRecord.Content = "Start";
                btnRecord.IsEnabled = true;
            }
        }

        private void CreateSoundRecorder()
        {
            try
            {
                _soundRecorder = new SoundRecorder();
                _soundRecorder.RecodingTimerStoped += RecodingTimerStoped;
            }
            catch (MicrophoneNotSupportedException ex)
            {
                txtSpeechOutput.Text = ex.Message;
            }
        }

        private void RecodingTimerStoped(object sender, EventArgs eventArgs)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => btnRecord_Click(this, new RoutedEventArgs()));
        }

        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string uriString { get; set; }
    }
}
