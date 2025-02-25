﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BitPack.MVVM.Converter
{
	class HistoryResultToResourceKeyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = value as string;
			string property = parameter as string;

			if (result.Split()[0] == "Surrendered") result = result.Split()[1];
			if (result == "Win" || result == "Loss")
			{
				if (property == "Foreground") return Application.Current.FindResource("White");
				if (property == "Background") return Application.Current.FindResource(result + "Color");
			}

			if (property == "Foreground") return Application.Current.FindResource("Foreground");
			if (property == "Background") return Application.Current.FindResource("Background");
			return Application.Current.FindResource("Transparent");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return "";
		}
	}
}
