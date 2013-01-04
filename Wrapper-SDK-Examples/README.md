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
1) Portable Wrapper library to develop great Apps and distribute on following 
platforms:

* Windows Store
* Windows Phone 8
* .NET Framework 4.5

This is the package for the Wrapper library.

2) In addition, AT&T Developer Program also provides Visual Studio Extensions for:

* Windows RT (http://visualstudiogallery.msdn.microsoft.com/1fb5b8ee-c981-4249-97a3-cccd2ecc664c)
* Windows Phone 8 (http://visualstudiogallery.msdn.microsoft.com/15f66b15-5063-46fd-bdf1-ae84ef6c6754)

These extensions can be directly downloaded from Microsoft Visual Studio Gallery
into Visual Studio IDE. For more details, please check:

https://developer.att.com/developer/include/Home/APIsTools/Docs/ToolsandPlugins/12500085

===================
API Access
===================
This release of Wrapper library provides access to the following APIs on AT&T Platform:

* SMS API
* MMS API
* Speech API

Please check http://developer.att.com/docs more details on these APIs.

===================
Using the Examples
===================

There are 3 complete Visual Studio Solutions here illustrating the use of API 
available in Wrapper SDK: 

* SendSMSApp
* SendMMSApp
* SpeechToTextApp
* SpeechToSMSApp

In each solution, update the following lines in MainPage.xaml.cs with your own
App Key and Secret Key.

            clientId = "your_att_app_key";
            clientSecret = "your_att_secret_key";

All examples are written for Windows Phone 8. You can easily adapt them to write
Windows Store Apps for Windows 8 and Windows RT.

==END==
