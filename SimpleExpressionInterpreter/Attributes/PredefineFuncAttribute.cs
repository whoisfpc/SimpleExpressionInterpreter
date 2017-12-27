using System;

namespace ExpressionInterpreter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PredefineFuncAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        public PredefineFuncAttribute(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
