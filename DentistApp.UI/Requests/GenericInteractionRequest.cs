//Author: Gerard Castelló Viader
//Date: 10/16/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.UI.Requests
{
    public class GenericInteractionRequest<T> : IGenericInteractionRequest<T>
    {
        public event EventHandler<GenericInteractionRequestEventArgs<T>> Raised;

        public void Raise(T entity, Action<T> callback, Action cancelCallback)
        {
            this.Raised(this, new GenericInteractionRequestEventArgs<T>(entity, callback, cancelCallback));
        }
    }
}
