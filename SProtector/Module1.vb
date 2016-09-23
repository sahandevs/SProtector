Imports dnlib.DotNet
'SProtector by sahandevs
'Github : https://github.com/sahand100/


Module Module1

    'Log with a blue bar back of text for Info messages
    Sub LogInfo(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.Blue
        Console.Write("   ")
        Console.BackgroundColor = k
        Console.WriteLine(" " + text)
    End Sub
    'Log with a red bar back of text for Error messages
    Sub LogError(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.Red
        Console.Write("   ")
        Console.BackgroundColor = k
        Console.WriteLine(" " + text)
    End Sub

    'Log with a green bar back of text for good messages
    Sub LogGood(ByVal text As String)
        Dim k = Console.BackgroundColor
        Console.BackgroundColor = ConsoleColor.DarkGreen
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
            input = sargs(0) 'read file path from first arg
#End If
        Catch ex As Exception
            Console.WriteLine("Please Drag & Drop A File") 'show this message if something goes wrong
            w()
            Return
        End Try
        Try

            SProtector.GlobAssembly.AsmInputPath = input 'set my global var " assembly path " to first arg
            If input.Contains(".exe") Then 'Check if it is a exe file
                SProtector.GlobAssembly.AsmOutputPath = input.Replace(".exe", "").Replace(".dll", "") + "-sprotected.exe" ' set my global var " assembly output path " to first arg + -protected.exe if file is exe file
            Else
                SProtector.GlobAssembly.AsmOutputPath = input.Replace(".exe", "").Replace(".dll", "") + "-sprotected.dll" ' set my global var " assembly output path " to first arg + -protected.dll if file is dll file
            End If
            SProtector.GlobAssembly.Asm2 = AssemblyDef.Load("Methods.exe") ' loads external exe files for injecting methods to my global var "asm2"
            SProtector.GlobAssembly.Asm = AssemblyDef.Load(SProtector.GlobAssembly.AsmInputPath) ' load main exe file for protecting to my global var "asm"

            ' Not by now you cant use both constants or crasher and antidebug together

            LogInfo("Protecting With [Junk Codes]...")
            SProtector.Protectors.Junkcodes.Protect() ' Protect with anti debug method
            LogGood("Protecting With [Junk Codes](Done)")
            SProtector.GlobAssembly.Asm2 = AssemblyDef.Load("Methods.exe") ' reloads external exe files for injecting methods to my global var "asm2"
            LogInfo("Protecting With [Rex Anti Debug]...")
            SProtector.Protectors.AntiDebug.Protect() ' Protect with anti debug method
            LogGood("Protecting With [Rex Anti Debug](Done)")

            SProtector.GlobAssembly.Asm2 = AssemblyDef.Load("Methods.exe") ' reloads external exe files for injecting methods to my global var "asm2"


            LogInfo("Protecting With [Constants]...")
            SProtector.Protectors.Constants.Protect() ' Protect with constants ( Crypting strings and integers ) method
            LogGood("Protecting With [Constants](Done)")



            LogInfo("Protecting With [AntiILDasm]...")
            SProtector.Protectors.AntiILDasm.Protect() ' Protect with AntiIlDasm with confuserex method
            LogGood("Protecting With [AntiILDasm](Done)")

            LogInfo("Protecting With [FakeModules]...")
            SProtector.Protectors.FakeModules.Protect() ' Protect file by adding a lot of fake modules
            LogGood("Protecting With [FakeModules](Done)")

            LogInfo("Protecting With [Renamer]...")
            SProtector.Protectors.Renamer.Protect() 'Protect method , fields and ... by changing their name to a random string
            LogGood("Protecting With [Renamer](Done)")


            If My.Computer.FileSystem.FileExists(SProtector.GlobAssembly.AsmOutputPath) Then My.Computer.FileSystem.DeleteFile(SProtector.GlobAssembly.AsmOutputPath)  ' delete output file if exists

            SProtector.GlobAssembly.Asm.Modules(0).Write(SProtector.GlobAssembly.AsmOutputPath) ' save file

            LogInfo("Saved In : " + SProtector.GlobAssembly.AsmOutputPath)
            w()
        Catch ex As Exception
#If DEBUG Then
             LogError(ex.ToString) ' log fully detail if it is on debug mode
#Else
            LogError(ex.Message) 'log error text
#End If
            w()
        End Try
    End Sub
    Sub w() ' a shortcut for console.readkey
        Console.ReadKey()
    End Sub
End Module
