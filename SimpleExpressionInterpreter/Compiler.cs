using System;
using System.Collections.Generic;
using ExpressionInterpreter.Tokens;

namespace ExpressionInterpreter
{
    // 暂时先通过解析后缀表达式来生成bytecode
    public class Compiler
    {
        private Lexer lexer;
        private Parser parser;

        public Compiler()
        {
            lexer = new Lexer();
            parser = new Parser();
        }

        public byte[] Compile(string source)
        {
            foreach (var token in lexer.Analyse(source))
            {
                Console.WriteLine("{0}: {1}", token.tokenType, token.value);
            }
            var postfix = parser.Parse(lexer.Analyse(source));

            Console.Write("postfix expression: ");
            foreach (var token in postfix)
            {
                Console.Write("{0} ", token);
            }
            Console.WriteLine();

            var bytes = new List<byte>();

            foreach (var token in postfix)
            {
                switch (token.tokenType)
                {
                    case TokenType.Id:
                        bytes.Add((byte)(Instruction.PushVariable));
                        bytes.AddRange(BitConverter.GetBytes(int.Parse(token.value.Substring(1))));
                        break;
                    case TokenType.Num:
                        bytes.Add((byte)Instruction.PushLiteral);
                        bytes.AddRange(BitConverter.GetBytes(float.Parse(token.value)));
                        break;
                    case TokenType.Plus:
                        bytes.Add((byte)Instruction.Add);
                        break;
                    case TokenType.Minus:
                        bytes.Add((byte)Instruction.Sub);
                        break;
                    case TokenType.Mul:
                        bytes.Add((byte)Instruction.Mul);
                        break;
                    case TokenType.Div:
                        bytes.Add((byte)Instruction.Div);
                        break;
                    default:
                        Console.WriteLine("Error: unexpect token");
                        break;
                }
            }
            bytes.Add((byte)Instruction.Ret);

            return bytes.ToArray();
        }

        public void PrintBytecode(byte[] bytes)
        {
            Console.WriteLine("===============print bytecode===============");
            int i = 0;
            while (i < bytes.Length)
            {
                var inst = (Instruction)bytes[i];
                i++;
                switch (inst)
                {
                    case Instruction.PushLiteral:
                        var num = BitConverter.ToSingle(bytes, i);
                        Console.WriteLine("{0}\t{1}", inst, num);
                        i += 4;
                        break;
                    case Instruction.PushVariable:
                        var varId = BitConverter.ToInt32(bytes, i);
                        Console.WriteLine("{0}\tid: {1}", inst, varId);
                        i += 4;
                        break;
                    default:
                        Console.WriteLine("{0}", inst);
                        break;
                }
            }
            Console.WriteLine("===============print bytecode===============");
        }
    }
}
