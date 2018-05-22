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
    using System.Drawing;
    using System.Windows.Forms;

    using PhoenixContact.PxC_Library.Util;

    /// <summary>
    /// Delegate for the error logging event,
    /// </summary>
    /// <param name="pMessage"></param>
    public delegate void ShowLogMessage(String pMessage);

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class frmMain : Form
    {
        // Create the instance from a select controller class
        // TODO Please select you controller type
        private App_ETH_BK_DI8_DO4 myApplication;
        //private App_IBS_PCI_SC_IT myApplication;

        // Different variables
        private const String separator = "\r\n-----------------------------------------------------------------------------------------\r\n";

        private Boolean OutputEdit;

        #region *** Constructor / Destructor / IDisposable Declaration ********************

        /// <summary>
        /// Default constructor.
        /// </summary>
        public frmMain()
        {
            // This two events catch all unhandled exceptions.
            Application.ThreadException += this.Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            this.InitializeComponent();

            // TODO Please select you controller type
            this.myApplication = new App_ETH_BK_DI8_DO4();
            //this.myApplication = new App_IBS_PCI_SC_IT();
        }
        
        #endregion *** Constructor / Destructor / IDisposable Declaration ********************

        #region *** Global Exception Handling *********************************************

        /// <summary>
        /// Different error types.
        /// </summary>
        private enum ErrorType
        {
            Unknown = 0,
            Application = 1,        // possible Application errors from 1 to 29999
            Domain = 30000     // possible Domain errors from 30000 to int.MaxValue
        }

        /// <summary>
        /// Handle the application exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">he exception.</param>
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            this.ShowError(ErrorType.Application, e.Exception);
        }

        /// <summary>
        /// Handle the application domain exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">he exception.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.ShowError(ErrorType.Domain, (Exception)e.ExceptionObject);
        }

        /// <summary>
        /// Show the error message.
        /// </summary>
        /// <param name="type">
        /// The error type.
        /// </param>
        /// <param name="e">
        /// The exception.
        /// </param>
        private void ShowError(ErrorType type, Exception e)
        {
            // Fehlermeldung beim öffnen der Datei
            MessageBox.Show("Error Source = " + type.ToString() +
                            Environment.NewLine + Environment.NewLine +
                            EnvironmentInfo.GetAllInformation(e), Application.ProductName);

            // Exception behavior
            Application.Exit();
        }

        #endregion *** Global Exception Handling *********************************************

        #region *** Windows Application Handling ******************************************

        /// <summary>
        /// Start the windows form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, System.EventArgs e)
        {
            this.tbxControllerType.Text = this.myApplication.Controller.Name;
            this.tbxConnection.Text = this.myApplication.Controller.Connection;
        }

        /// <summary>
        /// Exit the windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.myApplication != null)
            {
                this.myApplication.Dispose();
            }
        }

        /// <summary>
        /// Enables the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnable_Click(object sender, System.EventArgs e)
        {
            this.myApplication.Controller.Connection = this.tbxConnection.Text;
            this.myApplication.Enable();
        }

        /// <summary>
        /// Diasables the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisable_Click(object sender, System.EventArgs e)
        {
            this.myApplication.Disable();
        }

        /// <summary>
        /// Calls the alarmstop of the controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlarmStop_Click(object sender, System.EventArgs e)
        {
            this.myApplication.Controller.BusHandling.AlarmStop();
        }

        /// <summary>
        /// Calls the autostart of the controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoStart_Click(object sender, System.EventArgs e)
        {
            this.myApplication.Controller.AutoStart();
        }

        /// <summary>
        /// Clear the TextBox with the error messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.tbxMessages.Clear();
        }

        /// <summary>
        /// Clear the ListBox with the logging data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, System.EventArgs e)
        {
            this.lbxLogging.Items.Clear();
        }

        #endregion *** Windows Application Handling ******************************************

        #region *** Edit the Output Variables *********************************************

        /// <summary>
        /// This flag stopps the output update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOutput_Enter(object sender, System.EventArgs e)
        {
            this.OutputEdit = true;
        }
        
        /// <summary>
        /// Update the output variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteValues_Click(object sender, System.EventArgs e)
        {
            // Update the process data outputs
            // Boolean values
            this.myApplication.OutBit_0.State = this.cbxOutputBit_0.Checked;
            this.myApplication.OutBit_1.State = this.cbxOutputBit_1.Checked;

            // Integer variable
            this.myApplication.OutVariable.Value = Convert.ToUInt64(this.tbxOutputValue.Text, 16);

            // ByteArray
            var tmpByteArray = this.StringToByteArray(this.tbxOutputByteArray.Text);

            if (tmpByteArray.Length == this.myApplication.OutByteArray.ByteArray.Length) this.myApplication.OutByteArray.ByteArray = tmpByteArray;

            this.OutputEdit = false;
        }

        #endregion *** Edit the Output Variables *********************************************

        #region *** Read/write PCP data ***************************************************

        /// <summary>
        /// Enables the PCP device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnable_2_Click(object sender, System.EventArgs e)
        {
            this.myApplication.PcpRS232_1.CommReference = 2;
            this.myApplication.PcpRS232_1.Enable();
        }

        /// <summary>
        /// Disables the PCP device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisable_2_Click(object sender, System.EventArgs e)
        {
            this.myApplication.PcpRS232_1.Disable();
        }

        /// <summary>
        /// Read data from the PCP device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadData_2_Click(object sender, System.EventArgs e)
        {
            if (this.myApplication.PcpRS232_1.ReadRequest(0x5fff, 0x0000)) this.lbxLogging.Items.Add("ReadRequest (0x5fff, 0x0000)");
        }

        /// <summary>
        /// Write data to the PCP device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteData_2_Click(object sender, System.EventArgs e)
        {
            Byte[] locByte = new Byte[1];
            locByte[0] = 0x08;

            if (this.myApplication.PcpRS232_1.WriteRequest(0x5fff, 2, locByte)) this.lbxLogging.Items.Add("WriteRequest (0x5fff, 2, " + Util.ByteArrayToHexStringW(locByte, ' ') + ")");
        }

        #endregion *** Read/write PCP data ***************************************************

        #region *** Update the Data of the Form *******************************************

        /// <summary>
        /// Update the main form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrMainFormUpdate_Tick(object sender, EventArgs e)
        {
            // Show the controller state
            this.cbxConnect.Checked = this.myApplication.Controller.Connect;
            this.cbxRun.Checked = this.myApplication.Controller.Run;
            this.cbxError.Checked = this.myApplication.Controller.Error;
            this.cbxWatchdog.Checked = this.myApplication.Controller.WatchdogOccurred;

            // Show the INTERBUS state
            this.cbxBusReady.Checked = this.myApplication.Controller.BusDiag.StatusRegister.READY;
            this.cbxBusActive.Checked = this.myApplication.Controller.BusDiag.StatusRegister.ACTIVE;
            this.cbxBusRun.Checked = this.myApplication.Controller.BusDiag.StatusRegister.RUN;
            this.cbxBusDetect.Checked = this.myApplication.Controller.BusDiag.StatusRegister.DETECT;
            this.cbxBusPF.Checked = this.myApplication.Controller.BusDiag.StatusRegister.PF;
            this.cbxBusBUS.Checked = this.myApplication.Controller.BusDiag.StatusRegister.BUS;

            this.tbxBusParameterregister.Text = this.myApplication.Controller.BusDiag.ParameterRegister.ToString("X4");
            this.tbxBusParameterregister2.Text = this.myApplication.Controller.BusDiag.ExtendedParameterRegister.ToString("X4");

            // Show the process data inputs
            // Boolean values
            this.cbxInpBit_0.Checked = this.myApplication.InBit_0.State;
            this.cbxInpBit_1.Checked = this.myApplication.InBit_1.State;

            // Integer variable
            this.tbxInpValue.Text = this.myApplication.InVariable.Value.ToString("X");

            // ByteArray
            this.tbxInpByteArray.Text = String.Format(new BinaryFormatter(), "{0:H}", this.myApplication.InByteArray.ByteArray);

            // Show the process data outputs
            if (!this.OutputEdit)
            {
                // Display mode show data
                this.cbxOutputBit_0.BackColor = DefaultBackColor;
                this.cbxOutputBit_1.BackColor = DefaultBackColor;
                this.tbxOutputValue.BackColor = DefaultBackColor;
                this.tbxOutputByteArray.BackColor = DefaultBackColor;

                // Boolean values
                this.cbxOutputBit_0.Checked = this.myApplication.OutBit_0.State;
                this.cbxOutputBit_1.Checked = this.myApplication.OutBit_1.State;

                // Integer variable
                this.tbxOutputValue.Text = this.myApplication.OutVariable.Value.ToString("X");

                // ByteArray
                this.tbxOutputByteArray.Text = String.Format(new BinaryFormatter(), "{0:H}", this.myApplication.OutByteArray.ByteArray);
            }
            else
            {
                // Display mode edit data
                this.cbxOutputBit_0.BackColor = Color.Yellow;
                this.cbxOutputBit_1.BackColor = Color.Yellow;
                this.tbxOutputValue.BackColor = Color.Yellow;
                this.tbxOutputByteArray.BackColor = Color.Yellow;
            }

            // Show the PCP data
            // PCP object state
            this.cbxPCPReady.Checked = this.myApplication.PcpRS232_1.Ready;
            this.cbxPCPError.Checked = this.myApplication.PcpRS232_1.Error;
            this.cbxReadDataValid.Checked = this.myApplication.PcpRS232_1.ReadDataValid;
            this.cbxReadDataError.Checked = this.myApplication.PcpRS232_1.ReadDataError;
            this.cbxWriteDataDone.Checked = this.myApplication.PcpRS232_1.WriteDataDone;

            // PCP data read/write

            if (this.myApplication.PCP_ReadData.Length > 0)
            {
                this.lbxLogging.Items.Add("ReadConfirmation: " + Util.ByteArrayToHexStringW(this.myApplication.PCP_ReadData, ' '));
                this.myApplication.PCP_ReadDataClear();
            }

            if (this.myApplication.PCP_WriteDataOk)
            {
                this.lbxLogging.Items.Add("WriteConfirmation successful.");
                this.myApplication.PCP_WriteDataClear();
            }

            if (this.myApplication.ExceptionList.Count > 0)
            {
                String excptionMessage = Diagnostic.GetExceptionMessage(this.myApplication.ExceptionList.Dequeue());

                if (this.tbxMessages.Text.Length == 0)
                {
                    this.tbxMessages.Text += excptionMessage;
                }
                else
                {
                    this.tbxMessages.Text = excptionMessage + separator + this.tbxMessages.Text;
                }
            }
        }

        #endregion *** Update the Data of the Form *******************************************

        #region *** Convert String to ByteArray *******************************************

        /// <summary>
        /// Convert a HEX string to a ByteArray
        /// </summary>
        private Byte[] StringToByteArray(String String)
        {
            Byte[] retElements = new Byte[0];

            if (String != "")
            {
                try
                {
                    var byteElements = String.Split(' ');
                    
                    retElements = new Byte[byteElements.Length];

                    for (Int32 i = 0; i < byteElements.Length; i++)
                    {
                        retElements[i] = Convert.ToByte(byteElements[i], 16);
                    }
                }
                catch
                {
                    // On error return zero bytes
                    retElements = new Byte[0];
                }
            }
            return retElements;
        }

        #endregion *** Convert String to ByteArray *******************************************
    }
}