﻿// ComputerAlgebra Library
//
// Copyright © Medvedev Igor, Okulovsky Yuri, Borcheninov Jaroslav, 2013
// imedvedev3@gmail.com, yuri.okulovsky@gmail.com, yariksuperman@gmail.com
//

using System;
using System.Linq.Expressions;
using AIRLab.CA.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TreeTests
{
    [TestClass]
    public class ComplexTests : Tests
    {        
        // ((x+y)+0)*1 => x+y
        [TestMethod]
        public void FirstLevel()
        {
            Expression<del2> expression = (x, y) => ((x + y) + 0) * 1;
            Assert.AreEqual(
                 new Arithmetic.Plus<double>(VariableNode.Make<double>(0, "x"), VariableNode.Make<double>(1, "y")).ToString(),
                 SimplifyBinaryExpression(expression.Body).ToString());
        }
        // ((((x^1)-0)-((3+2)+(0/1)))∙y) => (x-5)*y
        [TestMethod]
        public void SecondLevel()
        {
            Expression<del2> expression = (x, y) => (((Math.Pow(x, 1)-0)-((3+2)+(0/1)))*y);
            Assert.AreEqual(
                 new Arithmetic.Product<double>(
                     new Arithmetic.Minus<double>(
                         VariableNode.Make<double>(0, "x"), Constant.Double(5.0)), 
                     VariableNode.Make<double>(1, "y")).ToString(),
                 SimplifyBinaryExpression(expression.Body).ToString());
        }
    }
}
