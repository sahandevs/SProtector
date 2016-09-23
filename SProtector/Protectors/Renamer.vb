Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit
Imports SProtector.SProtector.Tools.StringProtection
Namespace SProtector.Protectors

    'A helper class for creating random string that are not generated before
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
        Private Shared StringProtection As New generatedStrings ' Helper class for random string
        Private Shared TypeDefNames As New Dictionary(Of String, String)() ' list all of type defs names
        Private Shared namespaces As New Collection(Of TypeDef)() ' list of all namespaces
        Private Shared NamespaceNames As New List(Of String)() ' list of all namespaces names
        Private Shared NamespaceNamesEncrypt As New List(Of String)() ' list of all namespaces encrypted names

        'A Sub for doing protecting process
        Public Shared Sub Protect()
            For mDef = 0 To GlobAssembly.Asm.Modules.Count - 1 ' for every module in main assembly
                Dim moduleDef = GlobAssembly.Asm.Modules(mDef) ' get current
                For j = 0 To moduleDef.Types.Count - 1 ' for every type in current module
                    Dim td = moduleDef.Types(j) ' gets current type
                    renamer(td, GlobAssembly.Asm) ' do renamer method
                Next
                moduleDef.EntryPoint.Name = StringProtection.random(Int(Rnd() * 100)) ' generate a random name for entry point
            Next
        End Sub

        'renaming process
        Public Shared Sub renamer(td As TypeDef, asm As AssemblyDef)
            If td.HasNestedTypes Then 'check if type has nested types
                For i = 0 To td.NestedTypes.Count - 1 'get all nestedtypes
                    renamer(td.NestedTypes(i), asm) ' do renamer to the nested types too :D
                Next
            End If

            If Not NamespaceNames.Contains(td.[Namespace]) Then ' check if current type.namespace if it is crypted before or not
                NamespaceNames.Add(td.[Namespace]) 'if not add orginal name to list
                NamespaceNamesEncrypt.Add(StringProtection.random(Int(Rnd() * 100))) 'generate a random name for orginal name 
            End If

            Dim [Namespace] = NamespaceNamesEncrypt(NamespaceNames.IndexOf(td.[Namespace])) 'get current random name for current namespace !
            If checkType(td) Then ' run checktype functiong
                Dim text = StringProtection.random(Int(Rnd() * 100)) ' generate a random string for resoruce name and typename

                ' i dunnu why but i've rechecked these :|
                If td.BaseType Is Nothing Then
                    Return
                End If
                If td.IsNestedAssembly Then
                    Return
                End If

                For Each res In asm.ManifestModule.Resources ' get all resources in main assembly
                    If res.Name.Contains(td.[Namespace]) AndAlso td.[Namespace] <> "" AndAlso res.Name.Contains(td.Name) AndAlso td.Name <> "" Then ' check if we sould edit this resoruce or not
                        Dim resName As String = res.Name.Replace(".resources", "") ' rempve .resources from resource name
                        res.Name = res.Name.Replace(td.[Namespace], [Namespace]) 'change reource name's orginal namespace to generated one
                        res.Name = res.Name.Replace(td.Name, text) 'change resource name to generated one (text)

                        For Each mDef In td.Methods ' get all methods
                            If mDef.HasBody Then ' if method has a body
                                For Each instr As Instruction In mDef.Body.Instructions ' get all Instructions
                                    If instr.OpCode Is OpCodes.Ldstr Then 'check opcode is ldstr (setting an string)
                                        If resName IsNot Nothing AndAlso instr.Operand.ToString().Contains(resName) Then ' check if we sould change operand or not
                                            instr.Operand = instr.Operand.ToString().Replace(td.[Namespace], [Namespace]).Replace(td.Name, text) ' change orginal names to generated ones that we've generate ( if we dont app will not run ^_^ )
                                        End If
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next

                td.Name = text 'edit type name to generated one ( same as resource name )
                td.[Namespace] = [Namespace] 'change type's name space to generated one from list line 59

                For i = 0 To td.Methods.Count - 1 'get all methods again
                    Dim md = td.Methods(i) ' get current method
                    If checkMethod(md) Then ' check method
                        md.Name = StringProtection.random(Int(Rnd() * 100)) ' change method name to a random string
                        For Each current In md.ParamDefs ' get all methods'parameters
                            current.Name = StringProtection.random(Int(Rnd() * 100)) ' change methods'parameters to a random one
                        Next
                        If md.HasBody AndAlso md.Body.HasVariables Then ' check if method has body and var
                            For Each current2 In md.Body.Variables ' get all vars
                                current2.Name = StringProtection.random(Int(Rnd() * 100)) ' generate a random name for current local var
                            Next
                        End If
                    End If
                Next
                For i = 0 To td.Fields.Count - 1 ' get all type's fields
                    Dim fd = td.Fields(i)
                    If checkField(fd) Then
                        fd.Name = StringProtection.random(Int(Rnd() * 100)) ' generate a random name for the field
                    End If
                Next
                For i = 0 To td.Events.Count - 1 ' get all type's events
                    Dim ed = td.Events(i)
                    If checkEvent(ed) Then
                        ed.Name = StringProtection.random(Int(Rnd() * 100)) ' generate a random name for it
                    End If
                Next
                For i = 0 To td.Properties.Count - 1 ' change all type's properties
                    Dim pd = td.Properties(i)
                    If checkProperty(pd) Then
                        pd.Name = StringProtection.random(Int(Rnd() * 100)) ' generate a random name for it
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
            Return Not td.IsRuntimeSpecialName AndAlso Not td.IsSpecialName AndAlso Not td.IsNestedFamilyOrAssembly AndAlso Not td.IsNestedFamilyAndAssembly 'renamer shouldnt run if these things are true
        End Function
    End Class
End Namespace
