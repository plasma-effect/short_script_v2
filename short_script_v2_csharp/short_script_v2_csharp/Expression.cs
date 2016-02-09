using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public abstract partial class Expression
    {
        public abstract dynamic ValueEval(Dictionary<string, dynamic> local, ScriptRunner runner);
        public abstract CodeData Data { get; }
    }

    public class Value : Expression
    {
        string name;
        CodeData data;

        public override CodeData Data
        {
            get
            {
                return data;
            }
        }

        public override dynamic ValueEval(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            dynamic d;
            if (local.TryGetValue(name, out d))
                return d;
            if (runner.Global.TryGetValue(name, out d))
                return d;
            return null;
        }

        public Value(string name, CodeData data)
        {
            this.name = name;
            this.data = data;
        }
    }

    public class Function : Expression
    {
        IFunction func;
        IEnumerable<Expression> exprs;
        CodeData data;

        public override CodeData Data
        {
            get
            {
                return data;
            }
        }
        
        public override dynamic ValueEval(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            return func.Call((from e in exprs select e.ValueEval(local, runner)).ToArray(), data);
        }

        public Function(IFunction func,IEnumerable<Expression> exprs,CodeData data)
        {
            this.func = func;
            this.exprs = exprs;
            this.data = data;
        }
    }

    public class Literal : Expression
    {
        CodeData data;
        dynamic value;

        public override CodeData Data
        {
            get
            {
                return data;
            }
        }

        public override dynamic ValueEval(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            return value;
        }

        public Literal(dynamic value,CodeData data)
        {
            this.data = data;
            this.value = value;
        }
    }

    public partial class Expression
    {
        public static Expression MakeExpression(IEnumerable<TokenTree> tree, ScriptRunner runner)
        {
            var enu = tree.GetEnumerator();
            var top = enu.Current;
            var name = top.GetToken();
            if (name == null)
                throw new InnerException(top.GetData().ExceptionMessage("error function name"));
            IFunction func;
            if (!runner.Function.TryGetValue(name, out func))
                throw new InnerException(top.GetData().ExceptionMessage(string.Format("function {1} have not been be found.", name)));
            if(func.ArgumentLength==-1)
            {
                var d = new List<Expression>();
                foreach(var e in tree.Skip(1))
                {
                    d.Add(MakeExpression(e, runner));
                }
                return new Function(func, d, top.GetData());
            }
            else
            {
                var d = new List<Expression>();
                for (int i = 0; i < func.ArgumentLength - 1; ++i)
                {
                    if(!enu.MoveNext())
                    {
                        throw new InnerException(top.GetData().ExceptionMessage("provided arguments were too few."));
                    }
                    d.Add(MakeExpression(enu.Current, runner));

                }
                d.Add(MakeExpression(tree.Skip(func.ArgumentLength), runner));
                return new Function(func, d, top.GetData());
            }
            
        }
        public static Expression MakeExpression(TokenTree tree, ScriptRunner runner)
        {
            var token = tree.GetToken();
            if (token == null)
            {
                return MakeExpression(tree.GetTree(), runner);
            }
            foreach (var c in runner.LiteralChecker) 
            {
                dynamic d = c(token);
                if (d != null)
                    return new Literal(d, tree.GetData());
            }
            IFunction func;
            if (runner.Function.TryGetValue(token, out func))
                return new Function(func, new Expression[0], tree.GetData());
            return new Value(token, tree.GetData());
        }
    }
}
