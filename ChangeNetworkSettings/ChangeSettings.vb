Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text

Module ChangeSettings

    Sub Main()

        Dim current As New Process()
        current.StartInfo.FileName = "netsh"
        current.StartInfo.Arguments = "interface ip show address ""Ethernet"""
        current.StartInfo.UseShellExecute = False
        current.StartInfo.RedirectStandardOutput = True
        current.Start()

        Dim oldSettings As StreamReader = current.StandardOutput() '.ReadToEnd.ToString.Split(vbCrLf)
        current.WaitForExit()
        Dim output As String() = current.StandardOutput.ReadToEnd.Split(vbCrLf)
        Dim IPAddress As New StringBuilder
        Dim SubnetMask As New StringBuilder
        Dim Gateway As New StringBuilder

        For Each line In output
            If line.Contains("IP Address") Then
                Dim index As Integer = InStr(line, ".") - 4
                For i As Integer = index To line.Count() - 1
                    IPAddress.Append(line(i))
                Next
                Console.WriteLine(IPAddress)
            ElseIf line.Contains("mask")
                Dim index As Integer = InStr(line, "mask") + 4
                For i As Integer = index To line.Count() - 1
                    If line(i) <> ")" Then
                        SubnetMask.Append(line(i))
                    End If
                Next
                Console.WriteLine(SubnetMask)
            ElseIf line.Contains("Default Gateway")
                Dim index As Integer = InStr(line, ".") - 4
                For i As Integer = index To line.Count() - 1
                    Gateway.Append(line(i))
                Next
                Console.WriteLine(Gateway)
            End If
        Next

        Console.WriteLine("C'est finis.")
        Console.ReadKey()
        Exit Sub

        Dim changes As New AddressChange()
        Console.ReadKey()
        Try
            Dim oldStuff As Array = changes.Old()
            Console.WriteLine($"Hostname: {changes.HostName}{vbCrLf}" &
                              $"IP Address: {oldStuff(0)}{vbCrLf}" &
                              $"Subnet: {oldStuff(1)}")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Console.WriteLine(changes.Name())

        Console.WriteLine("Enter the new IP address: ")
        Try
            'Dim ipAddress As IPAddress = Dns.GetHostEntry(Console.ReadLine())
            'changes.IpAddress = ipAddress
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Console.WriteLine(changes.IpAddress)

        Console.WriteLine($"Default subnet address is {changes.Subnet}.  Do you wish to keep the default?")
        'Dim subNet As IPHostEntry = Console.ReadLine()

        Console.WriteLine("Enter the gateway address: ")
        'Dim gateWay As IPHostEntry = Console.ReadLine()

    End Sub

End Module

Friend Class AddressChange
    Inherits NetworkInterface

    Private _HostName As String
    Private _OldIpAddress As IPAddress
    Private _OldSubnet As IPAddress
    Private _OldGateway As IPAddress
    Private _IpAddress As IPAddress
    Private _Subnet As IPAddress
    Private _Gateway As IPAddress

    Public Sub New()

        Try
            _HostName = Dns.GetHostName()
            Dim addressList As IPHostEntry = Dns.GetHostByName(_HostName)
            Console.WriteLine(addressList)
            Console.ReadKey()
            _OldIpAddress = addressList.AddressList(0)
            _OldSubnet = addressList.AddressList(1)
            '_OldGateway = GatewayAddresses().First()
            '_OldGateway = Dns.GetHostEntry("")

            addressList = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Property HostName() As String
        Get
            Return _HostName
        End Get
        Set(value As String)
            _HostName = value
        End Set
    End Property

    Public Property IpAddress() As IPAddress
        Get
            Return _IpAddress
        End Get
        Set(ByVal value As IPAddress)
            _IpAddress = value
        End Set
    End Property

    Public Property Subnet() As IPAddress
        Get
            Return _Subnet
        End Get
        Set(value As IPAddress)
            _Subnet = value
        End Set
    End Property

    Public Property Gateway() As IPAddress
        Get
            Return _Gateway
        End Get
        Set(ByVal value As IPAddress)
            _Gateway = value
        End Set
    End Property

    Public Function Old() As Array
        Return {_OldIpAddress, _OldSubnet, _OldGateway}
    End Function
End Class