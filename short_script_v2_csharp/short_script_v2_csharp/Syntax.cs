using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public abstract class Sentence
    {
        public abstract dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner);
        public abstract CodeData GetData();
        public abstract IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner);
        public override string ToString()
        {
            return ToString(0);
        }
        public abstract string ToString(int v);
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            dynamic v = null;
            foreach(var s in sentences)
            {
                foreach(var r in s.CoRoutineRun(local, runner))
                {
                    v = r;
                    if (YieldReturn.IsYieldValue(r))
                    {
                        yield return r;
                    }
                    else if(r!=null)
                    {
                        goto end;
                    }
                }
            }
            v = null;
            end:
            yield return v;
        }
        
        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            ret += "multi sentence\n";
            foreach(var s in sentences)
            {
                ret += s.ToString(v + 1) + "\n";
            }
            return ret;
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
            local.Remove(name);
            return null;
        }

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            if (local.ContainsKey(name))
                throw new InnerException(data.ExceptionMessage(string.Format("counter value name '{0}' have been be defined.", name)));
            var las = last.ValueEval(local, runner);
            var ste = step.ValueEval(local, runner);
            bool c;
            dynamic v = null;

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
                foreach (var r in source.CoRoutineRun(local, runner))
                {
                    if (YieldReturn.IsYieldValue(r))
                    {
                        yield return r;
                    }
                    else if (r != null)
                    {
                        v = r;
                        goto end;
                    }
                }
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
            foreach(var r in source.CoRoutineRun(local, runner))
            {
                if (YieldReturn.IsYieldValue(r))
                    yield return r;
                else if (r != null)
                {
                    v = r;
                    goto end;
                }
            }
            v = null;
            end:
            local.Remove(name);
            yield return v;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            ret += "for:" + first.ToString() + " " + last.ToString() + " " + step.ToString() + "\n";
            return ret + source.ToString(v + 1) + "\n";
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            bool c;
            dynamic v = null;
            try
            {
                c = cond.ValueEval(local, runner);
            }
            catch (Exception)
            {
                throw new InnerException(data.ExceptionMessage("dynamic type error."));
            }
            while (c)
            {
                foreach (var r in source.CoRoutineRun(local, runner))
                {
                    if(YieldReturn.IsYieldValue(r))
                    {
                        yield return r;
                    }
                    else if (r != null)
                    {
                        v = r;
                        goto end;
                    }
                    try
                    {
                        c = cond.ValueEval(local, runner);
                    }
                    catch (Exception)
                    {
                        throw new InnerException(data.ExceptionMessage("dynamic type error."));
                    }
                }
            }
            v = null;
            end:
            yield return v;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            ret += "while:" + cond.ToString() + "\n";
            return ret + source.ToString(v + 1) + "\n";
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            dynamic v = null;
            foreach (var t in sentences)
            {
                bool c;
                try
                {
                    c = t.Item1.ValueEval(local, runner);
                }
                catch (Exception)
                {
                    throw new InnerException(data.ExceptionMessage("dynamic type error."));
                }
                if (c)
                {
                    foreach(var r in t.Item2.CoRoutineRun(local, runner))
                    {
                        if (YieldReturn.IsYieldValue(r))
                        {
                            yield return r;
                        }
                        else if (r != null)
                        {
                            v = r;
                            goto end;
                        }
                    }
                }
            }
            if (else_sentence != null)
            {
                foreach (var r in else_sentence.CoRoutineRun(local,runner))
                {
                    if (YieldReturn.IsYieldValue(r))
                    {
                        yield return r.Value;
                    }
                    else if (r != null)
                    {
                        v = r;
                        goto end;
                    }
                }
            }
            v = null;
            end:
            yield return v;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            ret += "if: " + sentences.Count() + "\n";
            foreach(var s in sentences)
            {
                for (int i = 0; i < v; ++i)
                    ret += "  ";
                ret += s.Item1.ToString() + "\n";
                ret += s.Item2.ToString(v + 1) + "\n";
            }
            if(else_sentence!=null)
            {
                for (int i = 0; i < v; ++i)
                    ret += "  ";
                ret += "else\n";
                ret += else_sentence.ToString(v + 1) + "\n";
            }
            return ret;
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            local[name] = expr.ValueEval(local, runner);
            yield return null;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            return ret + "let:" + name + " " + expr.ToString();
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            throw new NotImplementedException();
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            return ret + "global:" + name + " " + expr.ToString();
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            expr.ValueEval(local, runner);
            yield return null;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            return ret += "expr:" + expr.ToString();
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

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            yield return expr.ValueEval(local, runner);
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            return ret + "return:" + expr.ToString();
        }

        public Return(Expression expr,CodeData data)
        {
            this.data = data;
            this.expr = expr;
        }
    }

    public class YieldReturn : Sentence
    {
        CodeData data;
        Expression expr;
        
        public class YieldValue
        {
            dynamic value;
            public dynamic Value
            {
                get
                {
                    return value;
                }
            }

            public YieldValue(dynamic value)
            {
                this.value = value;
            }
        }
        
        public static bool IsYieldValue(YieldValue v)
        {
            return v != null;
        }

        public static bool IsYieldValue(dynamic v)
        {
            return false;
        }
        
        public static dynamic RemoveYield(YieldValue v)
        {
            return v != null ? v.Value : null;
        }

        public static dynamic RemoveYield(dynamic v)
        {
            return v;
        }

        public override CodeData GetData()
        {
            return data;
        }

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            yield return new YieldValue(expr.ValueEval(local, runner));
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            throw new InnerException(data.ExceptionMessage("yield_return in normal calling is not allowed"));
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            return ret + "yield return:" + expr.ToString();
        }

        public YieldReturn(Expression expr,CodeData data)
        {
            this.data = data;
            this.expr = expr;
        }
    }

    public class Foreach : Sentence
    {
        CodeData data;
        Sentence sentence;
        Expression expr;
        string name;

        public override CodeData GetData()
        {
            return data;
        }

        public override dynamic Run(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            if (local.ContainsKey(name))
                throw new InnerException(data.ExceptionMessage(string.Format("counter value name '{0}' have been be defined.", name)));
            foreach (var r in expr.ValueEval(local, runner))
            {
                local[name] = YieldReturn.RemoveYield(r);
                var v = sentence.Run(local, runner);
                if (v != null)
                    return v;
            }
            local.Remove(name);
            return null;
        }

        public override IEnumerable<dynamic> CoRoutineRun(Dictionary<string, dynamic> local, ScriptRunner runner)
        {
            dynamic v = null;
            if (local.ContainsKey(name))
                throw new InnerException(data.ExceptionMessage(string.Format("counter value name '{0}' have been be defined.", name)));
            foreach (var r in expr.ValueEval(local, runner))
            {
                local[name] = r;
                foreach(var u in sentence.CoRoutineRun(local, runner))
                {
                    if (YieldReturn.IsYieldValue(u))
                    {
                        yield return u;
                    }
                    else if (u != null)
                    {
                        v = u;
                        goto end;
                    }
                }
                
            }
            v = null;
            end:
            local.Remove(name);
            yield return v;
        }

        public override string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += "  ";
            ret += "foreach:" + name + " " + expr.ToString() + "\n";
            return ret + sentence.ToString(v + 1);
        }

        public Foreach(string name,Sentence sentence,Expression expr,CodeData data)
        {
            this.name = name;
            this.sentence = sentence;
            this.expr = expr;
            this.data = data;
        }
    }
}
