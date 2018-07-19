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
//    Description: Nunit Test class for the EasyModbusWrapper
//
//************************************************************************
namespace Qti.Autotron.ModbusAutotronAPI.Tests
{
    [TestFixture()]
    public class EasyModbusWrapperTests
    {
        [Test, TestCase("192.168.0.1", 502)]
        [TestCase("10.10.10.77", 502)]
        public void EasyModbusWrapperTest( string ipAddress, int portValue)
        {
            var TestObject = new EasyModbusWrapper(ipAddress, portValue);
            Assert.IsInstanceOf(typeof(EasyModbus.ModbusClient), TestObject.ModbusClient);
            Assert.AreEqual(ipAddress, TestObject.ModbusClient.IPAddress);
            Assert.AreEqual(portValue, TestObject.ModbusClient.Port);
        }
    }
}