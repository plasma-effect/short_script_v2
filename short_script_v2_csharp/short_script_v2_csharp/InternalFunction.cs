using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public abstract class IFunction
    {
        public abstract dynamic Call(dynamic[] argument, CodeData data);
        public abstract dynamic StaticCall(dynamic[] argument, CodeData data);
        public abstract int ArgumentLength { get; }
        public abstract string Name { get; }
    }

    public class InternalFunction<Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 0;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if(argument.Length!=0)
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func();
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 1;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 1 ||
                !(argument[0] is T0))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 2;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 2 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 3;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 3 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 4;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 4 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 5;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 5 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, T5, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4, T5> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 6;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 6 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4) ||
                !(argument[5] is T5))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4, T5> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, T5, T6, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4, T5, T6> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 7;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 7 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4) ||
                !(argument[5] is T5) ||
                !(argument[6] is T6))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4, T5, T6> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, T5, T6, T7, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4, T5, T6, T7> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 8;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 8 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4) ||
                !(argument[5] is T5) ||
                !(argument[6] is T6) ||
                !(argument[7] is T7))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4, T5, T6, T7> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, T5, T6, T7, T8, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4, T5, T6, T7, T8> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 9;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 9 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4) ||
                !(argument[5] is T5) ||
                !(argument[6] is T6) ||
                !(argument[7] is T7) ||
                !(argument[8] is T8))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7], argument[8]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4, T5, T6, T7, T8> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class InternalFunction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<Return, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return 10;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            if (argument.Length != 10 ||
                !(argument[0] is T0) ||
                !(argument[1] is T1) ||
                !(argument[2] is T2) ||
                !(argument[3] is T3) ||
                !(argument[4] is T4) ||
                !(argument[5] is T5) ||
                !(argument[6] is T6) ||
                !(argument[7] is T7) ||
                !(argument[8] is T8) ||
                !(argument[9] is T9))
            {
                throw new InnerException(data.ExceptionMessage(argument_message));
            }
            try
            {
                return func(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7], argument[8], argument[9]);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public InternalFunction(string name, Func<Return, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    public class VariadicFunction<T, Return> : IFunction
    {
        string name;
        string argument_message;
        string running_message;
        Func<T[], Return> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return -1;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            var a = new T[argument.Length];
            for (int i = 0; i < argument.Length; ++i)
            {
                if (!(argument[i] is T))
                    throw new InnerException(data.ExceptionMessage(argument_message));
                a[i] = argument[i];
            }
            try
            {
                return func(a);
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public VariadicFunction(string name, Func<T[], Return> func, bool can_static_call = false, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }
    
    public class NonAssistedFunction:IFunction
    {
        string name;
        int argument_length;
        Func<dynamic[], CodeData, dynamic> func;
        bool can_static_call;

        public override int ArgumentLength
        {
            get
            {
                return argument_length;
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            return func(argument, data);
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return can_static_call ? Call(argument, data) : null;
        }

        public NonAssistedFunction(string name, Func<dynamic[], CodeData, dynamic> func, bool can_static_call, int argument_length = 1)
        {
            this.name = name;
            this.argument_length = argument_length;
            this.func = func;
            this.can_static_call = can_static_call;
        }
    }

    public class UserDefinedFunction : IFunction
    {
        string name;
        IEnumerable<string> argument_name;
        Sentence start;
        ScriptRunner runner;

        public override int ArgumentLength
        {
            get
            {
                return argument_name.Count();
            }
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override dynamic Call(dynamic[] argument, CodeData data)
        {
            var a = ArgumentLength;
            if (argument.Length < a)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("too few arguments to function '{0}'", name)));
            }
            else if (argument.Length > a)
            {
                throw new InnerException(data.ExceptionMessage(string.Format("too many arguments to function '{0}'", name)));
            }
            int i = 0;
            Dictionary<string, dynamic> local = new Dictionary<string, dynamic>();
            foreach(var n in argument_name)
            {
                local.Add(n, argument[i++]);
            }
            return start.Run(local, runner);
        }

        public override dynamic StaticCall(dynamic[] argument, CodeData data)
        {
            return null;
        }

        public UserDefinedFunction(string name, IEnumerable<string>argument_name, Sentence start, ScriptRunner runner)
        {
            this.name = name;
            this.argument_name = argument_name;
            this.start = start;
            this.runner = runner;
        }
    }

}
