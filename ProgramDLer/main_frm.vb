Imports System.Net
Public Class main_frm
    Private Source As String = ""
    Private File As String = ""
    Private WithEvents httpclient As WebClient

    Private Sub main_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Contains("-test") Then
            MsgBox("ProgramDLer funtkioniert! Klicke auf OK, um fortzufahren.")
        End If
        Try
            Me.Text = "ProgramDLer - " + File
            Label1.Text = "ProgramDLer ... Initialization"
            httpclient = New WebClient

            httpclient.DownloadFileAsync(New Uri(Source), File.ToString & ".pdown")

            ProgressBar1.Value = 0
            ProgressBar1.Maximum = 100
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub httpclient_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles httpclient.DownloadFileCompleted
        Try
            IO.File.Move(File.ToString & ".pdown", File)
            Dim p As New Process
            p.StartInfo.FileName = File
            p.Start()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub httpclient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs) Handles httpclient.DownloadProgressChanged
        Try
            ProgressBar1.Value = e.ProgressPercentage
            Dim Totalbytes As Long = e.TotalBytesToReceive / 1024
            Dim Bytes As Long = e.BytesReceived / 1024
            Dim Totalmbytes As Long = Totalbytes / 1024
            Dim Mbytes As Long = Bytes / 1024

            Label1.Text = Bytes.ToString & " KB von " & Totalbytes.ToString & " KB (" & Mbytes.ToString & "MB von " & Totalmbytes.ToString & "MB) (" & e.ProgressPercentage & "%)"
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub
End Class
