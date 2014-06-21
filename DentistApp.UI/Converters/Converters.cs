using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DentistApp.Model;

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
           var s = (List<OperationAppointment>)value;


           var a = s.Select(d => d.Operation).ToList<Operation>().Select(e=>e.Description).ToArray<string>();
           
           
           return string.Join(",", a);
       }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           throw new NotSupportedException();
       }
   }
}
