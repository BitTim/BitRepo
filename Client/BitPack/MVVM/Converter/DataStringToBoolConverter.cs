﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace BitPack.MVVM.Converter
{
	class DataStringToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string val = (string)value;
			string param = (string)parameter;

			return val == param;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
