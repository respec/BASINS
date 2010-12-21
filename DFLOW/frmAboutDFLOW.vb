Public NotInheritable Class frmAboutDFLOW

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle) & ": DFLOW Plugin"
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description & vbCrLf & vbCrLf & _
          "DFLOW 4.0 is an implementation of the U.S. EPA " & vbCrLf & "methodology for Stream Design Flow for " & vbCrLf & _
          "Steady-State Modeling as set forth in Book VI of " & vbCrLf & "the Technical Guidance Manual for Performing " & vbCrLf & _
          "Wasteload Allocations. " & vbCrLf & vbCrLf & _
          "This implementation by AQUA TERRA " & vbCrLf & "Consultants and LimnoTech is integrated into " & vbCrLf & _
          "US(EPA) 's BASINS 4.0 modeling system. This " & vbCrLf & "supports improved access to data and " & vbCrLf & _
          "automatic update of USGS calculation routines."


    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
