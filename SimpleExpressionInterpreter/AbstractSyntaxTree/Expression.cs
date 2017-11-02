using System.Text;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public class Expression : BaseTree
    {
        public enum Operator
        {
            Add,
            Minus,
            Mul,
            Div
        }

        public virtual void Dump(StringBuilder stringBuilder, int indent)
        {
            if (stringBuilder == null)
            {
                return;
            }
        }

        public virtual string Dump()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dump(stringBuilder, 0);
            return stringBuilder.ToString();
        }
    }
}
