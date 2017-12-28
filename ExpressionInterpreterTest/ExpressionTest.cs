using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpressionInterpreter;
using System.Collections.Generic;

namespace ExpressionInterpreterTest
{
    [TestClass]
    public class ExpressionTest
    {
        private static Compiler compiler;
        private static Executor executor;

        [ClassInitialize]
        public static void Setup(TestContext tc)
        {
            compiler = new Compiler();
            executor = new Executor();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            compiler = null;
            executor = null;
        }

        [TestMethod]
        public void TestFloatCalc()
        {
            var expr = @"1+1";
            var result = executor.Execute(compiler.Compile(expr));
            Assert.AreEqual(2.0f, result);
        }

        [TestMethod]
        public void TestVectorCalc()
        {
            var expr = @"dot(scale(vec(1,2,3), 2), vec(1,1,1))";
            var result = executor.Execute(compiler.Compile(expr));
            Assert.AreEqual(12.0f, result);
        }

        [TestMethod]
        public void TestFloatCalc_Complex()
        {
            var expr = @"1+2*3/4-5+(3-2/3+0.5*3.14)";
            var result = executor.Execute(compiler.Compile(expr));
            var expect = 1f + 2f * 3f / 4f - 5f + (3f - 2f / 3f + 0.5f * 3.14f);
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void TestMixedCalc()
        {
            var expr = @"dot(vec(1,2,3), vec(max(6/3, 3*2), min(4,5), 4+5))";
            var result = executor.Execute(compiler.Compile(expr));
            Assert.AreEqual(41.0f, result);
        }

        [TestMethod]
        public void TestPredefineVar()
        {
            var variables = new List<float> { 0, 1, 2, 3 };
            var expr = @"dot(vec($1,$2,$3), vec(max(6/3, $3*2), min(4,5), 4+5))";
            var result = executor.Execute(compiler.Compile(expr), variables);
            Assert.AreEqual(41.0f, result);
        }
    }
}
