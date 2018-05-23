#region Copyright
///////////////////////////////////////////////////////////////////////////////
//
//  Copyright PHOENIX CONTACT Software GmbH
//
///////////////////////////////////////////////////////////////////////////////
#endregion

namespace HFI_Demo_Inline_CS
{
    using System;
    using System.Collections.Generic;

    using PhoenixContact.HFI.Inline;

    /// <summary>
    /// This class represents an application for one controler.
    /// </summary>
    public sealed class App_ETH_BK_DI8_DO4 : IDisposable
    {
        // Error logging
        public Queue<Exception> ExceptionList { get; private set; }

        #region *** Variable Declaration **************************************************

        // Create the controller
        public Controller_IBS_ETH Controller;

        // Create the variables for the input data
        // First input terminal DI 16
        public VarInput InBit_0 = new VarInput(2, PD_Length.Word, 1, 0);

        public VarInput InBit_1 = new VarInput(2, PD_Length.Word, 1, 1);
        public VarInput InVariable = new VarInput(2, PD_Length.Word, 12, 4);

        // Second input terminal DI 32
        public VarInput InByteArray = new VarInput(4, 4);

        // PCP terminal inputs (RS232 terminal)
        public VarInput InRS232_1 = new VarInput(8, PD_Length.Word, 16, 0);

        // Create the variables for the output data
        // First output terminal DO 16
        public VarOutput OutBit_0 = new VarOutput(2, PD_Length.Word, 1, 0);

        public VarOutput OutBit_1 = new VarOutput(2, PD_Length.Word, 1, 1);
        public VarOutput OutVariable = new VarOutput(2, PD_Length.Word, 12, 4);

        // Second output terminal DO 32
        public VarOutput OutByteArray = new VarOutput(4, 4);

        // PCP terminal outputs (RS232 terminal)
        public VarOutput OutRS232_1 = new VarOutput(8, PD_Length.Word, 16, 0);

        // Create the variables	for the PCP communication CR (RS232 terminal)
        public PCP PcpRS232_1 = new PCP("RS232_1", 2);

        private Boolean firstStartPcp;

        private byte[] pcpReadBuffer = new byte[0];
        private Boolean pcpWriteOk;

        #endregion *** Variable Declaration **************************************************

        #region *** Constructor / Destructor / IDisposable Declaration ********************

        /// <summary>
        /// Constructor
        /// </summary>
        public App_ETH_BK_DI8_DO4()
        {
            this.ExceptionList = new Queue<Exception>();

            // Create the controller with a name
            this.Controller = new Controller_IBS_ETH("ETH BK DI8 DO4");

            // Settings of the controller
            this.Controller.Description = "ETH BK DI8 DO4 for Demonstaration";
            this.Controller.Connection = Properties.Settings.Default.ipAddress;

            this.Controller.Startup = ControllerStartup.PhysicalConfiguration;

            this.Controller.UpdateProcessDataCycleTime = 20;
            this.Controller.UpdateMailboxTime = 50;

            // The Controller Configuration property contains special configurations for the controller
            //Controller.Configuration.DNS_NameResolution     = true;
            //Controller.Configuration.EnableIBS_Indications  = true;
            //Controller.Configuration.GetRevisionInfo        = false;
            //Controller.Configuration.Read_IBS_Cycletime     = false;
            //Controller.Configuration.UpdateControllerState  = 100;

            // Add input variables to the controller
            this.Controller.AddObject(this.InBit_0);
            this.Controller.AddObject(this.InBit_1);
            this.Controller.AddObject(this.InVariable);
            this.Controller.AddObject(this.InByteArray);

            this.Controller.AddObject(this.InRS232_1);

            // Add output variables to the controller
            this.Controller.AddObject(this.OutBit_0);
            this.Controller.AddObject(this.OutBit_1);
            this.Controller.AddObject(this.OutVariable);
            this.Controller.AddObject(this.OutByteArray);

            this.Controller.AddObject(this.OutRS232_1);

            // Add PCP objects to the controller
            this.Controller.AddObject(this.PcpRS232_1.ControllerConnection);

            // Callbacks for the controller

            // Called once for each bus cycle
            this.Controller.OnUpdateProcessData += this.Controller_OnUpdateProcessData;
            // Called once for each mailbox cycle
            this.Controller.OnUpdateMailbox += this.Controller_OnUpdateMailbox;

            // Called whenever an error occurs in the controller object
            this.Controller.OnException += this.Controller_OnException;

            // Events from PCP_2
            this.PcpRS232_1.OnEnableReady += this.PCP_RS232_1_OnEnableReady;
            this.PcpRS232_1.OnReadConfirmationReceived += this.PCP_RS232_1_ReadConfirmationReceived;
            this.PcpRS232_1.OnWriteConfirmationReceived += this.PCP_RS232_1_WriteConfirmationReceived;
            this.PcpRS232_1.OnException += this.PCP_RS232_1_OnException;
        }

        #endregion *** Constructor / Destructor / IDisposable Declaration ********************

        #region *** Events From the Controller ********************************************

        /// <summary>
        /// Called once for each bus cycle
        /// </summary>
        /// <param name="sender">The caller instance as object.</param>
        private void Controller_OnUpdateProcessData(object sender)
        {
            // TODO insert your process data handling (application) here

            // Test application for a counter
            if (this.OutVariable.Value < this.OutVariable.MaxValue)
            {
                this.OutVariable.Value++;
            }
            else
            {
                this.OutVariable.Value = this.OutVariable.MinValue;
            }
        }

        /// <summary>
        /// Called once for each mailbox cycle
        /// </summary>
        /// <param name="sender">The caller instance as object.</param>
        private void Controller_OnUpdateMailbox(object sender)
        {
            // Enable/disable the PCP device
            if (this.Controller.BusDiag.StatusRegister.RUN)
            {
                if (!this.PcpRS232_1.Ready && !this.PcpRS232_1.Error)
                {
                    if (!this.firstStartPcp)
                    {
                        this.firstStartPcp = true;
                        this.PcpRS232_1.Enable();
                    }
                }
            }
            else
            {
                if (this.PcpRS232_1.Ready || this.PcpRS232_1.Error)
                    this.PcpRS232_1.Disable();
            }

            // TODO insert your mailbox handling here (is called once for each MX cycle)
        }

        /// <summary>
        ///  Called whenever an error occurs in the controller object
        /// </summary>
        /// <param name="exceptionData">The exception.</param>
        private void Controller_OnException(Exception exceptionData)
        {
            // Shows each error message
            this.ExceptionList.Enqueue(exceptionData);

            // TODO your error handling can be inserted here
        }

        // Events from PCP_1

        /// <summary>
        /// Called, when enabling the pcp object succeeded. After this event you can
        /// use it.
        /// </summary>
        /// <param name="sender">The caller instance as object.</param>
        private void PCP_RS232_1_OnEnableReady(object sender)
        {
            // TODO insert your code here
        }

        /// <summary>
        /// Called for each successfull read confirmation
        /// </summary>
        /// <param name="sender">The caller instance as object.</param>
        /// <param name="data">The received read confirmation data.</param>
        private void PCP_RS232_1_ReadConfirmationReceived(object sender, byte[] data)
        {
            // TODO insert your code here
            lock (this.pcpReadBuffer)
            {
                this.pcpReadBuffer = new Byte[data.Length];
                this.pcpReadBuffer = data;
            }
        }

        /// <summary>
        /// Called for each successfull write confirmation
        /// </summary>
        /// <param name="sender">The caller instance as object.</param>
        private void PCP_RS232_1_WriteConfirmationReceived(object sender)
        {
            // TODO insert your code here
            this.pcpWriteOk = true;
        }

        /// <summary>
        /// Called whenever an error occurs in the pcp object
        /// </summary>
        /// <param name="exceptionData">The exception.</param>
        private void PCP_RS232_1_OnException(Exception exceptionData)
        {
            // Shows each error message
            this.ExceptionList.Enqueue(exceptionData);

            // TODO your error handling can be inserted here
        }

        #endregion *** Events From the Controller ********************************************

        #region *** Enable / Disable the Application **************************************

        /// <summary>
        /// This method enables the controller and the PCP devices
        /// </summary>
        public void Enable()
        {
            this.Controller.Enable();
            this.firstStartPcp = false;
        }

        /// <summary>
        /// This method disables the PCP devices and the controller
        /// </summary>
        public void Disable()
        {
            // Disable the PCP devices
            this.PcpRS232_1.Disable();

            // Waiting for the disconnection from the PCP terminal
            System.Threading.Thread.Sleep(this.Controller.UpdateMailboxTime * 4);

            // Disables the controller
            this.Controller.Disable();
        }

        #endregion *** Enable / Disable the Application **************************************

        #region *** Get the PCP-Data from the Controller **********************************

        /// <summary>
        /// Get the PCP read buffer
        /// </summary>
        public Byte[] PCP_ReadData
        {
            get
            {
                lock (this.pcpReadBuffer)
                {
                    return this.pcpReadBuffer;
                }
            }
        }

        /// <summary>
        /// Clear the PCP read buffer
        /// </summary>
        public void PCP_ReadDataClear()
        {
            lock (this.pcpReadBuffer)
            {
                this.pcpReadBuffer = new byte[0];
            }
        }

        /// <summary>
        /// Get the PCP write buffer
        /// </summary>
        public Boolean PCP_WriteDataOk
        {
            get { return this.pcpWriteOk; }
        }

        /// <summary>
        /// Clear the PCP write buffer
        /// </summary>
        public void PCP_WriteDataClear()
        {
            this.pcpWriteOk = false;
        }

        #endregion *** Get the PCP-Data from the Controller **********************************

        #region *** IDisposable Member ****************************************************

        public void Dispose()
        {
            if (this.Controller != null)
            {
                if (this.Controller.Connect || this.Controller.Error)
                {
                    this.Controller.Disable();

                    while (this.Controller.Connect || this.Controller.Error)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }

                this.Controller.Dispose();
            }
        }

        #endregion *** IDisposable Member ****************************************************
    }
}