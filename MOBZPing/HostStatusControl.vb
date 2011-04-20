Public Class HostStatusControl
  ' The percentage and color of the indicator bar
  Private _Percentage As Integer
  Private _Color As Color

  Public Event DeleteClicked(ByVal Sender As HostStatusControl)
  Public Event StartClicked(ByVal Sender As HostStatusControl)
  Public Event StopClicked(ByVal Sender As HostStatusControl)

  Public Property HostName() As String
    Get
      Return Me.NameLabel.Text
    End Get
    Set(ByVal value As String)
      Me.NameLabel.Text = value
      Me.DeleteButton.ToolTipText = "Remove " + value
      Me.PauseButton.ToolTipText = "Pause " + value
    End Set
  End Property

  Public Property Percentage() As Integer
    Get
      Return Me._Percentage
    End Get
    Set(ByVal value As Integer)
      Me._Percentage = value
      UpdateStatus()
    End Set
  End Property

  Public Property Color() As Color
    Get
      Return Me._Color
    End Get
    Set(ByVal value As Color)
      Me._Color = value
      UpdateStatus()
    End Set
  End Property

  Public Sub SetPaused(ByVal Paused As Boolean)
    Me.PauseButton.Checked = Paused
  End Sub

  Private Sub UpdateStatus()
    Me.StatusPictureBox.Invalidate()
  End Sub

  Private Sub StatusPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles StatusPictureBox.Paint
    If Me._Percentage >= 100 Then
      e.Graphics.Clear(Me._Color)
    Else
      Dim W As Integer = CInt((Me.StatusPictureBox.ClientSize.Width / 100) * Me._Percentage)
      e.Graphics.FillRectangle(New SolidBrush(Me._Color), 0, 0, W, Me.StatusPictureBox.ClientSize.Height)
      e.Graphics.FillRectangle(Brushes.Black, W, 0, Me.StatusPictureBox.ClientSize.Width - W, Me.StatusPictureBox.ClientSize.Height)
    End If
  End Sub

  Private Sub HostStatusControl_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
    If Me.StatusPictureBox.Bottom > Me.ClientSize.Height Then
      Me.StatusPictureBox.Visible = False
    Else
      Me.StatusPictureBox.Visible = True
    End If
  End Sub

  Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
    RaiseEvent DeleteClicked(Me)
  End Sub

  Private Sub PauseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PauseButton.Click
    If PauseButton.Checked Then
      RaiseEvent StopClicked(Me)
    Else
      RaiseEvent StartClicked(Me)
    End If
  End Sub

  Private Sub HostStatusControl_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
    If e.Button = Windows.Forms.MouseButtons.Left Then
      DoDragDrop(Me, DragDropEffects.Move)
    End If
  End Sub
End Class
