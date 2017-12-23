using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ExpressionInterpreter
{
    public class ExprCompileListener : ExprBaseListener
    {
        public override void EnterBinaryExpr([NotNull] ExprParser.BinaryExprContext context)
        {
            Console.WriteLine("Enter binary expr");
        }

        public override void EnterTermExpr([NotNull] ExprParser.TermExprContext context)
        {
            Console.WriteLine("Enter term expr");
        }

        public override void EnterProg([NotNull] ExprParser.ProgContext context)
        {
            Console.WriteLine("Enter prog");
        }

        public override void EnterSingleExpr([NotNull] ExprParser.SingleExprContext context)
        {
            Console.WriteLine("Enter single expr");
        }

        public override void EnterTerm([NotNull] ExprParser.TermContext context)
        {
            Console.WriteLine("Enter term");
        }
    }
}
