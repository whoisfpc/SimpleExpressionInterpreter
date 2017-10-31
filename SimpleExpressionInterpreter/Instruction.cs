namespace SimpleExpressionInterpreter
{
    /// <summary>
    /// 指令码
    /// </summary>
    public enum Instruction : byte
    {
        PushLiteral = 0x01,
        PushVariable = 0x02,
        Pop = 0x03,
        Add = 0x04,
        Sub = 0x05,
        Mul = 0x06,
        Div = 0x07,
        Ret = 0x08
    }
}
