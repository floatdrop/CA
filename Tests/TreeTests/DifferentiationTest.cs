using System;
using System.Linq.Expressions;
using AIRLab.CA;
using AIRLab.CA.Tools;
using AIRLab.CA.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TreeTests
{
    [TestClass]
    public class DifferentiationTest : Tests
    {
        // (-x) dif x => -1
        [TestMethod]
        public void DiffNegate()
        {
            Expression<del1> expression = (x) => -x;
            Assert.AreEqual(
                "-1",
                ComputerAlgebra.Differentiate(expression.Body, variable: "x").ToString());
        }

        // (x+y) dif x => 1
        [TestMethod]
        public void DiffPlus()
        {
            Expression<del2> expression = (x, y) => x + y;
            Assert.AreEqual(
                "1",
                ComputerAlgebra.Differentiate(expression.Body, variable: "x").ToString());
        }
        // (x-y) dif x => 1
        [TestMethod]
        public void DiffMinus()
        {
            Expression<del2> expression = (x, y) => x - y;
            Assert.AreEqual(
                "1",
                ComputerAlgebra.Differentiate(expression.Body, variable: "x").ToString());
        }
        // (x*y) dif x => y
        [TestMethod]
        public void DiffProduct()
        {
            Expression<del2> expression = (x, y) => x * y;
            Assert.AreEqual(
                "y",
                ComputerAlgebra.Differentiate(expression.Body, variable: "x").ToString());
        }
        // (x/y) dif x => y/y^2
        [TestMethod]
        public void DiffDivide()
        {
            Expression<del2> expression = (x, y) => x / y;
            Assert.AreEqual(
                new Arithmetic.Divide<double>(
                    VariableNode.Make<double>(1, "y"),
                    new Arithmetic.Pow<double>(VariableNode.Make<double>(1, "y"), Constant.Double(2))).ToString(),
                    ComputerAlgebra.Differentiate(Expressions2Tree.Parse(expression.Body), variable: "x").ToString());
        }

        // (x^C) dif x => C*x^(C-1)
        [TestMethod]
        public void DiffPowConstant()
        {
            Expression<del1> expression = (x) => Math.Pow(x, 3);
            Assert.AreEqual(
                new Arithmetic.Product<double>(Constant.Double(3.0), new Arithmetic.Pow<double>(VariableNode.Make<double>(0, "x"), Constant.Double(2.0))).ToString(),
                ComputerAlgebra.Differentiate(Expressions2Tree.Parse(expression.Body), variable: "x").ToString());
        }

        // (Ln(x^2)) dif x => 2x/x^2
        [TestMethod]
        public void DiffLn()
        {
            Expression<del1> expression = (x) => Math.Log(Math.Pow(x, 2));
            Assert.AreEqual(
                new Arithmetic.Divide<double>(new Arithmetic.Product<double>(Constant.Double(2), VariableNode.Make<double>(0, "x")),
                    new Arithmetic.Pow<double>(VariableNode.Make<double>(0, "x"), Constant.Double(2))).ToString(),
                ComputerAlgebra.Differentiate(Expressions2Tree.Parse(expression.Body), variable: "x").ToString()
            );
        }
        
        // (x^y) dif x => ((x^y)*(y/y))
        [TestMethod]
        public void DiffPowX()
        {
            Expression<del2> expression = (x, y) => Math.Pow(x, y);
            Assert.AreEqual(
                new Arithmetic.Product<double>(
                    new Arithmetic.Pow<double>(
                        VariableNode.Make<double>(0, "x"),
                        VariableNode.Make<double>(1, "y")),
                    new Arithmetic.Divide<double>(
                        VariableNode.Make<double>(1, "y"),
                        VariableNode.Make<double>(0, "x"))).ToString(),
                ComputerAlgebra.Differentiate(Expressions2Tree.Parse(expression.Body), variable: "x").ToString());
        }
        // (x^y) dif y => ((x^y)*(Ln(y)))
        [TestMethod]
        public void DiffPowY()
        {
            Expression<del2> expression = (x, y) => Math.Pow(x, y);
            Assert.AreEqual(
                new Arithmetic.Product<double>(
                    new Arithmetic.Pow<double>(
                        VariableNode.Make<double>(0, "x"),
                        VariableNode.Make<double>(1, "y")),
                    new Arithmetic.Ln(VariableNode.Make<double>(0, "x")))
                    .ToString(),
                ComputerAlgebra.Differentiate(Expressions2Tree.Parse(expression.Body), variable: "y").ToString());
        }
    }
}
