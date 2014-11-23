using System;
using System.Collections.Generic;
using System.Text;

using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MagicBallAdvisor.AttachedProperties
{
    public class Commands
    {


        public static ICommand GetHolding(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(HoldingProperty);
        }

        public static void SetHolding(DependencyObject obj, ICommand value)
        {
            obj.SetValue(HoldingProperty, value);
        }

        // Using a DependencyProperty as the backing store for Holding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoldingProperty =
            DependencyProperty.RegisterAttached("Holding",
            typeof(ICommand),
            typeof(object),
            new PropertyMetadata(null, OnHoldingCommandChange));

        private static void OnHoldingCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UIElement;
            if (control == null)
            {
                return;
            }
            control.Holding += (obj, args) =>
            {
                ICommand command = e.NewValue as ICommand;
                if (command == null)
                {
                    return;
                }
                command.Execute(null);
            };
        }

        

        public static ICommand GetDoubleTapped(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleTappedProperty);
        }

        public static void SetDoubleTapped(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleTappedProperty, value);
        }

        // Using a DependencyProperty as the backing store for DoubleTapped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleTappedProperty =
            DependencyProperty.RegisterAttached("DoubleTapped",
            typeof(ICommand), 
            typeof(object), 
            new PropertyMetadata(null, OnDoubleTappedCommandChange));

        private static void OnDoubleTappedCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UIElement;
            if (control == null)
            {
                return;
            }
            control.DoubleTapped += (obj, args) =>
            {
                ICommand command = e.NewValue as ICommand;
                if (command == null)
                {
                    return;
                }
                command.Execute(null);
            };
        }
    }
}
