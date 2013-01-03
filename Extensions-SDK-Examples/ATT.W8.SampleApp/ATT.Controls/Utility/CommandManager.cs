// <copyright file="CommandManager.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// The command manager
	/// </summary>
	public class CommandManager
	{
		private readonly List<WeakReference> _eventHandlers = new List<WeakReference>();
		private static CommandManager _current;
		private bool _hasCanExecuteQueued;

		/// <summary>
		/// Re-query suggested.
		/// </summary>
		public static event EventHandler RequerySuggested
		{
			add
			{
				AddWeakReferenceHandler(Current._eventHandlers, value);
			}

			remove
			{
				RemoveWeakReferenceHandler(Current._eventHandlers, value);
			}
		}

		/// <summary>
		/// Gets current <see cref="CommandManager"/>
		/// </summary>
		public static CommandManager Current
		{
			get
			{
				return _current ?? (_current = new CommandManager());
			}
		}

		/// <summary>
		/// The invalidate re-query suggested.
		/// </summary>
		public static void InvalidateRequerySuggested()
		{
			Current.RaiseCanExecuteChanged();
		}

		private static void RemoveWeakReferenceHandler(List<WeakReference> weakReferences, EventHandler handler)
		{
			for (int i = weakReferences.Count - 1; i >= 0; i--)
			{
				WeakReference reference = weakReferences[i];
				var target = reference.Target as EventHandler;
				if ((target == null) || (target == handler))
				{
					weakReferences.RemoveAt(i);
				}
			}
		}

		private static void AddWeakReferenceHandler(List<WeakReference> weakReferences, EventHandler handler)
		{
			weakReferences.Add(new WeakReference(handler));
		}

		private static void CallWeakReferenceHandlers(List<WeakReference> handlers)
		{
			if (handlers != null)
			{
				var handlerArray = new EventHandler[handlers.Count];
				int index = 0;
				for (int i = handlers.Count - 1; i >= 0; i--)
				{
					WeakReference reference = handlers[i];
					var target = reference.Target as EventHandler;
					if (target == null)
					{
						handlers.RemoveAt(i);
					}
					else
					{
						handlerArray[index] = target;
						index++;
					}
				}

				for (int j = 0; j < index; j++)
				{
					EventHandler handler2 = handlerArray[j];
					handler2(null, EventArgs.Empty);
				}
			}
		}

		private async void RaiseCanExecuteChanged()
		{
			if (_hasCanExecuteQueued)
			{
				return;
			}

			_hasCanExecuteQueued = true;

			await Task.Run(() => { 
				CallWeakReferenceHandlers(_eventHandlers);
				_hasCanExecuteQueued = false; 
			});			
		}
	}
}
