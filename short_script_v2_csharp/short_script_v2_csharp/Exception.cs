using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public class InnerException: Exception
    {
        public InnerException(string message) : base(message) { }
    }
}
