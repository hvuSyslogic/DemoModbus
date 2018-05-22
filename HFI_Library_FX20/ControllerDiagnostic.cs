// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.ControllerDiagnostic
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

namespace PhoenixContact.HFI
{
  public enum ControllerDiagnostic
  {
    Inactive = 0,
    NoError = 33536, // 0x00008300
    ConfirmationTimeout = 49156, // 0x0000C004
    NotSupported = 49408, // 0x0000C100
    NoValidConnectionString = 49409, // 0x0000C101
    OpenNodeErrorDTI = 49410, // 0x0000C102
    OpenNodeErrorMXI = 49411, // 0x0000C103
    UpdateInputBlockStartAddress = 49412, // 0x0000C104
    UpdateInputBlockLength = 49413, // 0x0000C105
    UpdateOutputBlockStartAddress = 49414, // 0x0000C106
    UpdateOutputBlockLength = 49415, // 0x0000C107
    FirmwareServiceStateError = 49416, // 0x0000C108
    ProcessDataCycleTimeOutOfRange = 49417, // 0x0000C109
    MailboxDataCycleTimeOutOfRange = 49418, // 0x0000C10A
    ControllerStateCycleTimeOutOfRange = 49419, // 0x0000C10B
    EnableWatchdog = 49419, // 0x0000C10B
    DisableWatchdog = 49420, // 0x0000C10C
    GetWatchdogState = 49421, // 0x0000C10D
    WatchdogOccurred = 49422, // 0x0000C10E
    GetDiagnostic = 49424, // 0x0000C110
    GetDiagnosticEx = 49425, // 0x0000C111
    GetSlaveDiagnostic = 49426, // 0x0000C112
    EnableNetfail = 49435, // 0x0000C11B
    DisableNetfail = 49436, // 0x0000C11C
    GetNetfailState = 49437, // 0x0000C11D
    NetfailOccurred = 49438, // 0x0000C11E
    ClearNetfail = 49439, // 0x0000C11F
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
    CpuLoadToHigh = 51456, // 0x0000C900
    CpuLoadOk = 51457, // 0x0000C901
  }
}
