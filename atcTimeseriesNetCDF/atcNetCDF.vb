Imports atcData
Imports atcUtility
Imports atcTimeseriesNetCDF.NetCDF
Imports MapWinUtility
Imports System.Text

Public Class atcNetCDF

    Private Shared pAttributeDefinitions As New atcCollection

    ''' <summary>
    ''' Wrapper that can be used around any NetCDF API function that returns non-zero on error.
    ''' Throws a descriptive exception if the result was not zero.
    ''' </summary>
    ''' <param name="aResult">Return value of a NetCDF API function</param>
    Public Shared Sub nc(ByVal aResult As Integer)
        If aResult <> 0 Then
            Throw New ApplicationException("NetCDF Error:" & NetCDF.nc_strerror(aResult))
        End If
    End Sub

    Private Shared Function AttributeTypeName(ByVal aAttributeType As NetCDF.nc_type) As String
        Return [Enum].GetName(aAttributeType.GetType, aAttributeType)
    End Function

    'Read an attribute, given an ncid for an open file, a varid (which can be 
    'NetCDF.NC_GLOBAL for file (i.e. global) attributes), and an attnum. This 
    'function will learn everything about the attribute, and log the results.

    ''' <summary>
    ''' Read a NetCDF attribute and add it to aAttributes
    ''' </summary>
    ''' <param name="ncid">NetCDF file ID</param>
    ''' <param name="varid">NetCDF variable ID</param>
    ''' <param name="attnum">NetCDF attribute number</param>
    ''' <param name="aAttributes">collection of atcDataAttributes to add this one to</param>
    ''' <remarks></remarks>
    Public Shared Sub ReadAttribute(ByVal ncid As Int32, _
                                    ByVal varid As Int32, _
                                    ByVal attnum As Int32, _
                                    ByVal aAttributes As atcDataAttributes)

        Dim lAttNameStringBuilder As New StringBuilder(NetCDF.netCDF_limits.NC_MAX_NAME)
        Dim lAttributeType As NetCDF.nc_type
        Dim lAttributeArrayLength As Int32
        'Dim lAttArrayIndex As Integer

        'First find the name, given the attnum.
        nc(NetCDF.nc_inq_attname(ncid, varid, attnum, lAttNameStringBuilder))

        Dim lAttributeName As String = lAttNameStringBuilder.ToString

        'Now find the type and length, using the name.
        nc(NetCDF.nc_inq_att(ncid, varid, lAttributeName, lAttributeType, lAttributeArrayLength))

        Dim lVarID_String As String
        If varid = NetCDF.NC_GLOBAL Then
            lVarID_String = "GLOBAL = -1"
        Else
            lVarID_String = "varid = " & Str(varid).PadLeft(3)
        End If

        Logger.Dbg(lVarID_String & " att# " & attnum & "  '" & lAttributeName & "' type: " & AttributeTypeName(lAttributeType) & " len: " & lAttributeArrayLength)

        Dim lAttributeDef As atcAttributeDefinition
        If pAttributeDefinitions.Keys.Contains(lAttributeName) Then
            lAttributeDef = pAttributeDefinitions.ItemByKey(lAttributeName)
        Else
            lAttributeDef = New atcAttributeDefinition
            With lAttributeDef
                .Name = lAttributeName
                .TypeString = lAttributeType.ToString
            End With
        End If

        'Now read the value, depending on the type.
        Select Case lAttributeType
            Case NetCDF.nc_type.NC_CHAR
                'There's possibly some size limits I should be checking for here?
                Dim lValue As New StringBuilder(lAttributeArrayLength)
                nc(NetCDF.nc_get_att_text(ncid, varid, lAttributeName, lValue))
                Logger.Dbg("text value: " & lValue.ToString)
                aAttributes.SetValue(lAttributeDef, lValue.ToString)

            Case NetCDF.nc_type.NC_BYTE
                Dim lValues(lAttributeArrayLength - 1) As Byte
                nc(NetCDF.nc_get_att_uchar(ncid, varid, lAttributeName, lValues))
                SetAttributeValue(lAttributeDef, lValues, aAttributes)

            Case NetCDF.nc_type.NC_INT
                Dim lValues(lAttributeArrayLength - 1) As Int32
                nc(NetCDF.nc_get_att_int(ncid, varid, lAttributeName, lValues))
                SetAttributeValue(lAttributeDef, lValues, aAttributes)
                'If lAttributeArrayLength = 1 Then
                '    aAttributes.SetValue(lAttributeDef, lValues(lAttArrayIndex))
                'Else
                '    aAttributes.SetValue(lAttributeDef, lValues)
                'End If
                'For lAttArrayIndex = 0 To lAttributeArrayLength - 1
                '    Logger.Dbg("index: " & lAttArrayIndex & "value: " & lValues(lAttArrayIndex))
                'Next

            Case NetCDF.nc_type.NC_DOUBLE
                Dim lValues(lAttributeArrayLength - 1) As Double
                nc(NetCDF.nc_get_att_double(ncid, varid, lAttributeName, lValues))
                SetAttributeValue(lAttributeDef, lValues, aAttributes)
                'If lAttributeArrayLength = 1 Then
                '    aAttributes.SetValue(lAttributeDef, lValues(lAttArrayIndex))
                'Else
                '    aAttributes.SetValue(lAttributeDef, lValues)
                'End If
                'For lAttArrayIndex = 0 To lAttributeArrayLength - 1
                '    Logger.Dbg("index: " & lAttArrayIndex & "value: " & DoubleToString(lValues(lAttArrayIndex)))
                'Next

            Case NetCDF.nc_type.NC_FLOAT
                Dim lValues(lAttributeArrayLength - 1) As Single
                nc(NetCDF.nc_get_att_float(ncid, varid, lAttributeName, lValues))
                SetAttributeValue(lAttributeDef, lValues, aAttributes)
                'If lAttributeArrayLength = 1 Then
                '    aAttributes.SetValue(lAttributeDef, lValues(lAttArrayIndex))
                'Else
                '    aAttributes.SetValue(lAttributeDef, lValues)
                'End If
                'For lAttArrayIndex = 0 To lAttributeArrayLength - 1
                '    Logger.Dbg("index: " & lAttArrayIndex & "value: " & DoubleToString(CDbl(lValues(lAttArrayIndex))))
                'Next

            Case NetCDF.nc_type.NC_SHORT
                Dim lValues(lAttributeArrayLength - 1) As Short
                nc(NetCDF.nc_get_att_short(ncid, varid, lAttributeName, lValues))
                SetAttributeValue(lAttributeDef, lValues, aAttributes)
                'If lAttributeArrayLength = 1 Then
                '    aAttributes.SetValue(lAttributeDef, lValues(lAttArrayIndex))
                'Else
                '    aAttributes.SetValue(lAttributeDef, lValues)
                'End If
                'For lAttArrayIndex = 0 To lAttributeArrayLength - 1
                '    Logger.Dbg("index: " & lAttArrayIndex & "value: " & lValues(lAttArrayIndex))
                'Next
            Case Else
                Throw New ApplicationException("Type not implemented: " & lAttributeType.ToString)
        End Select
    End Sub

    Private Shared Sub SetAttributeValue(ByVal aAttributeDef As atcAttributeDefinition, ByVal aValues As Array, ByVal aAttributes As atcDataAttributes)
        If aValues.Length = 1 Then
            aAttributes.SetValue(aAttributeDef, aValues(0))
            Logger.Dbg("value: " & Str(aValues(0)))
        Else
            aAttributes.SetValue(aAttributeDef, aValues)
            For lAttArrayIndex = 0 To aValues.Length - 1
                Logger.Dbg("value(" & lAttArrayIndex & ") = " & Str(aValues(lAttArrayIndex)))
            Next
        End If
    End Sub

End Class
