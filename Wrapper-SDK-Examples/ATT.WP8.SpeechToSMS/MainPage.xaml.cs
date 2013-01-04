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

            clientId = "your_att_app_key";
            clientSecret = "your_att_secret_key";
            uriString = "https://api.att.com";
            
            statusProgress.Visibility = Visibility.Collapsed;
            btnRecord.Content = "Speak";
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
                btnRecord.Content = "Go";
                _soundRecorder.StartRecording();
            }
            else
            {
                btnSendSMS.IsEnabled = false;
                statusProgress.Visibility = Visibility.Visible;
                _isRecording = false;
                btnRecord.Content = "Converting...";
                btnRecord.IsEnabled = false;
                txtSpeechOutput.IsReadOnly = true;
                _soundRecorder.StopRecording();

                //clientId = "your client id here";
                //clientSecret = "your client secret";
                try
                {
                    ContentInfo speechContentInfo = new ContentInfo();
                    speechContentInfo.Content = await _soundRecorder.GetBytes();
                    speechContentInfo.Name = _soundRecorder.FilePath;

                    SpeechService speechService = new SpeechService(new SDK.Entities.AttServiceSettings(clientId, clientSecret, new Uri(uriString)));
                    SpeechResponse speechResponse = await speechService.SpeechToText(speechContentInfo);
                    txtSpeechOutput.IsReadOnly = false;
                    if (null != speechResponse)
                    {
                        txtSpeechOutput.Text = speechResponse.GetTranscription();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnSendSMS.IsEnabled = true;
                statusProgress.Visibility = Visibility.Collapsed;
                btnRecord.Content = "Speak";
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

            try
            {
                //clientId = "your client id here";
                //clientSecret = "your client secret";

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
        }       
    }
}