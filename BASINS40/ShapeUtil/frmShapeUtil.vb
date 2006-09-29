Option Strict Off
Option Explicit On
Friend Class frmShapeUtil
	Inherits System.Windows.Forms.Form
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		If m_vb6FormDefInstance Is Nothing Then
			If m_InitializingDefInstance Then
				m_vb6FormDefInstance = Me
			Else
				Try 
					'For the start-up form, the first instance created is the default instance.
					If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
						m_vb6FormDefInstance = Me
					End If
				Catch
				End Try
			End If
		End If
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents timerShow As System.Windows.Forms.Timer
	Public WithEvents txtLicense As System.Windows.Forms.TextBox
	Public WithEvents lblStatus As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmShapeUtil))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ToolTip1.Active = True
		Me.timerShow = New System.Windows.Forms.Timer(components)
		Me.txtLicense = New System.Windows.Forms.TextBox
		Me.lblStatus = New System.Windows.Forms.Label
		Me.Text = "Shape Util"
		Me.ClientSize = New System.Drawing.Size(546, 472)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Icon = CType(resources.GetObject("frmShapeUtil.Icon"), System.Drawing.Icon)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmShapeUtil"
		Me.timerShow.Interval = 2000
		Me.timerShow.Enabled = True
		Me.txtLicense.AutoSize = False
		Me.txtLicense.Size = New System.Drawing.Size(529, 337)
		Me.txtLicense.Location = New System.Drawing.Point(8, 8)
		Me.txtLicense.MultiLine = True
		Me.txtLicense.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.txtLicense.WordWrap = False
		Me.txtLicense.TabIndex = 0
		Me.txtLicense.Text = "kThe projection library in use is based on code originally written" & Chr(13) & Chr(10) & "by Gerald Evenden, then of the USGS." & Chr(13) & Chr(10) & "" & Chr(13) & Chr(10) & "The current version of this library (4.4.7 is compiled as" & Chr(13) & Chr(10) & "projATC.dll in this package) is available at:" & Chr(13) & Chr(10) & "" & Chr(13) & Chr(10) & "http://remotesensing.org/proj/" & Chr(13) & Chr(10) & "" & Chr(13) & Chr(10) & "This"
		Me.txtLicense.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtLicense.AcceptsReturn = True
		Me.txtLicense.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLicense.BackColor = System.Drawing.SystemColors.Window
		Me.txtLicense.CausesValidation = True
		Me.txtLicense.Enabled = True
		Me.txtLicense.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLicense.HideSelection = True
		Me.txtLicense.ReadOnly = False
		Me.txtLicense.Maxlength = 0
		Me.txtLicense.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLicense.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLicense.TabStop = True
		Me.txtLicense.Visible = True
		Me.txtLicense.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLicense.Name = "txtLicense"
		Me.lblStatus.Size = New System.Drawing.Size(513, 89)
		Me.lblStatus.Location = New System.Drawing.Point(16, 368)
		Me.lblStatus.TabIndex = 1
		Me.lblStatus.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.Enabled = True
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Name = "lblStatus"
		Me.Controls.Add(txtLicense)
		Me.Controls.Add(lblStatus)
	End Sub
#End Region 
#Region "Upgrade Support "
	Private Shared m_vb6FormDefInstance As frmShapeUtil
	Private Shared m_InitializingDefInstance As Boolean
	Public Shared Property DefInstance() As frmShapeUtil
		Get
			If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
				m_InitializingDefInstance = True
				m_vb6FormDefInstance = New frmShapeUtil()
				m_InitializingDefInstance = False
			End If
			DefInstance = m_vb6FormDefInstance
		End Get
		Set
			m_vb6FormDefInstance = Value
		End Set
	End Property
#End Region 
	
	Private Sub timerShow_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles timerShow.Tick
		Me.Show()
		timerShow.Enabled = False
	End Sub
End Class