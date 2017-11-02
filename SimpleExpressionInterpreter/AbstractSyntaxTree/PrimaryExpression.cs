using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class PrimaryExpression : Expression
    {
        public enum PrimaryType
        {
            Id,
            Num
        }

        public override int Position => position;

        public string value;
        public PrimaryType primaryType;

        private int position;

        public PrimaryExpression(int position, PrimaryType primaryType, string value)
        {
            this.position = position;
            this.primaryType = primaryType;
            this.value = value;
        }

        public override void Dump(StringBuilder stringBuilder, int indent)
        {
            base.Dump(stringBuilder, indent);
            char[] indentChars = new char[indent];
            for (int i = 0; i < indent; i++)
            {
                indentChars[i] = ' ';
            }
            stringBuilder.Append(indentChars).Append("Primary "+ primaryType +"("+value+")").AppendLine();
        }

        public override void Compile(List<byte> bytecodes)
        {
            if (primaryType == PrimaryType.Id)
            {
                bytecodes.Add((byte)Instruction.PushVariable);
                bytecodes.AddRange(BitConverter.GetBytes(int.Parse(value.Substring(1))));
            }
            else
            {
                bytecodes.Add((byte)Instruction.PushLiteral);
                bytecodes.AddRange(BitConverter.GetBytes(float.Parse(value)));
            }
        }
    }
}
