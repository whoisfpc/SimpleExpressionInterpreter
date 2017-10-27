using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine();
                Console.Write("input expression:");
                var source = Console.ReadLine();
                var postfix = Convert2Postfix(source);
                Console.WriteLine("postfix expression: {0}", postfix);
                Console.WriteLine("calcuate result: {0}", CalcPostfix(postfix));
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }

        static string Convert2Postfix(string source)
        {
            StringBuilder postfix = new StringBuilder(source.Length);
            Stack<char> mark = new Stack<char>();
            Dictionary<char, int> priority = new Dictionary<char, int>();
            priority['+'] = 0;
            priority['-'] = 0;
            priority['*'] = 1;
            priority['/'] = 1;
            char last = ' ';

            foreach (var ch in source)
            {
                if ((ch >= '0' && ch <= '9') || ch == '.')
                {
                    postfix.Append(ch);
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                {
                    if (last >= '0' && last <= '9')
                    {
                        postfix.Append('#');
                    }
                    if (mark.Count > 0)
                    {
                        char top = mark.Peek();
                        while (top != '(' && priority[ch] <= priority[top])
                        {
                            postfix.Append(top);
                            mark.Pop();
                            if (mark.Count == 0)
                            {
                                break;
                            }
                            top = mark.Peek();
                        }
                    }
                    mark.Push(ch);
                }
                else if (ch == '(')
                {
                    if (last >= '0' && last <= '9')
                    {
                        postfix.Append('#');
                        mark.Push('*');
                    }
                    mark.Push(ch);
                }
                else if (ch == ')')
                {
                    postfix.Append('#');
                    while (mark.Peek() != '(')
                    {
                        postfix.Append(mark.Pop());
                    }
                    mark.Pop();
                }
                last = ch;
            }
            if (source.Last() != ')')
            {
                postfix.Append('#');
            }
            while (mark.Count > 0)
            {
                postfix.Append(mark.Pop());
            }
            return postfix.ToString();
        }

        static float CalcPostfix(string postfix)
        {
            Stack<float> result = new Stack<float>();
            StringBuilder numBuilder = new StringBuilder();
            float tmp;
            foreach(var ch in postfix)
            {
                if ((ch >= '0' && ch <= '9') || ch == '.')
                {
                    numBuilder.Append(ch);
                }
                else if (ch == '+')
                {
                    tmp = result.Pop();
                    tmp = result.Pop() + tmp;
                    result.Push(tmp);
                }
                else if (ch == '-')
                {
                    tmp = result.Pop();
                    tmp = result.Pop() - tmp;
                    result.Push(tmp);
                }
                else if (ch == '*')
                {
                    tmp = result.Pop();
                    tmp = result.Pop() * tmp;
                    result.Push(tmp);
                }
                else if (ch == '/')
                {
                    tmp = result.Pop();
                    tmp = result.Pop() / tmp;
                    result.Push(tmp);
                }
                else if (ch == '#')
                {
                    result.Push(float.Parse(numBuilder.ToString()));
                    numBuilder.Clear();
                }
            }
            return result.Pop();
        }
    }
}
