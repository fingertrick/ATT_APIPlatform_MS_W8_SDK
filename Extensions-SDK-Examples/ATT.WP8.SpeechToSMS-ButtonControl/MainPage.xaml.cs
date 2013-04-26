using ATT.WP8.Controls.Utils;
using ATT.WP8.SDK;
using ATT.WP8.SDK.Entities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ATT.WP8.SpeechToSMS
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SoundRecorder _soundRecorder;
        private bool _isRecording;

        PhoneNumberChooserTask _phoneNumberChooserTask;

        public MainPage()
        {
            InitializeComponent();
            
            statusProgress.Visibility = Visibility.Collapsed;
            btnRecord.Content = "Start";
            CreateSoundRecorder();

            Unloaded += (sender, args) => _soundRecorder.Dispose();

            _phoneNumberChooserTask = new PhoneNumberChooserTask();
            _phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
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
                
                //statusProgress.Visibility = Visibility.Visible;
                _isRecording = false;
                btnRecord.IsEnabled = false;
                _soundRecorder.StopRecording();

                try
                {
                    btnSpeech.AudioContent = await _soundRecorder.GetBytes();
                    btnSpeech.AudioContentName = _soundRecorder.FilePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

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
                MessageBox.Show(ex.Message);
            }
        }

        private void RecodingTimerStoped(object sender, EventArgs eventArgs)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => btnRecord_Click(this, new RoutedEventArgs()));
        }

        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string uriString { get; set; }

        private void txtPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            lblPhoneNumber.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                lblPhoneNumber.Visibility = Visibility.Visible;
            }
        }

        private void txtPhoneNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            lblPhoneNumber.Visibility = Visibility.Collapsed;
        }

        private void btnContacts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _phoneNumberChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred." + ex.Message);
            }
        }

        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                string phoneNumber = string.Empty;
                foreach (char ch in e.PhoneNumber)
                {
                    if (Char.IsDigit(ch))
                    {
                        phoneNumber += ch;
                    }
                }

                if (string.IsNullOrEmpty(txtPhoneNumber.Text))
                {
                    txtPhoneNumber.Text = phoneNumber;
                }
                else
                {
                    txtPhoneNumber.Text += ";" + phoneNumber;
                }

                lblPhoneNumber.Visibility = Visibility.Collapsed;
            }
        }

        private async void btnSendSMS_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            if (string.IsNullOrEmpty(txtSpeechOutput.Text))
            {
                txtResult.Text = "Specify message";
                return;
            }

            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                txtResult.Text = "Specify Phone Number";
                return;
            }

            statusProgress.Visibility = Visibility.Visible;
            txtResult.Visibility = Visibility.Collapsed;
            btnSendSMS.IsEnabled = false;
            btnRecord.IsEnabled = false;
            btnSpeech.IsEnabled = false;

            try
            {
                clientId = "your_att_app_key";
                clientSecret = "your_att_secret_key";
                uriString = "https://api.att.com";

                SmsService smsService = new SmsService(new AttServiceSettings(clientId, clientSecret, new Uri(uriString)));
                List<string> phoneNumbers = txtPhoneNumber.Text.Split(';').ToList<string>();
                SmsResponse response = await smsService.SendSms(phoneNumbers, txtSpeechOutput.Text);
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
            btnRecord.IsEnabled = true;
            btnSpeech.IsEnabled = true;
        }

        private void btnSpeech_Click(object sender, RoutedEventArgs e)
        {
            btnSendSMS.IsEnabled = false;
            btnRecord.IsEnabled = false;
            statusProgress.Visibility = Visibility.Visible;
        }

        private void btnSpeech_MessageTranscripted(object sender, Controls.TranscriptedMessageEventArgs e)
        {
            statusProgress.Visibility = Visibility.Collapsed;
            txtSpeechOutput.Text = btnSpeech.TranscriptedText;
            btnSendSMS.IsEnabled = true;
            btnRecord.IsEnabled = true;
        }

        private void btnSpeech_Error(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                MessageBox.Show(exception.Message);
            }

            btnSendSMS.IsEnabled = true;
            btnRecord.IsEnabled = true;
            statusProgress.Visibility = Visibility.Collapsed;
        }       
    }
}