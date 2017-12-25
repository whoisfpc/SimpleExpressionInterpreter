using System;
using System.Collections.Generic;

namespace ExpressionInterpreter
{
    // 没有容错
    public class Executor
    {
        private VirtualMachine vm;

        public Executor()
        {
            vm = new VirtualMachine();
        }

        public float Execute(byte[] bytecodes, IList<float> variables)
        {
            var result = 0f;
            try
            {
                result = _Execute(bytecodes, variables);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        private float _Execute(byte[] bytecodes, IList<float> variables)
        {
            if (bytecodes == null)
            {
                return 0;
            }

            vm.Reset();
            var i = 0;
            float tmp;
            while (i < bytecodes.Length)
            {
                var inst = (Instruction)bytecodes[i];
                i++;
                switch (inst)
                {
                    case Instruction.PushVariable:
                        int varIdx = BitConverter.ToInt32(bytecodes, i);
                        i += 4;
                        if (varIdx >= 0 && varIdx < variables.Count)
                        {
                            vm.Push(variables[varIdx]);
                        }
                        else
                        {
                            Console.WriteLine("Error: var not found, id: {0}", varIdx);
                            return 0;
                        }
                        break;
                    case Instruction.PushLiteral:
                        tmp = BitConverter.ToSingle(bytecodes, i);
                        vm.Push(tmp);
                        i += 4;
                        break;
                    case Instruction.Add:
                        tmp = vm.Pop<float>();
                        tmp = vm.Pop<float>() + tmp;
                        vm.Push(tmp);
                        break;
                    case Instruction.Sub:
                        tmp = vm.Pop<float>();
                        tmp = vm.Pop<float>() - tmp;
                        vm.Push(tmp);
                        break;
                    case Instruction.Mul:
                        tmp = vm.Pop<float>();
                        tmp = vm.Pop<float>() * tmp;
                        vm.Push(tmp);
                        break;
                    case Instruction.Div:
                        tmp = vm.Pop<float>();
                        tmp = vm.Pop<float>() / tmp;
                        vm.Push(tmp);
                        break;
                    case Instruction.Call:
                        var funcId = BitConverter.ToInt32(bytecodes, i);
                        i += 4;
                        vm.CallFunc(funcId);
                        break;
                    case Instruction.Ret:
                        return vm.Pop<float>();
                    default:
                        Console.WriteLine("Error: unexpect inst");
                        return 0;
                }
            }
            return 0;
        }
    }
}
