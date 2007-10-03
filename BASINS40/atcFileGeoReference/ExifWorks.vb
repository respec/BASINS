''' <summary>
''' Utility class for working with EXIF data in images. Provides abstraction
''' for most common data and generic utilities for work with all other. 
''' </summary>
''' <remarks>
''' Copyright (c) Michal A. Valášek - Altair Communications, 2003-2005
''' Copmany: http://software.altaircom.net, E-mail: support@altaircom.net
''' Private: http://www.rider.cz, E-mail: rider@rider.cz
''' This is free software licensed under GNU Lesser General Public License
''' </remarks>
''' <history>
''' [altair] 10.09.2003 Created
''' [altair] 12.06.2004 Added capability to write EXIF data
''' [altair] 11.07.2004 Added option to change encoding
''' [altair] 04.09.2005 Changed source of Width and Height properties from EXIF to image
''' [altair] 05.09.2005 Code clean-up and minor changes
''' </history>
Public Class ExifWorks
    Implements IDisposable

    Private _Image As System.Drawing.Bitmap
    Private _Encoding As System.Text.Encoding = System.Text.Encoding.UTF8

#Region " Type declarations "

    ''' <summary>
    ''' Contains possible values of EXIF tag names (ID)
    ''' </summary>
    ''' <remarks>See GdiPlusImaging.h</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum TagNames As Integer
        ExifIFD = &H8769
        GpsIFD = &H8825
        NewSubfileType = &HFE
        SubfileType = &HFF
        ImageWidth = &H100
        ImageHeight = &H101
        BitsPerSample = &H102
        Compression = &H103
        PhotometricInterp = &H106
        ThreshHolding = &H107
        CellWidth = &H108
        CellHeight = &H109
        FillOrder = &H10A
        DocumentName = &H10D
        ImageDescription = &H10E
        EquipMake = &H10F
        EquipModel = &H110
        StripOffsets = &H111
        Orientation = &H112
        SamplesPerPixel = &H115
        RowsPerStrip = &H116
        StripBytesCount = &H117
        MinSampleValue = &H118
        MaxSampleValue = &H119
        XResolution = &H11A
        YResolution = &H11B
        PlanarConfig = &H11C
        PageName = &H11D
        XPosition = &H11E
        YPosition = &H11F
        FreeOffset = &H120
        FreeByteCounts = &H121
        GrayResponseUnit = &H122
        GrayResponseCurve = &H123
        T4Option = &H124
        T6Option = &H125
        ResolutionUnit = &H128
        PageNumber = &H129
        TransferFuncition = &H12D
        SoftwareUsed = &H131
        DateTime = &H132
        Artist = &H13B
        HostComputer = &H13C
        Predictor = &H13D
        WhitePoint = &H13E
        PrimaryChromaticities = &H13F
        ColorMap = &H140
        HalftoneHints = &H141
        TileWidth = &H142
        TileLength = &H143
        TileOffset = &H144
        TileByteCounts = &H145
        InkSet = &H14C
        InkNames = &H14D
        NumberOfInks = &H14E
        DotRange = &H150
        TargetPrinter = &H151
        ExtraSamples = &H152
        SampleFormat = &H153
        SMinSampleValue = &H154
        SMaxSampleValue = &H155
        TransferRange = &H156
        JPEGProc = &H200
        JPEGInterFormat = &H201
        JPEGInterLength = &H202
        JPEGRestartInterval = &H203
        JPEGLosslessPredictors = &H205
        JPEGPointTransforms = &H206
        JPEGQTables = &H207
        JPEGDCTables = &H208
        JPEGACTables = &H209
        YCbCrCoefficients = &H211
        YCbCrSubsampling = &H212
        YCbCrPositioning = &H213
        REFBlackWhite = &H214
        ICCProfile = &H8773
        Gamma = &H301
        ICCProfileDescriptor = &H302
        SRGBRenderingIntent = &H303
        ImageTitle = &H320
        Copyright = &H8298
        ResolutionXUnit = &H5001
        ResolutionYUnit = &H5002
        ResolutionXLengthUnit = &H5003
        ResolutionYLengthUnit = &H5004
        PrintFlags = &H5005
        PrintFlagsVersion = &H5006
        PrintFlagsCrop = &H5007
        PrintFlagsBleedWidth = &H5008
        PrintFlagsBleedWidthScale = &H5009
        HalftoneLPI = &H500A
        HalftoneLPIUnit = &H500B
        HalftoneDegree = &H500C
        HalftoneShape = &H500D
        HalftoneMisc = &H500E
        HalftoneScreen = &H500F
        JPEGQuality = &H5010
        GridSize = &H5011
        ThumbnailFormat = &H5012
        ThumbnailWidth = &H5013
        ThumbnailHeight = &H5014
        ThumbnailColorDepth = &H5015
        ThumbnailPlanes = &H5016
        ThumbnailRawBytes = &H5017
        ThumbnailSize = &H5018
        ThumbnailCompressedSize = &H5019
        ColorTransferFunction = &H501A
        ThumbnailData = &H501B
        ThumbnailImageWidth = &H5020
        ThumbnailImageHeight = &H502
        ThumbnailBitsPerSample = &H5022
        ThumbnailCompression = &H5023
        ThumbnailPhotometricInterp = &H5024
        ThumbnailImageDescription = &H5025
        ThumbnailEquipMake = &H5026
        ThumbnailEquipModel = &H5027
        ThumbnailStripOffsets = &H5028
        ThumbnailOrientation = &H5029
        ThumbnailSamplesPerPixel = &H502A
        ThumbnailRowsPerStrip = &H502B
        ThumbnailStripBytesCount = &H502C
        ThumbnailResolutionX = &H502D
        ThumbnailResolutionY = &H502E
        ThumbnailPlanarConfig = &H502F
        ThumbnailResolutionUnit = &H5030
        ThumbnailTransferFunction = &H5031
        ThumbnailSoftwareUsed = &H5032
        ThumbnailDateTime = &H5033
        ThumbnailArtist = &H5034
        ThumbnailWhitePoint = &H5035
        ThumbnailPrimaryChromaticities = &H5036
        ThumbnailYCbCrCoefficients = &H5037
        ThumbnailYCbCrSubsampling = &H5038
        ThumbnailYCbCrPositioning = &H5039
        ThumbnailRefBlackWhite = &H503A
        ThumbnailCopyRight = &H503B
        LuminanceTable = &H5090
        ChrominanceTable = &H5091
        FrameDelay = &H5100
        LoopCount = &H5101
        PixelUnit = &H5110
        PixelPerUnitX = &H5111
        PixelPerUnitY = &H5112
        PaletteHistogram = &H5113
        ExifExposureTime = &H829A
        ExifFNumber = &H829D
        ExifExposureProg = &H8822
        ExifSpectralSense = &H8824
        ExifISOSpeed = &H8827
        ExifOECF = &H8828
        ExifVer = &H9000
        ExifDTOrig = &H9003
        ExifDTDigitized = &H9004
        ExifCompConfig = &H9101
        ExifCompBPP = &H9102
        ExifShutterSpeed = &H9201
        ExifAperture = &H9202
        ExifBrightness = &H9203
        ExifExposureBias = &H9204
        ExifMaxAperture = &H9205
        ExifSubjectDist = &H9206
        ExifMeteringMode = &H9207
        ExifLightSource = &H9208
        ExifFlash = &H9209
        ExifFocalLength = &H920A
        ExifMakerNote = &H927C
        ExifUserComment = &H9286
        ExifDTSubsec = &H9290
        ExifDTOrigSS = &H9291
        ExifDTDigSS = &H9292
        ExifFPXVer = &HA000
        ExifColorSpace = &HA001
        ExifPixXDim = &HA002
        ExifPixYDim = &HA003
        ExifRelatedWav = &HA004
        ExifInterop = &HA005
        ExifFlashEnergy = &HA20B
        ExifSpatialFR = &HA20C
        ExifFocalXRes = &HA20E
        ExifFocalYRes = &HA20F
        ExifFocalResUnit = &HA210
        ExifSubjectLoc = &HA214
        ExifExposureIndex = &HA215
        ExifSensingMethod = &HA217
        ExifFileSource = &HA300
        ExifSceneType = &HA301
        ExifCfaPattern = &HA302
        GpsVer = &H0
        GpsLatitudeRef = &H1
        GpsLatitude = &H2
        GpsLongitudeRef = &H3
        GpsLongitude = &H4
        GpsAltitudeRef = &H5
        GpsAltitude = &H6
        GpsGpsTime = &H7
        GpsGpsSatellites = &H8
        GpsGpsStatus = &H9
        GpsGpsMeasureMode = &HA
        GpsGpsDop = &HB
        GpsSpeedRef = &HC
        GpsSpeed = &HD
        GpsTrackRef = &HE
        GpsTrack = &HF
        GpsImgDirRef = &H10
        GpsImgDir = &H11
        GpsMapDatum = &H12
        GpsDestLatRef = &H13
        GpsDestLat = &H14
        GpsDestLongRef = &H15
        GpsDestLong = &H16
        GpsDestBearRef = &H17
        GpsDestBear = &H18
        GpsDestDistRef = &H19
        GpsDestDist = &H1A
    End Enum


    ''' <summary>
    ''' Real position of 0th row and column of picture
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum Orientations
        TopLeft = 1
        TopRight = 2
        BottomRight = 3
        BottomLeft = 4
        LeftTop = 5
        RightTop = 6
        RightBottom = 7
        LftBottom = 8
    End Enum


    ''' <summary>
    ''' Exposure programs
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum ExposurePrograms
        Manual = 1
        Normal = 2
        AperturePriority = 3
        ShutterPriority = 4
        Creative = 5
        Action = 6
        Portrait = 7
        Landscape = 8
    End Enum


    ''' <summary>
    ''' Exposure metering modes
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum ExposureMeteringModes
        Unknown = 0
        Average = 1
        CenterWeightedAverage = 2
        Spot = 3
        MultiSpot = 4
        MultiSegment = 5
        [Partial] = 6
        Other = 255
    End Enum


    ''' <summary>
    ''' Flash activity modes
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum FlashModes
        NotFired = 0
        Fired = 1
        FiredButNoStrobeReturned = 5
        FiredAndStrobeReturned = 7
    End Enum


    ''' <summary>
    ''' Possible light sources (white balance)
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Enum LightSources
        Unknown = 0
        Daylight = 1
        Fluorescent = 2
        Tungsten = 3
        Flash = 10
        StandardLightA = 17
        StandardLightB = 18
        StandardLightC = 19
        D55 = 20
        D65 = 21
        D75 = 22
        Other = 255
    End Enum


    ''' <summary>
    ''' EXIF data types
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 12.6.2004 Created
    ''' </history>

    Public Enum ExifDataTypes As Short
        UnsignedByte = 1
        AsciiString = 2
        UnsignedShort = 3
        UnsignedLong = 4
        UnsignedRational = 5
        SignedByte = 6
        Undefined = 7
        SignedShort = 8
        SignedLong = 9
        SignedRational = 10
        SingleFloat = 11
        DoubleFloat = 12
    End Enum


    ''' <summary>
    ''' Represents rational which is type of some Exif properties
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>

    Public Structure Rational
        Dim Numerator As Int32
        Dim Denominator As Int32


        ''' <summary>
        ''' Converts rational to string representation
        ''' </summary>
        ''' <param name="Delimiter">Optional, default "/". String to be used as delimiter of components.</param>
        ''' <returns>String representation of the rational.</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' [altair] 10.09.2003 Created
        ''' </history>

        Shadows Function ToString(Optional ByVal Delimiter As String = "/") As String
            Return Numerator & Delimiter & Denominator
        End Function


        ''' <summary>
        ''' Converts rational to double precision real number
        ''' </summary>
        ''' <returns>The rational as double precision real number.</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' [altair] 10.09.2003 Created
        ''' </history>

        Function ToDouble() As Double
            Return Numerator / Denominator
        End Function
    End Structure

#End Region

    ''' <summary>
    ''' Initializes new instance of this class.
    ''' </summary>
    ''' <param name="Bitmap">Bitmap to read exif information from</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Sub New(ByRef Bitmap As System.Drawing.Bitmap)
        If Bitmap Is Nothing Then Throw New ArgumentNullException("Bitmap")
        Me._Image = Bitmap
    End Sub

    ''' <summary>
    ''' Initializes new instance of this class.
    ''' </summary>
    ''' <param name="aFileName">Name of file to be loaded</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 13.06.2004 Created
    ''' </history>
    Public Sub New(ByVal aFileName As String)
        If aFileName.StartsWith("file://") Then aFileName = aFileName.Substring(7)
        Me._Image = DirectCast(System.Drawing.Bitmap.FromFile(aFileName), System.Drawing.Bitmap)
    End Sub

    ''' <summary>
    ''' Get or set encoding used for string metadata
    ''' </summary>
    ''' <value>Encoding used for string metadata</value>
    ''' <remarks>Default encoding is UTF-8</remarks>
    ''' <history>
    ''' [altair] 11.07.2004 Created
    ''' [altair] 05.09.2005 Changed from shared to instance member
    ''' </history>
    Public Property Encoding() As System.Text.Encoding
        Get
            Return Me._Encoding
        End Get
        Set(ByVal Value As System.Text.Encoding)
            If Value Is Nothing Then Throw New ArgumentNullException
            Me._Encoding = Encoding
        End Set
    End Property

    ''' <summary>
    ''' Returns copy of bitmap this instance is working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 13.06.2004 Created
    ''' </history>
    Public Function GetBitmap() As System.Drawing.Bitmap
        Return DirectCast(Me._Image.Clone(), System.Drawing.Bitmap)
    End Function

    ''' <summary>
    ''' Returns all available data in formatted string form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Overrides Function ToString() As String
        Dim SB As New System.Text.StringBuilder
        For Each lPID As Integer In Me._Image.PropertyIdList
            Dim lName As String = [Enum].GetName(GetType(TagNames), lPID)
            SB.Append(lName & " = " & Me.GetPropertyFormatted(lPID) & vbCrLf)
        Next


        SB.Append("Image:")
        SB.Append("\n\tDimensions:        " & Me.Width & " x " & Me.Height & " px")
        SB.Append("\n\tResolution:        " & Me.ResolutionX & " x " & Me.ResolutionY & " dpi")
        SB.Append("\n\tOrientation:       " & [Enum].GetName(GetType(Orientations), Me.Orientation))
        SB.Append(ToStringHelper("\n\tTitle:             ", Me.Title))
        SB.Append(ToStringHelper("\n\tDescription:       ", Me.Description))
        SB.Append(ToStringHelper("\n\tCopyright:         ", Me.Copyright))
        SB.Append("\nEquipment:")
        SB.Append(ToStringHelper("\n\tMaker:             ", Me.EquipmentMaker))
        SB.Append(ToStringHelper("\n\tModel:             ", Me.EquipmentModel))
        SB.Append(ToStringHelper("\n\tSoftware:          ", Me.Software))
        SB.Append("\nDate and time:")
        SB.Append(ToStringHelper("\n\tGeneral:           ", Me.DateTimeLastModified.ToString()))
        SB.Append(ToStringHelper("\n\tOriginal:          ", Me.DateTimeOriginal.ToString()))
        SB.Append(ToStringHelper("\n\tDigitized:         ", Me.DateTimeDigitized.ToString()))
        SB.Append("\nShooting conditions:")
        SB.Append(ToStringHelper("\n\tExposure time:     ", Me.ExposureTime.ToString("N4") & " s"))
        SB.Append(ToStringHelper("\n\tExposure program:  ", [Enum].GetName(GetType(ExposurePrograms), Me.ExposureProgram)))
        SB.Append(ToStringHelper("\n\tExposure mode:     ", [Enum].GetName(GetType(ExposureMeteringModes), Me.ExposureMeteringMode)))
        SB.Append(ToStringHelper("\n\tAperture:          F", Me.Aperture.ToString("N2")))
        SB.Append(ToStringHelper("\n\tISO sensitivity:   ", Me.ISO))
        SB.Append(ToStringHelper("\n\tSubject distance:  ", Me.SubjectDistance.ToString("N2") & " m"))
        SB.Append(ToStringHelper("\n\tFocal length:      ", Me.FocalLength))
        SB.Append(ToStringHelper("\n\tFlash:             ", [Enum].GetName(GetType(FlashModes), Me.FlashMode)))
        SB.Append(ToStringHelper("\n\tLight source (WB): ", [Enum].GetName(GetType(LightSources), Me.LightSource)))
        'If Me.IsPropertyDefined(TagNames.GpsLatitude) Then SB.Append("\n\tGPS Latitude       ", Me.GetCoordinateGPS(TagNames.GpsLatitude))
        'If Me.IsPropertyDefined(TagNames.GpsLongitude) Then SB.Append("\n\tGPS Longitude      ", Me.GetCoordinateGPS(TagNames.GpsLongitude))

        SB.Replace("\n", vbCrLf)
        SB.Replace("\t", vbTab)
        Return SB.ToString()
    End Function

    Private Function ToStringHelper(ByVal aLabel As String, ByVal aValue As String) As String
        If aValue.Length > 0 Then
            Return aLabel & aValue
        Else
            Return ""
        End If
    End Function

#Region " Nicely formatted well-known properties "

    ''' <summary>
    ''' Brand of equipment (EXIF EquipMake)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property EquipmentMaker() As String
        Get
            Return Me.GetPropertyString(TagNames.EquipMake)
        End Get
    End Property

    ''' <summary>
    ''' Model of equipment (EXIF EquipModel)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property EquipmentModel() As String
        Get
            Return Me.GetPropertyString(TagNames.EquipModel)
        End Get
    End Property

    ''' <summary>
    ''' Software used for processing (EXIF Software)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property Software() As String
        Get
            Return Me.GetPropertyString(TagNames.SoftwareUsed)
        End Get
    End Property

    ''' <summary>
    ''' Orientation of image (position of row 0, column 0) (EXIF Orientation)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property Orientation() As Orientations
        Get
            Dim X As Int32 = Me.GetPropertyInt16(TagNames.Orientation)

            If Not [Enum].IsDefined(GetType(Orientations), X) Then
                Return Orientations.TopLeft
            Else
                Return CType([Enum].Parse(GetType(Orientations), [Enum].GetName(GetType(Orientations), X)), Orientations)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Time when image was last modified (EXIF DateTime).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property DateTimeLastModified() As DateTime
        Get
            Try
                Return DateTime.ParseExact(Me.GetPropertyString(TagNames.DateTime), "yyyy\:MM\:dd HH\:mm\:ss", Nothing)
            Catch ex As Exception
                Return DateTime.MinValue
            End Try
        End Get
        Set(ByVal Value As DateTime)
            Try
                Me.SetPropertyString(TagNames.DateTime, Value.ToString("yyyy\:MM\:dd HH\:mm\:ss"))
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Time when image was taken (EXIF DateTimeOriginal).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property DateTimeOriginal() As DateTime
        Get
            Try
                Return DateTime.ParseExact(Me.GetPropertyString(TagNames.ExifDTOrig), "yyyy\:MM\:dd HH\:mm\:ss", Nothing)
            Catch ex As Exception
                Return DateTime.MinValue
            End Try
        End Get
        Set(ByVal Value As DateTime)
            Try
                Me.SetPropertyString(TagNames.ExifDTOrig, Value.ToString("yyyy\:MM\:dd HH\:mm\:ss"))
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Time when image was digitized (EXIF DateTimeDigitized).
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property DateTimeDigitized() As DateTime
        Get
            Try
                Return DateTime.ParseExact(Me.GetPropertyString(TagNames.ExifDTDigitized), "yyyy\:MM\:dd HH\:mm\:ss", Nothing)
            Catch ex As Exception
                Return DateTime.MinValue
            End Try
        End Get
        Set(ByVal Value As DateTime)
            Try
                Me.SetPropertyString(TagNames.ExifDTDigitized, Value.ToString("yyyy\:MM\:dd HH\:mm\:ss"))
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Image width
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 04.09.2005 Changed output to Int32, load from image instead of EXIF
    ''' </history>
    Public ReadOnly Property Width() As Int32
        Get
            Return Me._Image.Width
        End Get
    End Property

    ''' <summary>
    ''' Image height
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 04.09.2005 Changed output to Int32, load from image instead of EXIF
    ''' </history>
    Public ReadOnly Property Height() As Int32
        Get
            Return Me._Image.Height
        End Get
    End Property

    ''' <summary>
    ''' X resolution in dpi (EXIF XResolution/ResolutionUnit)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ResolutionX() As Double
        Get
            Dim R As Double = Me.GetPropertyRational(TagNames.XResolution).ToDouble()

            If Me.GetPropertyInt16(TagNames.ResolutionUnit) = 3 Then
                '-- resolution is in points/cm
                Return R * 2.54
            Else
                '-- resolution is in points/inch
                Return R
            End If
        End Get
    End Property

    ''' <summary>
    ''' Y resolution in dpi (EXIF YResolution/ResolutionUnit)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ResolutionY() As Double
        Get
            Dim R As Double = Me.GetPropertyRational(TagNames.YResolution).ToDouble()

            If Me.GetPropertyInt16(TagNames.ResolutionUnit) = 3 Then
                '-- resolution is in points/cm
                Return R * 2.54
            Else
                '-- resolution is in points/inch
                Return R
            End If
        End Get
    End Property

    ''' <summary>
    ''' Image title (EXIF ImageTitle)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property Title() As String
        Get
            Return Me.GetPropertyString(TagNames.ImageTitle)
        End Get
        Set(ByVal Value As String)
            Try
                Me.SetPropertyString(TagNames.ImageTitle, Value)
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' User comment (EXIF UserComment)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 13.06.2004 Created
    ''' </history>
    Public Property UserComment() As String
        Get
            Return Me.GetPropertyString(TagNames.ExifUserComment)
        End Get
        Set(ByVal Value As String)
            Try
                Me.SetPropertyString(TagNames.ExifUserComment, Value)
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Artist name (EXIF Artist)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 13.06.2004 Created
    ''' </history>
    Public Property Artist() As String
        Get
            Return Me.GetPropertyString(TagNames.Artist)
        End Get
        Set(ByVal Value As String)
            Try
                Me.SetPropertyString(TagNames.Artist, Value)
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Image description (EXIF ImageDescription)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property Description() As String
        Get
            Return Me.GetPropertyString(TagNames.ImageDescription)
        End Get
        Set(ByVal Value As String)
            Try
                Me.SetPropertyString(TagNames.ImageDescription, Value)
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Image copyright (EXIF Copyright)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Property Copyright() As String
        Get
            Return Me.GetPropertyString(TagNames.Copyright)
        End Get
        Set(ByVal Value As String)
            Try
                Me.SetPropertyString(TagNames.Copyright, Value.ToString)
            Catch ex As Exception
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Exposure time in seconds (EXIF ExifExposureTime/ExifShutterSpeed)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ExposureTime() As Double
        Get
            If Me.IsPropertyDefined(TagNames.ExifExposureTime) Then
                '-- Exposure time is explicitly specified
                Return Me.GetPropertyRational(TagNames.ExifExposureTime).ToDouble
            ElseIf Me.IsPropertyDefined(TagNames.ExifShutterSpeed) Then
                '-- Compute exposure time from shutter speed
                Return 1 / (2 ^ Me.GetPropertyRational(TagNames.ExifShutterSpeed).ToDouble)
            Else
                '-- Can't figure out 
                Return 0
            End If
        End Get
    End Property

    ''' <summary>
    ''' Aperture value as F number (EXIF ExifFNumber/ExifApertureValue)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property Aperture() As Double
        Get
            If Me.IsPropertyDefined(TagNames.ExifFNumber) Then
                Return Me.GetPropertyRational(TagNames.ExifFNumber).ToDouble()
            ElseIf Me.IsPropertyDefined(TagNames.ExifAperture) Then
                Return System.Math.Sqrt(2) ^ Me.GetPropertyRational(TagNames.ExifAperture).ToDouble()
            Else
                Return 0
            End If
        End Get
    End Property

    ''' <summary>
    ''' Exposure program used (EXIF ExifExposureProg)
    ''' </summary>
    ''' <value></value>
    ''' <remarks>If not specified, returns Normal (2)</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ExposureProgram() As ExposurePrograms
        Get
            Dim X As Int32 = Me.GetPropertyInt16(TagNames.ExifExposureProg)

            If [Enum].IsDefined(GetType(ExposurePrograms), X) Then
                Return CType([Enum].Parse(GetType(ExposurePrograms), [Enum].GetName(GetType(ExposurePrograms), X)), ExposurePrograms)
            Else
                Return ExposurePrograms.Normal
            End If
        End Get
    End Property

    ''' <summary>
    ''' ISO sensitivity
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ISO() As Int16
        Get
            Return Me.GetPropertyInt16(TagNames.ExifISOSpeed)
        End Get
    End Property

    ''' <summary>
    ''' Subject distance in meters (EXIF SubjectDistance)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property SubjectDistance() As Double
        Get
            Return Me.GetPropertyRational(TagNames.ExifSubjectDist).ToDouble()
        End Get
    End Property

    ''' <summary>
    ''' Exposure method metering mode used (EXIF MeteringMode)
    ''' </summary>
    ''' <value></value>
    ''' <remarks>If not specified, returns Unknown (0)</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property ExposureMeteringMode() As ExposureMeteringModes
        Get
            Dim X As Int32 = Me.GetPropertyInt16(TagNames.ExifMeteringMode)

            If [Enum].IsDefined(GetType(ExposureMeteringModes), X) Then
                Return CType([Enum].Parse(GetType(ExposureMeteringModes), [Enum].GetName(GetType(ExposureMeteringModes), X)), ExposureMeteringModes)
            Else
                Return ExposureMeteringModes.Unknown
            End If
        End Get
    End Property

    ''' <summary>
    ''' Focal length of lenses in mm (EXIF FocalLength)
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property FocalLength() As Double
        Get
            Return Me.GetPropertyRational(TagNames.ExifFocalLength).ToDouble
        End Get
    End Property

    ''' <summary>
    ''' Flash mode (EXIF Flash)
    ''' </summary>
    ''' <value></value>
    ''' <remarks>If not present, value NotFired (0) is returned</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property FlashMode() As FlashModes
        Get
            Dim X As Int32 = Me.GetPropertyInt16(TagNames.ExifFlash)

            If [Enum].IsDefined(GetType(FlashModes), X) Then
                Return CType([Enum].Parse(GetType(FlashModes), [Enum].GetName(GetType(FlashModes), X)), FlashModes)
            Else
                Return FlashModes.NotFired
            End If
        End Get
    End Property

    ''' <summary>
    ''' Light source / white balance (EXIF LightSource)
    ''' </summary>
    ''' <value></value>
    ''' <remarks>If not specified, returns Unknown (0).</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public ReadOnly Property LightSource() As LightSources
        Get
            Dim X As Int32 = Me.GetPropertyInt16(TagNames.ExifLightSource)

            If [Enum].IsDefined(GetType(LightSources), X) Then
                Return CType([Enum].Parse(GetType(LightSources), [Enum].GetName(GetType(LightSources), X)), LightSources)
            Else
                Return LightSources.Unknown
            End If
        End Get
    End Property

    ''' <summary>
    ''' GPS Latitude
    ''' </summary>
    ''' <remarks>If not specified, returns Double.NaN.</remarks>
    ''' <history>
    ''' [vatavian] 13 September 2007 Created
    ''' </history>
    Public Property GpsLatitude() As Double
        Get
            Return GetCoordinateGPS(TagNames.GpsLatitude)
        End Get
        Set(ByVal value As Double)
            SetCoordinateGPS(TagNames.GpsLatitude, value)
        End Set
    End Property

    ''' <summary>
    ''' GPS Longitude
    ''' </summary>
    ''' <remarks>If not specified, returns Double.NaN.</remarks>
    ''' <history>
    ''' [vatavian] 13 September 2007 Created
    ''' </history>
    Public Property GpsLongitude() As Double
        Get
            Return GetCoordinateGPS(TagNames.GpsLongitude)
        End Get
        Set(ByVal value As Double)
            SetCoordinateGPS(TagNames.GpsLongitude, value)
        End Set
    End Property

    '    double CExifGPS::GetCoordinate(Image* image, int tag, int reftag)
    '{
    '    double coordinate = 0.0;

    '    unsigned int size = 0;
    '    PropertyItem* propertyItem = NULL;

    '    size = image->GetPropertyItemSize(tag);
    '    if(size > 0)
    '    {
    '        propertyItem = (PropertyItem*)malloc(size);
    '        image->GetPropertyItem(tag, size, propertyItem);

    '        long* pVals = (long*)(propertyItem->value);

    '        // Degrees
    '        coordinate = pVals[0];
    '        coordinate /= pVals[1];

    '        // Minutes, either mm/1 or mmmm/100 for decimal minutes
    '        double minutes = pVals[2];
    '        minutes /= pVals[3];

    '        // Seconds, either ss/1 or ssss/100 for decimal seconds
    '        double seconds = pVals[4];
    '        seconds /= pVals[5];

    '        coordinate += minutes / 60.0 + seconds / (60.0 * 60.0);

    '        free(propertyItem);
    '    }

    '    return coordinate * GetReferenceFactor(image, reftag);
    '}

#End Region

#Region " Support methods for working with EXIF properties "

    ''' <summary>
    ''' Checks if current image has specified certain property
    ''' </summary>
    ''' <param name="PID">PropertyID</param>
    ''' <returns>True if image has specified property, False otherwise.</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Function IsPropertyDefined(ByVal PID As Int32) As Boolean
        Return CBool([Array].IndexOf(Me._Image.PropertyIdList, PID) > -1)
    End Function

    Public Function GetPropertyFormatted(ByVal PID As TagNames, Optional ByVal DefaultValue As String = "") As String
        If Me.IsPropertyDefined(PID) Then
            GetPropertyFormatted = ""
            Dim pi As Drawing.Imaging.PropertyItem = Me._Image.GetPropertyItem(PID)

            Select Case PID
                Case TagNames.GpsLatitude, TagNames.GpsLongitude
                    Dim lArray As Rational() = GetRationalArray(pi.Value)
                    Dim lCoordinate As Double = lArray(0).ToDouble _
                                              + lArray(1).ToDouble / 60.0 _
                                              + lArray(2).ToDouble / 3600.0
                    Return lCoordinate.ToString
            End Select

            Dim lType As ExifDataTypes = pi.Type
            Select Case lType
                Case ExifDataTypes.AsciiString
                    Try
                        Return GetString(pi.Value)
                    Catch e As Exception
                        Return ""
                    End Try
                Case ExifDataTypes.SignedByte, ExifDataTypes.Undefined
                    For Each lArrayValue As Byte In pi.Value
                        GetPropertyFormatted &= lArrayValue & ", "
                    Next
                    'Trim final ", "
                    GetPropertyFormatted = GetPropertyFormatted.Substring(0, GetPropertyFormatted.Length - 2)
                    Exit Function
                Case ExifDataTypes.SignedLong
                    Dim lArray As Int32() = GetInt32array(pi.Value)
                    For Each lArrayValue As Int32 In lArray
                        GetPropertyFormatted &= lArrayValue & ", "
                    Next
                    'Trim final ", "
                    GetPropertyFormatted = GetPropertyFormatted.Substring(0, GetPropertyFormatted.Length - 2)
                    Exit Function
                Case ExifDataTypes.UnsignedLong
                    Dim lArray As UInt32() = GetUInt32array(pi.Value)
                    For Each lArrayValue As UInt32 In lArray
                        GetPropertyFormatted &= lArrayValue & ", "
                    Next
                    'Trim final ", "
                    GetPropertyFormatted = GetPropertyFormatted.Substring(0, GetPropertyFormatted.Length - 2)
                    Exit Function

                Case ExifDataTypes.UnsignedRational, ExifDataTypes.SignedRational
                    Dim lArray As Rational() = GetRationalArray(pi.Value)
                    For Each lArrayValue As Rational In lArray
                        GetPropertyFormatted &= lArrayValue.ToDouble & ", "
                    Next
                    'Trim final ", "
                    GetPropertyFormatted = GetPropertyFormatted.Substring(0, GetPropertyFormatted.Length - 2)
                    Exit Function

                Case ExifDataTypes.UnsignedShort
                    Dim lArray As Int16() = GetInt16array(pi.Value)
                    For Each lArrayValue As Int16 In lArray
                        GetPropertyFormatted &= lArrayValue & ", "
                    Next
                    'Trim final ", "
                    GetPropertyFormatted = GetPropertyFormatted.Substring(0, GetPropertyFormatted.Length - 2)
                    Exit Function
            End Select
            Return "Type not yet supported (" & lType & ") " & pi.Len & " bytes"
        Else
            Return DefaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets specified Int32 property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="DefaultValue">Optional, default 0. Default value returned if property is not present.</param>
    ''' <remarks>Value of property or DefaultValue if property is not present.</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Function GetPropertyInt32(ByVal PID As Int32, Optional ByVal DefaultValue As Int32 = 0) As Int32
        If Me.IsPropertyDefined(PID) Then
            Return GetInt32(Me._Image.GetPropertyItem(PID).Value)
        Else
            Return DefaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets specified Int16 property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="DefaultValue">Optional, default 0. Default value returned if property is not present.</param>
    ''' <remarks>Value of property or DefaultValue if property is not present.</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Function GetPropertyInt16(ByVal PID As Int32, Optional ByVal DefaultValue As Int16 = 0) As Int16
        If Me.IsPropertyDefined(PID) Then
            Return GetInt16(Me._Image.GetPropertyItem(PID).Value)
        Else
            Return DefaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets specified string property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="DefaultValue">Optional, default String.Empty. Default value returned if property is not present.</param>
    ''' <returns></returns>
    ''' <remarks>Value of property or DefaultValue if property is not present.</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Function GetPropertyString(ByVal PID As Int32, Optional ByVal DefaultValue As String = "") As String
        If Me.IsPropertyDefined(PID) Then
            Return GetString(Me._Image.GetPropertyItem(PID).Value)
        Else
            Return DefaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets specified property in raw form
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="DefaultValue">Optional, default Nothing. Default value returned if property is not present.</param>
    ''' <returns></returns>
    ''' <remarks>Is recommended to use typed methods (like <see cref="GetPropertyString" /> etc.) instead, when possible.</remarks>
    ''' <history>
    ''' [altair] 05.09.2005 Created
    ''' </history>
    Public Function GetProperty(ByVal PID As Int32, Optional ByVal DefaultValue As Byte() = Nothing) As Byte()
        If Me.IsPropertyDefined(PID) Then
            Return Me._Image.GetPropertyItem(PID).Value
        Else
            Return DefaultValue
        End If
    End Function

    ''' <summary>
    ''' Gets specified rational property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <returns></returns>
    ''' <remarks>Value of property or 0/1 if not present.</remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Function GetPropertyRational(ByVal PID As Int32) As Rational
        If Me.IsPropertyDefined(PID) Then
            Return GetRational(Me._Image.GetPropertyItem(PID).Value)
        Else
            Dim R As Rational
            R.Numerator = 0
            R.Denominator = 1
            Return R
        End If
    End Function

    Public Function GetCoordinateGPS(ByVal PID As TagNames) As Double
        Dim pi As Drawing.Imaging.PropertyItem = Me._Image.GetPropertyItem(PID)
        If pi Is Nothing Then
            Return Double.NaN
        Else
            Dim degrees As Double = BitConverter.ToInt32(pi.Value, 0) / BitConverter.ToInt32(pi.Value, 4)
            Dim minutes As Double = BitConverter.ToInt32(pi.Value, 8) / BitConverter.ToInt32(pi.Value, 12)
            Dim seconds As Double = BitConverter.ToInt32(pi.Value, 16) / BitConverter.ToInt32(pi.Value, 20)
            Dim decimalDegrees As Double = degrees + minutes / 60 + seconds / 3600
            Select Case PID
                Case TagNames.GpsLongitude
                    If GetPropertyString(TagNames.GpsLongitudeRef, "E") = "W" Then decimalDegrees = -decimalDegrees
                Case TagNames.GpsLatitude
                    If GetPropertyString(TagNames.GpsLatitudeRef, "N") = "S" Then decimalDegrees = -decimalDegrees
            End Select
            Return decimalDegrees
        End If
    End Function

    Public Sub SetCoordinateGPS(ByVal PID As Int32, ByVal Coordinate As Double)

        If (PID = TagNames.GpsLongitude) Then
            If (Coordinate >= 0) Then
                SetPropertyString(TagNames.GpsLongitudeRef, "E")
            Else
                SetPropertyString(TagNames.GpsLongitudeRef, "W")
            End If
        Else
            If (Coordinate >= 0) Then
                SetPropertyString(TagNames.GpsLatitudeRef, "N")
            Else
                SetPropertyString(TagNames.GpsLatitudeRef, "S")
            End If
        End If

        Coordinate = Math.Abs(Coordinate)

        Dim lDegrees As Integer = CInt(Math.Floor(Coordinate))

        'Subtract degrees to see how many fractional degrees are left
        'Absolute value because sign is preserved in the degrees field, minutes and seconds are always positive
        Coordinate = Coordinate - lDegrees

        Dim lMinutes As Integer = CInt(Math.Floor(Coordinate * 60))
        Coordinate -= lMinutes / 60

        'seconds * 1000, we will put 1000 in the denominator to make it seconds
        Dim lSeconds As Integer = CInt(Math.Round(Coordinate * 3600000))

        Dim Data() As Byte
        ReDim Data(23)
        BitConverter.GetBytes(lDegrees).CopyTo(Data, 0) : BitConverter.GetBytes(CInt(1)).CopyTo(Data, 4)
        BitConverter.GetBytes(lMinutes).CopyTo(Data, 8) : BitConverter.GetBytes(CInt(1)).CopyTo(Data, 12)
        BitConverter.GetBytes(lSeconds).CopyTo(Data, 16) : BitConverter.GetBytes(CInt(1000)).CopyTo(Data, 20)

        SetProperty(PID, Data, ExifDataTypes.UnsignedRational)
    End Sub

    ''' <summary>
    ''' Sets specified string property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="Value">Value to be set</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 12.6.2004 Created
    ''' </history>
    Public Sub SetPropertyString(ByVal PID As Int32, ByVal Value As String)
        Dim Data() As Byte = Me._Encoding.GetBytes(Value & vbNullChar)
        SetProperty(PID, Data, ExifDataTypes.AsciiString)
    End Sub

    ''' <summary>
    ''' Sets specified Int16 property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="Value">Value to be set</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 12.6.2004 Created
    ''' </history>
    Public Sub SetPropertyInt16(ByVal PID As Int32, ByVal Value As Int16)
        Dim Data(1) As Byte
        Data(0) = CType(Value And &HFF, Byte)
        Data(1) = CType((Value And &HFF00) >> 8, Byte)
        SetProperty(PID, Data, ExifDataTypes.SignedShort)
    End Sub

    ''' <summary>
    ''' Sets specified Int32 property
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="Value">Value to be set</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 13.06.2004 Created
    ''' </history>
    Public Sub SetPropertyInt32(ByVal PID As Int32, ByVal Value As Int32)
        Dim Data(3) As Byte
        For I As Int32 = 0 To 3
            Data(I) = CType(Value And &HFF, Byte)
            Value >>= 8
        Next
        SetProperty(PID, Data, ExifDataTypes.SignedLong)
    End Sub

    ''' <summary>
    ''' Sets specified propery in raw form
    ''' </summary>
    ''' <param name="PID">Property ID</param>
    ''' <param name="Data">Raw data</param>
    ''' <param name="Type">EXIF data type</param>
    ''' <remarks>Is recommended to use typed methods (like <see cref="SetPropertyString" /> etc.) instead, when possible.</remarks>
    ''' <history>
    ''' [altair] 12.6.2004 Created
    ''' </history>
    Public Sub SetProperty(ByVal PID As Int32, ByVal Data() As Byte, ByVal Type As ExifDataTypes)
        Dim P As System.Drawing.Imaging.PropertyItem = Me._Image.PropertyItems(0)
        P.Id = PID
        P.Value = Data
        P.Type = Type
        P.Len = Data.Length
        Me._Image.SetPropertyItem(P)
    End Sub

    ''' <summary>
    ''' Reads Int32 from EXIF bytearray.
    ''' </summary>
    ''' <param name="B">EXIF bytearray to process</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 05.09.2005 Changed from public shared to private instance method
    ''' </history>
    Private Function GetInt32(ByVal B As Byte()) As Int32
        If B.Length < 4 Then Throw New ArgumentException("Data too short (4 bytes expected)", "B")
        Return B(3) << 24 Or B(2) << 16 Or B(1) << 8 Or B(0)
    End Function

    Private Function GetInt32array(ByVal B As Byte()) As Int32()
        Dim lArray As Int32()
        Dim lArrayBound As Integer = B.Length / 4 - 1
        ReDim lArray(lArrayBound)
        Dim lByteIndex As Integer
        For lIndex As Integer = 0 To lArrayBound
            lByteIndex = lIndex * 4
            lArray(lIndex) = B(lByteIndex + 3) << 24 Or B(lByteIndex + 2) << 16 Or B(lByteIndex + 1) << 8 Or B(lByteIndex)
        Next
        Return lArray
    End Function

    Private Function GetUInt32array(ByVal B As Byte()) As UInt32()
        Dim lArray As UInt32()
        Dim lArrayBound As Integer = B.Length / 4 - 1
        ReDim lArray(lArrayBound)
        Dim lByteIndex As Integer
        For lIndex As Integer = 0 To lArrayBound
            lByteIndex = lIndex * 4
            lArray(lIndex) = B(lByteIndex + 3) << 24 Or B(lByteIndex + 2) << 16 Or B(lByteIndex + 1) << 8 Or B(lByteIndex)
        Next
        Return lArray
    End Function

    ''' <summary>
    ''' Reads Int16 from EXIF bytearray.
    ''' </summary>
    ''' <param name="B">EXIF bytearray to process</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 05.09.2005 Changed from public shared to private instance method
    ''' </history>
    Private Function GetInt16(ByVal B As Byte()) As Int16
        If B.Length < 2 Then Throw New ArgumentException("Data too short (2 bytes expected)", "B")
        Return B(1) << 8 Or B(0)
    End Function

    Private Function GetInt16array(ByVal B As Byte()) As Int16()
        If B.Length < 2 Then Throw New ArgumentException("Data too short (at least 2 bytes expected)", "B")
        Dim lArray As Int16()
        Dim lArrayBound As Integer = B.Length / 2 - 1
        ReDim lArray(lArrayBound)
        For lIndex As Integer = 0 To lArrayBound
            lArray(lIndex) = B(lIndex * 2 + 1) << 8 Or B(lIndex * 2)
        Next
        Return lArray
    End Function

    ''' <summary>
    ''' Reads string from EXIF bytearray.
    ''' </summary>
    ''' <param name="B">EXIF bytearray to process</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 05.09.2005 Changed from public shared to private instance method
    ''' </history>
    Private Function GetString(ByVal B As Byte()) As String
        Dim R As String = Me._Encoding.GetString(B)
        If R.EndsWith(vbNullChar) Then R = R.Substring(0, R.Length - 1)
        Return R
    End Function

    ''' <summary>
    ''' Reads rational from EXIF bytearray.
    ''' </summary>
    ''' <param name="B">EXIF bytearray to process</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' [altair] 05.09.2005 Changed from public shared to private instance method
    ''' </history>
    Private Function GetRational(ByVal B As Byte()) As Rational
        Dim R As New Rational, N(3), D(3) As Byte
        Array.Copy(B, 0, N, 0, 4)
        Array.Copy(B, 4, D, 0, 4)
        R.Denominator = Me.GetInt32(D)
        R.Numerator = Me.GetInt32(N)
        Return R
    End Function

    Private Function GetRationalArray(ByVal B As Byte()) As Rational()
        If B.Length < 8 Then Throw New ArgumentException("Data too short (at least 8 bytes expected)", "B")
        Dim lArray As Rational()
        Dim lArrayBound As Integer = B.Length / 8 - 1
        ReDim lArray(lArrayBound)
        Dim lByteIndex As Integer
        Dim R As New Rational, N(3), D(3) As Byte
        For lIndex As Integer = 0 To lArrayBound
            lByteIndex = lIndex * 8
            Array.Copy(B, lByteIndex, N, 0, 4)
            Array.Copy(B, lByteIndex + 4, D, 0, 4)

            lArray(lIndex).Denominator = Me.GetInt32(D)
            lArray(lIndex).Numerator = Me.GetInt32(N)
        Next
        Return lArray
    End Function
#End Region

#Region " IDisposable implementation "

    ''' <summary>
    ''' Disposes unmanaged resources of this class
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' [altair] 10.09.2003 Created
    ''' </history>
    Public Sub Dispose() Implements System.IDisposable.Dispose
        Me._Image.Dispose()
    End Sub

#End Region

End Class
