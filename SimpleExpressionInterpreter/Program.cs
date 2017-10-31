using System;

namespace SimpleExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            var compiler = new Compiler();
            var executor = new Executor();
            do
            {
                Console.WriteLine();
                Console.Write("input expression:");
                var source = Console.ReadLine();

                var bytecodes = compiler.Compile(source);
                compiler.PrintBytecode(bytecodes);
                var result = executor.Execute(bytecodes);

                Console.WriteLine("calcuate result: {0}", result);
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
