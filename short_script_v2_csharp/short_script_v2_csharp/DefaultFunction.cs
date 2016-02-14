using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public class DefaultFunction
    {
        private static dynamic BiOperator(dynamic[] d,CodeData data,Func<dynamic,dynamic,dynamic> op,string opname)
        {
            try
            {
                return op(d[0], d[1]);
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("'{0}' {1} '{2}' are not allowed", d[0].GetType(), opname, d[1].GetType())));
            }
        }

        private static void MakeBiOperator(Dictionary<string, IFunction> dic, string opname, Func<dynamic, dynamic, dynamic> func)
        {
            dic.Add(opname, new NonAssistedFunction(opname, (d, data) => (BiOperator(d, data, func, opname)), true, 2));
        }

        public static Dictionary<string,IFunction> MakeBiOperatorFunctions()
        {
            var ret = new Dictionary<string, IFunction>();
            MakeBiOperator(ret, "+", (a, b) => a + b);
            MakeBiOperator(ret, "-", (a, b) => a - b);
            MakeBiOperator(ret, "*", (a, b) => a * b);
            MakeBiOperator(ret, "/", (a, b) => a / b);
            MakeBiOperator(ret, "%", (a, b) => a % b);

            MakeBiOperator(ret, "&", (a, b) => a & b);
            MakeBiOperator(ret, "|", (a, b) => a | b);
            MakeBiOperator(ret, "^", (a, b) => a ^ b);
            MakeBiOperator(ret, "<<", (a, b) => a << b);
            MakeBiOperator(ret, ">>", (a, b) => a >> b);
            MakeBiOperator(ret, "&&", (a, b) => a && b);
            MakeBiOperator(ret, "||", (a, b) => a || b);

            MakeBiOperator(ret, "=", (a, b) => a == b);
            MakeBiOperator(ret, "<>", (a, b) => a != b);
            MakeBiOperator(ret, "<", (a, b) => a < b);
            MakeBiOperator(ret, "<=", (a, b) => a <= b);
            MakeBiOperator(ret, ">", (a, b) => a > b);
            MakeBiOperator(ret, ">=", (a, b) => a >= b);
            
            return ret;
        }

        private static dynamic Print(dynamic[] d,CodeData data)
        {
            Console.Write(d[0]);
            return d[0];
        }

        private static dynamic PrintLine(dynamic[] d, CodeData data)
        {
            Console.WriteLine(d[0]);
            return d[0];
        }

        private static dynamic ArrowAccess(dynamic[] d,CodeData data)
        {
            int i;
            try
            {
                i = d[1];   
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("Transforming '{0}' to 'integer' is not  allowed.", d[1].GetType())));
            }
            try
            {
                return d[0][i];
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("'{0}' is not array or given argument Length is '0'.", d[0].GetType())));
            }
        }

        private static dynamic StringToInt(dynamic[] d, CodeData data)
        {
            string s;
            try
            {
                s = d[0];
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("Transforming '{0}' to 'string' is not  allowed.", d[1].GetType())));
            }
            try
            {
                return int.Parse(s);
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage("Given argument can not be parsed to integer."));
            }
        }

        private static dynamic StringToDouble(dynamic[] d, CodeData data)
        {
            string s;
            try
            {
                s = d[0];
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("Transforming '{0}' to 'string' is not allowed.", d[1].GetType())));
            }
            try
            {
                return double.Parse(s);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage("Given argument can not be parsed to double."));
            }
        }

        private static dynamic MakeArray(dynamic[] d,CodeData data)
        {
            return d;
        }

        private static Type GetTypeI<T>(T v)
        {
            return typeof(T);
        }

        private static Type GetType(dynamic[] d, CodeData data)
        {
            return GetTypeI(d[0]);
        }

        public static Dictionary<string,IFunction> MakeUtilityFunctions()
        {
            var ret = new Dictionary<string, IFunction>();
            ret.Add("print", new NonAssistedFunction("print", Print, false));
            ret.Add("println", new NonAssistedFunction("println", PrintLine, false));
            ret.Add("at", new NonAssistedFunction("at", ArrowAccess, true, 2));
            ret.Add("int.parse", new NonAssistedFunction("int.parse", StringToInt, true));
            ret.Add("double.parse", new NonAssistedFunction("double.parse", StringToDouble, true));
            ret.Add("array", new NonAssistedFunction("array", MakeArray, true, -1));
            ret.Add("type", new NonAssistedFunction("type", GetType, false));
            return ret;
        }

        public static Dictionary<string,IFunction> GetDefaultFunctions()
        {
            var ret = MakeBiOperatorFunctions();
            ret = ret.Concat(MakeUtilityFunctions()).ToDictionary(x => x.Key, x => x.Value);
            return ret;
        }

        public static Dictionary<string,SFunction> MakeUtilityCommand()
        {
            var ret = new Dictionary<string, SFunction>();
            return ret;
        }

        public static Dictionary<string,SFunction> GetDefaultCommand()
        {
            var ret = MakeUtilityCommand();
            return ret;
        }

        private static dynamic LiteralCheckInteger(string str)
        {
            int v;
            return int.TryParse(str, out v) ? v as dynamic : null as dynamic;
        }

        private static dynamic LiteralCheckString(string str)
        {
            return str[0] == '"' && str[str.Length - 1] == '"' ? str.Substring(1, str.Length - 2) as dynamic : null;
        }

        private static dynamic LiteralCheckDouble(string str)
        {
            double v;
            return double.TryParse(str, out v) ? v as dynamic : null as dynamic;
        }

        private static dynamic LiteralCheckBoolean(string str)
        {
            return str == "true" ? true as dynamic : str == "false" ? false as dynamic : null;
        }

        public static List<Func<string,dynamic>> GetDefaultLiteralChecker()
        {
            List<Func<string, dynamic>> ret = new List<Func<string, dynamic>>();
            ret.Add(LiteralCheckInteger);
            ret.Add(LiteralCheckString);
            ret.Add(LiteralCheckDouble);
            ret.Add(LiteralCheckBoolean);
            return ret;
        }
    }
}
