Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim port As Integer
        Dim buf As String, nick As String, myowner As String, server As String, chan As String
        Dim sock As New Net.Sockets.TcpClient()
        Dim input As IO.TextReader
        Dim output As IO.TextWriter
        Dim channel_joined As Boolean
        nick = "pingspammer"
        server = "irc.freenode.net"
        myowner = "pingspammer"
        port = 6667
        chan = "#oktw"
        sock.Connect(server, port)
        If Not sock.Connected Then
            MsgBox("Failed to connect!")
            Return
        End If
        input = New IO.StreamReader(sock.GetStream())
        output = New IO.StreamWriter(sock.GetStream())

        output.Write("USER " & nick & " 0 * :" & myowner & vbCr & vbLf & "NICK " & nick & vbCr & vbLf)
        output.Flush()
        While True
            buf = input.ReadLine()
            If buf.StartsWith("PING ") Then
                output.Write(buf.Replace("PING", "PONG") & vbCr & vbLf)
                output.Flush()
            End If
            If buf.Split(" "c)(1) = "001" Then
                output.Write("JOIN " & chan & vbCrLf)
                output.Flush()
                channel_joined = True
            End If
            If channel_joined = True Then
                output.Write("PRIVMSG #oktw :ping" & vbCrLf)
                output.Flush()
                Threading.Thread.Sleep(1000)
            End If
        End While
    End Sub
End Class
