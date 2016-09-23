#Region "Using"

Imports dnlib.DotNet

#End Region

Namespace SProtector


    Public Class GlobAssembly
        Public Shared Asm As AssemblyDef ' loaded main assembly
        Public Shared AsmInputPath As String ' main assembly input path
        Public Shared AsmOutputPath As String ' main assembly output path
        Public Shared Asm2 As AssemblyDef 'loaded helper exe for injecting methods
    End Class
End Namespace

