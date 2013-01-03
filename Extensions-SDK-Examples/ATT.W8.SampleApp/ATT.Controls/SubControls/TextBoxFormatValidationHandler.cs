// <copyright file="TextBoxFormatValidationHandler.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using ATT.Services.Impl;
using ATT.Controls.Utility;

namespace ATT.Controls.SubControls
{
	/// <summary>
	/// Validation handler class for TextBox
	/// </summary>
	public class TextBoxFormatValidationHandler
	{
		private TextBox _textBox;

		internal void Detach()
		{
			_textBox = null;
			_textBox.TextChanged -= OnTextBoxTextChanged;
		}

		internal void Attach(TextBox textBox)
		{
			if (_textBox == textBox)
			{
				return;
			}

			if (_textBox != null)
			{
				Detach();
			}

			_textBox = textBox;
			_textBox.TextChanged += OnTextBoxTextChanged;

			Validate();
		}

		internal void Validate()
		{
			var format = TextBoxValidationExtensions.GetFormat(_textBox);

			var expectNonEmpty = format.HasFlag(ValidTextBoxFormats.NonEmpty);
			var isEmpty = String.IsNullOrWhiteSpace(_textBox.Text);

			if (expectNonEmpty && isEmpty)
			{
				MarkInvalid();
				return;
			}

			var expectNumber = format.HasFlag(ValidTextBoxFormats.Numeric);

			if (expectNumber &&
				!isEmpty &&
				!IsNumeric())
			{
				MarkInvalid();
				return;
			}

			var expectPhone = format.HasFlag(ValidTextBoxFormats.Phone);
			if (expectPhone)
			{
				if (!isEmpty)
				{
					IEnumerable<string> phones = PhoneNumbersInputParser.Parse(_textBox.Text);

					bool isValid = phones.All(n => PhoneNumberIsdnFormatValidator.Validate(n));

					if (isValid)
					{
						MarkValid();
					}
					else
					{
						MarkInvalid();
					}
				}
				else
				{
					MarkInvalid();
				}

				return;
			}

			MarkValid();
		}

		private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			Validate();
		}

		private bool IsNumeric()
		{
			double number;
			return double.TryParse(_textBox.Text, out number);
		}

		/// <summary>
		/// Mark text box as text box with valid entered value.
		/// </summary>
		protected virtual void MarkValid()
		{
			var brush = TextBoxValidationExtensions.GetValidBrush(_textBox);
			_textBox.Background = brush;
			TextBoxValidationExtensions.SetIsValid(_textBox, true);
		}

		/// <summary>
		/// Mark text box as text box with invalid entered value.
		/// </summary>
		protected virtual void MarkInvalid()
		{
			var brush = TextBoxValidationExtensions.GetInvalidBrush(_textBox);
			_textBox.Background = brush;
			TextBoxValidationExtensions.SetIsValid(_textBox, false);
		}
	}
}
