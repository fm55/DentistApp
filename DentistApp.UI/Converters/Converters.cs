using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DentistApp.Model;
using DentistApp.BL;

namespace DentistApp.UI.Converters
{
   public class DateConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                DateTime s = (DateTime)value;
                return string.Format("{0}/{1}", s.Day, s.Month);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotSupportedException();
            }
    }

   public class OperationsToListConverter : IValueConverter
   {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           var appId = (int)value;
           AppointmentController ac = new AppointmentController();
           var operations = ac.GetOperationsOfAppointment(appId);
           var a = operations.Select(e => e.Description + " (" + e.Amount + ")").ToArray<string>();
           
           
           return string.Join(", \n", a);
       }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           throw new NotSupportedException();
       }
   }

   public class TeethConverter : IValueConverter
   {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           var appId = (int)value;
           AppointmentController ac = new AppointmentController();
           var teeth = ac.GetTeethOfAppointment(appId);
           var a = teeth.Select(e => e.Name).ToArray<string>();


           return string.Join(", \n", a);
       }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           throw new NotSupportedException();
       }
   }
}
