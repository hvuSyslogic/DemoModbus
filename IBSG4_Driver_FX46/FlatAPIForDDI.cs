using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixContact.DDI
{
    class FlatAPIForDDI
    {
        static string ConnectionName = @"IBETHIP[192.168.0.1]N1_D";
        static string _connectionDTI;
        static string _connectionMXI;
        /// <summary>
        /// a managed static method that gets called from native code
        /// </summary>
        public static int ManagedMethodCalledFromExtension(string args)
        {
            // need to return an integer: the length of the args string
            switch (args)
            {
                case "": // start the ethernet modbus...
                    break;
                case "1":
                    break;
                default:
                    break;
            }
            return args.Length;
        }
        public static int Enable(string connectionName)
        {
            return 0;

        }

        public static int Disable(string connectionNameHandler)
        {
            return 0xAA;
        }
    }
}
