

Imports System.Collections.Generic
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit


Namespace SProtector.Protectors
    Friend Class AntiDebug
        Private Shared tdAntiDebug As TypeDef
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
                        tdAntiDebug = moduleDefAsm2.Types(j) ' set tdantidebug type def variable to current type from helepr exe file
                        tdAntiDebug.[Namespace] = "Anti" ' set namespace for antidebug class
                        moduleDefAsm2.Types.Remove(tdAntiDebug) 'remove antidebug type from main module
                        typeadd(moduleAnti, tdAntiDebug) ' add antidebug type to module
                    End If
                Next


                For mDef = 0 To GlobAssembly.Asm.Modules.Count - 1 ' get all main assembly modules
                    Dim moduleDef = GlobAssembly.Asm.Modules(mDef) ' get current
                    For j = 0 To moduleDef.Types.Count - 1 ' get all types
                        Dim td = moduleDef.Types(j) ' get current type
                        antiDebug(td, GlobAssembly.Asm) 'inject antidebug checker ( checkdebugger() ) 
                    Next
                Next
            Next


        End Sub

        Public Shared Sub antiDebug(td As TypeDef, asm As AssemblyDef)
            If td.[Namespace] = "Anti" Then ' check if it is not itself :|
                Return
            End If

            If td.HasNestedTypes Then ' if typedef has nested type
                For i = 0 To td.NestedTypes.Count - 1 ' get all nested types
                    antiDebug(td.NestedTypes(i), asm) 'inject to nested types too !
                Next
            End If
            For Each mDef As MethodDef In td.Methods ' get all methods
                If mDef.HasBody Then ' check if it has body
                    mDef.Body.Instructions.Insert(0, New Instruction(OpCodes.[Call], tdAntiDebug.Methods(13))) ' inject checkdebugger first of method
                    Continue For
                End If
            Next
        End Sub
    End Class
End Namespace
