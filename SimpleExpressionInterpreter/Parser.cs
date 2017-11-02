using System;
using System.Collections.Generic;
using ExpressionInterpreter.Tokens;
using ExpressionInterpreter.AbstractSyntaxTree;

/// <summary>
/// RootExpression = 
///     | Expression
///     ;
/// 
/// Expression = 
///     | BinaryExpression
///     | PrimaryExprssion
///     | (Expression)
///     ;
/// 
/// BinaryExpression = 
///     | Expression + Expression
///     | Expression - Expression
///     | Expression * Expression
///     | Expression / Expression
///     ;
///     
/// PrimaryExpression
///     | Id
///     | Num
///     ;
/// </summary>

namespace ExpressionInterpreter
{
    public class Parser
    {
        private Dictionary<TokenType, int> priority;

        public Parser()
        {
            priority = new Dictionary<TokenType, int>
            {
                [TokenType.Plus] = 0,
                [TokenType.Minus] = 0,
                [TokenType.Mul] = 1,
                [TokenType.Div] = 1
            };
        }

        public RootExpression Parse(Lexer lexer)
        {
            Stack<Expression> stack = new Stack<Expression>();
            IList<Token> postfix;
            try
            {
                postfix = Infix2Postfix(lexer);
            }
            catch (LexicalInvalidException e)
            {
                throw new ParseFailedException("parse failed at Infix2Postfix", e);
            }
            Expression tmp1 = null;
            Expression tmp2 = null;
            foreach (var token in postfix)
            {
                switch (token.tokenType)
                {
                    case TokenType.Id:
                        stack.Push(new PrimaryExpression(token.position, PrimaryExpression.PrimaryType.Id, token.value));
                        break;
                    case TokenType.Num:
                        stack.Push(new PrimaryExpression(token.position, PrimaryExpression.PrimaryType.Num, token.value));
                        break;
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Mul:
                    case TokenType.Div:
                        if (TryPop(stack, ref tmp1) && TryPop(stack, ref tmp2))
                        {
                            stack.Push(new BinaryExpression(
                                tmp2.Position,
                                Token2Operator(token.tokenType),
                                tmp2,
                                tmp1
                                ));
                        }
                        else
                        {
                            throw new ParseFailedException("parse failed, pop on a empty stack");
                        }
                        break;
                    default:
                        throw new ParseFailedException("parse failed, Parentheses don't match");
                }
            }
            return new RootExpression(0, stack.Pop());
        }

        private Expression.Operator Token2Operator(TokenType t)
        {
            switch (t)
            {
                case TokenType.Plus:
                    return Expression.Operator.Add;
                case TokenType.Minus:
                    return Expression.Operator.Sub;
                case TokenType.Mul:
                    return Expression.Operator.Mul;
                case TokenType.Div:
                    return Expression.Operator.Div;
                default:
                    Console.WriteLine("这不可能发生。。。");
                    return Expression.Operator.Add;
            }
        }

        private IList<Token> Infix2Postfix(Lexer lexer)
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
                        throw new LexicalInvalidException(
                            string.Format("unexpected character {0}, at {1}", token.value, token.position)
                            );
                }
            }
            while (mark.Count > 0)
            {
                postfix.Add(mark.Pop());
            }
            return postfix;
        }

        private bool TryPop<T>(Stack<T> stack, ref T value)
        {
            if (stack.Count > 0)
            {
                value = stack.Pop();
                return true;
            }
            return false;
        }
    }
}
