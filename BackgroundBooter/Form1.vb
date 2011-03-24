Imports System.Management
Public Class Form1
    Public ibss As String = ""
    Public kernel As String = ""
    Public SkipIPSWExtraction As Boolean
    Public FormClosed1 As Boolean = False

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Select()

        Dim a_strArgs() As String

        Dim i As Integer

        a_strArgs = Split(Command$, " ")
        For i = LBound(a_strArgs) To UBound(a_strArgs)
            Select Case LCase(a_strArgs(i))
                Case "-s"
                    MsgBox("Silent mode" & vbCrLf & "Runs in background and when DFU device is detected, it boots it.")
                    Me.Visible = False
                    SilentMode()
                Case ""

                Case "-h"
                    MsgBox("-s = Silent mode" & vbCrLf & " -Runs in background and when DFU device is detected, it boots it." & _
                            vbCrLf & "-h = Shows this menu")
                    Me.Close()
                Case Else
                    MsgBox("Invalid argument")
                    Me.Close()
            End Select
        Next
    End Sub

    Private Sub Form1_Closed(ByVal sender As System.Object, ByVal e As System.EventArgs) _
Handles Me.Closed
        FormClosed1 = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Label1.Select()
        Button1.Enabled = False
        System.IO.File.WriteAllBytes(temp & "\tetheredboot.exe", My.Resources.tetheredboot)
        If Not System.IO.Directory.Exists(Tetheredbootdir) Then
            System.IO.Directory.CreateDirectory(Tetheredbootdir)
        End If
        ipsw.ShowDialog()
        If ipsw.FileName = "" Then
            MsgBox("Nothing selected", MsgBoxStyle.Exclamation)
            Label1.Select()
            Button1.Enabled = True
            Exit Sub
        End If
        Delay(1)
        TextBox1.Text = "Status: Extracting"
        Label1.Select()
        Delay(2)
        If System.IO.Directory.Exists(Tetheredbootdir & "\IPSW") Then
            Try
                My.Computer.FileSystem.DeleteDirectory(Tetheredbootdir & "\IPSW", FileIO.DeleteDirectoryOption.DeleteAllContents)
            Catch ex As Exception
                Exit Sub
            End Try
        End If
        extractIPSW(ipsw.FileName)
        GetDeviceInfo()
        If device = "Not supported" Then
            Button1.Enabled = True
            Exit Sub
        End If
        TextBox3.Visible = True
        TextBox3.Text = "Device: " & devicename
        Label1.Select()
        Delay(3)
        If System.IO.File.Exists(temp & "\iBSS." & device & ".RELEASE.dfu") Then
            System.IO.File.Delete(temp & "\iBSS." & device & ".RELEASE.dfu")
        End If
        If System.IO.File.Exists(temp & "kernelcache.release." & model) Then
            System.IO.File.Delete(temp & "kernelcache.release." & model)
        End If
        System.IO.File.Copy(Tetheredbootdir & "\IPSW\Firmware\dfu\iBSS." & device & ".RELEASE.dfu", temp & "\iBSS." & device & ".RELEASE.dfu")
        System.IO.File.Copy(Tetheredbootdir & "\IPSW\kernelcache.release." & model, temp & "kernelcache.release." & model)
        BootTethered(temp & "\iBSS." & device & ".RELEASE.dfu", temp & "\kernelcache.release." & model)
        TextBox1.Text = "Status: Done!"
        Label1.Select()
        MsgBox("Your device will stay at a white screen. DO NOT TURN OFF, Your device is still booting")
        Button1.Enabled = True
    End Sub

    Private Sub TextBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.Click
        Label1.Select()
    End Sub

    Private Sub TextBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.Click
        Label1.Select()
    End Sub

    Private Sub TextBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.Click
        Label1.Select()
    End Sub

    Private Sub dfudetect_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles dfudetect.DoWork
        ChDir(Tetheredbootdir)
        Do Until IsInDFU = True
            Do Until IsInDFU = True
                IsInDFUText = " "
                Dim searcher As New  _
                    ManagementObjectSearcher( _
                          "root\CIMV2", _
                          "SELECT * FROM Win32_PnPEntity WHERE Description = 'Apple Recovery (DFU) USB Driver'")
                For Each queryObj As ManagementObject In searcher.Get()

                    IsInDFUText += (queryObj("Description"))
                Next
                If IsInDFUText.Contains("DFU") Then
                    'System.IO.File.WriteAllBytes(dropwnDir + "\DFU.True", My.Resources.bspatch_exe)
                    'System.IO.File.WriteAllText(Tetheredbootdir & "\DFU.True", IsInDFUText.ToString())
                    IsInDFU = True
                End If
            Loop
        Loop
    End Sub

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        MsgBox("Silent mode" & vbCrLf & "Runs tetheredboot.exe in background and when DFU device is detected, it boots it.")
        Me.Visible = False
        SilentMode()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Help.Show()
    End Sub
End Class
