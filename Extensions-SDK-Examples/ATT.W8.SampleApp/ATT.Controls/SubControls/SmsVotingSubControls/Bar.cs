// <copyright file="Bar.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.ComponentModel;

namespace ATT.Controls.SubControls.SmsVotingSubControls
{
	/// <summary>
	/// Class represent Bar on <see cref="SmsVotingChart"/>
	/// </summary>
	public class Bar : INotifyPropertyChanged
	{
		private string _label;	  
		private double _value;

		/// <summary>
		/// Creates instance of <see cref="Bar"/>
		/// </summary>
		/// <param name="label">Label text</param>
		/// <param name="value">Value</param>
		public Bar(string label, double value)
		{
			_label = label;
			_value = value;
		}

		/// <summary>
		/// Gets or sets label text
		/// </summary>
		public string Label
		{
			get 
			{ 
				return _label; 
			}
			set 
			{ 
				_label = value;
				NotifyPropertyChanged("Label");
			}
		}

		/// <summary>
		/// Gets or sets value
		/// </summary>
		public double Value
		{
			get 
			{ 
				return _value;
			}
			set
			{ 
				_value = value;
				NotifyPropertyChanged("Value");
			}
		}

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Event occurs when some property is modified.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}

		#endregion
	}
}
