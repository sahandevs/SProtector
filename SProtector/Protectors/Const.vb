

Imports System.Collections.Generic
Imports System.Globalization
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector



Namespace SProtector.Protectors
    Public Class Constants
        Private Shared StringProtection As New Tools.StringProtection

        Public Shared Sub Protect()
            For mDef As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1
                Dim moduleDef As ModuleDef = GlobAssembly.Asm.Modules(mDef)

                constants(moduleDef)

                moduleDef.EntryPoint.Name = StringProtection.Random(30)
            Next
            If GlobAssembly.reConstant = False Then
                GlobAssembly.reConstant = True
            Else
                GlobAssembly.crasher = True
            End If

        End Sub

        Public Shared Sub constants(moduleDef As ModuleDef)
            Dim body = New CilBody()
            '  Dim body2 = New CilBody()
            Dim methImplFlags = MethodImplAttributes.IL Or MethodImplAttributes.Managed
            Dim methFlags = MethodAttributes.[Public] Or MethodAttributes.[Static] Or MethodAttributes.HideBySig Or MethodAttributes.ReuseSlot
            Dim prefix As String = ""
            If GlobAssembly.reConstant = True Then
                prefix = "re"
            ElseIf GlobAssembly.crasher = True Then
                prefix = "crasher"
            End If

            Dim type1 As TypeDef = New TypeDefUser(prefix + "Constants", "Decrypt", moduleDef.CorLibTypes.[Object].TypeDefOrRef)
            type1.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass
            moduleDef.Types.Add(type1)
            Dim type2 As TypeDef = New TypeDefUser(prefix + "Constants2", "Decrypt2", moduleDef.CorLibTypes.[Object].TypeDefOrRef)
            type2.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass
            moduleDef.Types.Add(type2)
            Dim type3 As TypeDef = New TypeDefUser(prefix + "Constants3", "Decrypt3", moduleDef.CorLibTypes.[Object].TypeDefOrRef)
            type3.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass
            moduleDef.Types.Add(type3)
            Dim Text = "‮ ‪"

            Dim tmp = StringProtection.Random(30)
            Dim meth1 As MethodDef = New MethodDefUser(tmp, MethodSig.CreateStatic(moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String]), methImplFlags, methFlags)
            Dim meth2 As MethodDef = New MethodDefUser(tmp + Text, MethodSig.CreateStatic(moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String]), methImplFlags, methFlags)
            Dim meth3 As MethodDef = New MethodDefUser(tmp + Text + Text, MethodSig.CreateStatic(moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String]), methImplFlags, methFlags)
            Dim moduleDefAsm2 = GlobAssembly.Asm2.Modules(0)
            For j = 0 To moduleDefAsm2.Types.Count - 1
                '  MsgBox(moduleDefAsm2.Types(j).FullName)
                If moduleDefAsm2.Types(j).FullName = "Methods.Module1" Then

                    Dim td = moduleDefAsm2.Types(j)
                    For i = 0 To td.Methods.Count - 1
                        '     MsgBox(td.Methods(i).FullName)
                        If td.Methods(i).FullName.Contains("::CryptDecrypt(") Then
                            Dim md = td.Methods(i)
                            '         body.Instructions = md.Body.Instructions
                            body.Instructions.Clear()
                            For Each item In md.Body.Instructions
                                body.Instructions.Add(item)
                            Next

                            body.Variables.Clear()
                            For Each item In md.Body.Variables
                                body.Variables.Add(item)
                            Next
                            '   body.Variables = md.Body.Variables

                            type1.Methods.Add(meth1)
                            meth1.Body = body
                            meth1.ParamDefs.Add(New ParamDefUser("text", 1))

                        End If
                        If td.Methods(i).FullName.Contains("::DEncrypt(") Then
                            Dim md = td.Methods(i)
                            '         body.Instructions = md.Body.Instructions
                            body.Instructions.Clear()
                            For Each item In md.Body.Instructions
                                body.Instructions.Add(item)
                            Next

                            body.Variables.Clear()
                            For Each item In md.Body.Variables
                                body.Variables.Add(item)
                            Next
                            '   body.Variables = md.Body.Variables

                            type2.Methods.Add(meth2)
                            meth2.Body = body
                            meth2.ParamDefs.Add(New ParamDefUser("TheText", 1))

                        End If

                        If td.Methods(i).FullName.Contains("::CryptDecrypt2(") Then
                            Dim md = td.Methods(i)
                            '         body.Instructions = md.Body.Instructions
                            body.Instructions.Clear()
                            For Each item In md.Body.Instructions
                                body.Instructions.Add(item)
                            Next

                            body.Variables.Clear()
                            For Each item In md.Body.Variables
                                body.Variables.Add(item)
                            Next
                            '   body.Variables = md.Body.Variables

                            type3.Methods.Add(meth3)
                            meth3.Body = body
                            meth3.ParamDefs.Add(New ParamDefUser("s", 1))

                        End If

                    Next
                End If
            Next


            Dim keys = New List(Of String)()
            Dim keysInt = New List(Of String)()

            For j = 0 To moduleDef.Types.Count - 1
                If moduleDef.Types(j).[Namespace] = "Anti" Or moduleDef.Types(j).FullName.Contains("Constants") Or moduleDef.Types(j).FullName.ToLower.Contains("anti") Then
                    GoTo endfor
                End If
                On Error Resume Next

                Dim td = moduleDef.Types(j)
                For qq = 0 To td.Methods.Count - 1
                    Dim mmDef = td.Methods(qq)
                    If Not mmDef.HasBody Then
                        GoTo HasntBody
                    End If
                    Dim instrCount = mmDef.Body.Instructions.Count
                    For i = 0 To mmDef.Body.Instructions.Count - 1
                        Dim cur = mmDef.Body.Instructions(i)

                        If cur.OpCode Is OpCodes.Ldstr AndAlso Not keys.Contains(cur.Operand.ToString()) AndAlso Not mmDef.FullName.Contains(meth1.Name) AndAlso Not mmDef.FullName.Contains(meth2.Name) Then
                            '  MsgBox(i)
                            Dim rnd = New Random()
                            Dim nextValue = rnd.Next(3)
                            Select Case nextValue
                                Case 0, 1, 2
                                    Dim key = StringProtection.CryptDecrypt(cur.Operand.ToString)
                                    cur.Operand = key
                                    '  mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, key(0)))
                                    '   mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.[Call], meth1))
                                    Dim ran As Integer = 50
                                    If GlobAssembly.reConstant = True Then ran = 100

                                    mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, StringProtection.Random(ran)))
                                    mmDef.Body.Instructions.Insert(i + 2, New Instruction(OpCodes.Call, meth1))
                                    If GlobAssembly.reConstant = True Then
                                        instrCount += 2
                                    Else
                                        i += 2
                                        instrCount += 2
                                    End If

                                    '  mmDef.Body.KeepOldMaxStack = True
                                    mmDef.Body.OptimizeBranches()

                                    mmDef.Body.SimplifyBranches()

                                    '        keys.Add(key)
                                    Exit Select
                                    'Case 1
                                    '    Dim key = StringProtection.Encrypt(cur.Operand.ToString)
                                    '    cur.Operand = key
                                    '    '  mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, key(0)))
                                    '    '   mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.[Call], meth1))
                                    '    mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, StringProtection.Random(10)))
                                    '    mmDef.Body.Instructions.Insert(i + 2, New Instruction(OpCodes.Call, meth2))
                                    '    instrCount += 2
                                    '    '  mmDef.Body.KeepOldMaxStack = True
                                    '    mmDef.Body.OptimizeBranches()

                                    '    mmDef.Body.SimplifyBranches()

                                    '    keys.Add(key)
                                    '    Exit Select
                                    'Case 2
                                    '    Dim key = StringProtection.CryptDecrypt2(cur.Operand.ToString)
                                    '    cur.Operand = key
                                    '    '  mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, key(0)))
                                    '    '   mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.[Call], meth1))
                                    '    mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, StringProtection.Random(50)))
                                    '    mmDef.Body.Instructions.Insert(i + 2, New Instruction(OpCodes.Call, meth3))
                                    '    instrCount += 2
                                    '    '  mmDef.Body.KeepOldMaxStack = True
                                    '    mmDef.Body.OptimizeBranches()

                                    '    mmDef.Body.SimplifyBranches()

                                    '    keys.Add(key)
                                    '    Exit Select
                            End Select

                        End If
                        'If (cur.OpCode Is OpCodes.Ldc_I4 Or cur.OpCode Is OpCodes.Ldc_I4_S) AndAlso Not keysInt.Contains(cur.Operand.ToString()) AndAlso Not mmDef.FullName.Contains(meth1.Name) AndAlso Not mmDef.FullName.Contains(meth2.Name) Then

                        '    Dim key = (Int(cur.Operand) + Int(Rnd() * 100) + Math.Sin(Int(cur.Operand))
                        '    cur.Operand = key


                        '    keysInt.Add(key)
                        'End If
                    Next
HasntBody:


                Next
Endfor:
            Next
        End Sub


        Public Shared Function FromHex(value As String) As Integer
            ' strip the leading 0x
            If value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) Then
                value = value.Substring(2)
            End If
            Return Integer.Parse(value, NumberStyles.HexNumber)
        End Function

        Public Shared Function IterateType(td As TypeDef, asm As AssemblyDef) As IList(Of Instruction)
            Return Nothing
        End Function
    End Class
End Namespace
