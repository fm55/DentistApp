using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.UI
{
    public interface IGenericInteractionView<T>
    {
        void SetEntity(T entity);
        T GetEntity();
    }
}
