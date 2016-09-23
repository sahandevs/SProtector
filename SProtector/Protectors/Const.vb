

Imports System.Collections.Generic
Imports System.Globalization
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector



Namespace SProtector.Protectors
    Public Class Constants
        Private Shared StringProtection As New Tools.StringProtection 'a helper class fro generating random strings

        Public Shared Sub Protect()
            For mDef As Integer = 0 To GlobAssembly.Asm.Modules.Count - 1 ' get all modules
                Dim moduleDef As ModuleDef = GlobAssembly.Asm.Modules(mDef) ' get current
                constants(moduleDef) ' do constans process
                moduleDef.EntryPoint.Name = StringProtection.Random(30) ' generate a random name for entry point
            Next
        End Sub

        Public Shared Sub constants(moduleDef As ModuleDef)
            Dim body = New CilBody() 'create an empty Il body ( we wanna load crypt class from methods.exe ) 
            Dim methImplFlags = MethodImplAttributes.IL Or MethodImplAttributes.Managed ' some ImplFlags for the method we wanna add
            Dim methFlags = MethodAttributes.[Public] Or MethodAttributes.[Static] Or MethodAttributes.HideBySig Or MethodAttributes.ReuseSlot ' some Flags for the method we wanna add
            Dim type1 As TypeDef = New TypeDefUser("Constants", "Decrypt", moduleDef.CorLibTypes.[Object].TypeDefOrRef) 'create new type with decrypt method
            type1.Attributes = TypeAttributes.[Public] Or TypeAttributes.AutoLayout Or TypeAttributes.[Class] Or TypeAttributes.AnsiClass ' setting Type Attributes
            moduleDef.Types.Add(type1) ' add it to module
            Const Text = "‮ ‪" 'this is not just an empty string :D look sharper ^_^
            Dim tmp = StringProtection.Random(30) ' genereate a random string for method name
            Dim meth1 As MethodDef = New MethodDefUser(tmp + Text, MethodSig.CreateStatic(moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String], moduleDef.CorLibTypes.[String]), methImplFlags, methFlags) ' create method with settings
            Dim moduleDefAsm2 = GlobAssembly.Asm2.Modules(0) ' get helper exe's first module
            For j = 0 To moduleDefAsm2.Types.Count - 1 ' get all helper exe's types
                If moduleDefAsm2.Types(j).FullName = "Methods.Module1" Then ' check if the one we want
                    Dim td = moduleDefAsm2.Types(j) ' get curent
                    For i = 0 To td.Methods.Count - 1 ' get all methods
                        If td.Methods(i).FullName.Contains("::CryptDecrypt(") Then ' check if its the method we want
                            Dim md = td.Methods(i) ' get current method

                            'because  body.Instructions,variables is readonly i bypassed it :D

                            'set body with readed method
                            body.Instructions.Clear()
                            For Each item In md.Body.Instructions
                                body.Instructions.Add(item)
                            Next

                            'set body vars with readed vars
                            body.Variables.Clear()
                            For Each item In md.Body.Variables
                                body.Variables.Add(item)
                            Next

                            'add method to type we have added to module
                            type1.Methods.Add(meth1)
                            meth1.Body = body ' set method body
                            meth1.ParamDefs.Add(New ParamDefUser("text", 1)) 'add parameters to method

                        End If
                    Next
                End If
            Next


            Dim keys = New List(Of String)() ' a key list for prevent code to re crypt a string

            For j = 0 To moduleDef.Types.Count - 1
                If moduleDef.Types(j).[Namespace] = "Anti" Or moduleDef.Types(j).FullName.Contains("Constants") Or moduleDef.Types(j).FullName.ToLower.Contains("anti") Then ' check if it is not antidebugger or ctypt method
                    GoTo Endfor
                End If
                On Error Resume Next ' ignore errors

                Dim td = moduleDef.Types(j) ' get current module
                For qq = 0 To td.Methods.Count - 1 ' get all method
                    Dim mmDef = td.Methods(qq) ' get current method
                    If mmDef.HasBody Then ' check if method has body
                        For i = 0 To mmDef.Body.Instructions.Count - 1 ' get all body Instructions
                            Dim cur = mmDef.Body.Instructions(i) ' get current
                            If cur.OpCode Is OpCodes.Ldstr AndAlso Not keys.Contains(cur.Operand.ToString()) AndAlso Not mmDef.FullName.Contains(meth1.Name) Then ' check these things if its ok then we will crypt string and OpCodes.Ldstr is for setting a string
                                Dim key = StringProtection.CryptDecrypt(cur.Operand.ToString) ' crpyt orginal string with method
                                cur.Operand = key ' set it to fist parameter of crypt method
                                Const ran As Integer = 50 ' set random constant for random string lenght
                                mmDef.Body.Instructions.Insert(i + 1, New Instruction(OpCodes.Ldstr, StringProtection.Random(ran))) ' set a random string with random const lenght to second fake parameter
                                mmDef.Body.Instructions.Insert(i + 2, New Instruction(OpCodes.Call, meth1)) 'add Instructions to call decrypt method
                                keys.Add(key) ' add current operand for line 80 check
                                i += 2 ' skid Instructions we have added
                                mmDef.Body.OptimizeBranches() ' OptimizeBranches
                                mmDef.Body.SimplifyBranches() ' SimplifyBranches

                            End If

                            'follow code is for integer but now compelted yet :|

                            'If (cur.OpCode Is OpCodes.Ldc_I4 Or cur.OpCode Is OpCodes.Ldc_I4_S) AndAlso Not keysInt.Contains(cur.Operand.ToString()) AndAlso Not mmDef.FullName.Contains(meth1.Name) AndAlso Not mmDef.FullName.Contains(meth2.Name) Then

                            '    Dim key = (Int(cur.Operand) + Int(Rnd() * 100) + Math.Sin(Int(cur.Operand))
                            '    cur.Operand = key


                            '    keysInt.Add(key)
                            'End If
                        Next
                    End If
                Next
Endfor:
            Next
        End Sub
    End Class
End Namespace
