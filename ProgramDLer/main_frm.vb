Imports System.Net
Imports System.Math

Public Class main_frm
    Private Source As String = ""
    Private File As String = ""
    Private WithEvents httpclient As WebClient
    Private speed As String = "0"
    Dim sbytes As String = "0"
    Dim ebytes As String = "0"
    Dim time As String

    Private Sub main_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Button1.Text = "Download " + File.ToString
            IO.File.Delete(File)
            NotifyIcon1.Visible = True
            Timer1.Enabled = True
            httpclient = New WebClient

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
            Button1.Text = "Open " + File
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
            time = (Totalbytes - Bytes) / speed / 60
            time = Truncate(Int(time))
            If time = "0" Then
                time = "< " & time & " minutes"
            Else
                time = time & " minutes"
            End If
            sbytes = Bytes.ToString
            Label1.Text = "Size: " & Totalmbytes.ToString & " MB"
            Label2.Text = "Downloaded: " & Mbytes.ToString & " MB (" & e.ProgressPercentage & "%)"
            Label3.Text = "Speed: " & speed.ToString & " KB/s"
            Label4.Text = "Time: " & time.ToString
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        speed = sbytes.ToString - ebytes.ToString
        ebytes = sbytes.ToString
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ProgressBar1.Value = "0" Then
            Try
                Button1.Text = "Loading " + File
                httpclient.DownloadFileAsync(New Uri(Source), File.ToString & ".pdown")
            Catch ex As Exception
                MsgBox(ex.Message)
                Me.Close()
            End Try
        ElseIf ProgressBar1.Value = "100" Then
            Try
                Dim p As New Process
                p.StartInfo.FileName = File
                p.Start()
                NotifyIcon1.Visible = False
                Me.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                Me.Close()
            End Try
        Else
            Try
                Button1.Text = "Canceling download of " + File
                httpclient.CancelAsync()
                httpclient.Dispose()
                MsgBox("Download canceled!")
                IO.File.Delete(File.ToString)
                ProgressBar1.Value = "0"
                Label1.Text = "Size: 0 KB / 0 MB"
                Label2.Text = "Downloaded: 0%"
                Label3.Text = "Speed: 0 KB/s"
                Label4.Text = "Time: 0 minutes"
                Button1.Text = "Download " + File
            Catch ex As Exception
                MsgBox(ex.Message)
                Me.Close()
            End Try
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If Me.Visible = True Then
            Me.Hide()
        ElseIf Me.Visible = False Then
            Me.Show()
        End If
    End Sub
End Class
