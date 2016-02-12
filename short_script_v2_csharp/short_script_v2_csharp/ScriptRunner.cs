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
            foreach (var t in trees)
            {
                ++i;
                var tree = t.GetTree();
                if (tree.Count == 0)
                    continue;
                var top = tree.First().GetToken();
                if (top != "def") continue;
                if (tree.Count == 1)
                    throw new InnerException(t.GetData().ExceptionMessage("null function name."));
                var name = tree.Skip(1).First().GetToken() ?? Utility.Throw(new InnerException(t.GetData().ExceptionMessage("invalid function name."))) as string;
                List<string> arg = new List<string>();
                foreach(var u in tree.Skip(2))
                {
                    arg.Add(u.GetToken() ?? Utility.Throw(new InnerException("invalid argument name.")) as string);
                }
                var sentence = Sentence.MakeSentence(trees.Skip(i), this);
                function.Add(name, new UserDefinedFunction(name, arg, sentence, this));
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

        public dynamic Run(string entry_point,dynamic[] varg)
        {
            IFunction func;
            if (!function.TryGetValue(entry_point, out func))
                throw new InnerException(CodeData.ExceptionMessage(string.Format("entry point '{0}' is not exist.", entry_point), 0, 0, filename));
            return func.ArgumentLength == 0 ? func.Call(new dynamic[0], new CodeData(0, 0, filename)) :
                func.ArgumentLength == 1 ? func.Call(new dynamic[1] { varg }, new CodeData(0, 0, filename)) :
                func.Call(new dynamic[2] { varg, varg.Length }, new CodeData(0, 0, filename));
        }

        public ScriptRunner(string filename)
        {
            var source_code = new List<string>();
            using (var reader = new System.IO.StreamReader(filename))
            {
                while (reader.Peek() >= 0)
                {
                    source_code.Add(reader.ReadLine());
                }
            }
            this.filename = filename;
            this.global = new Dictionary<string, dynamic>();
            this.function = DefaultFunction.GetDefaultFunctions();
            this.system_command = DefaultFunction.GetDefaultCommand();
            this.constant_value = new Dictionary<string, dynamic>();
            this.literal_checker = DefaultFunction.GetDefaultLiteralChecker();
            var trees = source_code.Select((t, index) => new Tree(t, index, filename) as TokenTree);
            int i = 0;
            foreach (var t in trees)
            {
                ++i;
                var tree = t.GetTree();
                if (tree.Count == 0)
                    continue;
                var top = tree.First().GetToken();
                if (top != "def") continue;
                if (tree.Count == 1)
                    throw new InnerException(t.GetData().ExceptionMessage("null function name."));
                var name = tree.Skip(1).First().GetToken() ?? Utility.Throw(new InnerException(t.GetData().ExceptionMessage("invalid function name."))) as string;
                List<string> arg = new List<string>();
                foreach (var u in tree.Skip(2))
                {
                    arg.Add(u.GetToken() ?? Utility.Throw(new InnerException("invalid argument name.")) as string);
                }
                var sentence = Sentence.MakeSentence(trees.Skip(i), this);
                function.Add(name, new UserDefinedFunction(name, arg, sentence, this));
            }
        }
    }
}
