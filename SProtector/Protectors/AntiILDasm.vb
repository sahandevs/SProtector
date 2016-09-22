Imports dnlib.DotNet
Imports dnlib.DotNet.Emit


Namespace SProtector.Protectors
    Public Class AntiILDasm
        Public Shared Sub Protect()
            For Each [module] As ModuleDef In GlobAssembly.Asm.Modules
                Dim attrRef As TypeRef = [module].CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressIldasmAttribute")
                Dim ctorRef = New MemberRefUser([module], ".ctor", MethodSig.CreateInstance([module].CorLibTypes.Void), attrRef)

                Dim attr = New CustomAttribute(ctorRef)
                [module].CustomAttributes.Add(attr)
            Next

        End Sub
    End Class
End Namespace

