Imports System.Net
Public Class main_frm

    Private WithEvents httpclient As WebClient

    Private Sub main_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Contains("-test") Then
            MsgBox("ProgramDLer v2 funtkioniert! Klicke auf OK, um fortzufahren.")
        End If
        Try
            Me.Text = "ProgramDLer v2 - " + My.Settings.download_file
            httpclient = New WebClient

            Dim SourceURL As String = My.Settings.download_url
            Dim ZielDatei As String = My.Settings.download_file

            httpclient.DownloadFileAsync(New Uri(SourceURL), ZielDatei)

            ProgressBar1.Value = 0
            ProgressBar1.Maximum = 100
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub

    Private Sub httpclient_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles httpclient.DownloadFileCompleted
        Try
            Dim p As New Process
            p.StartInfo.FileName = My.Settings.download_file
            p.Start()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub

    Private Sub httpclient_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs) Handles httpclient.DownloadProgressChanged
        Try
            ProgressBar1.Value = e.ProgressPercentage

            Dim Totalbytes As Long = e.TotalBytesToReceive / 1024 / 1024
            Dim Bytes As Long = e.BytesReceived / 1024 / 1024

            Label1.Text = Bytes.ToString & " MB von " & Totalbytes.ToString & " MB (" & e.ProgressPercentage & "%)"
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub
End Class
