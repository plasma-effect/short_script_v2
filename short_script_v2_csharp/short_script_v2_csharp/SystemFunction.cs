using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public abstract class SFunction
    {
        public abstract void Call(dynamic[] argument, ScriptRunner runner, CodeData data);
        public abstract int ArgumentLength { get; }
        public abstract string Name { get;}
    }

    public class SystemFunction<T0> : SFunction
    {
        Action<T0, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 1
                || !(argument[0] is T0))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], runner);
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name,Action<T0,ScriptRunner> action, string argument_message = "Argument Type Error",string running_message="Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1> : SFunction
    {
        Action<T0, T1, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 2
|| !(argument[0] is T0)
|| !(argument[1] is T1))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2> : SFunction
    {
        Action<T0, T1, T2, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 3
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3> : SFunction
    {
        Action<T0, T1, T2, T3, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 4
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4> : SFunction
    {
        Action<T0, T1, T2, T3, T4, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 5
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4, T5> : SFunction
    {
        Action<T0, T1, T2, T3, T4, T5, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 6
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4)
|| !(argument[5] is T5))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, T5, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4, T5, T6> : SFunction
    {
        Action<T0, T1, T2, T3, T4, T5, T6, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 7
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4)
|| !(argument[5] is T5)
|| !(argument[6] is T6))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, T5, T6, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4, T5, T6, T7> : SFunction
    {
        Action<T0, T1, T2, T3, T4, T5, T6, T7, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 8
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4)
|| !(argument[5] is T5)
|| !(argument[6] is T6)
|| !(argument[7] is T7))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, T5, T6, T7, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4, T5, T6, T7, T8> : SFunction
    {
        Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 9
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4)
|| !(argument[5] is T5)
|| !(argument[6] is T6)
|| !(argument[7] is T7)
|| !(argument[8] is T8))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7], argument[8], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class SystemFunction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : SFunction
    {
        Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            if (argument.Length != 10
|| !(argument[0] is T0)
|| !(argument[1] is T1)
|| !(argument[2] is T2)
|| !(argument[3] is T3)
|| !(argument[4] is T4)
|| !(argument[5] is T5)
|| !(argument[6] is T6)
|| !(argument[7] is T7)
|| !(argument[8] is T8)
|| !(argument[9] is T9))
                throw new InnerException(data.ExceptionMessage(argument_message));
            try
            {
                action(argument[0], argument[1], argument[2], argument[3], argument[4], argument[5], argument[6], argument[7], argument[8], argument[9], runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public SystemFunction(string name, Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }
    public class VariadicSystemFunction<T> : SFunction
    {
        Action<T[], ScriptRunner> action;
        string name;
        string argument_message;
        string running_message;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            var a = new T[argument.Length];
            for(int i=0;i<argument.Length;++i)
            {
                if (!(argument[i] is T))
                    throw new InnerException(data.ExceptionMessage(argument_message));
                a[i] = argument[i];
            }
            try
            {
                action(a, runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage(running_message));
            }
        }

        public VariadicSystemFunction(string name, Action<T[], ScriptRunner> action, string argument_message = "Argument Type Error", string running_message = "Running Error")
        {
            this.action = action;
            this.name = name;
            this.argument_message = argument_message;
            this.running_message = running_message;
        }
    }

    public class NonAssistedSystemFunction : SFunction
    {
        Action<dynamic[], ScriptRunner, CodeData> action;
        string name;
        int argument_length;

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

        public override void Call(dynamic[] argument, ScriptRunner runner, CodeData data)
        {
            action(argument, runner, data);
        }

        public NonAssistedSystemFunction(string name, Action<dynamic[], ScriptRunner, CodeData> action, int argument_length)
        {
            this.action = action;
            this.name = name;
            this.argument_length = argument_length;
        }
    }
}
