﻿using System;
using System.Windows.Input;

namespace BitPack.Core.Shared
{
	class RelayCommand : ICommand
	{
		private Action<object> _execute;
		private Func<object, bool> _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			(_execute, _canExecute) = (execute, canExecute);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public void Execute(object parameter) { _execute(parameter); }
	}
}
