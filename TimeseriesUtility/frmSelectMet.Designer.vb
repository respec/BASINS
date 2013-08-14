<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectMet
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectMet))
        Me.grpComputations = New System.Windows.Forms.GroupBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.grpDisaggregations = New System.Windows.Forms.GroupBox()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.grpComputations.SuspendLayout()
        Me.grpDisaggregations.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpComputations
        '
        Me.grpComputations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpComputations.Controls.Add(Me.Button6)
        Me.grpComputations.Controls.Add(Me.Button5)
        Me.grpComputations.Controls.Add(Me.Button4)
        Me.grpComputations.Controls.Add(Me.Button3)
        Me.grpComputations.Controls.Add(Me.Button2)
        Me.grpComputations.Controls.Add(Me.Button1)
        Me.grpComputations.Location = New System.Drawing.Point(12, 12)
        Me.grpComputations.Name = "grpComputations"
        Me.grpComputations.Size = New System.Drawing.Size(193, 194)
        Me.grpComputations.TabIndex = 0
        Me.grpComputations.TabStop = False
        Me.grpComputations.Text = "Computations"
        '
        'Button6
        '
        Me.Button6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button6.Location = New System.Drawing.Point(6, 164)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(181, 23)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Cloud Cover"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.Location = New System.Drawing.Point(6, 135)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(181, 23)
        Me.Button5.TabIndex = 4
        Me.Button5.Text = "Wind Travel"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Location = New System.Drawing.Point(6, 106)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(181, 23)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Penman Pan Evaporation"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(6, 77)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(181, 23)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Jensen PET"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Location = New System.Drawing.Point(6, 48)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(181, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Hamon PET"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(6, 19)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(181, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Solar Radiation"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'grpDisaggregations
        '
        Me.grpDisaggregations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpDisaggregations.Controls.Add(Me.Button7)
        Me.grpDisaggregations.Controls.Add(Me.Button8)
        Me.grpDisaggregations.Controls.Add(Me.Button10)
        Me.grpDisaggregations.Controls.Add(Me.Button11)
        Me.grpDisaggregations.Controls.Add(Me.Button12)
        Me.grpDisaggregations.Location = New System.Drawing.Point(211, 12)
        Me.grpDisaggregations.Name = "grpDisaggregations"
        Me.grpDisaggregations.Size = New System.Drawing.Size(193, 194)
        Me.grpDisaggregations.TabIndex = 1
        Me.grpDisaggregations.TabStop = False
        Me.grpDisaggregations.Text = "Disaggregations"
        '
        'Button7
        '
        Me.Button7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button7.Location = New System.Drawing.Point(6, 164)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(181, 23)
        Me.Button7.TabIndex = 5
        Me.Button7.Text = "Precipitation"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button8.Location = New System.Drawing.Point(6, 135)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(181, 23)
        Me.Button8.TabIndex = 4
        Me.Button8.Tag = "Wind (Disaggregate)"
        Me.Button8.Text = "Wind"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button10.Location = New System.Drawing.Point(6, 77)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(181, 23)
        Me.Button10.TabIndex = 2
        Me.Button10.Text = "Temperature"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button11.Location = New System.Drawing.Point(6, 48)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(181, 23)
        Me.Button11.TabIndex = 1
        Me.Button11.Text = "Evapotranspiration"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button12.Location = New System.Drawing.Point(6, 19)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(181, 23)
        Me.Button12.TabIndex = 0
        Me.Button12.Tag = "Solar Radiation (Disaggregate)"
        Me.Button12.Text = "Solar Radiation"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'frmSelectMet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(416, 218)
        Me.Controls.Add(Me.grpDisaggregations)
        Me.Controls.Add(Me.grpComputations)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmSelectMet"
        Me.Text = "Select Meteorologic Operation"
        Me.grpComputations.ResumeLayout(False)
        Me.grpDisaggregations.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpComputations As System.Windows.Forms.GroupBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents grpDisaggregations As System.Windows.Forms.GroupBox
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
End Class
