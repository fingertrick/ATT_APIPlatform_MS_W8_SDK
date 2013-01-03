// <copyright file="RelayCommand.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Diagnostics;
using System.Windows.Input;
using ATT.Utility;
using System.Collections.Generic;

namespace ATT.Controls.Utility
{
	/// <summary>
	/// Taken from http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090048
	/// </summary>
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _commandAction;
		private readonly Func<bool> _canExecute;

		/// <summary>
		/// Creates instance of <see cref="RelayCommand"/>
		/// </summary>
		/// <param name="execute">The execution logic</param>
		/// <param name="canExecute">The execution status logic</param>
		/// <exception cref="ArgumentNullException">Thrown if the execute argument is null</exception>
		public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
		{
			Argument.ExpectNotNull(() => execute);

			_commandAction = execute;
			_canExecute = canExecute;
		}



		private List<EventHandler> _canExecuteChangedHandlers = new List<EventHandler>();


		/// <summary>
		/// Deactivates command, must be called when appropriate control/presenter is unloaded
		/// </summary>
		public void Deactivate()
		{
			foreach(var eh in _canExecuteChangedHandlers)
			{
				CommandManager.RequerySuggested -= eh;
			}
		}

		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecute != null)
				{
					CommandManager.RequerySuggested += value;
					_canExecuteChangedHandlers.Add(value);
				}
			}

			remove
			{
				if (_canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="CanExecuteChanged"/> event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">
		/// This parameter will always be ignored.
		/// </param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute();
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		/// This parameter will always be ignored.
		/// </param>
		public void Execute(object parameter)
		{
			_commandAction(parameter);
		}
	}
}