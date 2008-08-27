Imports System.Windows.Forms

Public Class frmTextReport
    Inherits System.Windows.Forms.Form
    Private pTitle As String
    Private pReportBody As String

    Public Property Title() As String
        Get
            Return pTitle
        End Get
        Set(ByVal aTitle As String)
            pTitle = aTitle
        End Set
    End Property

    Public Property ReportBody() As String
        Get
            Return pReportBody
        End Get
        Set(ByVal aReport As String)
            pReportBody = aReport
            displayReport()
        End Set
    End Property
    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pTitle = ""
        pReportBody = ""

    End Sub

    Public Sub displayReport()
        txtReport.Clear()
        If pReportBody.Trim().Length > 10 Then
            lblTxtReportTitle.Text = pTitle
            txtReport.Text = pReportBody
        Else
            lblTxtReportTitle.Text = "Alert!"
            txtReport.Text = vbCrLf & vbCrLf & "Report text was not created properly, please try again."
        End If
        Me.Show()

    End Sub

    Public Overloads Sub dispose()


    End Sub

End Class