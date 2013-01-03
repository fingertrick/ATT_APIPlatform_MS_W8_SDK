// <copyright file="MessageDeliveryListener.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using ATT.Services.Impl.WeakEvent;
using ATT.Utility;

namespace ATT.Services.Impl.Delivery
{
	/// <summary>
	/// Handles message delivery status polling
	/// </summary>
	public abstract class MessageDeliveryListener
	{
		private WeakEventDispatcher<MessageStatusEventArgs> _statusChangeDispatcher;

		/// <summary>
		/// is raised whenever an status changed
		/// </summary>
		public event EventHandler<MessageStatusEventArgs> StatusChangedEvent
		{
			add { _statusChangeDispatcher += value; }
			remove { _statusChangeDispatcher -= value; }
		}

		private OutboundMessage _message;
		private readonly TimeSpan _pollPeriod;
		private readonly TimeSpan _timeout;
		private volatile bool _listen;

		private MessageDeliveryStatus _messageStatus = MessageDeliveryStatus.DeliveredToNetwork;

		/// <summary>
		/// Creates new instance of <see cref="MessageDeliveryListener"/>
		/// </summary>
		/// <param name="message">message to be listened.</param>
		/// <param name="pollPeriod">time interval between delivery status polls</param>
		/// <param name="timeout">if delivery status is not changed after this time interval then listener will stop polling</param>
		/// <exception cref="System.ArgumentNullException">message is null or pollPeriod is null or timeout is null</exception>
		protected MessageDeliveryListener(
			OutboundMessage message,
			TimeSpan pollPeriod,
			TimeSpan timeout)
		{
			Argument.ExpectNotNull(() => message);
			Argument.ExpectNotNull(() => pollPeriod);
			Argument.ExpectNotNull(() => timeout);

			_message = message;
			_pollPeriod = pollPeriod;
			_timeout = timeout;
		}

		/// <summary>
		/// Gets message that is listened.
		/// </summary>
		public OutboundMessage Message
		{
			get
			{
				return _message;
			}
		}

		/// <summary>
		/// Starts listener by beginning to poll message delivery status.
		/// </summary>
		public void ListenForMessageDelivered()
		{
			var cancellationTokenSource = new CancellationTokenSource(_timeout);
			_listen = true;
			Listen(cancellationTokenSource.Token);
		}

		/// <summary>
		/// Stops listener.
		/// </summary>
		public void Stop()
		{
			_listen = false;
		}

		private async void Listen(CancellationToken ct)
		{
			while (_listen)
			{
				try
				{
					try
					{
						_messageStatus = await PollMessageStatus();

						if (_messageStatus != MessageDeliveryStatus.DeliveredToNetwork)
						{
							OnStatusChangeEvent(new MessageStatusEventArgs(_messageStatus, _message));
							return;
						}
					}
					catch (Exception ex)// if there were any exceptions then skip this iteration to try again
					{
						System.Diagnostics.Debug.WriteLine(ex);
					}

					ct.ThrowIfCancellationRequested();

					await Task.Delay(_pollPeriod);
				}
				catch (OperationCanceledException)
				{
					OnStatusChangeEvent(new MessageStatusEventArgs(MessageDeliveryStatus.Error, _message));
					Stop();
				}
			}
		}

		/// <summary>
		/// Gets poll message status.
		/// </summary>
		/// <returns>Poll message status.</returns>
		protected abstract Task<MessageDeliveryStatus> PollMessageStatus();

		/// <summary>
		/// Handler for _messageStatus's StatusChanged Event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStatusChangeEvent(MessageStatusEventArgs e)
		{
			if (_statusChangeDispatcher != null)
			{
				_statusChangeDispatcher.Invoke(this, e);
			}
		}
	}
}
