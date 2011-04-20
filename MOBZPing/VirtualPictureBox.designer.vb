<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VirtualPictureBox
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
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
    Me.VerticalScrollBar = New System.Windows.Forms.VScrollBar
    Me.HorizontalScrollBar = New System.Windows.Forms.HScrollBar
    Me.MainPictureBox = New System.Windows.Forms.PictureBox
    CType(Me.MainPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'VerticalScrollBar
    '
    Me.VerticalScrollBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.VerticalScrollBar.Location = New System.Drawing.Point(513, 0)
    Me.VerticalScrollBar.Name = "VerticalScrollBar"
    Me.VerticalScrollBar.Size = New System.Drawing.Size(61, 52)
    Me.VerticalScrollBar.TabIndex = 1
    '
    'HorizontalScrollBar
    '
    Me.HorizontalScrollBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.HorizontalScrollBar.Location = New System.Drawing.Point(0, 174)
    Me.HorizontalScrollBar.Name = "HorizontalScrollBar"
    Me.HorizontalScrollBar.Size = New System.Drawing.Size(228, 33)
    Me.HorizontalScrollBar.TabIndex = 0
    '
    'MainPictureBox
    '
    Me.MainPictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.MainPictureBox.BackColor = System.Drawing.Color.Black
    Me.MainPictureBox.Location = New System.Drawing.Point(15, 17)
    Me.MainPictureBox.Name = "MainPictureBox"
    Me.MainPictureBox.Padding = New System.Windows.Forms.Padding(0, 10, 10, 0)
    Me.MainPictureBox.Size = New System.Drawing.Size(426, 147)
    Me.MainPictureBox.TabIndex = 2
    Me.MainPictureBox.TabStop = False
    '
    'VirtualPictureBox
    '
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
    Me.AutoScroll = True
    Me.Controls.Add(Me.MainPictureBox)
    Me.Controls.Add(Me.HorizontalScrollBar)
    Me.Controls.Add(Me.VerticalScrollBar)
    Me.Name = "VirtualPictureBox"
    Me.Size = New System.Drawing.Size(574, 207)
    CType(Me.MainPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Private WithEvents MainPictureBox As System.Windows.Forms.PictureBox
  Private WithEvents VerticalScrollBar As System.Windows.Forms.VScrollBar
  Private WithEvents HorizontalScrollBar As System.Windows.Forms.HScrollBar
End Class
