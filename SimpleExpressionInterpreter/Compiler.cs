using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ExpressionInterpreter
{
    public class Compiler
    {
        public byte[] Compile(string source)
        {
            var inputStream = new AntlrInputStream(source);
            var lexer = new ExprLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokens);
            var tree = parser.prog();
            if (parser.NumberOfSyntaxErrors > 0)
            {
                Console.WriteLine("{0} syntax errors found, compile failed, 现在并不想写优雅的错误处理", parser.NumberOfSyntaxErrors);
                return null;
            }
            var compileListener = new ExprCompileListener();
            try
            {
                ParseTreeWalker.Default.Walk(compileListener, tree);
            }
            catch (ParseFailedException e)
            {
                Console.WriteLine(e.Message);
            }
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
                    case Instruction.Call:
                        var funcId = BitConverter.ToInt32(bytes, i);
                        Console.WriteLine("{0}\t function id: {1}", inst, VirtualMachine.GetPredefineFuncName(funcId));
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
            var inputStream = new AntlrInputStream(source);
            var lexer = new ExprLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokens);
            var tree = parser.prog();
            var printListener = new ExprPrintListener();
            ParseTreeWalker.Default.Walk(printListener, tree);

            var s = printListener.AbsynString;
            Console.Write(s);
        }
    }
}
