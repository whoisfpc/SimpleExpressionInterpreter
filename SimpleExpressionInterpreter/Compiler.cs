using System;
using System.Collections.Generic;

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
            var bytes = new List<byte>();
            var absyn = parser.Parse(lexer.Analyse(source));
            absyn.Compile(bytes);
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

        public void PrintAbsyn(string source)
        {
            var absyn = parser.Parse(lexer.Analyse(source));
            Console.Write(absyn.Dump());
        }
    }
}
