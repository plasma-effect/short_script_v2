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
        public abstract dynamic StaticEval(ScriptRunner runner);
        public abstract override string ToString();
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
            throw new InnerException(data.ExceptionMessage(string.Format("value name '{0}' is not defined.", name)));
        }

        public override dynamic StaticEval(ScriptRunner runner)
        {
            dynamic d;
            return runner.ConstantValue.TryGetValue(name, out d) ? d : null;
        }

        public override string ToString()
        {
            return "value: " + name;
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

        public override dynamic StaticEval(ScriptRunner runner)
        {
            var d = (from e in exprs select e.StaticEval(runner)).ToArray();
            return d.Contains(null) ? null : func.StaticCall(d, data);
        }

        public override string ToString()
        {
            string ret = "function: " + func.Name + "(";
            foreach(var str in exprs)
            {
                ret += str.ToString();
            }
            return ret + ")";
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

        public override dynamic StaticEval(ScriptRunner runner)
        {
            return value;
        }

        public override string ToString()
        {
            return "literal: " + value.ToString();
        }

        public Literal(dynamic value,CodeData data)
        {
            this.data = data;
            this.value = value;
        }
    }

    public partial class Expression
    {
        public static Expression MakeExpression(IEnumerable<TokenTree> tree, ScriptRunner runner, IFunction from, CodeData data)
        {
            if (tree.Count() == 0)
                throw new InnerException(data.ExceptionMessage("null expression is not allowed"));

            if (tree.Count() == 1)
                return MakeExpression(tree.First(), runner, from, data);

            var top = tree.First();
            var name = top.GetToken();
            if (name == null)
                throw new InnerException(top.GetData().ExceptionMessage("error function name"));
            if (from.Name == name)
            {
                if (from.ArgumentLength == -1)
                {
                    var d = new List<Expression>();
                    int i = 0;
                    foreach (var e in tree.Skip(1))
                    {
                        var expr = MakeExpression(tree.Skip(++i).First(), runner, from, e.GetData());
                        var v = expr.StaticEval(runner);
                        if (v != null) expr = new Literal(v, expr.Data);
                        d.Add(expr);
                    }
                    return new Function(from, d, top.GetData());
                }
                else
                {
                    if (tree.Count() <= from.ArgumentLength)
                        throw new InnerException(top.GetData().ExceptionMessage(string.Format("too few arguments to function '{0}'", from.Name)));
                    var d = new List<Expression>();
                    for (int i = 0; i < from.ArgumentLength - 1; ++i)
                    {
                        var tu = tree.Skip(i + 1).First();
                        var expr = MakeExpression(tu, runner, from, tu.GetData());
                        var v = expr.StaticEval(runner);
                        if (v != null) expr = new Literal(v, expr.Data);
                        d.Add(expr);
                    }
                    var t = tree.Skip(from.ArgumentLength);
                    d.Add(MakeExpression(t, runner, from, t.First().GetData()));
                    return new Function(from, d, top.GetData());
                }
            }

            IFunction func;
            if (!runner.Function.TryGetValue(name, out func))
                throw new InnerException(top.GetData().ExceptionMessage(string.Format("function {0} have not been be found.", name)));
            if (func.ArgumentLength == -1)
            {
                var d = new List<Expression>();
                int i = 0;
                foreach (var e in tree.Skip(1))
                {
                    var expr = MakeExpression(tree.Skip(++i).First(), runner, from, e.GetData());
                    var v = expr.StaticEval(runner);
                    if (v != null) expr = new Literal(v, expr.Data);
                    d.Add(expr);
                }
                return new Function(func, d, top.GetData());
            }
            else
            {
                if (tree.Count() <= func.ArgumentLength)
                    throw new InnerException(top.GetData().ExceptionMessage(string.Format("too few arguments to function '{0}'", func.Name)));
                var d = new List<Expression>();
                for (int i = 0; i < func.ArgumentLength - 1; ++i)
                {
                    var tu = tree.Skip(i + 1).First();
                    var expr = MakeExpression(tu, runner, from, tu.GetData());
                    var v = expr.StaticEval(runner);
                    if (v != null) expr = new Literal(v, expr.Data);
                    d.Add(expr);
                }
                var t = tree.Skip(func.ArgumentLength);
                d.Add(MakeExpression(t, runner, from, t.First().GetData()));
                return new Function(func, d, top.GetData());
            }

        }
        public static Expression MakeExpression(TokenTree tree, ScriptRunner runner, IFunction from, CodeData data)
        {
            var token = tree.GetToken();
            if (token == null)
            {
                return MakeExpression(tree.GetTree(), runner, from, data);
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
            dynamic dy;
            if (runner.ConstantValue.TryGetValue(token, out dy))
                return new Literal(dy, tree.GetData());
            return new Value(token, tree.GetData());
        }
    }
}
