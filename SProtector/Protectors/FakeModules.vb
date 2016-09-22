
Imports System.Collections.Generic
Imports System.Globalization
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector



Namespace SProtector.Protectors


    Public Class FakeModules
        Private Shared StringProtection As New Tools.StringProtection
        Public Shared Sub Protect()
            For mDef As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1
                Dim moduleDef As ModuleDef = GlobAssembly.Asm.Modules(mDef)
                Dim ran = Int(Rnd() * 100) + 100
                For i = 0 To ran
                    Dim type1 As TypeDef = New TypeDefUser(StringProtection.Random(30), StringProtection.Random(10), moduleDef.CorLibTypes.[Object].TypeDefOrRef)
                    type1.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass
                    moduleDef.Types.Add(type1)
                Next
                moduleDef.EntryPoint.Name = StringProtection.Random(30)
            Next
        End Sub


    End Class
End Namespace