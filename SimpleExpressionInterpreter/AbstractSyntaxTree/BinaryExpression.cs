using System.Collections.Generic;
using System.Text;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class BinaryExpression : Expression
    {
        public override int Position => position;

        public Operator op;
        public Expression left;
        public Expression right;
        private int position;

        public BinaryExpression(int position, Operator op, Expression left, Expression right)
        {
            this.position = position;
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

        public override void Compile(List<byte> bytecodes)
        {
            left.Compile(bytecodes);
            right.Compile(bytecodes);
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
    }
}
