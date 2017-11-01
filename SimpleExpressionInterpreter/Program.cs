using System;
using System.Collections.Generic;

namespace SimpleExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            var compiler = new Compiler();
            var executor = new Executor();
            var variables = new Dictionary<int, float> { { 1, 15 }, { 2, 27.2f }, { 0, 777 } };
            do
            {
                Console.WriteLine();
                Console.Write("input expression:");
                var source = Console.ReadLine();

                var bytecodes = compiler.Compile(source);
                compiler.PrintBytecode(bytecodes);
                var result = executor.Execute(bytecodes,variables);

                Console.WriteLine("calcuate result: {0}", result);
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }

    static class VarMap
    {
        public static int GetVarId(string var)
        {
            switch (var)
            {
                case "a":
                    return 1;
                case "b":
                    return 2;
                default:
                    break;
            }
            return 0;
        }
    }
}
