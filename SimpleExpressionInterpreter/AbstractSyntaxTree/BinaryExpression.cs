using System.Text;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class BinaryExpression : Expression
    {
        public Operator op;
        public Expression left;
        public Expression right;

        public BinaryExpression(Operator op, Expression left, Expression right)
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }

        public override void Dump(StringBuilder stringBuilder, int indent)
        {
            base.Dump(stringBuilder, indent);
            char[] indentChars = new char[indent];
            for (int i = 0; i < indent; i++)
            {
                indentChars[i] = ' ';
            }
            stringBuilder.Append(indentChars).Append("Binary("+op).AppendLine();
            left.Dump(stringBuilder, indent + 2);
            right.Dump(stringBuilder, indent + 2);
            stringBuilder.Append(indentChars).Append(")").AppendLine();
        }
    }
}
