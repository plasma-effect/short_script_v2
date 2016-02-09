using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public class ScriptRunner
    {
        Dictionary<string, dynamic> global;
        Dictionary<string, IFunction> function;
        
        IEnumerable<Func<string, dynamic>> literal_checker;
        

        public Dictionary<string, dynamic> Global
        {
            get
            {
                return global;
            }
        }

        public Dictionary<string, IFunction> Function
        {
            get
            {
                return function;
            }
        }

        public IEnumerable<Func<string,dynamic>> LiteralChecker
        {
            get
            {
                return literal_checker;
            }
        }
    }
}
