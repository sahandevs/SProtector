

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
            For mDef2 As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1
                On Error Resume Next


                Dim moduleAnti = GlobAssembly.Asm.Modules(mDef2)

                Dim moduleDefAsm2 = GlobAssembly.Asm2.Modules(0)

                For j = 0 To moduleDefAsm2.Types.Count - 1
                    '   MsgBox(moduleDefAsm2.Types(j).FullName)
                    If moduleDefAsm2.Types(j).FullName = "Methods.Module1" Then
                        tdAntiDebug = moduleDefAsm2.Types(j)
                        moduleDefAsm2.Types.Remove(tdAntiDebug)
                        tdAntiDebug.[Namespace] = "Anti"

                        typeadd(moduleAnti, tdAntiDebug)
                        '    moduleAnti.Types.Add()
                        '         MsgBox(moduleDefAsm2.Types(j).FullName)
                    End If
                    '     MsgBox("oked")
                Next
                'For Each m In tdAntiDebug.Methods
                '    MsgBox(m.Name)
                'Next
                For mDef = 0 To GlobAssembly.Asm.Modules.Count - 1
                    Dim moduleDef = GlobAssembly.Asm.Modules(mDef)
                    For j = 0 To moduleDef.Types.Count - 1
                        Dim td = moduleDef.Types(j)
                        antiDebug(td, GlobAssembly.Asm)
                    Next
                Next
            Next
        End Sub

        Public Shared Sub antiDebug(td As TypeDef, asm As AssemblyDef)
            If td.[Namespace] = "Anti" Or td.Name.ToLower.Contains("my") = False Then
                Return
            End If
            If td.HasNestedTypes Then
                For i = 0 To td.NestedTypes.Count - 1
                    antiDebug(td.NestedTypes(i), asm)
                Next
            End If
            '   Dim kk As Integer = 0
            '   MsgBox("lol")
            'For Each mt In tdAntiDebug.Methods
            '    MsgBox(mt.Name.ToString)
            'Next
            For Each mDef As MethodDef In td.Methods
                '      MsgBox(kk.ToString + " : " + mDef.FullName.ToString + " : " + td.Methods.Count.ToString)
                '  kk += 1
                If mDef.HasBody Then
                    Dim instrCount = mDef.Body.Instructions.Count
                    Dim nopPoint = New List(Of Integer)()
                    For Each InStr As Instruction In mDef.Body.Instructions
                        If InStr.OpCode Is OpCodes.Nop Then
                            nopPoint.Add(mDef.Body.Instructions.IndexOf(InStr))
                        End If
                    Next
                    If nopPoint.Count = 0 Then

                        '   Continue For
                    End If

                    '     Dim RandomNop = RandomNopPoint(nopPoint, instrCount / 3)
                    Dim Diff = 0
                    '      MsgBox(mDef.FullName)
                    '  mDef.Body.Instructions.Insert(0, New Instruction(OpCodes.[Call], tdAntiDebug.Methods(13)))
                    Continue For
                    For Each point In nopPoint

                        mDef.Body.Instructions.Insert(point + Diff, New Instruction(OpCodes.[Call], tdAntiDebug.Methods(13)))
                        Diff += 1
                    Next

                End If
            Next
        End Sub

        Public Shared Function RandomNopPoint(pointList As List(Of Integer), count As Integer) As List(Of Integer)
            Dim random = New Random()
            Dim points = New List(Of Integer)()
            For i = 0 To count - 1
                points.Add(pointList(random.[Next](pointList.Count)))
            Next

            Return points
        End Function
    End Class
End Namespace
