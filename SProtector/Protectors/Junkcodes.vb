Imports dnlib.DotNet
Imports dnlib.DotNet.Emit

Namespace SProtector.Protectors

    Public Class Junkcodes
        Private Shared junktype As TypeDef
        Private Shared Sub typeadd(moduleDef As ModuleDef, tdef As TypeDef)
            moduleDef.Types.Add(tdef)
        End Sub
        Public Shared Sub Protect()
            For mDef2 As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1 ' get all modules
                On Error Resume Next ' ignore erros
                Dim moduleAnti = GlobAssembly.Asm.Modules(mDef2) ' get current module
                Dim moduleDefAsm2 = GlobAssembly.Asm2.Modules(0) ' get first modules from helper exe
                For j = 0 To moduleDefAsm2.Types.Count - 1 'get all helper exe's types
                    If moduleDefAsm2.Types(j).FullName = "Methods.Module1" Then 'check if its the one we want or not
                        junktype = moduleDefAsm2.Types(j) ' set tdantidebug type def variable to current type from helepr exe file
                        junktype.[Namespace] = "junk" ' set namespace for junktype class
                        moduleDefAsm2.Types.Remove(junktype) 'remove junktype type from helper module
                        typeadd(moduleAnti, junktype) ' add junktype type to module
                    End If
                Next
                For mDef = 0 To GlobAssembly.Asm.Modules.Count - 1 ' get all main assembly modules
                    Dim moduleDef = GlobAssembly.Asm.Modules(mDef) ' get current
                    For j = 0 To moduleDef.Types.Count - 1 ' get all types
                        Dim td = moduleDef.Types(j) ' get current type
                        antiDebug(td, GlobAssembly.Asm) 'inject fake method
                    Next
                Next
            Next


        End Sub

        Public Shared Sub antiDebug(td As TypeDef, asm As AssemblyDef)
            If td.[Namespace] = "junk" Then ' check if it is not itself :|
                Return
            End If

            If td.HasNestedTypes Then ' if typedef has nested type
                For i = 0 To td.NestedTypes.Count - 1 ' get all nested types
                    antiDebug(td.NestedTypes(i), asm) 'inject to nested types too !
                Next
            End If
            For Each mDef As MethodDef In td.Methods ' get all methods
                If mDef.HasBody Then ' check if it has body
                    Dim numberofjunks = Int(mDef.Body.Instructions.Count / 4) 'get number of junks to add numberofjunks
                    Dim beforeproceesscount = mDef.Body.Instructions.Count


                    For inst = 0 To mDef.Body.Instructions.Count - 1
                        On Error Resume Next
                        If mDef.Body.Instructions(inst).OpCode Is OpCodes.Call Then
                            mDef.Body.Instructions.Insert(inst + 1, New Instruction(OpCodes.[Call], junktype.Methods(0))) ' inject junkcode to a random point
                            inst += 3
                        End If

                    Next

                    mDef.Body.OptimizeBranches() ' OptimizeBranches
                    mDef.Body.SimplifyBranches() ' SimplifyBranches
                    Continue For
                End If
            Next
        End Sub
    End Class
End Namespace

