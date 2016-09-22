Imports Microsoft.VisualBasic.CompilerServices

Namespace SProtector.Tools
    Public Class StringProtection
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
        Public Function Encrypt(TheText As String) As String
            Dim text As String = ""
            Dim arg_13_0 As Integer = 1
            Dim num As Integer = Strings.Len(TheText)
            Dim num2 As Integer = arg_13_0
            ' The following expression was wrapped in a checked-statement
            While True
                Dim arg_4B_0 As Integer = num2
                Dim num3 As Integer = num
                If arg_4B_0 > num3 Then
                    Exit While
                End If
                Dim str As String = Conversions.ToString(Strings.Chr(Strings.Asc(Strings.Mid(TheText, num2, 1)) + 2))
                text += str
                num2 += 1
            End While
            Return Strings.Trim(text)
        End Function
        Public Function CryptDecrypt2(s As String) As String
            Dim arg_0B_0 As Long = 1L
            Dim num As Long = CLng(Strings.Len(s))
            Dim num2 As Long = arg_0B_0
            ' The following expression was wrapped in a checked-statement
            Dim text As String
            While True
                Dim arg_3D_0 As Long = num2
                Dim num3 As Long = num
                If arg_3D_0 > num3 Then
                    Exit While
                End If
                text += Conversions.ToString(Strings.Chr(Strings.Asc(Strings.Mid(s, CInt(num2), 1)) Xor 255))
                num2 += 1L
            End While
            Return text
        End Function
        Public Function CryptDecrypt(text As String) As String
            Dim text2 As String = ""
            Dim arg_12_0 As Integer = 0
            ' The following expression was wrapped in a checked-statement
            Dim num As Integer = text.Length - 1
            Dim num2 As Integer = arg_12_0
            While True
                Dim arg_EF_0 As Integer = num2
                Dim num3 As Integer = num
                If arg_EF_0 > num3 Then
                    Exit While
                End If
                Dim c As Char = Convert.ToChar(text.Substring(num2, 1))
                Dim num4 As Integer = AscW(c)
                Dim num5 As Integer = num4 + 13
                Dim flag As Boolean = num4 <= 90 AndAlso num4 >= 65
                If flag Then
                    Dim flag2 As Boolean = num5 > 90
                    Dim value As Char
                    If flag2 Then
                        Dim num6 As Integer = num5 - 90
                        value = Strings.ChrW(64 + num6)
                    Else
                        value = Strings.ChrW(num5)
                    End If
                    text2 += Conversions.ToString(value)
                Else
                    Dim flag2 As Boolean = num4 <= 122 AndAlso num4 >= 97
                    If flag2 Then
                        flag = (num5 > 122)
                        Dim value As Char
                        If flag Then
                            Dim num6 As Integer = num5 - 122
                            value = Strings.ChrW(96 + num6)
                        Else
                            value = Strings.ChrW(num5)
                        End If
                        text2 += Conversions.ToString(value)
                    Else
                        text2 += Conversions.ToString(c)
                    End If
                End If
                num2 += 1
            End While
            Return text2
        End Function
    End Class
End Namespace

