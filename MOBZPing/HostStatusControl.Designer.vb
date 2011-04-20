<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HostStatusControl
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
    Me.HostToolStrip = New System.Windows.Forms.ToolStrip
    Me.PauseButton = New System.Windows.Forms.ToolStripButton
    Me.DeleteButton = New System.Windows.Forms.ToolStripButton
    Me.NameLabel = New System.Windows.Forms.ToolStripLabel
    Me.StatusPictureBox = New System.Windows.Forms.PictureBox
    Me.HostToolStrip.SuspendLayout()
    CType(Me.StatusPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'HostToolStrip
    '
    Me.HostToolStrip.CanOverflow = False
    Me.HostToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
    Me.HostToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PauseButton, Me.DeleteButton, Me.NameLabel})
    Me.HostToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
    Me.HostToolStrip.Location = New System.Drawing.Point(5, 5)
    Me.HostToolStrip.Name = "HostToolStrip"
    Me.HostToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
    Me.HostToolStrip.Size = New System.Drawing.Size(141, 25)
    Me.HostToolStrip.Stretch = True
    Me.HostToolStrip.TabIndex = 2
    '
    'PauseButton
    '
    Me.PauseButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
    Me.PauseButton.CheckOnClick = True
    Me.PauseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.PauseButton.Image = Global.MOBZPing.My.Resources.Resources.PauseHS
    Me.PauseButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.PauseButton.Name = "PauseButton"
    Me.PauseButton.Size = New System.Drawing.Size(23, 22)
    Me.PauseButton.ToolTipText = "Pause"
    '
    'DeleteButton
    '
    Me.DeleteButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
    Me.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.DeleteButton.Image = Global.MOBZPing.My.Resources.Resources.DeleteHS
    Me.DeleteButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.DeleteButton.Name = "DeleteButton"
    Me.DeleteButton.Size = New System.Drawing.Size(23, 22)
    Me.DeleteButton.Text = "ToolStripButton1"
    Me.DeleteButton.ToolTipText = "Remove this host"
    '
    'NameLabel
    '
    Me.NameLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
    Me.NameLabel.Name = "NameLabel"
    Me.NameLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
    Me.NameLabel.Size = New System.Drawing.Size(64, 22)
    Me.NameLabel.Text = "(HostName)"
    '
    'StatusPictureBox
    '
    Me.StatusPictureBox.Dock = System.Windows.Forms.DockStyle.Top
    Me.StatusPictureBox.Location = New System.Drawing.Point(5, 30)
    Me.StatusPictureBox.Margin = New System.Windows.Forms.Padding(3, 10, 3, 10)
    Me.StatusPictureBox.Name = "StatusPictureBox"
    Me.StatusPictureBox.Size = New System.Drawing.Size(141, 12)
    Me.StatusPictureBox.TabIndex = 1
    Me.StatusPictureBox.TabStop = False
    '
    'HostStatusControl
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.Controls.Add(Me.StatusPictureBox)
    Me.Controls.Add(Me.HostToolStrip)
    Me.Name = "HostStatusControl"
    Me.Padding = New System.Windows.Forms.Padding(5)
    Me.Size = New System.Drawing.Size(151, 70)
    Me.HostToolStrip.ResumeLayout(False)
    Me.HostToolStrip.PerformLayout()
    CType(Me.StatusPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents StatusPictureBox As System.Windows.Forms.PictureBox
  Friend WithEvents HostToolStrip As System.Windows.Forms.ToolStrip
  Friend WithEvents DeleteButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents PauseButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents NameLabel As System.Windows.Forms.ToolStripLabel

End Class
