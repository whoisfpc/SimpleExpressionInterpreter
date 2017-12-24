using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ExpressionInterpreter
{
    // 暂时先通过解析后缀表达式来生成bytecode
    public class Compiler
    {
        public byte[] Compile(string source)
        {
            var inputStream = new AntlrInputStream(source);
            var lexer = new ExprLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokens);

            var compileListener = new ExprCompileListener();
            ParseTreeWalker.Default.Walk(compileListener, parser.prog());

            return compileListener.bytecodes.ToArray();
        }

        public void PrintBytecode(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }
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
            throw new NotImplementedException();
        }
    }
}
