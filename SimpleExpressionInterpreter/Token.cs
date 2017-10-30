using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpressionInterpreter.Tokens
{
    public enum TokenType
    {
        /// <summary>
        /// 无用类型
        /// </summary>
        None,
        /// <summary>
        /// 标识符
        /// </summary>
        Id,
        /// <summary>
        /// 数字
        /// </summary>
        Num,
        /// <summary>
        /// 加法运算符
        /// </summary>
        Plus,
        /// <summary>
        /// 减法运算符
        /// </summary>
        Minus,
        /// <summary>
        /// 乘法运算符
        /// </summary>
        Mul,
        /// <summary>
        /// 除法运算符
        /// </summary>
        Div,
        /// <summary>
        /// 左括号
        /// </summary>
        LP,
        /// <summary>
        /// 有括号
        /// </summary>
        RP
    }

    public abstract class Token
    {
        /// <summary>
        /// Token的类型
        /// </summary>
        public TokenType tokenType;
        /// <summary>
        /// 对应的值，（对于Var， Num类型），延后实际值的转换
        /// </summary>
        public string value;

        public override string ToString()
        {
            return value;
        }
    }

    public sealed class None : Token
    {
        public None()
        {
            tokenType = TokenType.None;
        }
    }

    public sealed class Id : Token
    {
        public Id(string value)
        {
            tokenType = TokenType.Id;
            this.value = value;
        }
    }

    public sealed class Num : Token
    {
        public Num(string value)
        {
            tokenType = TokenType.Num;
            this.value = value;
        }
    }

    public sealed class Plus : Token
    {
        public Plus()
        {
            tokenType = TokenType.Plus;
            this.value = "+";
        }
    }

    public sealed class Minus : Token
    {
        public Minus()
        {
            tokenType = TokenType.Minus;
            this.value = "-";
        }
    }

    public sealed class Mul : Token
    {
        public Mul()
        {
            tokenType = TokenType.Mul;
            this.value = "*";
        }
    }

    public sealed class Div : Token
    {
        public Div()
        {
            tokenType = TokenType.Div;
            this.value = "/";
        }
    }

    public sealed class LP : Token
    {
        public LP()
        {
            tokenType = TokenType.LP;
            this.value = "(";
        }
    }

    public sealed class RP : Token
    {
        public RP()
        {
            tokenType = TokenType.RP;
            this.value = ")";
        }
    }
}
