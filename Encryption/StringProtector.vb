Namespace SEncryption
    Public Class StringEncryption
        Public Function Random(len As Integer) As String

            Dim text As String
            text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRST"
            Dim array As Char() = New Char(len - 1) {}
            For i As Integer = 0 To len - 1
                array(i) = text(Int(Rnd() * text.Length))
            Next

            Dim res = New String(array)
            Return res
        End Function
    End Class
End Namespace

