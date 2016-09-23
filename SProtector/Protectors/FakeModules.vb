
Imports System.Collections.Generic
Imports System.Globalization
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector
Namespace SProtector.Protectors
    Public Class FakeModules
        Private Shared StringProtection As New Tools.StringProtection 'Creating a var for use StringProtection's random class
        Public Shared Sub Protect()

            For mDef As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1 ' for every modules in main assembly
                Dim moduleDef As ModuleDef = GlobAssembly.Asm.Modules(mDef) ' gets current module
                Dim ran As Integer = Int(Rnd() * 100) + 100 ' create a random number for how many fake modules will add
                For i = 0 To ran
                    Dim type1 As TypeDef = New TypeDefUser(StringProtection.Random(30), StringProtection.Random(10), moduleDef.CorLibTypes.[Object].TypeDefOrRef) 'Create a new type ( empty type ) to add into the module as a fake module
                    type1.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass ' setting the type Attributes
                    moduleDef.Types.Add(type1) ' add created type to the module
                Next
                moduleDef.EntryPoint.Name = StringProtection.Random(30) ' generate a random entry point name
            Next
        End Sub


    End Class
End Namespace