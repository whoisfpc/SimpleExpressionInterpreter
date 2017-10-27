using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleExpressionInterpreter.Tokens;

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
                var postfix = Convert2Postfix(new Lexer(source));
                Console.Write("postfix expression: ");
                foreach (var token in postfix)
                {
                    Console.Write("{0} ", token);
                }
                Console.WriteLine();
                Console.WriteLine("calcuate result: {0}", CalcPostfix(postfix));
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }

        static IList<Token> Convert2Postfix(Lexer lexer)
        {
            List<Token> postfix = new List<Token>();
            Stack<Token> mark = new Stack<Token>();
            Dictionary<TokenType, int> priority = new Dictionary<TokenType, int>();
            priority[TokenType.Plus] = 0;
            priority[TokenType.Minus] = 0;
            priority[TokenType.Mul] = 1;
            priority[TokenType.Div] = 1;
            Token last = new None();

            foreach(var token in lexer)
            {
                switch (token.tokenType)
                {
                    case TokenType.Num:
                        postfix.Add(token);
                        break;
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Mul:
                    case TokenType.Div:
                        if (mark.Count > 0)
                        {
                            var top = mark.Peek();
                            while(top.tokenType != TokenType.LP
                                && priority[token.tokenType] <= priority[token.tokenType])
                            {
                                postfix.Add(top);
                                mark.Pop();
                                if (mark.Count == 0)
                                {
                                    break;
                                }
                                top = mark.Peek();
                            }
                        }
                        mark.Push(token);
                        break;
                    case TokenType.LP:
                        if (last.tokenType == TokenType.Num)
                        {
                            mark.Push(new Mul());
                        }
                        mark.Push(token);
                        break;
                    case TokenType.RP:
                        while (mark.Peek().tokenType != TokenType.LP)
                        {
                            postfix.Add(mark.Pop());
                        }
                        mark.Pop();
                        break;
                    default:
                        break;
                }
                last = token;
            }
            while (mark.Count > 0)
            {
                postfix.Add(mark.Pop());
            }
            return postfix;
        }

        static float CalcPostfix(IList<Token> postfix)
        {
            Stack<float> result = new Stack<float>();
            float tmp;
            foreach(var token in postfix)
            {
                switch (token.tokenType)
                {
                    case TokenType.Num:
                        result.Push(float.Parse(token.value));
                        break;
                    case TokenType.Plus:
                        tmp = result.Pop();
                        tmp = result.Pop() + tmp;
                        result.Push(tmp);
                        break;
                    case TokenType.Minus:
                        tmp = result.Pop();
                        tmp = result.Pop() - tmp;
                        result.Push(tmp);
                        break;
                    case TokenType.Mul:
                        tmp = result.Pop();
                        tmp = result.Pop() * tmp;
                        result.Push(tmp);
                        break;
                    case TokenType.Div:
                        tmp = result.Pop();
                        tmp = result.Pop() / tmp;
                        result.Push(tmp);
                        break;
                    default:
                        break;
                }
            }
            return result.Pop();
        }
    }
}
