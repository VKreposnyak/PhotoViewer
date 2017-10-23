using System;

namespace PhotoViewer.Mvvm
{
    public class ActionCommand<T> : CommandBase
    {
        private Action<T> _action;
        private Func<bool> _canExecute;

        public ActionCommand(Action<T> action, Func<bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public override void Execute(object parameter)
        {
            if (_action != null)
            {
                var castParameter = (T)Convert.ChangeType(parameter, typeof(T));
                _action(castParameter);
            }
        }
    }
}