using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionInterpreter
{
    public class VirtualMachine
    {
        private static Dictionary<int, MethodBase> defineFuncs = new Dictionary<int, MethodBase>();
        private static Dictionary<string, int> defineFuncNames = new Dictionary<string, int>();
        private static Dictionary<int, string> defineFuncIds = new Dictionary<int, string>();

        private Stack<object> stack = new Stack<object>(128);

        static VirtualMachine()
        {
            var methods = typeof(VirtualMachine).GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
            foreach(var method in methods)
            {
                var attr = method.GetCustomAttribute<PredefineFuncAttribute>(false);
                if (attr == null)
                {
                    continue;
                }
                if (defineFuncNames.ContainsKey(attr.Name))
                {
                    Console.WriteLine("duplicate predefine function name");
                    continue;
                }
                else if (defineFuncNames.ContainsValue(attr.Id))
                {
                    Console.WriteLine("duplicate predefine function id");
                    continue;
                }
                defineFuncNames.Add(attr.Name, attr.Id);
                defineFuncIds.Add(attr.Id, attr.Name);
                defineFuncs.Add(attr.Id, method);
            }
        }

        public void Reset()
        {
            stack.Clear();
        }

        public void Push(object obj)
        {
            stack.Push(obj);
        }

        public T Pop<T>()
        {
            if (stack.Count <= 0)
            {
                throw new IndexOutOfRangeException("try get value but stack is empty!");
            }
            var obj = stack.Pop();
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                throw new TypeMismatchException(string.Format("try get `{0}`, but `{1}` found", typeof(T).Name, obj.GetType().Name));
            }
        }

        public void CallFunc(int funcId)
        {
            if (!defineFuncs.ContainsKey(funcId))
            {
                throw new ExecuteFailedException("function id " + funcId + " not define");
            }
            var func = defineFuncs[funcId];
            var paraInfos = func.GetParameters();
            var paras = new object[paraInfos.Length];
            for (int i = paraInfos.Length - 1; i >= 0; i--)
            {
                var obj = Pop<object>();
                if (obj.GetType() != paraInfos[i].ParameterType)
                {
                    throw new TypeMismatchException(string.Format("try get `{0}`, but `{1}` found", paraInfos[i].ParameterType.Name, obj.GetType().Name));
                }
                paras[i] = obj;
            }
            Push(func.Invoke(null, paras));
        }

        public static bool CheckFuncDefine(string funcName)
        {
            return defineFuncNames.ContainsKey(funcName);
        }

        public static int GetPredefineFuncId(string funcName)
        {
            return defineFuncNames[funcName];
        }

        public static string GetPredefineFuncName(int funcId)
        {
            return defineFuncIds[funcId];
        }

        [PredefineFunc("max", 1)]
        private static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        [PredefineFunc("min", 2)]
        private static float Min(float a, float b)
        {
            return a > b ? b : a;
        }

        [PredefineFunc("vec", 3)]
        private static Vector Vec(float x, float y, float z)
        {
            return new Vector(x, y, z);
        }

        [PredefineFunc("dot", 4)]
        private static float Dot(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        [PredefineFunc("scale", 5)]
        private static Vector Scale(Vector a, float s)
        {
            return new Vector(a.x * s, a.y * s, a.z * s);
        }

        // 和unity一样使用左手坐标系
        [PredefineFunc("cross", 6)]
        private static Vector Cross(Vector a, Vector b)
        {
            return new Vector(b.y * a.z - b.z * a.y, b.z * a.x - b.x * a.z, b.x * a.y - b.y * a.x);
        }
    }

    public struct Vector
    {
        public float x;
        public float y;
        public float z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return string.Format("vec({0}, {1}, {2})", x, y, z);
        }
    }
}
