using System;
using System.Collections.Generic;

namespace SimpleExpressionInterpreter
{
    // 没有容错
    public class Executor
    {
        private Stack<float> stack;

        public Executor()
        {
            stack = new Stack<float>(128);
        }

        public float Execute(byte[] bytecodes)
        {
            var i = 0;
            float tmp;
            while (i < bytecodes.Length)
            {
                var inst = (Instruction)bytecodes[i];
                i++;
                switch (inst)
                {
                    case Instruction.PushLiteral:
                        tmp = BitConverter.ToSingle(bytecodes, i);
                        stack.Push(tmp);
                        i += 4;
                        break;
                    case Instruction.Add:
                        tmp = stack.Pop();
                        tmp = stack.Pop() + tmp;
                        stack.Push(tmp);
                        break;
                    case Instruction.Sub:
                        tmp = stack.Pop();
                        tmp = stack.Pop() - tmp;
                        stack.Push(tmp);
                        break;
                    case Instruction.Mul:
                        tmp = stack.Pop();
                        tmp = stack.Pop() * tmp;
                        stack.Push(tmp);
                        break;
                    case Instruction.Div:
                        tmp = stack.Pop();
                        tmp = stack.Pop() / tmp;
                        stack.Push(tmp);
                        break;
                    case Instruction.Ret:
                        return stack.Pop();
                    default:
                        Console.WriteLine("Error: unexpect inst");
                        return 0;
                }
            }
            return 0;
        }
    }
}
