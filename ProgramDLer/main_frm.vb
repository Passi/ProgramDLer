Imports System.Net
Public Class main_frm
    Private Source As String = ""
    Private File As String = ""
    Private WithEvents httpclient As WebClient
    Private speed As String = "0"
    Dim sbytes As String = "0"
    Dim ebytes As String = "0"

    Private Sub main_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            IO.File.Delete(File)
            NotifyIcon1.Visible = True
            Timer1.Enabled = True
            Me.Text = "ProgramDLer - " + File
            Label1.Text = "Push image to start download"
            NotifyIcon1.BalloonTipText = "Push image to start download"
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

            sbytes = Bytes.ToString
            Label1.Text = Bytes.ToString & " KB von " & Totalbytes.ToString & " KB (" & Mbytes.ToString & " MB von " & Totalmbytes.ToString & " MB) (" & e.ProgressPercentage & "%) (" & speed.ToString & " KB/s)"
            NotifyIcon1.BalloonTipText = Bytes.ToString & " KB von " & Totalbytes.ToString & " KB (" & Mbytes.ToString & " MB von " & Totalmbytes.ToString & " MB) (" & e.ProgressPercentage & "%) (" & speed.ToString & " KB/s)"
        Catch ex As Exception
            MsgBox(ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        speed = sbytes.ToString - ebytes.ToString
        ebytes = sbytes.ToString
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If ProgressBar1.Value = "0" Then
            Try
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

    Private Sub NotifyIcon1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        NotifyIcon1.ShowBalloonTip(5)
    End Sub
End Class
