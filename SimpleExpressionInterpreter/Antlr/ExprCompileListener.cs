using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ExpressionInterpreter
{
    public class ExprCompileListener : ExprBaseListener
    {
        public List<byte> bytecodes =  new List<byte>();

        public override void ExitProg([NotNull] ExprParser.ProgContext context)
        {
            base.ExitProg(context);
            bytecodes.Add((byte)Instruction.Ret);
        }

        public override void ExitBinaryExpr([NotNull] ExprParser.BinaryExprContext context)
        {
            var op = BinaryOp(context);
            switch (op)
            {
                case Operator.Add:
                    bytecodes.Add((byte)Instruction.Add);
                    break;
                case Operator.Sub:
                    bytecodes.Add((byte)Instruction.Sub);
                    break;
                case Operator.Mul:
                    bytecodes.Add((byte)Instruction.Mul);
                    break;
                case Operator.Div:
                    bytecodes.Add((byte)Instruction.Div);
                    break;
            }
        }

        public override void EnterTerm([NotNull] ExprParser.TermContext context)
        {
            if (context.NUM() != null)
            {
                bytecodes.Add((byte)Instruction.PushLiteral);
            }
            else if (context.ID() != null)
            {
                bytecodes.Add((byte)Instruction.PushVariable);
            }
        }

        public override void ExitTerm([NotNull] ExprParser.TermContext context)
        {
            if (context.NUM() != null)
            {
                var value = context.NUM().GetText();
                bytecodes.AddRange(BitConverter.GetBytes(float.Parse(value)));
            }
            else if (context.ID() != null)
            {
                var value = context.ID().GetText();
                bytecodes.AddRange(BitConverter.GetBytes(int.Parse(value.Substring(1))));
            }
        }

        private enum Operator
        {
            None,
            Add,
            Sub,
            Mul,
            Div
        }
        private Operator BinaryOp([NotNull] ExprParser.BinaryExprContext context)
        {
            if (context.ADD() != null)
            {
                return Operator.Add;
            }
            else if (context.SUB() != null)
            {
                return Operator.Sub;
            }
            else if (context.MUL() != null)
            {
                return Operator.Mul;
            }
            else if (context.DIV() != null)
            {
                return Operator.Div;
            }
            return Operator.None;
        }
    }
}
