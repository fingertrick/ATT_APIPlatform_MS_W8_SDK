// <copyright file="App.xaml.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2013
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2013 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ATT.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ATT.SampleApp
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
	{
		/// <summary>
		/// Text file logger
		/// </summary>
		private static class TextLogger
		{
			/// <summary>
			/// Log file name
			/// </summary>
			private const string LogFileName = "error.log";

			/// <summary>
			/// Semaphore for writing to file from one thread simultaneously
			/// </summary>
			private static readonly SemaphoreSlim _threadSemaphore = new SemaphoreSlim(initialCount: 1);

			/// <summary>
			/// Writes exception info to log file
			/// </summary>
			/// <param name="e"></param>
			public static async void WriteLogInfo(UnhandledExceptionEventArgs e)
			{
				await _threadSemaphore.WaitAsync();
				try
				{
					StorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(LogFileName, CreationCollisionOption.OpenIfExists);
					if (e.Message != null)
					{
						await FileIO.AppendTextAsync(storageFile, String.Format("\n========{0}\n{1}", DateTime.Now.ToString("HH:mm:ss.ff"), e.Message));
					}
					if (e.Exception.StackTrace != null)
					{
						await FileIO.AppendTextAsync(storageFile, e.Exception.StackTrace);
					}

					var msg = new Windows.UI.Popups.MessageDialog(e.Message, "Unhandled Exception");
					await msg.ShowAsync();
				}
				finally
				{
					_threadSemaphore.Release();
				}
			}
		}

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;


            AttSettings.ApiKey = "your_att_app_key";
            AttSettings.SecretKey = "your_att_secret_key";

			var settings = ApplicationData.Current.RoamingSettings;
			if (settings.Values.ContainsKey("currentTheme") && (string)settings.Values["currentTheme"] == "light")
				RequestedTheme = ApplicationTheme.Light;
			else
				RequestedTheme = ApplicationTheme.Dark;

			UnhandledException += Current_UnhandledException;
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used when the application is launched to open a specific file, to display
		/// search results, and so forth.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs args)
		{
			// Do not repeat app initialization when already running, just ensure that
			// the window is active
			if (args.PreviousExecutionState == ApplicationExecutionState.Running)
			{
				Window.Current.Activate();
				return;
			}

			if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
			{
				//TODO: Load state from previously suspended application
			}

			// Create a Frame to act navigation context and navigate to the first page
			var rootFrame = new Frame();
			if (!rootFrame.Navigate(typeof(MainPage)))
			{
				throw new Exception("Failed to create initial page");
			}

			// Place the frame in the current Window and ensure that it is active
			Window.Current.Content = rootFrame;
			Window.Current.Activate();
		}

		/// <summary>
		/// Event handler for unhandled exception.
		/// </summary>
		private void Current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			TextLogger.WriteLogInfo(e);
			e.Handled = true;
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
