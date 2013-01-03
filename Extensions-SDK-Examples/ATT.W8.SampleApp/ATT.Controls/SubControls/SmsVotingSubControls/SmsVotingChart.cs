// <copyright file="SmsVotingChart.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ATT.Controls.SubControls.SmsVotingSubControls
{
	/// <summary>
	/// Represent chart for <see cref="SmsVotingControl"/>
	/// </summary>
	public sealed class SmsVotingChart : Control
	{
		#region Dependency properties

		#region public IEnumerable ItemsSource
		/// <summary>
		/// Gets or sets a collection used to contain the data points of the Series.
		/// </summary>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Identifies the ItemsSource dependency property.
		/// </summary>	 
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SmsVotingChart), new PropertyMetadata(null, OnItemsSourceChanged));

		/// <summary>
		/// ItemsSourceProperty property changed callback.
		/// </summary>
		/// <param name="o">Series for which the ItemsSource changed.</param>
		/// <param name="e">Event arguments.</param>
		private static void OnItemsSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((SmsVotingChart)o).OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
		}

		/// <summary>
		/// Called when the ItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the ItemsSource property.</param>
		/// <param name="newValue">New value of the ItemsSource property.</param>
		private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			// Remove handler for oldValue.CollectionChanged (if present)
			var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;
			if (null != oldValueINotifyCollectionChanged)
			{
				oldValueINotifyCollectionChanged.CollectionChanged -= BoundCollectionChanged;
				foreach (object obj in oldValue)
				{
					UnsubscribeNotifyPropertyChanged(obj);
				}
			}

			// Add handler for newValue.CollectionChanged (if possible)
			var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
			if (null != newValueINotifyCollectionChanged)
			{
				newValueINotifyCollectionChanged.CollectionChanged += BoundCollectionChanged;
				foreach (object obj in newValue)
				{
					SubscribeNotifyPropertyChanged(obj);
				}
			}

			Draw();
		}


		/// <summary>
		/// Handles events which are raised when the bound collection changes (i.e. items added/removed)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (null != e.OldItems)
			{
				foreach (object obj in e.OldItems)
				{
					UnsubscribeNotifyPropertyChanged(obj);
				}
			}

			if (null != e.NewItems)
			{
				foreach (object obj in e.NewItems)
				{
					SubscribeNotifyPropertyChanged(obj);
				}
			}

			Draw();
		}

		private void ElementChanged(object sender, PropertyChangedEventArgs e)
		{
			Draw();
		}

		private void UnsubscribeNotifyPropertyChanged(object obj)
		{
			var objINotifyPropertyChanged = obj as INotifyPropertyChanged;
			if (null != objINotifyPropertyChanged)
			{
				objINotifyPropertyChanged.PropertyChanged -= ElementChanged;
			}
		}

		private void SubscribeNotifyPropertyChanged(object obj)
		{
			var objINotifyPropertyChanged = obj as INotifyPropertyChanged;
			if (null != objINotifyPropertyChanged)
			{
				objINotifyPropertyChanged.PropertyChanged += ElementChanged;
			}
		}

		#endregion public IEnumerable ItemsSource

		/// <summary>
		/// Style for textBox with open file path
		/// </summary>
		public static readonly DependencyProperty DarkBarBrushProperty = DependencyProperty.Register("DarkBarBrush", typeof(Brush), typeof(SmsVotingChart), new PropertyMetadata(null));

		/// <summary>
		/// Style for textBox with open file path
		/// </summary>
		public static readonly DependencyProperty LightBarBrushProperty = DependencyProperty.Register("LightBarBrush", typeof(Brush), typeof(SmsVotingChart), new PropertyMetadata(null));

		/// <summary>
		/// Style for textBox with open file path
		/// </summary>
		public static readonly DependencyProperty ForegroundBrushProperty = DependencyProperty.Register("ForegroundBrush", typeof(Brush), typeof(SmsVotingChart), new PropertyMetadata(null));

		#endregion

		/// <summary>
		/// Gets or sets style for open file path textBox
		/// </summary>
		public Brush DarkBarBrush
		{
			get { return (Brush)GetValue(DarkBarBrushProperty); }
			set { SetValue(DarkBarBrushProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for open file path textBox
		/// </summary>
		public Brush LightBarBrush
		{
			get { return (Brush)GetValue(LightBarBrushProperty); }
			set { SetValue(LightBarBrushProperty, value); }
		}

		/// <summary>
		/// Gets or sets style for open file path textBox
		/// </summary>
		public Brush ForegroundBrush
		{
			get { return (Brush)GetValue(ForegroundBrushProperty); }
			set { SetValue(ForegroundBrushProperty, value); }
		}

		private const double MinBarWidth = 40;
		private const double MaxBarWidth = 120;
		private const double StepBarWidth = 10;
		private double _barWidth;
		private const double DefaultSpaceBetweenBars = 40;
		private double _spaceBetweenBars;
		private const double DistanceBetweenLines = 30;

		private const double DefaultLeft = 5;
		private double _left = 5;
		private double _top/* = 0*/;

		private const double TopMargin = 10;
		private const double TopLabelMargin = 5;
		private const double LeftMargin = 0;
		private const double BottomMargin = 0;
		private const double RightMargin = 0;
		private const double HorizontalGridLineThickness = 0.3;

		private const double DefaultFontSize = 9;			

		private bool _loaded/* = false*/;

		private Canvas _chartArea;
		private Canvas _labelArea;

		/// <summary>
		/// Creates instance of <see cref="SmsVotingChart"/>
		/// </summary>
		public SmsVotingChart()
		{
			Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("ms-appx:///ATT.Controls/Themes/Styles/Default.xaml") });
			DefaultStyleKey = typeof(SmsVotingChart);

			Loaded += (o, e) => { ChartLoaded(); };
		}

		#region Private

		private void ChartLoaded()
		{	   
			_labelArea = GetTemplateChild("labelArea") as Canvas;
			_chartArea = GetTemplateChild("chartArea") as Canvas;

			_chartArea.Loaded += (o, e) => { ChildLoaded(); };
		}

		private void ChildLoaded()
		{
			_loaded = true;
			_labelArea.Width = ActualWidth;
			_chartArea.Width = ActualWidth;
			Draw();
		}

		private void DrawHorizontalGrid()
		{
			for (int i = 0; i < _chartArea.ActualHeight / DistanceBetweenLines; i++)
			{
				DrawHorizontalGridLine((i + 1) * DistanceBetweenLines);
			}
		}

		private void DrawHorizontalGridLine(double y)
		{
			var gridLine = new Line
							   {
								   X1 = 0,
								   Y1 = y,
								   X2 = double.IsNaN(_chartArea.Width) ? _chartArea.ActualWidth : _chartArea.Width,
								   Y2 = y,
								   StrokeThickness = HorizontalGridLineThickness,
								   Stroke = new SolidColorBrush(Colors.Black)
							   };

			_chartArea.Children.Add(gridLine);
		}

		private double MaxValue
		{
			get
			{
				return (ItemsSource as IEnumerable<Bar>).Max(e => e.Value);
			}
		}

		private void Draw()
		{
			if (!_loaded)
				return;

			int itemsCount = (ItemsSource as IEnumerable<Bar>).Count();

			double freeArea = _chartArea.Width - LeftMargin - RightMargin;
			double widthPerItem = freeArea / itemsCount;
			_barWidth = MaxBarWidth;
			_spaceBetweenBars = (widthPerItem - _barWidth);
			while ((freeArea - _barWidth * itemsCount) / (itemsCount + 1) < _barWidth && _barWidth != MinBarWidth)
			{
				_barWidth -= StepBarWidth;
				_spaceBetweenBars = (freeArea - _barWidth * itemsCount) / (itemsCount + 1);
			}

			if (_barWidth == MinBarWidth)
			{
				_spaceBetweenBars = DefaultSpaceBetweenBars;
				double newWidth = itemsCount * (MinBarWidth + _spaceBetweenBars) + LeftMargin + RightMargin + DefaultSpaceBetweenBars;
				if (newWidth > _chartArea.Width)
				{
					UpdateWidth(newWidth);
				}
			}
			if (itemsCount == 1)
				_spaceBetweenBars = _spaceBetweenBars / 2;

			_chartArea.Children.Clear();
			_labelArea.Children.Clear();
			_left = DefaultLeft;

			DrawHorizontalGrid();

			int i = 1;
			foreach (Bar bar in ItemsSource)
			{
				DrawXAxisLabel(bar);
				DrawVerticalBar(bar, i % 2 == 0);
				i++;
			}
		}

		private void UpdateWidth(double newWidth)
		{		   
			_chartArea.Width = newWidth;
			_labelArea.Width = newWidth;
		}

		private void DrawXAxisLabel(Bar bar)
		{
			var markText = new TextBlock
							   {
								   Text = bar.Label,
								   Width = _barWidth,
								   HorizontalAlignment = HorizontalAlignment.Stretch,
								   TextWrapping = TextWrapping.Wrap,
								   Foreground = ForegroundBrush,
								   TextAlignment = TextAlignment.Center,
								   FontSize = DefaultFontSize,
								   FontFamily = new FontFamily("Segoe UI Semilight")
							   };

			_labelArea.Children.Add(markText);
			Canvas.SetTop(markText, TopLabelMargin);
			Canvas.SetLeft(markText, _left + _spaceBetweenBars + LeftMargin);
		}

		private void DrawVerticalBar(Bar bar, bool isOdd)
		{
			// Calculate bar value.
			double realHeight = _chartArea.ActualHeight > 0 ? ((bar.Value * 100 / MaxValue)) * (_chartArea.ActualHeight - BottomMargin - TopMargin) / 100 : 0;


			var rect = new Rectangle();

			SetVerticalBarAttributes(realHeight, rect);

			rect.Fill = FillBrush(isOdd);

			// Calculate bar top and left position.
			_top = (_chartArea.ActualHeight - BottomMargin) - rect.Height;
			Canvas.SetTop(rect, _top);
			Canvas.SetLeft(rect, _left + _spaceBetweenBars + LeftMargin);

			rect.Tag = bar;
			_chartArea.Children.Add(rect);

			_left = _left + _barWidth + _spaceBetweenBars;
		}

		private void SetVerticalBarAttributes(double realHeight, Rectangle rect)
		{
			rect.Width = _barWidth;
			rect.Height = realHeight;
			rect.HorizontalAlignment = HorizontalAlignment.Left;
			rect.VerticalAlignment = VerticalAlignment.Center;
			rect.StrokeThickness = 1;
		}

		private Brush FillBrush(bool isOdd)
		{
			if (isOdd)
			{
				return LightBarBrush;
			}
			return DarkBarBrush;
		}

		#endregion
	}
}
