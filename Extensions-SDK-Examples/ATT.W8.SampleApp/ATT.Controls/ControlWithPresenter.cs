// <copyright file="ControlBase.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using Windows.UI.Xaml;
using ATT.Controls.Presenters;

namespace ATT.Controls
{
	/// <summary>
	/// Abstract base class all ATT controls will inherit from; adds a presenter (gui) instance.
	/// </summary>
	public abstract class ControlWithPresenter : AttControl
	{
		/// <summary>
		/// Presenter instance for control.
		/// Note: We cannot make this generic on presenter type because VS does not handle generic controls properly during design-time
		/// </summary>
		protected PresenterBase Presenter
		{
			get { return DataContext as PresenterBase; }
		}

		/// <summary>
		/// This constructor creates an instance of base class for AT&amp;T SDK Extension Controls.
		/// </summary>
		protected ControlWithPresenter()
		{
			Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("ms-appx:///ATT.Controls/Themes/Styles/Default.xaml") });

			DefaultStyleKey = this.GetType();

			if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				SizeChanged += (o, e) =>
				{
					Width = e.NewSize.Width;
					Height = e.NewSize.Height;
				};

				Loaded += (o, e) =>
				{
					DataContext = InitializePresenter();
				};

				Unloaded += (o, e) =>
				{
					if (Presenter != null)
					{
						Presenter.Unload();
					}
				};
			}
		}

		/// <summary>
		/// Creates and initializes presenter instance.
		/// </summary>
		/// <returns>Returns created presenter instance.</returns>
		protected abstract PresenterBase InitializePresenter();
	}
}
