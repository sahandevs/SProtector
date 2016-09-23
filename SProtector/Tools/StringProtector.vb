Imports Microsoft.VisualBasic.CompilerServices

Namespace SProtector.Tools
    Public Class StringProtection

        'Generates random string with lenght(len) you want
        Public Function Random(len As Integer) As String

            Dim charset As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRST" 'charset
            Dim array As Char() = New Char(len - 1) {} ' array of random string
            For i As Integer = 0 To len - 1
                array(i) = charset(Int(Rnd() * charset.Length)) ' put a random char into the array
            Next
            Return New String(array)
        End Function
        'Crypt/Decrpy text with RO13 method
        Public Function CryptDecrypt(text As String) As String
            Dim text2 As String = ""
            Dim arg_12_0 As Integer = 0
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
                    Dim value As Char
                    If num5 > 90 Then
                        Dim num6 As Integer = num5 - 90
                        value = Strings.ChrW(64 + num6)
                    Else
                        value = Strings.ChrW(num5)
                    End If
                    text2 += (value).ToString
                Else
                    If num4 <= 122 AndAlso num4 >= 97 Then

                        Dim value As Char
                        If (num5 > 122) Then
                            Dim num6 As Integer = num5 - 122
                            value = Strings.ChrW(96 + num6)
                        Else
                            value = Strings.ChrW(num5)
                        End If
                        text2 += (value).ToString
                    Else
                        text2 += (c).ToString
                    End If
                End If
                num2 += 1
            End While
            Return text2
        End Function




        'Public Function Encrypt(TheText As String) As String
        '    Dim text As String = ""
        '    Dim arg_13_0 As Integer = 1
        '    Dim num As Integer = Strings.Len(TheText)
        '    Dim num2 As Integer = arg_13_0
        '    While True
        '        Dim arg_4B_0 As Integer = num2
        '        Dim num3 As Integer = num
        '        If arg_4B_0 > num3 Then
        '            Exit While
        '        End If
        '        Dim str As String = Conversions.ToString(Strings.Chr(Strings.Asc(Strings.Mid(TheText, num2, 1)) + 2))
        '        text += str
        '        num2 += 1
        '    End While
        '    Return Strings.Trim(text)
        'End Function
        'Public Function CryptDecrypt2(s As String) As String
        '    Dim arg_0B_0 As Long = 1L
        '    Dim num As Long = CLng(Strings.Len(s))
        '    Dim num2 As Long = arg_0B_0
        '    ' The following expression was wrapped in a checked-statement
        '    Dim text As String
        '    While True
        '        Dim arg_3D_0 As Long = num2
        '        Dim num3 As Long = num
        '        If arg_3D_0 > num3 Then
        '            Exit While
        '        End If
        '        text += Conversions.ToString(Strings.Chr(Strings.Asc(Strings.Mid(s, CInt(num2), 1)) Xor 255))
        '        num2 += 1L
        '    End While
        '    Return text
        'End Function
    End Class
End Namespace

