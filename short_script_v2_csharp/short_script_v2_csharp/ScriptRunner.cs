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
        Dictionary<string, SFunction> system_command;
        Dictionary<string, dynamic> constant_value;

        IEnumerable<Func<string, dynamic>> literal_checker;

        string filename;

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

        public Dictionary<string, SFunction> SystemCommand
        {
            get
            {
                return system_command;
            }
        }

        public Dictionary<string, dynamic> ConstantValue
        {
            get
            {
                return constant_value;
            }
        }
        
        public string Filename
        {
            get
            {
                return filename;
            }
        }

        public ScriptRunner(
            string filename,
            IEnumerable<string> source_code,
            Dictionary<string,dynamic> default_global,
            Dictionary<string,IFunction> default_function,
            Dictionary<string,SFunction> default_command,
            Dictionary<string,dynamic> default_constant,
            IEnumerable<Func<string,dynamic>> literal_checker)
        {
            this.filename = filename;
            this.global = default_global;
            this.function = default_function;
            this.system_command = default_command;
            this.constant_value = default_constant;
            this.literal_checker = literal_checker;
            var trees = source_code.Select((t, index) => new Tree(t, index, filename) as TokenTree);
            
            int i = 0;
            foreach (var v in trees)
            {
                ++i;
                var tree = v.GetTree();
                if (tree.Count == 0)
                    continue;
                var top = tree.First().GetToken() ?? Utility.Throw(new InnerException(tree.First().GetData().ExceptionMessage("invalid command")));
                SFunction func;
                if (system_command.TryGetValue(top, out func))
                {
                    int l = func.ArgumentLength;

                    if (l == -1)
                    {
                        var d = new List<dynamic>();
                        int u = 0;
                        foreach (var e in tree.Skip(1))
                        {
                            var expr = Expression.MakeExpression(tree.Skip(++u).First(), this, new NullFunction(), e.GetData());
                            var k = expr.StaticEval(this);
                            if (k != null)
                                throw new InnerException(e.GetData().ExceptionMessage("this expression is not constant."));
                            d.Add(k);
                        }
                        func.Call(d.ToArray(), this, tree[0].GetData());
                    }
                    else
                    {
                        if (tree.Count() <= func.ArgumentLength)
                            throw new InnerException(top.GetData().ExceptionMessage(string.Format("too few arguments to function '{0}'", func.Name)));
                        var d = new List<dynamic>();
                        for (int m = 0; m < func.ArgumentLength - 1; ++m)
                        {
                            var tu = tree[m + 1];
                            var expr = Expression.MakeExpression(tu, this, new NullFunction(), tu.GetData());
                            var k = expr.StaticEval(this);
                            if (k != null)
                                throw new InnerException(tree[m + 1].GetData().ExceptionMessage("this expression is not constant."));
                            d.Add(k);
                        }
                        var t = tree.Skip(func.ArgumentLength);
                        d.Add(Expression.MakeExpression(t, this, new NullFunction(), t.First().GetData()));
                        func.Call(d.ToArray(), this, tree[0].GetData());
                    }
                }

                if (top == "const")
                {
                    if (tree.Count < 3)
                        throw new InnerException(tree.First().GetData().ExceptionMessage("too few argument to 'const'"));
                    var name = tree.Skip(1).First().GetToken() ?? Utility.Throw(new InnerException(tree.Skip(1).First().GetData().ExceptionMessage("invalid constant value name"))) as string;
                    if (this.constant_value.ContainsKey(name))
                        throw new InnerException(tree.First().GetData().ExceptionMessage(string.Format("constant value '{0}' has been defined", name)));
                    var val = Expression.MakeExpression(tree.Skip(2), this, new NullFunction(), tree.First().GetData()).StaticEval(this);
                }
                if(top=="def")
                {
                    if (tree.Count == 1)
                        throw new InnerException(tree[0].GetData().ExceptionMessage("too few argument to 'def'"));
                    var name = tree[1].GetToken() ?? Utility.Throw(new InnerException(tree[1].GetData().ExceptionMessage("invalid function name")));
                    var lis = new List<string>();
                    foreach (var u in tree.Skip(2))
                    {
                        lis.Add(u.GetToken() ?? Utility.Throw(new InnerException(u.GetData().ExceptionMessage("invalid argument name"))));
                    }
                    this.function.Add(name, new UserDefinedFunction(name, lis, trees.Skip(i), this));
                }
                if(top == "async")
                {
                    if (tree.Count == 1)
                        throw new InnerException(tree[0].GetData().ExceptionMessage("too few argument to 'def'"));
                    var name = tree[1].GetToken() ?? Utility.Throw(new InnerException(tree[1].GetData().ExceptionMessage("invalid function name")));
                    var lis = new List<string>();
                    foreach (var u in tree.Skip(2))
                    {
                        lis.Add(u.GetToken() ?? Utility.Throw(new InnerException(u.GetData().ExceptionMessage("invalid argument name"))));
                    }
                    this.function.Add(name, new UserDefineCoRoutine(name, lis, trees.Skip(i), this));
                }
            }
        }

        public ScriptRunner(
            string filename,
            IEnumerable<string> source_code,
            Dictionary<string,IFunction> default_function,
            Dictionary<string,SFunction> default_command,
            IEnumerable<Func<string,dynamic>> literal_checker):
                this(filename,source_code,new Dictionary<string, dynamic>(),default_function,default_command,new Dictionary<string, dynamic>(),literal_checker)
        {}

        public ScriptRunner(
            string filename,
            IEnumerable<string> source_code):
                this(filename, source_code, DefaultFunction.GetDefaultFunctions(), DefaultFunction.GetDefaultCommand(),DefaultFunction.GetDefaultLiteralChecker())
        {
            
        }

        public ScriptRunner(string filename)
        {
            this.filename = filename;
            this.global = new Dictionary<string, dynamic>();
            this.function = DefaultFunction.GetDefaultFunctions();
            this.system_command = DefaultFunction.GetDefaultCommand();
            this.constant_value = new Dictionary<string, dynamic>();
            this.literal_checker = DefaultFunction.GetDefaultLiteralChecker();
            var lis = new List<string>();
            using (var stream = new System.IO.StreamReader(filename))
            {
                while (stream.Peek() >= 0)
                {
                    lis.Add(stream.ReadLine());
                }
            }
            var trees = lis.Select((t, index) => new Tree(t, index, filename) as TokenTree);
            
            int i = 0;
            foreach (var v in trees)
            {
                ++i;
                var tree = v.GetTree();
                if (tree.Count == 0)
                    continue;
                var top = tree.First().GetToken() ?? Utility.Throw(new InnerException(tree.First().GetData().ExceptionMessage("invalid command")));
                SFunction func;
                if (system_command.TryGetValue(top, out func))
                {
                    int l = func.ArgumentLength;

                    if (l == -1)
                    {
                        var d = new List<dynamic>();
                        int u = 0;
                        foreach (var e in tree.Skip(1))
                        {
                            var expr = Expression.MakeExpression(tree.Skip(++u).First(), this, new NullFunction(), e.GetData());
                            var k = expr.StaticEval(this);
                            if (k != null)
                                throw new InnerException(e.GetData().ExceptionMessage("this expression is not constant."));
                            d.Add(k);
                        }
                        func.Call(d.ToArray(), this, tree[0].GetData());
                    }
                    else
                    {
                        if (tree.Count() <= func.ArgumentLength)
                            throw new InnerException(top.GetData().ExceptionMessage(string.Format("too few arguments to function '{0}'", func.Name)));
                        var d = new List<dynamic>();
                        for (int m = 0; m < func.ArgumentLength - 1; ++m)
                        {
                            var tu = tree[m + 1];
                            var expr = Expression.MakeExpression(tu, this, new NullFunction(), tu.GetData());
                            var k = expr.StaticEval(this);
                            if (k != null)
                                throw new InnerException(tree[m + 1].GetData().ExceptionMessage("this expression is not constant."));
                            d.Add(k);
                        }
                        var t = tree.Skip(func.ArgumentLength);
                        d.Add(Expression.MakeExpression(t, this, new NullFunction(), t.First().GetData()));
                        func.Call(d.ToArray(), this, tree[0].GetData());
                    }
                }

                if (top == "const")
                {
                    if (tree.Count < 3)
                        throw new InnerException(tree.First().GetData().ExceptionMessage("too few argument to 'const'"));
                    var name = tree.Skip(1).First().GetToken() ?? Utility.Throw(new InnerException(tree.Skip(1).First().GetData().ExceptionMessage("invalid constant value name"))) as string;
                    if (this.constant_value.ContainsKey(name))
                        throw new InnerException(tree.First().GetData().ExceptionMessage(string.Format("constant value '{0}' has been defined", name)));
                    var val = Expression.MakeExpression(tree.Skip(2), this, new NullFunction(), tree.First().GetData()).StaticEval(this);
                }
                if (top == "def")
                {
                    if (tree.Count == 1)
                        throw new InnerException(tree[0].GetData().ExceptionMessage("too few argument to 'def'"));
                    var name = tree[1].GetToken() ?? Utility.Throw(new InnerException(tree[1].GetData().ExceptionMessage("invalid function name")));
                    var cas = new List<string>();
                    foreach (var u in tree.Skip(2))
                    {
                        cas.Add(u.GetToken() ?? Utility.Throw(new InnerException(u.GetData().ExceptionMessage("invalid argument name"))));
                    }
                    this.function.Add(name, new UserDefinedFunction(name, cas, trees.Skip(i), this));
                }
                if (top == "async")
                {
                    if (tree.Count == 1)
                        throw new InnerException(tree[0].GetData().ExceptionMessage("too few argument to 'def'"));
                    var name = tree[1].GetToken() ?? Utility.Throw(new InnerException(tree[1].GetData().ExceptionMessage("invalid function name")));
                    var cas = new List<string>();
                    foreach (var u in tree.Skip(2))
                    {
                        cas.Add(u.GetToken() ?? Utility.Throw(new InnerException(u.GetData().ExceptionMessage("invalid argument name"))));
                    }
                    this.function.Add(name, new UserDefineCoRoutine(name, cas, trees.Skip(i), this));
                }
            }
        }
        public dynamic Run(string entry_point,dynamic[] varg)
        {
            IFunction func;
            if (!function.TryGetValue(entry_point, out func))
                throw new InnerException(CodeData.ExceptionMessage(string.Format("entry point '{0}' is not exist.", entry_point), 0, 0, filename));
            return func.ArgumentLength == 0 ? func.Call(new dynamic[0], new CodeData(0, 0, filename)) :
                func.ArgumentLength == 1 ? func.Call(new dynamic[1] { varg }, new CodeData(0, 0, filename)) :
                func.Call(new dynamic[2] { varg, varg.Length }, new CodeData(0, 0, filename));
        }

        
    }
}
