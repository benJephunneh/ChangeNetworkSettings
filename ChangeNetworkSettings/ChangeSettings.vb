Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text

Module ChangeSettings

    Public ipAddress As StringBuilder
    Public SubnetMask As StringBuilder
    Public Gateway As StringBuilder
    Public output As String()

    Sub Main()

        'Create a netsh process to get current network settings for the Local Area Connection.
        Dim current As New Process()
        current.StartInfo.FileName = "netsh"
        'current.StartInfo.Arguments = "interface ip show address ""Ethernet"""
        current.StartInfo.Arguments = "interface ip show address ""Local Area Connection"""
        current.StartInfo.UseShellExecute = False
        current.StartInfo.RedirectStandardOutput = True
        current.Start()

        'Wait for the process to finish, then put output stream into a string.
        current.WaitForExit()
        output = current.StandardOutput.ReadToEnd.Split(vbCrLf)

        Dim oldSettings As New AddressChange
        GetCurrentSettings()

        Console.WriteLine("C'est finis.")
        Console.ReadKey()
        Exit Sub

        'Dim changes As New AddressChange()
        'Console.ReadKey()
        'Try
        '    Dim oldStuff As Array = changes.Old()
        '    Console.WriteLine($"Hostname: {changes.HostName}{vbCrLf}" &
        '                      $"IP Address: {oldStuff(0)}{vbCrLf}" &
        '                      $"Subnet: {oldStuff(1)}")
        'Catch ex As Exception
        '    Console.WriteLine(ex.Message)
        'End Try
        'Console.WriteLine(changes.Name())

        'Console.WriteLine("Enter the new IP address: ")
        'Try
        '    'Dim ipAddress As IPAddress = Dns.GetHostEntry(Console.ReadLine())
        '    'changes.IpAddress = ipAddress
        'Catch ex As Exception
        '    Console.WriteLine(ex.Message)
        'End Try
        'Console.WriteLine(changes.IpAddress)

        'Console.WriteLine($"Default subnet address is {changes.Subnet}.  Do you wish to keep the default?")
        ''Dim subNet As IPHostEntry = Console.ReadLine()

        'Console.WriteLine("Enter the gateway address: ")
        ''Dim gateWay As IPHostEntry = Console.ReadLine()

    End Sub


    Private Sub GetCurrentSettings()
        'For Each line In output
        '    'ExtractAddress(output, "IP Address", line, IPAddress)
        '    If line.Contains("IP Address") Then
        '        IPAddress = New StringBuilder
        '        ExtractAddress(line, IPAddress)

        '    ElseIf line.Contains("mask") Then
        '        SubnetMask = New StringBuilder
        '        Dim index As Integer = InStr(line, "mask") + 4
        '        For i As Integer = index To line.Count() - 1
        '            SubnetMask.Append(If(line(i) <> ")", line(i), Nothing))
        '            'If line(i) = ")" Then
        '            '    Exit For
        '            'End If
        '        Next
        '        Console.WriteLine(SubnetMask)

        '    ElseIf line.Contains("Default Gateway") Then
        '        Gateway = New StringBuilder
        '        ExtractAddress(line, Gateway)
        '    End If


        'If line.Contains("IP Address") Then
        '    Dim index As Integer = InStr(line, ".") - 4
        '    Dim number As Integer
        '    index = If(Integer.TryParse(line(index), number), index, index + 1)
        '    For i As Integer = index To line.Count() - 1
        '        IPAddress.Append(line(i))
        '    Next
        '    Console.WriteLine(IPAddress)
        'ElseIf line.Contains("mask") Then
        '    Dim index As Integer = InStr(line, "mask") + 4
        '    For i As Integer = index To line.Count() - 1
        '        If line(i) = ")" Then
        '            Exit For
        '        End If
        '        SubnetMask.Append(line(i))
        '    Next
        '    Console.WriteLine(SubnetMask)
        'ElseIf line.Contains("Default Gateway") Then
        '    Dim index As Integer = InStr(line, ".") - 4
        '    Dim number As Integer
        '    index = If(Integer.TryParse(line(index), number), index, index + 1)
        '    'If Integer.TryParse(line(index), number) = False Then
        '    '        index += 1
        '    '    End If
        '    For i As Integer = index To line.Count() - 1
        '        Gateway.Append(line(i))
        '    Next
        '    Console.WriteLine(Gateway)
        '    Exit For
        'End If
        'Next
    End Sub
End Module

Friend Class AddressChange
    Inherits NetworkInterface 'We'll see if this inheritance is necessary.

    Private _InterfaceIP As StringBuilder
    Private _SubnetMask As StringBuilder
    Private _Gateway As StringBuilder

    Private _HostName As String
    'Private _OldIpAddress As IPAddress
    'Private _OldSubnet As IPAddress
    'Private _OldGateway As IPAddress
    'Private _IpAddress As IPAddress
    'Private _Subnet As IPAddress
    'Private _Gateway As IPAddress

    Public Sub New()

        For Each line In output
            'ExtractAddress(output, "IP Address", line, IPAddress)
            If line.Contains("IP Address") Then
                _InterfaceIP = New StringBuilder
                ExtractAddress(line, _InterfaceIP)

            ElseIf line.Contains("mask") Then
                _SubnetMask = New StringBuilder
                Dim index As Integer = InStr(line, "mask") + 4
                For i As Integer = index To line.Count() - 1
                    _SubnetMask.Append(If(line(i) <> ")", line(i), Nothing))
                    'If line(i) = ")" Then
                    '    Exit For
                    'End If
                Next
                Console.WriteLine(_SubnetMask)

            ElseIf line.Contains("Default Gateway") Then
                _Gateway = New StringBuilder
                ExtractAddress(line, _Gateway)
                Exit For
            End If
        Next

        'Try
        '        _HostName = Dns.GetHostName()
        '        Dim addressList As IPHostEntry = Dns.GetHostByName(_HostName)
        '        Console.WriteLine(addressList)
        '        Console.ReadKey()
        '        _OldIpAddress = addressList.AddressList(0)
        '        _OldSubnet = addressList.AddressList(1)
        '        '_OldGateway = GatewayAddresses().First()
        '        '_OldGateway = Dns.GetHostEntry("")

        '        addressList = Nothing
        '    Catch ex As Exception
        '        Console.WriteLine(ex.Message)
        'End Try
    End Sub

    Friend Sub ExtractAddress(line As String, address As StringBuilder)
        Dim index As Integer = InStr(line, ".") - 4
        Dim number As Integer
        index = If(Integer.TryParse(line(index), number), index, index + 1)
        For i As Integer = index To line.Count() - 1
            address.Append(line(i))
        Next
        Console.WriteLine(address)
    End Sub

    Public Property HostName() As String
        Get
            Return _HostName
        End Get
        Set(value As String)
            _HostName = value
        End Set
    End Property

    'Public Property IpAddress() As IPAddress
    '    Get
    '        Return _IpAddress
    '    End Get
    '    Set(ByVal value As IPAddress)
    '        _IpAddress = value
    '    End Set
    'End Property

    'Public Property Subnet() As IPAddress
    '    Get
    '        Return _Subnet
    '    End Get
    '    Set(value As IPAddress)
    '        _Subnet = value
    '    End Set
    'End Property

    'Public Property Gateway() As IPAddress
    '    Get
    '        Return _Gateway
    '    End Get
    '    Set(ByVal value As IPAddress)
    '        _Gateway = value
    '    End Set
    'End Property

    'Public Function Old() As Array
    '    Return {_OldIpAddress, _OldSubnet, _OldGateway}
    'End Function
End Class