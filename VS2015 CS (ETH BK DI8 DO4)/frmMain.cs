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
    using System.Windows.Forms;

    using PhoenixContact.HFI.Inline;
    using PhoenixContact.PxC_Library.Util;

    public partial class frmMain : Form
    {
        // Variable for the instance from the application class
        private HFI_Appl myApplication;

        public frmMain()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.myApplication.Dispose();

                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            // Create the instance from the application class
            this.myApplication = new HFI_Appl();

            // Initialize the controller control
            // Add a controller to the control
            this._ctrlController1.AddObject(this.myApplication.Controller);

            // Calling from the controller event for the first initialization
            this._ctrlController1_OnSelectController(this, this.myApplication.Controller);

            // Assign update timer to the controls
            this._ctrlController1.UpdateData = this.tmrUpdate;
            //this._ctrlVarInput1.UpdateData = this.tmrUpdate;
            //this._ctrlVarOutput1.UpdateData = this.tmrUpdate;
            //this._ctrlMessageClient1.UpdateData = this.tmrUpdate;
            //this._ctrlIBS_Diag1.UpdateData = this.tmrUpdate;

            // Fill the controlls with actual objects
            this._ctrlController1.OnSelectController += this._ctrlController1_OnSelectController;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.myApplication != null)
            {
                this.myApplication.Dispose();
            }
        }

        private void _ctrlController1_OnSelectController(object Sender, Object Controller)
        {
            // Add Inputs
            //this._ctrlVarInput1.ClearObject();

            //foreach (VarInput i in this.myApplication.Controller.InputObjectList)
            //{
            //    this._ctrlVarInput1.AddObject(i);
            //}

            //// Add Outputs
            //this._ctrlVarOutput1.ClearObject();

            //foreach (VarOutput i in this.myApplication.Controller.OutputObjectList)
            //{
            //    this._ctrlVarOutput1.AddObject(i);
            //}

            // Add Message Clients
            //this._ctrlMessageClient1.ClearObject();

            //foreach (MessageClient i in this.myApplication.Controller.MessageObjectList)
            //{
            //    this._ctrlMessageClient1.AddObject(i);
            //}

            //// Assign the actual controller to the ctrlIBS_Diag control
            //this._ctrlIBS_Diag1.ControlerConnection = Controller as IInterbusG4;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (this.myApplication.ExceptionList.Count > 0)
            {
                String errorText = Diagnostic.GetExceptionMessage(this.myApplication.ExceptionList.Dequeue());
                MessageBox.Show(errorText);
            }
        }
    }
}