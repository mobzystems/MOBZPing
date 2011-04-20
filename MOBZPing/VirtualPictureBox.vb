Public Class VirtualPictureBox
  ' The virtual mode values
  Public Enum VirtualModeType
    Both            ' Horizontal and vertical (default)
    HorizontalOnly  ' Horizontal only
    VerticalOnly    ' Vertical only
  End Enum

  ' The virtual size of this control
  Protected _VirtualSize As Size

  ' The current virtual mode
  Protected _VirtualMode As VirtualModeType = VirtualModeType.Both

  ' This event is raised when the picture box requires repainting
  Public Event VirtualPaint(ByVal Sender As VirtualPictureBox, ByVal e As PaintEventArgs, ByVal Offset As Point)
  Public Shadows Event MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

#Region "Constructors"
  ''' <summary>
  ''' Default constructor. Positions child controls.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub New()
    InitializeComponent()

    PositionScrollBars()
  End Sub
#End Region

#Region "Properties"
  ''' <summary>
  ''' Get the client size of the control, i.e. excluding the scroll bars
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property PictureSize() As Size
    Get
      Return Me.MainPictureBox.Size
    End Get
  End Property

  ''' <summary>
  ''' Get the picture box
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property PictureBox() As PictureBox
    Get
      Return Me.MainPictureBox
    End Get
  End Property

  ''' <summary>
  ''' Get/Set the virtual size of the control
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property VirtualSize() As Size
    Get
      Return Me._VirtualSize
    End Get
    Set(ByVal value As Size)
      Me._VirtualSize = value
      ' When the size is set, update the scroll bars
      UpdateScrollBars()
    End Set
  End Property

  ''' <summary>
  ''' The background color of the 'virtual' area of the control
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property VirtualBackColor() As Color
    Get
      Return Me.MainPictureBox.BackColor
    End Get
    Set(ByVal value As Color)
      Me.MainPictureBox.BackColor = value
    End Set
  End Property

  ''' <summary>
  ''' Get/Set the tab stop for ourselves and the scroll bars
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shadows Property TabStop() As Boolean
    Get
      Return MyBase.TabStop
    End Get
    Set(ByVal value As Boolean)
      MyBase.TabStop = value
      Me.HorizontalScrollBar.TabStop = value
      Me.VerticalScrollBar.TabStop = value
    End Set
  End Property

  ''' <summary>
  ''' Get/Set the virtual mode of this user control
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  <System.ComponentModel.DefaultValue(GetType(VirtualModeType), "Both")> _
  Public Property VirtualMode() As VirtualModeType
    Get
      Return Me._VirtualMode
    End Get
    Set(ByVal value As VirtualModeType)
      Me._VirtualMode = value
      PositionScrollBars()
      UpdateScrollBars()
    End Set
  End Property

  ''' <summary>
  ''' Get/Set the small change values of the scroll bars
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SmallChange() As Size
    Get
      Return New Size(Me.HorizontalScrollBar.SmallChange, Me.VerticalScrollBar.SmallChange)
    End Get
    Set(ByVal value As Size)
      Me.HorizontalScrollBar.SmallChange = value.Width
      Me.VerticalScrollBar.SmallChange = value.Height
    End Set
  End Property

  ''' <summary>
  ''' Own Cursor property. Goes for PictureBox only!
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Overloads Property Cursor() As Cursor
    Get
      Return Me.MainPictureBox.Cursor
    End Get
    Set(ByVal value As Cursor)
      Me.MainPictureBox.Cursor = Cursor
    End Set
  End Property

  ''' <summary>
  ''' Get the offset of the virtual picture box
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property Offset() As Point
    Get
      Return New Point(Me.HorizontalScrollBar.Value, Me.VerticalScrollBar.Value)
    End Get
  End Property
#End Region

#Region "Private Methods"
  ''' <summary>
  ''' Set the positions of the scroll bars and show/hide them if necessary
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub PositionScrollBars()
    Dim W As Integer = SystemInformation.VerticalScrollBarWidth
    Dim H As Integer = SystemInformation.HorizontalScrollBarHeight

    Select Case Me._VirtualMode
      Case VirtualModeType.Both
        ' Leave both sizes
      Case VirtualModeType.HorizontalOnly
        W = 0
      Case VirtualModeType.VerticalOnly
        H = 0
    End Select

    ' Put the scrollbars into place
    Me.HorizontalScrollBar.Size = New Size(Me.ClientSize.Width - W, H)
    Me.HorizontalScrollBar.Location = New Point(0, Me.ClientSize.Height - H)

    Me.VerticalScrollBar.Size = New Size(W, Me.ClientSize.Height - H)
    Me.VerticalScrollBar.Location = New Point(Me.ClientSize.Width - W, 0)

    ' Position the picture box
    Me.MainPictureBox.Location = New Point(0, 0)
    Me.MainPictureBox.Size = New Size(Me.ClientSize.Width - Me.VerticalScrollBar.Width, Me.ClientSize.Height - Me.HorizontalScrollBar.Height)
  End Sub

  ''' <summary>
  ''' Update the scroll bars. Enable/Disable them and set their sizes
  ''' </summary>
  ''' <remarks></remarks>
  Protected Overridable Sub UpdateScrollBars()
    UpdateScrollBar(Me.HorizontalScrollBar, Me._VirtualSize.Width, Me.MainPictureBox.ClientSize.Width, Me._VirtualMode = VirtualModeType.Both Or Me._VirtualMode = VirtualModeType.HorizontalOnly)
    UpdateScrollBar(Me.VerticalScrollBar, Me._VirtualSize.Height, Me.MainPictureBox.ClientSize.Height, Me._VirtualMode = VirtualModeType.Both Or Me._VirtualMode = VirtualModeType.VerticalOnly)

    ForceRepaint()
  End Sub

  ''' <summary>
  ''' Update an individual scroll bar
  ''' </summary>
  ''' <param name="ScrollBar"></param>
  ''' <param name="VirtualSize"></param>
  ''' <param name="ClientSize"></param>
  ''' <remarks></remarks>
  Protected Overridable Sub UpdateScrollBar(ByVal ScrollBar As ScrollBar, ByVal VirtualSize As Integer, ByVal ClientSize As Integer, ByVal IsVisible As Boolean)
    ScrollBar.Visible = IsVisible
    If ClientSize >= VirtualSize Then
      ' The virtual size is smaller than the actual size.
      ' Disable the scroll bar
      ScrollBar.Minimum = 0
      ScrollBar.Value = 0
      ScrollBar.Maximum = 0
      ScrollBar.LargeChange = 0
      ScrollBar.Enabled = False
    Else
      ScrollBar.Minimum = 0
      If Not (ScrollBar.Value >= ScrollBar.Minimum And ScrollBar.Value < VirtualSize) Then
        ScrollBar.Value = 0
      ElseIf ScrollBar.Value >= VirtualSize - ClientSize Then
        ScrollBar.Value = VirtualSize - ClientSize
      End If
      ScrollBar.Maximum = VirtualSize - 1
      ScrollBar.LargeChange = ClientSize
      ScrollBar.Enabled = True
    End If
  End Sub

  ''' <summary>
  ''' Force a repaint of the picture box
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub ForceRepaint()
    Me.MainPictureBox.Invalidate()
    ' Me.MainPictureBox.Update()
  End Sub
#End Region

#Region "Event handlers"
  ''' <summary>
  '''  Update the scroll bars when the size of the user control changes
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub VirtualScrollPanel_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
    UpdateScrollBars()
  End Sub

  ''' <summary>
  ''' Prepare the picture box for painting
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub MainPictureBox_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MainPictureBox.Paint
    RaiseEvent VirtualPaint(Me, e, New Point(Me.HorizontalScrollBar.Value, Me.VerticalScrollBar.Value))
  End Sub

  '''' <summary>
  '''' Repaint the picture box when the scrollbars scroll (not necessary, ValueChanged handles all)
  '''' </summary>
  '''' <param name="sender"></param>
  '''' <param name="e"></param>
  '''' <remarks></remarks>
  'Private Sub ScrollBar_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HorizontalScrollBar.Scroll, VerticalScrollBar.Scroll
  '  ' Me.MainPictureBox.Invalidate()
  'End Sub

  Private Sub ScrollBar_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HorizontalScrollBar.ValueChanged, VerticalScrollBar.ValueChanged
    ForceRepaint()
  End Sub

  ''' <summary>
  ''' Forward the MouseMove event, corrected for scroll positions
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub MainPictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPictureBox.MouseMove
    RaiseEvent MouseMove(sender, New MouseEventArgs(e.Button, e.Clicks, e.X + Me.HorizontalScrollBar.Value, e.Y + Me.VerticalScrollBar.Value, e.Delta))
  End Sub
#End Region
End Class
