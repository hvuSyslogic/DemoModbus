#region Copyright
///////////////////////////////////////////////////////////////////////////////
//
//  Copyright PHOENIX CONTACT Software GmbH
//
///////////////////////////////////////////////////////////////////////////////
#endregion

namespace VS2015_CS_ETH_BK_DI8_DO4
{
    using System;
    using System.Collections.Generic;

    using PhoenixContact.HFI.Inline;
    using PhoenixContact.PxC_Library.Util;

    public sealed class HFI_Appl : IDisposable
    {
        // Information:
        // If you are using this programm you have to disable the
        // PnP Mode of the ETH BK DI8 DO4

        // Error logging
        public Queue<Exception> ExceptionList { get; private set; }

        // Create the controller
        public Controller_IBS_ETH Controller { get; private set; }

        #region *** Variable Declaration **************************************************
              
        #region *** Create input variables

        private VarInput   MODULE_2_IN  = new VarInput(0,PD_Length.Word,8,0);

        #endregion

        #region *** Create output variables

        private VarOutput  MODULE_1_OUT = new VarOutput(0,PD_Length.Word,4,0);
        private VarOutput  MODULE_3_OUT = new VarOutput(2,PD_Length.Word,8,0);

        #endregion

        #region *** Create PCP variables


        #endregion

        #endregion *** Variable Declaration **************************************************

        #region *** Constructor Declaration ***********************************************

        /// <summary>
        /// Constructor
        /// </summary>
        public HFI_Appl()
        {
            this.ExceptionList = new Queue<Exception>();

            // Create the controller
            this.Controller = new Controller_IBS_ETH("ETH BK DI8 DO4");

            // Settings for the controller
            this.Controller.Description = "ETH BK DI8 DO4 for Demonstaration";

            this.Controller.Startup = ControllerStartup.PhysicalConfiguration;
           //this.Controller.Connection = "192.168.0.1";
           this.Controller.Connection = "192.168.0.1";

            this.Controller.UpdateProcessDataCycleTime = 20;
            this.Controller.UpdateMailboxTime = 50;

            // The Controller.Configuration property contains special configurations for the controller
            //Controller.Configuration.DNS_NameResolution     = true;
            //Controller.Configuration.EnableIBS_Indications  = true;
            //Controller.Configuration.Read_IBS_Cycletime     = false;
            //Controller.Configuration.UpdateControllerState  = 100;

            #region *** Add Input variables

            Controller.AddObject(MODULE_2_IN);

            #endregion

            #region *** Add output variables

            Controller.AddObject(MODULE_1_OUT);
            Controller.AddObject(MODULE_3_OUT);

            #endregion

            #region *** Add PCP variables


            #endregion

            // Callbacks from the controller

            // Called once for each bus cycle
            this.Controller.OnUpdateProcessData += this.Controller_OnUpdateProcessData;

            // Called once for each mailbox cycle
            this.Controller.OnUpdateMailbox += this.Controller_OnUpdateMailbox;

            // Called whenever an error occurs in the controller object
            this.Controller.OnException += this.Controller_OnException;

            #region *** Create PCP callbacks


            #endregion
        }

        #endregion *** Constructor Declaration ***********************************************

        #region *** Events From the Controller ********************************************

        /// <summary>
        /// Called once for each bus cycle
        /// </summary>
        /// <param name="Sender"></param>
        private void Controller_OnUpdateProcessData(object Sender)
        {
            // TODO insert your process data handling (application) here
        }

        /// <summary>
        /// Called once for each mailbox cycle
        /// </summary>
        /// <param name="Sender"></param>
        private void Controller_OnUpdateMailbox(object Sender)
        {
            // TODO insert your mailbox handling here (is called once for each MX cycle)
        }

        /// <summary>
        ///  Called whenever an error occurs in the controller object
        /// </summary>
        /// <param name="ExceptionData"></param>
        private void Controller_OnException(Exception ExceptionData)
        {
            // Shows each error message
            this.ExceptionList.Enqueue(ExceptionData);

            // TODO your error handling can be inserted here
        }

        // Events from the PCP devices

        #region *** Create PCP events


        #endregion

        #endregion *** Events From the Controller ********************************************

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
