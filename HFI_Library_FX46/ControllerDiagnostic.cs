// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.ControllerDiagnostic
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  public enum ControllerDiagnostic
  {
    Inactive = 0,
    NoError = 33536, // 0x00008300
    ConfirmationTimeout = 49156, // 0x0000C004
    PingTimeout = 49157, // 0x0000C005
    NotSupported = 49408, // 0x0000C100
    NoValidConnectionString = 49409, // 0x0000C101
    UpdateInputBlockStartAddress = 49412, // 0x0000C104
    UpdateInputBlockLength = 49413, // 0x0000C105
    UpdateOutputBlockStartAddress = 49414, // 0x0000C106
    UpdateOutputBlockLength = 49415, // 0x0000C107
    FirmwareServiceStateError = 49416, // 0x0000C108
    ProcessDataCycleTimeOutOfRange = 49417, // 0x0000C109
    MailboxDataCycleTimeOutOfRange = 49418, // 0x0000C10A
    ControllerStateCycleTimeOutOfRange = 49419, // 0x0000C10B
    GetDiagnostic = 49424, // 0x0000C110
    GetDiagnosticEx = 49425, // 0x0000C111
    GetSlaveDiagnostic = 49426, // 0x0000C112
    EnableNetfail = 49435, // 0x0000C11B
    DisableNetfail = 49436, // 0x0000C11C
    GetNetfailState = 49437, // 0x0000C11D
    NetfailOccurred = 49438, // 0x0000C11E
    ClearNetfail = 49439, // 0x0000C11F
    OnUpdateProcessData = 49440, // 0x0000C120
    OnUpdateMailbox = 49441, // 0x0000C121
    OnStateChangeEvents = 49442, // 0x0000C122
    CreateOnUpdateProcessData = 49443, // 0x0000C123
    EnableWatchdog = 49451, // 0x0000C12B
    DisableWatchdog = 49452, // 0x0000C12C
    GetWatchdogState = 49453, // 0x0000C12D
    WatchdogOccurred = 49454, // 0x0000C12E
    ParaErrGetByteFromBuffer = 49665, // 0x0000C201
    ParaErrPutByteToBuffer = 49666, // 0x0000C202
    ParaErrMessageClient = 49667, // 0x0000C203
    RetErrReceiveMessage = 49921, // 0x0000C301
    RetErrSendMessage = 49922, // 0x0000C302
    RetErrReadData = 49923, // 0x0000C303
    RetErrWriteData = 49924, // 0x0000C304
    NoValidMessageObject = 50176, // 0x0000C400
    NegConfSendMessage = 50177, // 0x0000C401
    ControllerIndication = 50178, // 0x0000C402
    IODiagnosticError = 50432, // 0x0000C500
    InterbusHandlingDiagnostic = 51713, // 0x0000CA01
    InterbusDriverDiagnostic = 51714, // 0x0000CA02
  }
}
