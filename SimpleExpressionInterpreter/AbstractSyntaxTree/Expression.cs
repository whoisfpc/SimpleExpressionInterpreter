using System.Text;
using System.Collections.Generic;

namespace ExpressionInterpreter.AbstractSyntaxTree
{
    public abstract class Expression : BaseTree
    {
        public enum Operator
        {
            Add,
            Sub,
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

        public abstract void Compile(List<byte> bytecodes);
    }
}
