Imports System.IO

Public Class Form1
    Dim selpro As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        Button1.BackColor = Color.FromArgb(128, 0, 255, 0)
        Button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 0, 255, 0)
        Button2.BackColor = Color.FromArgb(128, 200, 200, 0)
        Button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 200, 200, 0)
        Button3.BackColor = Color.FromArgb(128, 150, 0, 0)
        Button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 150, 0, 0)

        If My.Settings.path = Nothing Then
            Timer1.Start()
        Else
            Try
                For Each folder In IO.Directory.GetDirectories(My.Settings.path & "\valley-switcher\")
                    ListBox1.Items.Add(folder.ToString.Replace(My.Settings.path & "\valley-switcher\", ""))
                Next
                Me.Enabled = True
            Catch ex As Exception
                Timer1.Start()
            End Try
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Dim fbro As New FolderBrowserDialog
        MsgBox("Please select the Stardew Valley Folder.", MsgBoxStyle.OkOnly, "Valley Switcher")
anotherone:
        If fbro.ShowDialog() = DialogResult.OK Then
            If IO.Directory.Exists(fbro.SelectedPath & "\mods\") = True Then
                Panel1.Visible = True
                IO.Directory.CreateDirectory(fbro.SelectedPath & "\valley-switcher\")
                IO.Directory.CreateDirectory(fbro.SelectedPath & "\valley-switcher\No Mods\")
                My.Settings.path = fbro.SelectedPath
                My.Settings.Save()
                Timer2.Start()
            Else
                Panel1.Visible = False
                MsgBox("Cant find the mods folder in " & fbro.SelectedPath & " please check if the correct folder was choosen and smapi is installed!", MsgBoxStyle.OkOnly, "Valley Switcher")
                GoTo anotherone
            End If
        Else
            Application.Exit()
        End If

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        My.Settings.Reset()
        My.Settings.Save()
        Application.Exit()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Timer2.Stop()
        Try
            My.Computer.FileSystem.CopyDirectory(My.Settings.path & "\mods\", My.Settings.path & "\valley-switcher\All Mods\")
        Catch ex As Exception
        End Try
        Me.Enabled = True
        Panel1.Visible = False
        For Each folder In IO.Directory.GetDirectories(My.Settings.path & "\valley-switcher\")
            ListBox1.Items.Add(folder.ToString.Replace(My.Settings.path & "\valley-switcher\", ""))
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If selpro = Nothing Then
        Else
            Label2.Text = "Starting Profile: " & selpro & vbCrLf & "be starting soon!"
            Panel1.Visible = True
            Me.Enabled = False
            startprofile.Start()
        End If
    End Sub

    Private Sub ListBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedValueChanged
        selpro = ListBox1.SelectedItem.ToString
    End Sub

    Private Sub startprofile_Tick(sender As Object, e As EventArgs) Handles startprofile.Tick
        startprofile.Stop()
        IO.Directory.Delete(My.Settings.path & "\mods\", True)
            My.Computer.FileSystem.CopyDirectory(My.Settings.path & "\valley-switcher\" & selpro & "\", My.Settings.path & "\mods\")
            Process.Start(My.Settings.path & "\StardewModdingAPI.exe")
        Application.Exit()
    End Sub
End Class
