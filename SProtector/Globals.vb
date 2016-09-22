#Region "Using"

Imports dnlib.DotNet

#End Region

Namespace SProtector


    Public Class GlobAssembly
        Public Shared Asm As AssemblyDef
        Public Shared AsmInputPath As String
        Public Shared AsmOutputPath As String
        Public Shared reConstant As Boolean = False
        Public Shared crasher As Boolean = False
        Public Shared Asm2 As AssemblyDef
    End Class
End Namespace

