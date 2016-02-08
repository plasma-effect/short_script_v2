using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public class CodeData
    {
        string filename;
        int line;
        int column;
        public CodeData(int line, int column, String filename)
        {
            this.filename = filename;
            this.line = line;
            this.column = column;
        }

        public string ExceptionMessage(string message)
        {
            return string.Format(@"short script error ""{0}"" {1}-{2}: ", filename, line + 1, column + 1) + message;
        }

        public static string ExceptionMessage(string message,int line,int column, string filename)
        {
            return string.Format(@"short script error ""{0}"" {1}-{2}: ", filename, line + 1, column + 1) + message;
        }
    }
}
