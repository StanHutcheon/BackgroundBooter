Imports Ionic.Zip
Imports System.Management

Module cmds
    Private Property winstyle As ProcessWindowStyle
    Public device As String = ""
    Public devicename As String = ""
    Public model As String = ""
    Public tetheredboot As Process = New Process()
    Public temp = My.Computer.FileSystem.SpecialDirectories.Temp
    Public Tetheredbootdir = temp & "\tetheredbootdir"
    Public IsInDFU As Boolean = False
    Public IsInDFUText As String = ""

    Sub Delay(ByVal dblSecs As Double)

        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            Application.DoEvents()
        Loop
    End Sub

    Public Sub ShellWait(ByVal file As String, ByVal arg As String)
        Dim procNlite As New Process
        winstyle = 1
        procNlite.StartInfo.FileName = file
        procNlite.StartInfo.Arguments = " " & arg
        procNlite.StartInfo.WindowStyle = winstyle
        Application.DoEvents()
        procNlite.Start()
        Do Until procNlite.HasExited
            Application.DoEvents()
            For i = 0 To 5000000
                Application.DoEvents()
            Next
        Loop
        procNlite.WaitForExit()
    End Sub

    Public Sub BootTethered(ByVal iBSS As String, ByVal kernelcache As String)
        Form1.TextBox1.Text = "Status: Booting"
        Form1.Label1.Select()
        ShellWait(temp & "\tetheredboot.exe", " -i " & iBSS & " -k " & kernelcache)
    End Sub

    Public Sub GetDeviceInfo()
        ChDir(Tetheredbootdir)
        If System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n90ap.RELEASE.dfu") Then
            device = "n90ap"
            model = "n90"
            devicename = "iPhone 4"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n81ap.RELEASE.dfu") Then
            device = "n81ap"
            model = "n81"
            devicename = "iPod Touch 4G"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.k48ap.RELEASE.dfu") Then
            device = "k48ap"
            model = "k48"
            devicename = "iPad"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.k66ap.RELEASE.dfu") Then
            device = "k66ap"
            model = "k66"
            devicename = "Apple TV 2"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n88ap.RELEASE.dfu") Then
            device = "n88ap"
            model = "n88"
            devicename = "iPhone 3GS"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n18ap.RELEASE.dfu") Then
            device = "n18ap"
            model = "n18"
            devicename = "iPod Touch 3G"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n92ap.RELEASE.dfu") Then
            device = "n92ap"
            model = "n92"
            devicename = "iPhone 4 [Verizon]"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n72ap.RELEASE.dfu") Then
            device = "n72ap"
            model = "n72"
            devicename = "iPod Touch 2G"
        ElseIf System.IO.File.Exists(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS.n82ap.RELEASE.dfu") Then
            device = "n82ap"
            model = "n82"
            devicename = "iPhone 3G"
        Else
            MsgBox("Device not supported", MsgBoxStyle.Exclamation)
            device = "Not supported"
        End If
    End Sub

    Public Sub extractIPSW(ByVal IPSW As String)
        Using zip1 As Ionic.Zip.ZipFile = ZipFile.Read(IPSW)
            zip1.ExtractAll(Tetheredbootdir + "\IPSW\")
            zip1.Dispose()
        End Using
    End Sub

    Public Sub SilentMode()
        System.IO.File.WriteAllBytes(temp & "\tetheredboot.exe", My.Resources.tetheredboot)
        If Not System.IO.Directory.Exists(Tetheredbootdir) Then
            System.IO.Directory.CreateDirectory(Tetheredbootdir)
        End If
        Form1.ipsw.ShowDialog()
        If Form1.ipsw.FileName = "" Then
            Form1.Close()
        End If
        Form1.ShowInTaskbar = False
        If System.IO.Directory.Exists(Tetheredbootdir & "\IPSW") Then
            Try
                My.Computer.FileSystem.DeleteDirectory(Tetheredbootdir & "\IPSW", FileIO.DeleteDirectoryOption.DeleteAllContents)
            Catch ex As Exception
                Exit Sub
            End Try
        End If
        extractIPSW(Form1.ipsw.FileName)
        GetDeviceInfo()
        Do Until System.IO.File.Exists(temp & "\Close.Boot")
            BootTethered(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS." & device & ".RELEASE.dfu", Tetheredbootdir & "\IPSW\kernelcache.release." & model)
        Loop
        System.IO.File.Delete(temp & "\Close.Boot")
        Form1.Close()
    End Sub
End Module
