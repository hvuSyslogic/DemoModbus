using NUnit.Framework;
using Qti.Autotron.ModbusAutotronAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//************************************************************************
//
//    This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 07/09/2018
//    Description: Nunit Test class for the VarInput
//
//************************************************************************
namespace Qti.Autotron.ModbusAutotronAPI.Tests
{
    [TestFixture()]
    public class VarInputTests
    {
        /// <summary>
        /// ctor Test
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="byteLength"></param>
        /// <param name="inputName"></param>
        [Test, TestCase(0x8000, 8, "FirstInput")]
        [TestCase(0x8001, 8, "SecondInput")]
        [TestCase(0x8011, 8, "FirstOutput")]
        public void VarInputTest(int baseAddress, int byteLength, string inputName)
        {
            var ObjectToTest = new VarInput(baseAddress, byteLength, inputName);
            Assert.AreEqual(baseAddress, ObjectToTest.BaseAddress);
            Assert.AreEqual(byteLength, ObjectToTest.ByteLength);
            Assert.AreEqual(inputName, ObjectToTest.Name);
            Assert.IsNotNull(ObjectToTest.ByteArray);
        }
    }
}