﻿using System;
using System.Windows.Input;

namespace PhotoViewer.Mvvm
{
    public abstract class CommandBase : ICommand
    {
        public virtual event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}