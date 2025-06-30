using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.api.exceptions
{
    public class HasReferenceException : Exception
    {
        public HasReferenceException(string message) : base(message)
        {
            
        }
    }
}
