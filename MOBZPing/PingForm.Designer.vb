<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PingForm
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
    Me.components = New System.ComponentModel.Container
    Dim ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Dim ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Dim ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PingForm))
    Me.PingTimer = New System.Windows.Forms.Timer(Me.components)
    Me.MainStatusStrip = New System.Windows.Forms.StatusStrip
    Me.StatusStatusLabel = New System.Windows.Forms.ToolStripStatusLabel
    Me.LinkLabel = New System.Windows.Forms.ToolStripStatusLabel
    Me.MainToolStrip = New System.Windows.Forms.ToolStrip
    Me.StartButton = New System.Windows.Forms.ToolStripButton
    Me.StopButton = New System.Windows.Forms.ToolStripButton
    Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
    Me.AddHostTextBox = New System.Windows.Forms.ToolStripTextBox
    Me.AddHostButton = New System.Windows.Forms.ToolStripButton
    Me.ResolutionComboBox = New System.Windows.Forms.ToolStripComboBox
    Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
    Me.TimeComboBox = New System.Windows.Forms.ToolStripComboBox
    Me.MainSplitContainer = New System.Windows.Forms.SplitContainer
    Me.BottomFillPanel = New System.Windows.Forms.Panel
    Me.MainNotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
    Me.NotifyContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
    Me.StartAllMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.StopAllMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
    Me.RestoreMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
    Me.ExitMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.GraphPictureBox = New MOBZPing.VirtualPictureBox
    ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
    ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel
    ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
    ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel
    Me.MainStatusStrip.SuspendLayout()
    Me.MainToolStrip.SuspendLayout()
    Me.MainSplitContainer.Panel1.SuspendLayout()
    Me.MainSplitContainer.Panel2.SuspendLayout()
    Me.MainSplitContainer.SuspendLayout()
    Me.NotifyContextMenu.SuspendLayout()
    Me.SuspendLayout()
    '
    'ToolStripLabel1
    '
    ToolStripLabel1.Name = "ToolStripLabel1"
    ToolStripLabel1.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    ToolStripLabel1.Size = New System.Drawing.Size(59, 22)
    ToolStripLabel1.Text = "&Add host:"
    '
    'ToolStripLabel2
    '
    ToolStripLabel2.Name = "ToolStripLabel2"
    ToolStripLabel2.Size = New System.Drawing.Size(84, 22)
    ToolStripLabel2.Text = "One bar equals:"
    '
    'ToolStripSeparator1
    '
    ToolStripSeparator1.Name = "ToolStripSeparator1"
    ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
    '
    'ToolStripLabel3
    '
    ToolStripLabel3.Name = "ToolStripLabel3"
    ToolStripLabel3.Size = New System.Drawing.Size(76, 22)
    ToolStripLabel3.Text = "Show the last:"
    '
    'PingTimer
    '
    Me.PingTimer.Enabled = True
    Me.PingTimer.Interval = 1000
    '
    'MainStatusStrip
    '
    Me.MainStatusStrip.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusStatusLabel, Me.LinkLabel})
    Me.MainStatusStrip.Location = New System.Drawing.Point(0, 523)
    Me.MainStatusStrip.Name = "MainStatusStrip"
    Me.MainStatusStrip.Size = New System.Drawing.Size(970, 22)
    Me.MainStatusStrip.TabIndex = 3
    Me.MainStatusStrip.Text = "StatusStrip1"
    '
    'StatusStatusLabel
    '
    Me.StatusStatusLabel.Name = "StatusStatusLabel"
    Me.StatusStatusLabel.Size = New System.Drawing.Size(803, 17)
    Me.StatusStatusLabel.Spring = True
    Me.StatusStatusLabel.Text = "Ready"
    Me.StatusStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'LinkLabel
    '
    Me.LinkLabel.IsLink = True
    Me.LinkLabel.Margin = New System.Windows.Forms.Padding(0)
    Me.LinkLabel.Name = "LinkLabel"
    Me.LinkLabel.Size = New System.Drawing.Size(152, 22)
    Me.LinkLabel.Text = "MOBZPing v# by MOBZystems"
    '
    'MainToolStrip
    '
    Me.MainToolStrip.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
    Me.MainToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartButton, Me.StopButton, Me.ToolStripSeparator4, ToolStripLabel1, Me.AddHostTextBox, Me.AddHostButton, ToolStripSeparator1, ToolStripLabel2, Me.ResolutionComboBox, Me.ToolStripSeparator3, ToolStripLabel3, Me.TimeComboBox})
    Me.MainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
    Me.MainToolStrip.Location = New System.Drawing.Point(0, 0)
    Me.MainToolStrip.Name = "MainToolStrip"
    Me.MainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
    Me.MainToolStrip.Size = New System.Drawing.Size(970, 25)
    Me.MainToolStrip.TabIndex = 4
    Me.MainToolStrip.Text = "ToolStrip1"
    '
    'StartButton
    '
    Me.StartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.StartButton.Enabled = False
    Me.StartButton.Image = Global.MOBZPing.My.Resources.Resources.PlayHS
    Me.StartButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.StartButton.Name = "StartButton"
    Me.StartButton.Size = New System.Drawing.Size(23, 22)
    Me.StartButton.ToolTipText = "Start all"
    '
    'StopButton
    '
    Me.StopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.StopButton.Image = Global.MOBZPing.My.Resources.Resources.StopHS
    Me.StopButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.StopButton.Name = "StopButton"
    Me.StopButton.Size = New System.Drawing.Size(23, 22)
    Me.StopButton.Text = "Stop all"
    '
    'ToolStripSeparator4
    '
    Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
    Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
    '
    'AddHostTextBox
    '
    Me.AddHostTextBox.Name = "AddHostTextBox"
    Me.AddHostTextBox.Size = New System.Drawing.Size(150, 25)
    Me.AddHostTextBox.ToolTipText = "Enter host name and press Enter to add"
    '
    'AddHostButton
    '
    Me.AddHostButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
    Me.AddHostButton.Enabled = False
    Me.AddHostButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.AddHostButton.Name = "AddHostButton"
    Me.AddHostButton.Size = New System.Drawing.Size(23, 22)
    Me.AddHostButton.Text = "+"
    Me.AddHostButton.ToolTipText = "Add host"
    '
    'ResolutionComboBox
    '
    Me.ResolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.ResolutionComboBox.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ResolutionComboBox.Items.AddRange(New Object() {"1 second", "10 seconds", "30 seconds", "1 minute", "10 minutes", "30 minutes", "1 hour"})
    Me.ResolutionComboBox.Name = "ResolutionComboBox"
    Me.ResolutionComboBox.Size = New System.Drawing.Size(121, 25)
    Me.ResolutionComboBox.ToolTipText = "Resolution"
    '
    'ToolStripSeparator3
    '
    Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
    Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
    '
    'TimeComboBox
    '
    Me.TimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.TimeComboBox.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.TimeComboBox.MaxDropDownItems = 15
    Me.TimeComboBox.Name = "TimeComboBox"
    Me.TimeComboBox.Size = New System.Drawing.Size(121, 25)
    Me.TimeComboBox.ToolTipText = "Display time"
    '
    'MainSplitContainer
    '
    Me.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
    Me.MainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
    Me.MainSplitContainer.Location = New System.Drawing.Point(0, 25)
    Me.MainSplitContainer.Name = "MainSplitContainer"
    '
    'MainSplitContainer.Panel1
    '
    Me.MainSplitContainer.Panel1.AllowDrop = True
    Me.MainSplitContainer.Panel1.AutoScroll = True
    Me.MainSplitContainer.Panel1.Controls.Add(Me.BottomFillPanel)
    Me.MainSplitContainer.Panel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    '
    'MainSplitContainer.Panel2
    '
    Me.MainSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark
    Me.MainSplitContainer.Panel2.Controls.Add(Me.GraphPictureBox)
    Me.MainSplitContainer.Size = New System.Drawing.Size(970, 498)
    Me.MainSplitContainer.SplitterDistance = 235
    Me.MainSplitContainer.TabIndex = 5
    Me.MainSplitContainer.TabStop = False
    '
    'BottomFillPanel
    '
    Me.BottomFillPanel.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.BottomFillPanel.Location = New System.Drawing.Point(0, 476)
    Me.BottomFillPanel.Name = "BottomFillPanel"
    Me.BottomFillPanel.Size = New System.Drawing.Size(235, 22)
    Me.BottomFillPanel.TabIndex = 0
    '
    'MainNotifyIcon
    '
    Me.MainNotifyIcon.ContextMenuStrip = Me.NotifyContextMenu
    Me.MainNotifyIcon.Icon = CType(resources.GetObject("MainNotifyIcon.Icon"), System.Drawing.Icon)
    Me.MainNotifyIcon.Text = "MOBZPing"
    '
    'NotifyContextMenu
    '
    Me.NotifyContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartAllMenuItem, Me.StopAllMenuItem, Me.ToolStripSeparator5, Me.RestoreMenuItem, Me.ToolStripSeparator2, Me.ExitMenuItem})
    Me.NotifyContextMenu.Name = "NotifyContextMenu"
    Me.NotifyContextMenu.Size = New System.Drawing.Size(163, 104)
    '
    'StartAllMenuItem
    '
    Me.StartAllMenuItem.Image = Global.MOBZPing.My.Resources.Resources.PlayHS
    Me.StartAllMenuItem.Name = "StartAllMenuItem"
    Me.StartAllMenuItem.Size = New System.Drawing.Size(162, 22)
    Me.StartAllMenuItem.Text = "Start all"
    Me.StartAllMenuItem.ToolTipText = "Start all hosts"
    '
    'StopAllMenuItem
    '
    Me.StopAllMenuItem.Image = Global.MOBZPing.My.Resources.Resources.StopHS
    Me.StopAllMenuItem.Name = "StopAllMenuItem"
    Me.StopAllMenuItem.Size = New System.Drawing.Size(162, 22)
    Me.StopAllMenuItem.Text = "Stop all"
    Me.StopAllMenuItem.ToolTipText = "Stop all hosts"
    '
    'ToolStripSeparator5
    '
    Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
    Me.ToolStripSeparator5.Size = New System.Drawing.Size(159, 6)
    '
    'RestoreMenuItem
    '
    Me.RestoreMenuItem.Name = "RestoreMenuItem"
    Me.RestoreMenuItem.Size = New System.Drawing.Size(162, 22)
    Me.RestoreMenuItem.Text = "&Open MOBZPing"
    Me.RestoreMenuItem.ToolTipText = "Show the main MOBZPing window"
    '
    'ToolStripSeparator2
    '
    Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
    Me.ToolStripSeparator2.Size = New System.Drawing.Size(159, 6)
    '
    'ExitMenuItem
    '
    Me.ExitMenuItem.Name = "ExitMenuItem"
    Me.ExitMenuItem.Size = New System.Drawing.Size(162, 22)
    Me.ExitMenuItem.Text = "E&xit MOBZPing"
    Me.ExitMenuItem.ToolTipText = "Close MOBZPing"
    '
    'GraphPictureBox
    '
    Me.GraphPictureBox.AutoScroll = True
    Me.GraphPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.GraphPictureBox.Location = New System.Drawing.Point(0, 0)
    Me.GraphPictureBox.Margin = New System.Windows.Forms.Padding(0)
    Me.GraphPictureBox.Name = "GraphPictureBox"
    Me.GraphPictureBox.Size = New System.Drawing.Size(731, 498)
    Me.GraphPictureBox.SmallChange = New System.Drawing.Size(0, 0)
    Me.GraphPictureBox.TabIndex = 1
    Me.GraphPictureBox.TabStop = False
    Me.GraphPictureBox.VirtualBackColor = System.Drawing.Color.Black
    Me.GraphPictureBox.VirtualMode = MOBZPing.VirtualPictureBox.VirtualModeType.HorizontalOnly
    Me.GraphPictureBox.VirtualSize = New System.Drawing.Size(0, 0)
    '
    'PingForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(970, 545)
    Me.Controls.Add(Me.MainSplitContainer)
    Me.Controls.Add(Me.MainToolStrip)
    Me.Controls.Add(Me.MainStatusStrip)
    Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "PingForm"
    Me.Text = "MOBZPing"
    Me.MainStatusStrip.ResumeLayout(False)
    Me.MainStatusStrip.PerformLayout()
    Me.MainToolStrip.ResumeLayout(False)
    Me.MainToolStrip.PerformLayout()
    Me.MainSplitContainer.Panel1.ResumeLayout(False)
    Me.MainSplitContainer.Panel2.ResumeLayout(False)
    Me.MainSplitContainer.ResumeLayout(False)
    Me.NotifyContextMenu.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Private WithEvents PingTimer As System.Windows.Forms.Timer
  Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
  Friend WithEvents MainToolStrip As System.Windows.Forms.ToolStrip
  Friend WithEvents StartButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents MainSplitContainer As System.Windows.Forms.SplitContainer
  Friend WithEvents ResolutionComboBox As System.Windows.Forms.ToolStripComboBox
  Friend WithEvents StatusStatusLabel As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents StopButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents AddHostTextBox As System.Windows.Forms.ToolStripTextBox
  Friend WithEvents AddHostButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents LinkLabel As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents MainNotifyIcon As System.Windows.Forms.NotifyIcon
  Friend WithEvents NotifyContextMenu As System.Windows.Forms.ContextMenuStrip
  Friend WithEvents RestoreMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents ExitMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents GraphPictureBox As MOBZPing.VirtualPictureBox
  Friend WithEvents TimeComboBox As System.Windows.Forms.ToolStripComboBox
  Friend WithEvents BottomFillPanel As System.Windows.Forms.Panel
  Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents StartAllMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents StopAllMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator

End Class
