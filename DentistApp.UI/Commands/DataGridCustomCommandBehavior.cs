using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace DentistApp.UI.Commands
{
    public class DataGridCustomCommandBehavior:CommandBehaviorBase<DataGrid>
    {
        public DataGridCustomCommandBehavior(DataGrid dg)
            : base(dg)
        {
            dg.CurrentCellChanged += new EventHandler<EventArgs>(dg_CurrentCellChanged);

        }

        void dg_CurrentCellChanged(object sender, EventArgs e)
        {
            this.ExecuteCommand();
        }
    }


    public class CurrentCellChangedClass
    {
        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty) as object;
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CurrentCellChangedClass), new PropertyMetadata(OnSetCommandParameterCallback));


        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, object value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CurrentCellChangedClass), new PropertyMetadata(OnSetCommandCallback));


        public void SetClickCommandBehavior(DependencyObject obj, DataGridCustomCommandBehavior value)
        {
            obj.SetValue(ClickCommandBehaviorProperty, value);
        }

        public DataGridCustomCommandBehavior GetClickCommandBehavior(DependencyObject obj)
        {
            return (DataGridCustomCommandBehavior) obj.GetValue(ClickCommandBehaviorProperty);
        }

        public static readonly DependencyProperty ClickCommandBehaviorProperty =
            DependencyProperty.RegisterAttached("ClickCommandBehavior", typeof(DataGridCustomCommandBehavior), typeof(CurrentCellChangedClass), null);


        public static DataGridCustomCommandBehavior GetOrCreateDataGridBehavior(DataGrid dg)
        {
            var dgBehavior = dg.GetValue(ClickCommandBehaviorProperty) as DataGridCustomCommandBehavior;
            if (dgBehavior == null)
            {
                dgBehavior = new DataGridCustomCommandBehavior(dg);
                dg.SetValue(ClickCommandBehaviorProperty, dgBehavior);

            }
            return dgBehavior;
        }

        public static void OnSetCommandCallback(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = dep as DataGrid;
            if (dataGrid != null)
            {
                DataGridCustomCommandBehavior beh = GetOrCreateDataGridBehavior(dataGrid);
                beh.Command = e.NewValue as ICommand;
            }
        }


        private static void OnSetCommandParameterCallback(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = dep as DataGrid;
            if (dataGrid != null)
            {
                DataGridCustomCommandBehavior beh = GetOrCreateDataGridBehavior(dataGrid);
                beh.CommandParameter = e.NewValue;
            }
        }

    }

}
