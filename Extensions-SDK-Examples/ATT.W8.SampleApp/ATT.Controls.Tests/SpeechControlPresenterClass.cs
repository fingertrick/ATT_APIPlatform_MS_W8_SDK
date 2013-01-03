// <copyright file="SpeechControlPresenterClass.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;
using ATT.Controls.Presenters;
using ATT.Services;

namespace ATT.Controls.Tests
{
	[TestClass]
	public sealed class SpeechControlPresenterClass
	{
		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class CanSendSpeechProperty
		{
			[TestMethod]
			public void ShouldBeFalseWhenControlInitialized()
			{
				var presenter = new SpeechControlPresenter(new StubSpeechService());
				Assert.IsFalse(presenter.CanSendSpeech);
			}

			[TestMethod]
			public void ShouldBeTrueWhenFilePathAreSpecified()
			{
				var presenter = new SpeechControlPresenter(new StubSpeechService());
				presenter.File = GetTestFile();
				Assert.IsTrue(presenter.CanSendSpeech);
			}
		}

		[TestClass]
		// Ignore CodeIt.Right rule for this line
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public class SendSpeechCommand
		{
			[TestMethod]
			public void ShouldNotBeExecutableBeforeFilePathAreSpecified()
			{
				var presenter = new SpeechControlPresenter(new StubSpeechService());
				Assert.IsFalse(presenter.SendSpeech.CanExecute(null));
			}

			[TestMethod]
			public void ShouldBeExecutableAfterFilePathAreSpecified()
			{
				var presenter = new SpeechControlPresenter(new StubSpeechService());
				presenter.File = GetTestFile();
				Assert.IsTrue(presenter.SendSpeech.CanExecute(null));
			}

			[TestMethod]
			public void ShouldSendSpeech()
			{
				var srv = new StubSpeechService();
				var presenter = new SpeechControlPresenter(srv);
				presenter.File = GetTestFile();
				presenter.SendSpeech.Execute(null);

				Assert.IsTrue(srv.SendMethodWasCalled);
			}
		}

		private static StorageFile GetTestFile()
		{
			var folder = ApplicationData.Current.LocalFolder;
			var option = CreationCollisionOption.GenerateUniqueName;
			Windows.Foundation.IAsyncOperation<StorageFile> operation = folder.CreateFileAsync("test", option);
			Task<StorageFile> task = operation.AsTask<StorageFile>();
			task.Wait(1000);
			StorageFile file = task.Result;
			return file;
		}
	}

	internal class StubSpeechService : ISpeechService
	{
		public StorageFile SpeechSent;
		public bool SendMethodWasCalled;

		/// <summary>
		/// Sends speech message.
		/// </summary>
		/// <param name="speech">Instance of <see cref="StorageFile"/> to be sent.</param>
		/// <returns>Transcript text (instance of <see cref="SpeechResponse"/>)</returns>
		public Task<SpeechResponse> Send(StorageFile speech)
		{
			SendMethodWasCalled = true;
			SpeechSent = speech;
			var task = new Task<SpeechResponse>(() => new SpeechResponse("Result"));
			task.Start();
			return task;
		}
	}
}
