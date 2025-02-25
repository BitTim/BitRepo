﻿using System;
using System.Globalization;
using System.Windows.Controls;

namespace BitPack.MVVM.Validation
{
	public class TierValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string strValue = Convert.ToString(value);

			if (string.IsNullOrEmpty(strValue))
				return new ValidationResult(false, $"This field cannot be empty, last known value will be used");
			bool canConvert = false;

			int val;
			canConvert = int.TryParse(strValue, out val);
			if (!canConvert) return new ValidationResult(false, $"Input must be a number, last known value will be used");

			if (val < 1) return new ValidationResult(false, $"Input cannot be less than 1");
			if (val > 10) return new ValidationResult(false, $"Input cannot be larger than 10");
			return new ValidationResult(true, null);
		}
	}
}
