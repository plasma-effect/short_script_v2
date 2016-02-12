using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public abstract partial class Sentence
    {
        public abstract dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner);
        public abstract CodeData GetData();
    }

    public class MultiSentence : Sentence
    {
        IEnumerable<Sentence> sentences;
        CodeData data;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            foreach(var s in sentences)
            {
                var r = s.Run(local, runner);
                if (r != null)
                    return r;
            }
            return null;
        }

        public MultiSentence(IEnumerable<Sentence> sentences, CodeData data)
        {
            this.sentences = sentences;
            this.data = data;
        }
    }

    public class For : Sentence
    {
        CodeData data;
        Sentence source;
        string name;
        Expression first;
        Expression last;
        Expression step;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            if (local.ContainsKey(name))
                throw new InnerException(data.ExceptionMessage(string.Format("counter value name '{0}' have been be defined.", name)));
            var las = last.ValueEval(local, runner);
            var ste = step.ValueEval(local, runner);
            bool c;

            local[name] = first.ValueEval(local, runner);
            try
            {
                c = (local[name] != las);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage("dynamic type error."));
            }
            while (c)
            {
                var r = source.Run(local, runner);
                if (r != null)
                    return r;
                try
                {
                    local[name] = local[name] + ste;
                    c = (local[name] != las);
                }
                catch (Exception)
                {
                    throw new InnerException(data.ExceptionMessage("dynamic type error."));
                }
            }
            source.Run(local, runner);
            return null;
        }

        public For(Sentence source,string name,Expression first,Expression last,Expression step,CodeData data)
        {
            this.data = data;
            this.source = source;
            this.name = name;
            this.first = first;
            this.last = last;
            this.step = step;
        }
    }

    public class While : Sentence
    {
        CodeData data;
        Expression cond;
        Sentence source;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            bool c;
            try
            {
                c = cond.ValueEval(local, runner);
            }
            catch(Exception)
            {
                throw new InnerException(data.ExceptionMessage("dynamic type error."));
            }
            while (c)
            {
                var r = source.Run(local, runner);
                if (r != null)
                    return r;
                try
                {
                    c = cond.ValueEval(local, runner);
                }
                catch (Exception)
                {
                    throw new InnerException(data.ExceptionMessage("dynamic type error."));
                }
            }
            return null;
        }

        public While(Expression cond,Sentence source,CodeData data)
        {
            this.data = data;
            this.cond = cond;
            this.source = source;
        }
    }

    public class If : Sentence
    {
        CodeData data;
        IEnumerable<Tuple<Expression, Sentence>> sentences;
        Sentence else_sentence;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            foreach(var t in sentences)
            {
                bool c;
                try
                {
                    c = t.Item1.ValueEval(local, runner);
                }
                catch(Exception)
                {
                    throw new InnerException(data.ExceptionMessage("dynamic type error."));
                }
                if (c)
                    return t.Item2.Run(local, runner);
            }
            if (else_sentence != null)
                return else_sentence.Run(local, runner);
            return null;
        }

        public If(IEnumerable<Tuple<Expression,Sentence>> sentences, Sentence else_sentence,CodeData data)
        {
            this.data = data;
            this.sentences = sentences;
            this.else_sentence = else_sentence;
        }

        public If(IEnumerable<Tuple<Expression, Sentence>> sentences, CodeData data)
        {
            this.data = data;
            this.sentences = sentences;
            this.else_sentence = null;
        }
    }

    public class Local : Sentence
    {
        CodeData data;
        string name;
        Expression expr;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            local[name] = expr.ValueEval(local, runner);
            return null;
        }

        public Local(string name,Expression expr, CodeData data)
        {
            this.data = data;
            this.name = name;
            this.expr = expr;
        }
    }

    public class Global : Sentence
    {
        CodeData data;
        string name;
        Expression expr;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            runner.Global[name] = expr.ValueEval(local, runner);
            return null;
        }

        public Global(string name,Expression expr,CodeData data)
        {
            this.data = data;
            this.name = name;
            this.expr = expr;
        }
    }

    public class Expr : Sentence
    {
        CodeData data;
        Expression expr;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            expr.ValueEval(local, runner);
            return null;
        }
        
        public Expr(Expression expr,CodeData data)
        {
            this.data = data;
            this.expr = expr;
        }
    }

    public class Return : Sentence
    {
        CodeData data;
        Expression expr;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            return expr.ValueEval(local, runner);
        }

        public Return(Expression expr,CodeData data)
        {
            this.data = data;
            this.expr = expr;
        }
    }

    public abstract partial class Sentence
    {
        public static Sentence MakeSentence(IEnumerable<TokenTree> source_codes, ScriptRunner runner)
        {
            return MakeSentenceImpl(source_codes, runner).Item2;
        }

        private static Tuple<int, Sentence> MakeSentenceImpl(IEnumerable<TokenTree> source_codes,ScriptRunner runner)
        {
            int i = 0;
            int skip = 0;
            List<Sentence> sentences = new List<Sentence>();
            var top_d = source_codes.First().GetData();
            foreach(var v in source_codes)
            {
                ++i;
                if (skip > 0)
                {
                    --skip;
                    continue;
                }
                var tree = v.GetTree();
                var data = v.GetData();
                if (tree.Count == 0)
                    continue;
                var top = tree.First().GetToken();
                if (top == null)
                    throw new InnerException(v.GetData().ExceptionMessage("invalid command name."));
                SFunction func;
                if (runner.SystemCommand.TryGetValue(top, out func))
                {
                    CallSystemFunction(func, tree.Skip(1), runner, data);
                    continue;
                }
                else if (MakeSentenceImplBreakStatement(top))
                    break;
                else if (top == "for")
                {
                    if (tree.Count < 4)
                        throw new InnerException(data.ExceptionMessage("too few argument to 'for'"));
                    var name = tree.Skip(1).First().GetToken();
                    if (name == null)
                        throw new InnerException(data.ExceptionMessage("invalid value name"));
                    var first = Expression.MakeExpression(tree.Skip(2).First(), runner);
                    var last = Expression.MakeExpression(tree.Skip(3).First(), runner);
                    var step = tree.Count == 4 ? new Literal(1, data) : Expression.MakeExpression(tree.Skip(4), runner);
                    var source = MakeSentenceImpl(source_codes.Skip(i), runner);

                    sentences.Add(new For(source.Item2, name, first, last, step, data));
                    skip += source.Item1;
                }
                else if (top == "while")
                {
                    if (tree.Count == 1)
                        throw new InnerException(data.ExceptionMessage("null expression error."));
                    var expr = Expression.MakeExpression(tree.Skip(1), runner);
                    var source = MakeSentenceImpl(source_codes.Skip(i), runner);

                    sentences.Add(new While(expr, source.Item2, data));
                    skip += source.Item1;
                }
                else if (top == "if")
                {
                    var s = MakeSentenceIf(tree, runner);
                    sentences.Add(s.Item2);
                    skip += s.Item1;
                }
                else if (top == "let")
                {
                    if (tree.Count < 3)
                        throw new InnerException(data.ExceptionMessage("too few argument to 'let'"));
                    var name = tree.Skip(1).First().GetToken() ??
                        Utility.Throw(new InnerException(data.ExceptionMessage("invalid value name")));
                    sentences.Add(new Local(name, Expression.MakeExpression(tree.Skip(2), runner), data));
                }
                else if (top == "global")
                {
                    if (tree.Count < 3)
                        throw new InnerException(data.ExceptionMessage("too few argument to 'global'"));
                    var name = tree.Skip(1).First().GetToken() ??
                        Utility.Throw(new InnerException(data.ExceptionMessage("invalid value name")));
                    sentences.Add(new Global(name, Expression.MakeExpression(tree.Skip(2), runner), data));
                }
                else if(top=="def")
                {
                    throw new InnerException(data.ExceptionMessage("don't define function before defend another function."));
                }
                else if(top=="return")
                {
                    sentences.Add(tree.Count == 1 ? new Return(new Literal(0, data), data) : new Return(Expression.MakeExpression(tree.Skip(1), runner), data));
                }
                else
                {
                    sentences.Add(new Expr(Expression.MakeExpression(tree, runner), data));
                }
            }
            return new Tuple<int, Sentence>(i, new MultiSentence(sentences, top_d));
        }

        static private void CallSystemFunction(SFunction func,IEnumerable<TokenTree> tree,ScriptRunner runner,CodeData data)
        {
            var e = tree.GetEnumerator();
            var d = new dynamic[func.ArgumentLength];
            for (int i = 0; i < func.ArgumentLength - 1; ++i)
            {
                d[i] = Expression.MakeExpression(e.Current, runner).StaticEval(runner) ?? Utility.Throw(new InnerException(e.Current.GetData().ExceptionMessage("this exprssion is not usable in a constant expression.")));
                if (!e.MoveNext())
                    throw new InnerException(data.ExceptionMessage(string.Format("too few argument to call command '{0}'", func.Name)));
            }
            d[func.ArgumentLength - 1] = 
                Expression.MakeExpression(tree.Skip(func.ArgumentLength - 1), runner) ?? 
                Utility.Throw(new InnerException(e.Current.GetData().ExceptionMessage("this exprssion is not usable in a constant expression.")));
            func.Call(d, runner, data);
        }

        static private bool MakeSentenceImplBreakStatement(string str)
        {
            return str == "next" || str == "loop" || str == "endif" || str == "else" || str == "elif";
        }

        static private Tuple<int,Sentence> MakeSentenceIf(IEnumerable<TokenTree> tree,ScriptRunner runner)
        {
            int i = 0;

            var ret = new List<Tuple<Expression, Sentence>>();
            
            var t = tree.First();
            var t2 = t.GetTree();
            if (t2.Count == 1)
                throw new InnerException(t.GetData().ExceptionMessage("null expression error."));
            var data = t.GetData();
            var co = Expression.MakeExpression(t2.Skip(1), runner);
            var se = MakeSentenceImpl(tree.Skip(1), runner);
            ret.Add(Tuple.Create(co, se.Item2));
            int skip = se.Item1;
            foreach(var v in tree)
            {
                ++i;
                if (skip > 0)
                {
                    --skip;
                    continue;
                }
                var tree0 = v.GetTree();
                var top = tree0.First().GetToken();
                if (top == null)
                    continue;
                if (top == "endif")
                {
                    return Tuple.Create(i, new If(ret, data) as Sentence);
                }
                else if (top == "elif")
                {
                    var t_ = v.GetTree();
                    if (t_.Count == 1)
                        throw new InnerException(v.GetData().ExceptionMessage("null expression error."));
                    var data_ = v.GetData();
                    var co_ = Expression.MakeExpression(t_.Skip(1), runner);
                    var se_ = MakeSentenceImpl(tree.Skip(i), runner);
                    ret.Add(Tuple.Create(co_, se_.Item2));
                    skip += se_.Item1;
                    continue;
                }
                else if (top == "else")
                {
                    var s = MakeSentenceImpl(tree.Skip(i), runner);
                    return Tuple.Create(i + s.Item1, new If(ret, s.Item2, data) as Sentence);
                }
            }
            throw new InnerException(data.ExceptionMessage("terminate of 'if' statements hasn't been "));
        }
    }
}
