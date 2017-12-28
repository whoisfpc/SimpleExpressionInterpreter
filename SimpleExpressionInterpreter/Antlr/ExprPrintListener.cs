using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace ExpressionInterpreter
{
    public class ExprPrintListener : ExprBaseListener
    {
        private int indent = 0;
        private StringBuilder builder = new StringBuilder();
        public string AbsynString
        {
            get { return builder.ToString(); }
        }

        private void AddIndent()
        {
            for (int i = 0; i < indent; i++)
            {
                if (i % 2 == 0)
                    builder.Append('|');
                else
                    builder.Append(' ');
            }
        }

        public override void EnterProg([NotNull] ExprParser.ProgContext context)
        {
            AddIndent();
            builder.Append("Prog (\n");
            indent+=2;
        }

        public override void ExitProg([NotNull] ExprParser.ProgContext context)
        {
            indent-=2;
            AddIndent();
            builder.Append(")\n");
        }

        public override void EnterBinaryExpr([NotNull] ExprParser.BinaryExprContext context)
        {
            AddIndent();
            builder.Append("BinOP ");
            if (context.ADD() != null)
            {
                builder.Append('+');
            }
            else if (context.SUB() != null)
            {
                builder.Append('-');
            }
            else if (context.MUL() != null)
            {
                builder.Append('*');
            }
            else if (context.DIV() != null)
            {
                builder.Append('/');
            }
            builder.Append("(\n");
            indent+=2;
        }

        public override void ExitBinaryExpr([NotNull] ExprParser.BinaryExprContext context)
        {
            indent-=2;
            AddIndent();
            builder.Append(")\n");
        }

        public override void EnterFuncExpr([NotNull] ExprParser.FuncExprContext context)
        {
            AddIndent();
            builder.Append("func" + context.ID().GetText() + " (");
            if (context.exprList() != null)
            {
                builder.Append('\n');
            }
            indent+=2;
        }

        public override void ExitFuncExpr([NotNull] ExprParser.FuncExprContext context)
        {
            indent-=2;
            AddIndent();
            builder.Append(")\n");
        }

        public override void EnterTerm([NotNull] ExprParser.TermContext context)
        {
            AddIndent();
            if (context.NUM() != null)
            {
                builder.Append(context.NUM().GetText());
            }
            else
            {
                builder.Append(context.PREVAR().GetText());
            }
            builder.Append('\n');
        }
    }
}
