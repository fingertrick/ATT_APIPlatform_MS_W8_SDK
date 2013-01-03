// <copyright file="PresenterBase.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using ATT.Controls.Utility;

namespace ATT.Controls.Presenters
{
	/// <summary>
	/// Base class for all presenters
	/// </summary>
	public abstract class PresenterBase : NotifyPropertyChangedBase
	{
		/// <summary>
		/// Delegate used for ClearCommand in MMS controls.
		/// </summary>
		public delegate RelayCommand RelayCommandHandler();

		private bool _isNewMessageEnabled = true;
		private bool _isEditableEnabled = true;
		private string _errorMessage = String.Empty;

		/// <summary>
		/// Handles exception and displays it's text in message dialog.
		/// </summary>
		/// <param name="e">Occurred exception</param>
		protected void HandleException(Exception e)
		{
			ErrorMessage = e.Message;
		}

		/// <summary>
		/// Gets or sets enabled/disabled status for NewMessage button
		/// </summary>
		public bool IsNewMessageEnabled
		{
			get
			{
				return _isNewMessageEnabled;
			}
			protected set
			{
				if (_isNewMessageEnabled != value)
				{
					_isNewMessageEnabled = value;
					OnPropertyChanged(() => IsNewMessageEnabled);
				}
			}
		}

		/// <summary>
		/// Gets or sets enabled/disabled status for edit
		/// </summary>
		public bool IsEditableEnabled
		{
			get
			{
				return _isEditableEnabled;
			}
			protected set
			{
				if (_isEditableEnabled != value)
				{
					_isEditableEnabled = value;
					OnPropertyChanged(() => IsEditableEnabled);
				}
			}
		}

		/// <summary>
		/// Gets or sets error message
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			protected set
			{
				if (_errorMessage != value)
				{
					_errorMessage = value;
					OnPropertyChanged(() => ErrorMessage);
				}
			}
		}

		/// <summary>
		/// Unload presenter. Release resources which was used in presenter
		/// </summary>
		public abstract void Unload();
	}
}
