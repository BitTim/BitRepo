﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BitPack.MVVM.Converter
{
	class BoolStringToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string val = value as string;
			bool convValue = bool.Parse(val);

			if (convValue) return Visibility.Visible;
			return Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
