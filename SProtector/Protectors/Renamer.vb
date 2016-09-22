Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector.Tools.StringProtection
Namespace SProtector.Protectors
    Class generatedStrings
        Private strings As New List(Of String)
        Private Shared StringProtection As New Tools.StringProtection
        Public Function random(ByVal len As Integer) As String
            Dim tmp
Back:
            tmp = StringProtection.Random(len + 30)

            If strings.IndexOf(tmp) <> -1 Then
                GoTo Back
            End If


            strings.Add(tmp)
            Return tmp
        End Function
    End Class

    Class Renamer
        Private Shared StringProtection As New generatedStrings
        Private Shared TypeDefNames As New Dictionary(Of String, String)()
        Private Shared namespaces As New Collection(Of TypeDef)()
        Private Shared NamespaceNames As New List(Of String)()
        Private Shared NamespaceNamesEncrypt As New List(Of String)()
        Public Shared Sub Protect()
            For mDef = 0 To GlobAssembly.Asm.Modules.Count - 1
                Dim moduleDef = GlobAssembly.Asm.Modules(mDef)
                For j = 0 To moduleDef.Types.Count - 1
                    Dim td = moduleDef.Types(j)
                    renamer(td, GlobAssembly.Asm)
                Next
                moduleDef.EntryPoint.Name = StringProtection.random(Int(Rnd() * 100))
            Next
        End Sub

        Public Shared Sub renamer(td As TypeDef, asm As AssemblyDef)
            If td.HasNestedTypes Then
                For i = 0 To td.NestedTypes.Count - 1
                    renamer(td.NestedTypes(i), asm)
                Next
            End If

            If Not NamespaceNames.Contains(td.[Namespace]) Then
                NamespaceNames.Add(td.[Namespace])
                NamespaceNamesEncrypt.Add(StringProtection.random(Int(Rnd() * 100)))
            End If
            Dim [Namespace] = NamespaceNamesEncrypt(NamespaceNames.IndexOf(td.[Namespace]))
            If checkType(td) Then
                Dim text = StringProtection.random(Int(Rnd() * 100))

                If td.BaseType Is Nothing Then
                    Return
                End If
                If td.IsNestedAssembly Then
                    Return
                End If
                'if (td.BaseType.Name.Contains("Form"))
                '{
                For Each res In asm.ManifestModule.Resources
                    If res.Name.Contains(td.[Namespace]) AndAlso td.[Namespace] <> "" AndAlso res.Name.Contains(td.Name) AndAlso td.Name <> "" Then
                        Dim resName As String = res.Name.Replace(".resources", "")
                        res.Name = res.Name.Replace(td.[Namespace], [Namespace])
                        res.Name = res.Name.Replace(td.Name, text)

                        For Each mDef In td.Methods
                            'if (!mDef.HasBody && !mDef.FullName.Contains("getRes")) continue;
                            If mDef.HasBody Then
                                For Each instr As Instruction In mDef.Body.Instructions
                                    If instr.OpCode Is OpCodes.Ldstr Then

                                        If resName IsNot Nothing AndAlso instr.Operand.ToString().Contains(resName) Then
                                            instr.Operand = instr.Operand.ToString().Replace(td.[Namespace], [Namespace]).Replace(td.Name, text)
                                        End If
                                    End If
                                Next
                                ' }
                            End If
                        Next
                    End If
                Next

                td.Name = text
                td.[Namespace] = [Namespace]

                For i = 0 To td.Methods.Count - 1
                    Dim md = td.Methods(i)
                    If checkMethod(md) Then
                        md.Name = StringProtection.random(Int(Rnd() * 100))
                        For Each current In md.ParamDefs
                            current.Name = StringProtection.random(Int(Rnd() * 100))
                        Next
                        If md.HasBody AndAlso md.Body.HasVariables Then
                            For Each current2 In md.Body.Variables
                                current2.Name = StringProtection.random(Int(Rnd() * 100))
                            Next
                        End If
                    End If
                Next
                For i = 0 To td.Fields.Count - 1
                    Dim fd = td.Fields(i)
                    If checkField(fd) Then
                        fd.Name = StringProtection.random(Int(Rnd() * 100))
                    End If
                Next
                For i = 0 To td.Events.Count - 1
                    Dim ed = td.Events(i)
                    If checkEvent(ed) Then
                        ed.Name = StringProtection.random(Int(Rnd() * 100))
                    End If
                Next
                For i = 0 To td.Properties.Count - 1
                    Dim pd = td.Properties(i)
                    If checkProperty(pd) Then
                        pd.Name = StringProtection.random(Int(Rnd() * 100))
                    End If
                Next
            End If
        End Sub

        Public Shared Function checkMethod(md As MethodDef) As Boolean
            If Not md.IsConstructor AndAlso Not md.IsFamilyAndAssembly AndAlso Not md.IsSpecialName AndAlso Not md.IsRuntimeSpecialName AndAlso Not md.IsRuntime AndAlso Not md.IsFamily Then
                If md.DeclaringType.BaseType IsNot Nothing Then
                    If Not md.DeclaringType.BaseType.Name.Contains("Delegate") Then
                        Return True
                    End If
                End If
                Return False
            End If
            Return False
        End Function

        Public Shared Function checkField(fd As FieldDef) As Boolean
            Return Not fd.IsFamilyOrAssembly AndAlso Not fd.IsSpecialName AndAlso Not fd.IsRuntimeSpecialName AndAlso Not fd.IsFamily AndAlso Not fd.DeclaringType.IsEnum AndAlso Not fd.DeclaringType.BaseType.Name.Contains("Delegate")
        End Function

        Public Shared Function checkProperty(pd As PropertyDef) As Boolean
            Return Not pd.IsSpecialName AndAlso Not pd.IsRuntimeSpecialName AndAlso Not pd.DeclaringType.Name.Contains("AnonymousType")
        End Function

        Public Shared Function checkEvent(ed As EventDef) As Boolean
            Return Not ed.IsSpecialName AndAlso Not ed.IsRuntimeSpecialName
        End Function

        Public Shared Function checkType(td As TypeDef) As Boolean
            Return Not td.IsRuntimeSpecialName AndAlso Not td.IsSpecialName AndAlso Not td.IsNestedFamilyOrAssembly AndAlso Not td.IsNestedFamilyAndAssembly
        End Function
    End Class
End Namespace
