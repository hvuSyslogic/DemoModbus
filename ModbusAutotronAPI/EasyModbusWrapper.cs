using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class EasyModbusWrapper
    {
        private EasyModbus.ModbusClient _modbusClient;
        public EasyModbusWrapper(string IPAddress, int PortValue)
        {
            _modbusClient = new EasyModbus.ModbusClient(IPAddress, PortValue);
            _modbusClient.ReceiveDataChanged += new EasyModbus.ModbusClient.ReceiveDataChangedHandler(UpdateReceiveData);
            _modbusClient.SendDataChanged += new EasyModbus.ModbusClient.SendDataChangedHandler(UpdateSendData);
            _modbusClient.ConnectedChanged += new EasyModbus.ModbusClient.ConnectedChangedHandler(UpdateConnectedChanged);
            _modbusClient.LogFileFilename = "EasyModbusWrapper.txt";
            _modbusClient.Connect();
        }

       

        private void UpdateConnectedChanged(object sender)
        {
            //throw new NotImplementedException();
            FlatAPIForDDI.DataArray = new byte[] { 0xFF, 0xFF, 0xFF, 0x10, 0x10, 0x10, 0x10 };
        }

        private void UpdateSendData(object sender)
        {
            //throw new NotImplementedException();
        }

        private void UpdateReceiveData(object sender)
        {
            //throw new NotImplementedException();
        }
    }
}
