//Author: Gerard Castelló Viader
//Date: 10/16/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DentistApp.UI.Requests
{
    public interface IGenericInteractionRequest<T>
    {
        event EventHandler<GenericInteractionRequestEventArgs<T>> Raised;
    }
}
