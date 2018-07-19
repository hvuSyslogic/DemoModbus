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
//    Creation Date: 06/18/2018
//    Description: EasyModbusWrapper to work with Easymodbus.dll
//    this class may not needed if the DDIDriver can read and write to the IOs using it
//    backplan bus (Interbus).
//
//************************************************************************

using System;
using EasyModbus;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class EasyModbusWrapper : IEasyModbusWrapper
    {
        public EasyModbusWrapper(string IPAddress, int PortValue)
        {
            ModbusClient = new EasyModbus.ModbusClient();
            //ModbusClient.SerialPort = "";
            ModbusClient.receiveDataChanged += new EasyModbus.ModbusClient.ReceiveDataChanged(UpdateReceiveData);
            ModbusClient.sendDataChanged += new EasyModbus.ModbusClient.SendDataChanged(UpdateSendData);
            ModbusClient.connectedChanged += new EasyModbus.ModbusClient.ConnectedChanged(UpdateConnectedChanged);
            ModbusClient.LogFileFilename = "AutoTronEasyModbusWrapper.txt";
            ModbusClient.IPAddress = IPAddress;
            ModbusClient.Port = PortValue;
        }
        ~EasyModbusWrapper()
        {
            ModbusClient.receiveDataChanged -= new EasyModbus.ModbusClient.ReceiveDataChanged(UpdateReceiveData);
            ModbusClient.sendDataChanged -= new EasyModbus.ModbusClient.SendDataChanged(UpdateSendData);
            ModbusClient.connectedChanged -= new EasyModbus.ModbusClient.ConnectedChanged(UpdateConnectedChanged);
            ModbusClient.LogFileFilename = "";
        }
        public void CheckStatus(Object stateInfo)
        {
        }
        public void Connect()
        {
            try
            {
                if (ModbusClient != null)
                {
                    if (!ModbusClient.Connected)
                    {
                        ModbusClient.Connect();
                    }
                }
            }
            catch (EasyModbus.Exceptions.ModbusException ex)
            {
                FlatAPIForDDI.DataArray[0] = 0x20;
                FlatAPIForDDI.DataArray[1] = 0xFF;
                this._aExceptions.Push(ex);
                if (ex.Message.Contains("connection time"))
                {
                    Trace.WriteLine("IN KeepModbusClientConnecting NOT connected TIMEOUT" + ex.Message);
                }
                else
                    Trace.WriteLine("IN KeepModbusClientConnecting NOT connected" + ex.Message);
            }
            return;

        }

        public ModbusClient ModbusClient
        {
            get;
            private set;
        }
        public void AddItemToProcess(EasyModbusItem item)
        {
            //Make sure the process thread is alive first
            if (_TaskKeepAliveModbusClient != null && _TaskKeepAliveModbusClient.IsAlive)
            {
                //Push one element.
                _SendItems.Enqueue(item);
                return;
            }
            else
            {
                if (!_aExceptions.Peek().Message.Equals(_strProcessThreadNotStartedYet))
                {
                    Trace.WriteLine("***Should show once***" + _strProcessThreadNotStartedYet);
                    _aExceptions.Push(new Exception(_strProcessThreadNotStartedYet));
                }
            }
        }

        bool KeepAliveModbusClient
        { get; set; }

        ICollection<string> GetErrorMessages(bool bAll)
        {
            if (bAll) { return _aExceptions.Select(x => x.Message).ToArray(); }
            else
            {
                return new[] { _aExceptions.Pop().Message };
            }
        }
        private void UpdateConnectedChanged(object sender)
        {
            var modbusClientObj = (ModbusClient)sender;

            if (modbusClientObj != null)
            {
                if (!modbusClientObj.Connected)
                {
                    Trace.WriteLine("IN UpdateConnectedChanged NOT connected");
                    FlatAPIForDDI.DataArray[0] = 0x20;
                    FlatAPIForDDI.DataArray[1] = 0xFF;
                    if (_TaskKeepAliveModbusClient != null)
                    {
                        KeepAliveModbusClient = false;
                    }
                }
                else
                {
                    Trace.WriteLine("IN UpdateConnectedChanged connected");
                    FlatAPIForDDI.DataArray[0] = 0x20;
                    FlatAPIForDDI.DataArray[1] = 0;
                    // start the Task to send out request to ModbusClient
                    KeepAliveModbusClient = true;
                    if (_TaskKeepAliveModbusClient == null) // first time so start new thread
                    {
                        _TaskKeepAliveModbusClient = new Thread(KeepModbusClientConnecting);//KeepModbusClientConnecting
                        if (_TaskKeepAliveModbusClient != null)
                        {
                            _TaskKeepAliveModbusClient.Name = "EasyModbusWrapper_kKeepAliveModbusClient";// For Debugging purpose
                            _TaskKeepAliveModbusClient.Start(sender);
                        }
                    }
                }
            }
        }

        private void KeepModbusClientConnecting(object sender)
        {
            Trace.WriteLine("IN KeepModbusClientConnecting START");
            var modbusClientObj = (ModbusClient)sender;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (modbusClientObj != null)
                // do work here
                while (KeepAliveModbusClient)
                {
                    try
                    {
                        Trace.WriteLine("**while IN KeepModbusClientConnecting START 1");
                        //Wait in the loop, while the queue is busy.
                        var ModbusItem = _SendItems.Dequeue();
                        sw.Stop();
                        Trace.WriteLine("**while IN KeepModbusClientConnecting START time : " + sw.Elapsed.ToString());
                        int[] ReturnedData;
                        switch (ModbusItem.FunctionCode)
                        {
                            case 3://fc3
                                ReturnedData = modbusClientObj.ReadHoldingRegisters(ModbusItem.StartingAddress, ModbusItem.Quantity);
                                _ReceiveItems.UpdateWithReturnedValues(new EasyModbusItem(ModbusItem), ReturnedData);
                                break;
                            case 4://fc4 
                                ReturnedData = modbusClientObj.ReadInputRegisters(ModbusItem.StartingAddress, ModbusItem.Quantity);
                                _ReceiveItems.UpdateWithReturnedValues(new EasyModbusItem(ModbusItem), ReturnedData);
                                break;
                            case 6: //fc6 
                                modbusClientObj.WriteSingleRegister(ModbusItem.StartingAddress, ModbusItem.Quantity);
                                break;
                            case 23: //fc23
                                modbusClientObj.WriteMultipleRegisters(ModbusItem.StartingAddress, ModbusItem.DataSource);
                                break;
                            default:
                                ReturnedData = modbusClientObj.ReadHoldingRegisters(7996, 1);// Read modbus status
                                _ReceiveItems.UpdateWithReturnedValues(new EasyModbusItem(3, 7886, 1), ReturnedData);
                                break;
                            case 0x00010000:// stop Modbus comm.
                                KeepAliveModbusClient = false;// shut down both Receive and Proceed data
                                Trace.WriteLine("IN KeepModbusClientConnecting STOP BY 0x00010000");
                                break;

                        }

                    }
                    catch (EasyModbus.Exceptions.ModbusException exModbus)
                    {
                        if (exModbus.Message.Contains(@"connection time"))
                        {
                            Trace.WriteLine("IN KeepModbusClientConnecting **" + exModbus.Message);
                            FlatAPIForDDI.DataArray[0] = 0x20;
                            FlatAPIForDDI.DataArray[1] = 0xFE;
                            if (modbusClientObj.Connected)
                            {
                                modbusClientObj.Disconnect();
                                Thread.Sleep(50);
                            }
                            this._aExceptions.Push(exModbus);
                            KeepAliveModbusClient = true;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _aExceptions.Push(ex);
                        if (modbusClientObj.Connected)
                        {
                            modbusClientObj.Disconnect();
                        }
                        KeepAliveModbusClient = false;
                        Trace.WriteLine("IN KeepModbusClientConnecting" + ex.Message);
                    }

                }
            Trace.WriteLine("IN KeepModbusClientConnecting Exit KeepModbusClientConnecting Thread");
        }

        private void UpdateSendData(object sender)
        {
            var modbusClientObj = (ModbusClient)sender;
            EasyModbusItem item = new EasyModbusItem(modbusClientObj.sendData);
            _ReceiveItems.Add(item);
        }

        private void UpdateReceiveData(object sender)
        {
            var modbusClientObj = (ModbusClient)sender;
            if (modbusClientObj != null)
            {
                EasyModbusItem item = new EasyModbusItem(modbusClientObj.sendData);
                _ReceiveItems.UpdateWithRxData(item, modbusClientObj.receiveData);
                if (_TaskProcessReceivedData == null)
                {
                    _TaskProcessReceivedData = new Thread(ProcessReceivedData);
                    _TaskProcessReceivedData.Name = "EasyModbusWrapper_ProcessReceivedData";
                    _TaskProcessReceivedData.Start();
                }
                Trace.WriteLine(new SoapHexBinary(modbusClientObj.receiveData).ToString());
                if ((modbusClientObj.receiveData[0] & 0x01) == 0x01)
                {
                    return;
                }

            }
            //throw new NotImplementedException();
        }

        private void ProcessReceivedData(Object obj)
        {
            Trace.WriteLine("Start ProcessReceivedData");
            while (KeepAliveModbusClient)
            {
                // Here, use data initialized by the other thread
                lock (_ReceiveItems)
                {

                    var item = _ReceiveItems.RemoveFirstItem();
                    switch (item.StartingAddress) 
                    {
                        case 7996:// modbus diagnostic
                            ProceedModbusDiagnosticData(item);
                            break;
                        case 384:// static address table for First DO
                            ProcessWriteSingleRegister(item);
                            break;
                        default:
                            Trace.WriteLine("Default ProcessReceivedData");
                            break;
                    }
                }
            }

            // Make sure queue is not empty by adding read modbus diagnostic ...
            if (_SendItems.Count() == 0) AddItemToProcess(new EasyModbusItem(3, 7996, 1));
        }

        private void ProcessWriteSingleRegister(EasyModbusItem item)
        {
            if (item.TxBuffer.SequenceEqual(item.RxBuffer))// This is successful written.
            {
                return;
            }
            int usFunctionCodeWithError = Convert.ToInt32(item.RxBuffer[7]);
            int returnValue = Convert.ToInt32(item.RxBuffer[8]);
            StringBuilder str = new StringBuilder("Write Register Error:");
            str.AppendFormat("FC {0} Address:{1} Quantity: {2} returned Value: {3}.\n", item.FunctionCode, item.StartingAddress, item.Quantity, returnValue);
            str.AppendLine();
            // refer to IL ETH BK DI8 DO4 2TX-PAC .pdf documentation from Hardware Vendor
            if ((usFunctionCodeWithError & 0x0000008F) == (item.FunctionCode | 0x00000080))
            {
                GetModbusExceptionResponse(item, returnValue, str);
            }
            else
            {
                str.AppendFormat("Unknown data code, message returned form server is: {0}", new SoapHexBinary(item.RxBuffer));
            }
            _aExceptions.Push(new Exception(str.ToString()));
         }

        private static void GetModbusExceptionResponse(EasyModbusItem item, int returnValue, StringBuilder str)
        {
            switch (returnValue)// Modbus exception response code
            {
                case 1:
                    str.Append("The Function Code is unknown to the Server");
                    break;
                case 2:
                    str.Append("Illegal Address or Quantity");
                    break;
                case 3:
                    str.Append("Illegal Data");
                    break;
                case 4:
                    str.Append("The server failed during the execution");
                    break;
                case 5:
                    str.Append("The server accepted the service invocation but the service requires a relatively long time to execute. The server therefore returns only an acknowledgement of the service invocation receipt.");
                    break;
                case 6:
                    str.Append("The server was unable to accept the MB Request PDU. The client application has the responsibility of deciding if and when to re-send the request.");
                    break;
                case 10:
                    str.Append("Gateway paths not available.");
                    break;
                case 11:
                    str.Append("The targeted device failed to respond. The gateway generates this exception.");
                    break;
                default:
                    str.AppendFormat("Default case: Unknown data code, message returned form server is: {0}", new SoapHexBinary(item.RxBuffer));
                    break;
            }
        }

        private void ProceedModbusDiagnosticData(EasyModbusItem item)
        {
            Debug.Assert(item.RxBuffer[7] == item.FunctionCode);
            Debug.Assert(item.RxBuffer[8] >= (byte)2);
            ushort returnValue = item.RxBuffer[9];
            returnValue <<= 8;
            returnValue += item.RxBuffer[10];
            // refer to IL ETH BK DI8 DO4 2TX-PAC .pdf documentation from Hardware Vendor
            switch (returnValue)// base on section 15.7.1 Register 7996: status register
            {
                case 1: // good, no error --
                    FlatAPIForDDI.DataArray[1] = 0;// set shared memory, no error.
                    break;
                case 0:
                case 2:
                    // Need to read more data
                    AddItemToProcess(new EasyModbusItem(3, 7997, 1));
                    FlatAPIForDDI.DataArray[1] = 1;// set shared memory, error 1.
                    break;
                case 3:// Net Fail occur
                    AddItemToProcess(new EasyModbusItem(3, 2004, 1));
                    FlatAPIForDDI.DataArray[1] = 3;// set shared memory, error 3.
                    break;
                default:
                    if (item.ShouldRetry) AddItemToProcess(item);
                    break;
            }
        }
        // error messages, can be stacked or queue.
        Stack<Exception> _aExceptions = new Stack<Exception>();
        // process thread for modbus request from meb or other client
        private Thread _TaskKeepAliveModbusClient = null;
        BlockingQueue<EasyModbusItem> _SendItems = new BlockingQueue<EasyModbusItem>();
        // receive data from Easymodbus and decode it.
        private Thread _TaskProcessReceivedData = null;
        BlockingHashSet _ReceiveItems = new BlockingHashSet();
        // string constants for error message.
        const string _strProcessThreadNotStartedYet = @"rProcessThreadNotStartedYet";
    }
}
