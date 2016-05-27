using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace gitdoko.Extensions
{
    static class ModelStateDictionaryExtensions
    {
        public static void AddErrors<T>( this ModelStateDictionary modelState, IEnumerable<T> errors, Func<T, string> errorMessageSelector )
        {
            foreach ( var error in errors )
            {
                if ( !modelState.TryAddModelError("", errorMessageSelector(error)) )
                {
                    break;
                }
            }
        }
    }
}
