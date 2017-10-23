using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PhotoViewer.Mvvm
{
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof (string),
            typeof (EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof (ICommand), typeof (EventToCommandBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments",
            typeof (bool), typeof (EventToCommandBehavior), new PropertyMetadata(false));

        private Delegate _handler;
        private EventInfo _oldEvent;

        public string Event
        {
            get { return (string) GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool PassArguments
        {
            get { return (bool) GetValue(PassArgumentsProperty); }
            set { SetValue(PassArgumentsProperty, value); }
        }


        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior) d;

            if (beh.AssociatedObject != null) 
                beh.AttachHandler((string) e.NewValue);
        }

        protected override void OnAttached()
        {
            AttachHandler(Event); 
        }

        private void AttachHandler(string eventName)
        {
            _oldEvent?.RemoveEventHandler(AssociatedObject, _handler);

            if (!string.IsNullOrEmpty(eventName))
            {
                var eventInfo = AssociatedObject.GetType().GetEvent(eventName);
                if (eventInfo != null)
                {
                    var methodInfo = GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
                    eventInfo.AddEventHandler(AssociatedObject, _handler);
                    _oldEvent = eventInfo; 
                }
                else
                    throw new ArgumentException($"The event '{eventName}' was not found on type '{AssociatedObject.GetType().Name}'");
            }
        }

        private void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = PassArguments ? e : null;
            if (Command != null)
            {
                if (Command.CanExecute(parameter))
                    Command.Execute(parameter);
            }
        }
    }
}