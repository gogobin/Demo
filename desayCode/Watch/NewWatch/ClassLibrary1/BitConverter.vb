Public Class BitConverterEV2300
    Public Function ByteToDouble(ByVal vMSByte As Byte, ByVal vMidHiByte As Byte, ByVal vMidLoByte As Byte, ByVal vLSByte As Byte) As Double

        Dim bIsPositive As Boolean

        Dim fExponent As Double

        Dim fResult As Double
        Try
            ' // Get the sign, its in the 0x00 80 00 00 bit

            If (vMidHiByte And 128) = 0 Then bIsPositive = True

            ' // Get the exponent, it's 2^(MSbyte - 0x80)

            fExponent = 2 ^ (vMSByte - 128)

            ' // orelse in 0x80 to the MidHiByte

            vMidHiByte = vMidHiByte Or 128

            ' // get value out of midhi byte

            fResult = (vMidHiByte) * 2 ^ 16

            ' // add in midlow byte

            fResult = fResult + (vMidLoByte * 2 ^ 8)

            ' // add in LS byte

            fResult = fResult + vLSByte

            ' // multiply by 2^-24 to get the actual fraction

            fResult = fResult * 2 ^ -24

            ' // multiply fraction by the ??exponent?? part

            fResult = fResult * fExponent

            ' // Make negative if necessary

            If False = bIsPositive Then fResult = -fResult

        Catch ex As Exception
        End Try

        Return fResult

    End Function

    ''' <summary>
    ''' ?ϥΤ@?Ӥouble?缆?p?⥼?Ӣyte?缆  
    ''' </summary>
    ''' <param name="X">?ݭn ????double?ƭȦlt;/param>
    Public Function DoubleToByteArray(ByVal X As Double) As Byte()
        Dim iByte1 As Byte
        Dim iByte2 As Byte
        Dim iByte3 As Byte
        Dim iByte4 As Byte
        Try
            Dim iExp As Integer
            Dim bNegative As Boolean
            Dim fMantissa As Double


            '// Don't blow up with logs of zero
            If X = 0 Then X = 0.0000001


            If X < 0 Then
                bNegative = True
                X = -X
            End If


            '// find the correct exponent
            iExp = Int((Math.Log(X) / Math.Log(2)) + 1) '// remember - log of any base is ln(x)/ln(base)


            '// MS byte is the exponent + 0x80 
            iByte1 = iExp + 128


            '// Divide input by this exponent to get mantissa
            fMantissa = X / 2 ^ iExp


            '// Scale it up
            fMantissa = fMantissa / 2 ^ -24


            '// Split the mantissa into 3 bytes 
            iByte2 = fMantissa \ 2 ^ 16
            iByte3 = (fMantissa - (iByte2 * 2 ^ 16)) \ 2 ^ 8
            iByte4 = fMantissa - (iByte2 * 2 ^ 16) - (iByte3 * 2 ^ 8)


            '// subtract the sign bit if number is positive
            If False = bNegative Then iByte2 = iByte2 And &H7F
        Catch ex As Exception

            iByte1 = 0
            iByte2 = 0
            iByte3 = 0
            iByte4 = 0
        End Try
        Return New Byte() {iByte1, iByte2, iByte3, iByte4}
    End Function



End Class
