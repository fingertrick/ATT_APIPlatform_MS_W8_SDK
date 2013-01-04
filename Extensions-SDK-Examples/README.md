===================
Introduction
===================
The AT&T API Platform SDKs for Windows ( ‘AT&T SDKs’) combine the power of the 
Microsoft® .NET platform and AT&T API platform so that developers can quickly 
bring robust C# and VisualBasic applications to market.

Please check http://developer.att.com for more details and how you can sign up
and start using the API right away.

===================
AT&T SDKs
===================
1) AT&T SDKs include Visual Studio Extensions for:

* Windows 8 and Windows RT (http://visualstudiogallery.msdn.microsoft.com/1fb5b8ee-c981-4249-97a3-cccd2ecc664c)
* Windows Phone 8 (http://visualstudiogallery.msdn.microsoft.com/15f66b15-5063-46fd-bdf1-ae84ef6c6754)

Extensions can be directly downloaded from Microsoft Visual Studio Gallery
into Visual Studio IDE. 

2) In addition, AT&T Developer Program also provides a Portable Wrapper 
library to access AT&T APIs. This Wrapper can be used to develop great 
Windows Apps and distribute on following platforms:

* Windows Store
* Windows Phone 8
* .NET Framework 4.5

For more details, please visit:

https://developer.att.com/developer/include/Home/APIsTools/Docs/ToolsandPlugins/12500085

===================
API Access
===================

Windows 8 and Windows RT Controls can access the following APIs:

* SMS
* MMS
* Speech

Windows Phone 8 Controls can access the following APIs:

* Speech

Please check http://developer.att.com/docs more details on these APIs.

===================
Using the Examples
===================

There are 2 complete Visual Studio Solutions here illustrating the use of AT&T
Controls in this SDK: 

1) ATT.W8.SampleApp
-------------------

Update the following lines in App.xaml.cs with your own App Key and Secret Key.

	AttSettings.ApiKey = "your_att_app_key";
	AttSettings.SecretKey = "your_att_secret_key";

Update the following line in SmsVotingControlPage.xaml.cs with your own Shortcode.

	private const string ShortCode = "your_shortcode";


ATT.W8.SampleApp can run on both Windows 8 and Windows RT platforms.


2) ATT.WP8.SampleApp
--------------------

In this solution, update the following Properties of AT&T Controls with your
own Api Key and Secret Key.

	ApiKey
	SecretKey

3) ATT.WP8.SpeechToSMS-ButtonControl
------------------------------------
This is a mash-up application using SpeechButton control and SMS Wrapper API.

In this solution, update the following:

3.a) Properties of AT&T Control ('Convert' Button) with your own Api Key and 
Secret Key.

	ApiKey
	SecretKey

3.b) Update the following lines in MainPage.xaml.cs with your own App Key and
Secret Key.

        clientId = "your_att_app_key";
        clientSecret = "your_att_secret_key";

===================
Known Limitations
===================
AT&T Speech API processes .wav or .amr audio files in 8,000Hz 8-bit format 
64 Kbit/s Bitrate that is no longer than 4 minutes (approx) at a time.

==END==
