using System.Collections.Generic;
using SimpleExpressionInterpreter.Tokens;

namespace SimpleExpressionInterpreter
{
    public class Parser
    {
        private Dictionary<TokenType, int> priority;

        public Parser()
        {
            priority = new Dictionary<TokenType, int>();
            priority[TokenType.Plus] = 0;
            priority[TokenType.Minus] = 0;
            priority[TokenType.Mul] = 1;
            priority[TokenType.Div] = 1;
        }

        // TODO: 需要构造抽象语法树来提供更好的扩展性, 现在先返回后缀表达式，没有容错处理
        public IList<Token> Parse(Lexer lexer)
        {
            List<Token> postfix = new List<Token>();
            Stack<Token> mark = new Stack<Token>();

            foreach (var token in lexer)
            {
                switch (token.tokenType)
                {
                    case TokenType.Id:
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
                            while (top.tokenType != TokenType.LP
                                && priority[token.tokenType] <= priority[top.tokenType])
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
                        continue;
                }
            }
            while (mark.Count > 0)
            {
                postfix.Add(mark.Pop());
            }
            return postfix;
        }
    }
}
