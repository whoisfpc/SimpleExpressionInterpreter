using System.Text;
using System.Collections.Generic;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class RootExpression : Expression
    {
        public Expression exp;

        public RootExpression(Expression exp)
        {
            this.exp = exp;
        }

        public override void Dump(StringBuilder stringBuilder, int indent)
        {
            base.Dump(stringBuilder, indent);
            char[] indentChars = new char[indent];
            for (int i = 0; i < indent; i++)
            {
                indentChars[i] = ' ';
            }
            stringBuilder.Append(indentChars).Append("Root(").AppendLine();
            exp.Dump(stringBuilder, indent+2);
            stringBuilder.Append(indentChars).Append(")").AppendLine();
        }

        public override void Compile(List<byte> bytecodes)
        {
            exp.Compile(bytecodes);
        }
    }
}
