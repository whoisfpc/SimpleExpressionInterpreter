using System;
using System.Collections.Generic;

namespace ExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            var compiler = new Compiler();
            var executor = new Executor();
            var variables = new Dictionary<int, float> { { 1, 15 }, { 2, 27.2f }, { 0, 777 } };
            Console.WriteLine("variables, {0}={1}, {2}={3}", VarMap.GetVarName(1), variables[1], VarMap.GetVarName(2), variables[2]);
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

        public static string GetVarName(int varId)
        {
            switch (varId)
            {
                case 1:
                    return "a";
                case 2:
                    return "b";
                default:
                    return "none";
            }
        }
    }
}
