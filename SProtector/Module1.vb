Imports dnlib.DotNet



Module Module1
    Sub LogInfo(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.Blue
        Console.Write("   ")
        Console.BackgroundColor = k
        Console.WriteLine(" " + text)
    End Sub
    Sub LogError(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.Red
        Console.Write("   ")
        Console.BackgroundColor = k
        Console.WriteLine(" " + text)
    End Sub
    Sub LogGood(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.Green
        Console.Write("   ")
        Console.BackgroundColor = k
        Console.WriteLine(" " + text)
    End Sub
    Sub Main(sargs() As String)
        Dim input
        Try
#If DEBUG Then
            input = "1.exe"
#Else
 input = sargs(0).ToString()
#End If
        Catch ex As Exception
            Console.WriteLine("Please Drag & Drop A File")
            w()
            Return
        End Try
        Try

            SProtector.GlobAssembly.AsmInputPath = input
            If input.Contains(".exe") Then
                SProtector.GlobAssembly.AsmOutputPath = input.Replace(".exe", "").Replace(".dll", "") + "-sprotected.exe"
            Else
                SProtector.GlobAssembly.AsmOutputPath = input.Replace(".exe", "").Replace(".dll", "") + "-sprotected.dll"
            End If
            SProtector.GlobAssembly.Asm2 = AssemblyDef.Load("Methods.exe")
            SProtector.GlobAssembly.Asm = AssemblyDef.Load(SProtector.GlobAssembly.AsmInputPath)

            'LogInfo("Protecting With [Rex Anti Debug]...")
            'SProtector.Protectors.AntiDebug.Protect()
            'LogGood("Protecting With [Rex Anti Debug](Done)")

            LogInfo("Protecting With [Constants]...")
            SProtector.Protectors.Constants.Protect()
            LogGood("Protecting With [Constants](Done)")

            LogInfo("Protecting With [Crasher]...")
            SProtector.Protectors.Constants.Protect()
            LogGood("Protecting With [Crasher](Done)")



            LogInfo("Protecting With [AntiILDasm]...")
            SProtector.Protectors.AntiILDasm.Protect()
            LogGood("Protecting With [AntiILDasm](Done)")

            LogInfo("Protecting With [FakeModules]...")
            SProtector.Protectors.FakeModules.Protect()
            LogGood("Protecting With [FakeModules](Done)")





            LogInfo("Protecting With [Renamer]...")
            SProtector.Protectors.Renamer.Protect()
            LogGood("Protecting With [Renamer](Done)")


            LogInfo("Saved In : " + SProtector.GlobAssembly.AsmOutputPath)
            '   Process.Start(SProtector.GlobAssembly.AsmOutputPath)


            Try
                My.Computer.FileSystem.DeleteFile(SProtector.GlobAssembly.AsmOutputPath)
            Catch ex As Exception

            End Try

            SProtector.GlobAssembly.Asm.Modules(0).Write(SProtector.GlobAssembly.AsmOutputPath)

            w()
        Catch ex As Exception
            LogError(ex.ToString)
            '      Console.WriteLine("Error : " + ex.ToString)
            w()
        End Try
    End Sub
    Sub w()
        Console.ReadKey()
    End Sub
End Module
