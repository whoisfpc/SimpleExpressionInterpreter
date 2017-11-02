using System.Text;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class PrimaryExpression : Expression
    {
        public string value;

        public PrimaryExpression(string value)
        {
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
            stringBuilder.Append(indentChars).Append("Primary("+value+")").AppendLine();
        }
    }
}
