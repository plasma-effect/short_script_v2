using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortScriptV2
{
    public interface TokenTree
    {
        string GetToken();
        List<TokenTree> GetTree();
        CodeData GetData();
        string ToString();
        string ToString(int v);
    }

    public class Token : TokenTree
    {
        string token;
        CodeData data;

        public string GetToken()
        {
            return token;
        }

        public List<TokenTree> GetTree()
        {
            return null;
        }

        public CodeData GetData()
        {
            return data;
        }

        public string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += " ";
            return ret + token;
        }

        public override string ToString()
        {
            return ToString(-1);
        }

        public Token(string token,CodeData data)
        {
            this.token = token;
            this.data = data;
        }

        public Token(string str,CodeData data,ref int column,bool flag)
        {
            this.data = data;
            int fir = column - (flag ? 1 : 0);
            for(;column<str.Length;++column)
            {
                if (str[column] == (flag ? '"' : ' ')) 
                {
                    this.token = str.Substring(fir, column - fir + (flag ? 1 : 0));
                    return;
                }
            }
            this.token = str.Substring(fir, column - fir + (flag ? 1 : 0));
        }
    }

    public class Tree : TokenTree
    {
        List<TokenTree> tree;
        CodeData data;

        public CodeData GetData()
        {
            return data;
        }

        public string GetToken()
        {
            return null;
        }

        public List<TokenTree> GetTree()
        {
            return tree;
        }

        public string ToString(int v)
        {
            string ret = "";
            for (int i = 0; i < v; ++i)
                ret += " ";
            ret += "----";
            foreach (var u in tree)
            {
                ret += "\n" + u.ToString(v + 1);
            }
            return ret;
        }

        public override string ToString()
        {
            return ToString(-1);
        }

        public Tree(string str, int line, string filename)
        {
            int column = 0;
            this.tree = InitialParse(str, line, ref column, filename);
            this.data = new CodeData(line, 0, filename);
        }

        public Tree(List<TokenTree> tree,CodeData data)
        {
            this.tree = tree;
            this.data = data;
        }

        public List<TokenTree> InitialParse(string str,int line,ref int column,string filename)
        {
            var ret = new List<TokenTree>();
            for (; column < str.Length; ++column)
            {
                if (str[column] == '"')
                {
                    ret.Add(new Token(str, new CodeData(line, column, filename), ref column, true));
                }
                else if (str[column] == '(')
                {
                    ++column;
                    int c = column;
                    ret.Add(new Tree(InitialParse(str, line, ref column, filename), new CodeData(line, c, filename)));
                }
                else if (str[column] == ')' || str[column] == '#') 
                {
                    return ret;
                }
                else if(str[column]!='\t' && str[column]!=' ')
                {
                    ret.Add(new Token(str, new CodeData(line, column, filename), ref column, false));
                }
            }
            return ret;
        }
    }
}
