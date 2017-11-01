namespace ExpressionInterpreter.Tokens
{
    public enum TokenType
    {
        /// <summary>
        /// 无效的Token
        /// </summary>
        Error,
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
        RP,
        /// <summary>
        /// 空格
        /// </summary>
        Space
    }

    public abstract class Token
    {
        /// <summary>
        /// Token的类型
        /// </summary>
        public TokenType tokenType;
        /// <summary>
        /// 对应的值，（对于Id， Num类型），延后实际值的转换
        /// </summary>
        public string value;
        /// <summary>
        /// 该Token是否需要被忽略，（如注释，空格等）
        /// </summary>
        public virtual bool Ignore => false;

        public override string ToString()
        {
            return value;
        }
    }

    [TokenRegex(@"\G.", Priority = 0)]
    public sealed class Error : Token
    {
        public Error(string value)
        {
            tokenType = TokenType.Error;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\s+", Priority = 1)]
    public sealed class Space : Token
    {
        public override bool Ignore => true;

        public Space(string value)
        {
            tokenType = TokenType.Space;
            this.value = value;
        }
    }

    [TokenRegex(@"\G[a-zA-Z$]([a-zA-Z$\d])*", Priority = 1)]
    public sealed class Id : Token
    {
        public Id(string value)
        {
            tokenType = TokenType.Id;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\d+(\.\d+)?", Priority = 2)]
    public sealed class Num : Token
    {
        public Num(string value)
        {
            tokenType = TokenType.Num;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\+", Priority = 2)]
    public sealed class Plus : Token
    {
        public Plus(string value)
        {
            tokenType = TokenType.Plus;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\-", Priority = 2)]
    public sealed class Minus : Token
    {
        public Minus(string value)
        {
            tokenType = TokenType.Minus;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\*", Priority = 2)]
    public sealed class Mul : Token
    {
        public Mul(string value)
        {
            tokenType = TokenType.Mul;
            this.value = value;
        }
    }

    [TokenRegex(@"\G/", Priority = 2)]
    public sealed class Div : Token
    {
        public Div(string value)
        {
            tokenType = TokenType.Div;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\(", Priority = 2)]
    public sealed class LP : Token
    {
        public LP(string value)
        {
            tokenType = TokenType.LP;
            this.value = value;
        }
    }

    [TokenRegex(@"\G\)", Priority = 2)]
    public sealed class RP : Token
    {
        public RP(string value)
        {
            tokenType = TokenType.RP;
            this.value = value;
        }
    }
}
