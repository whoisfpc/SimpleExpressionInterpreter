using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            Console.WriteLine("predefined variables: 1, 2, 3");
            do
            {
                Console.WriteLine();
                Console.Write("input expression:");
                var source = Console.ReadLine();
                var inputStream = new AntlrInputStream(source);
                var lexer = new ExprLexer(inputStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new ExprParser(tokens);
                var compileListener = new ExprCompileListener();

                ParseTreeWalker.Default.Walk(compileListener, parser.prog());

                //Console.WriteLine(parser.prog().ToStringTree());
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
