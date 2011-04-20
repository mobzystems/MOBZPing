Option Explicit On
Option Strict On

Imports System.Collections.Generic
Imports System.Net.NetworkInformation
Imports System.IO
Imports System.Xml

Public Class PingForm

#Region "TimeSetting Class"
  ''' <summary>
  ''' A settings class for the TimeComboBox
  ''' </summary>
  ''' <remarks></remarks>
  Private Class TimeSetting
    Private _DisplayName As String
    Private _NumberOfSeconds As Integer

    Public Sub New(ByVal DisplayName As String, ByVal NumberOfSeconds As Integer)
      Me._DisplayName = DisplayName
      Me._NumberOfSeconds = NumberOfSeconds
    End Sub

    ReadOnly Property DisplayName() As String
      Get
        Return _DisplayName
      End Get
    End Property

    ReadOnly Property NumberOfSeconds() As Integer
      Get
        Return _NumberOfSeconds
      End Get
    End Property
  End Class
#End Region

#Region "PingResult Class"
  ''' <summary>
  ''' A result that can be displayed in a graph.
  ''' </summary>
  ''' <remarks></remarks>
  Private Class PingResult
    Public Const RESPONSE_ERROR As Integer = -1
    Public Const RESPONSE_NODATA As Integer = 0

    ' The starting time of the result
    Public TimeStamp As DateTime
    ' The average response time in this result. If there is no data for this interval,
    ' the average response time is RESPONSE_NODATA
    ' If there are one or more errors in the interval, the average response time
    ' is RESPONSE_ERROR
    Public AverageResponseTime As Long

    ' The status of the result. Either Success or TimeOut or another error code.
    Public Status As IPStatus

    Public Function IsSuccess() As Boolean
      Return Status = IPStatus.Success
    End Function

    Public Function IsTimeout() As Boolean
      Return Status = IPStatus.TimedOut
    End Function

    Public Function IsError() As Boolean
      Return Not IsSuccess() And Not IsTimeout()
    End Function
  End Class
#End Region

#Region "PingHost Class"
  ''' <summary>
  ''' A continuous ping to a host
  ''' </summary>
  ''' <remarks></remarks>
  Private Class PingHost
    ' The host name
    Private _HostName As String

    ' The background worker that sends the pings
    Private _PingThread As PingThread
    ' The results, harvested through PingReceived
    Private _Results As New List(Of PingResult)
    ' An internal lock object
    Private _ResultsLock As New Object

    ' The control used to display this host
    Private _StatusControl As HostStatusControl

    ' The last results painted of this host painted
    Private _LastResults() As PingResult

    Private _IsRunning As Boolean

    ''' <summary>
    ''' Constructor with a host name of the form 'www.google.com' or '192.168.1.1'
    ''' </summary>
    ''' <param name="HostName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal HostName As String)
      Me._HostName = HostName
      Me._PingThread = New PingThread(HostName, PINGTIMEOUT)
      AddHandler Me._PingThread.PingReceived, AddressOf Me.PingReceived
      AddHandler Me._PingThread.PingError, AddressOf Me.PingError
    End Sub

    ''' <summary>
    ''' The host name of this PingHost
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HostName() As String
      Get
        Return Me._HostName
      End Get
    End Property

    ''' <summary>
    ''' Get/Set the status control associated with this PingHost
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StatusControl() As HostStatusControl
      Get
        Return _StatusControl
      End Get
      Set(ByVal value As HostStatusControl)
        _StatusControl = value
      End Set
    End Property

    ''' <summary>
    ''' The last set of results that was obtained
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastResults() As PingResult()
      Get
        Return Me._LastResults
      End Get
    End Property

    ''' <summary>
    ''' Is the PingHost background thread running?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IsRunning() As Boolean
      Get
        Return Me._IsRunning
      End Get
    End Property

    ''' <summary>
    ''' Start the ping thread
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Start()
      Me._PingThread.Start()
      Me._IsRunning = True
    End Sub

    ''' <summary>
    ''' Abort the ping thread
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Abort()
      Me._PingThread.Abort()
      Me._IsRunning = False
    End Sub

    ''' <summary>
    ''' A ping reply was received. Store it in the results list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="Reply"></param>
    ''' <remarks></remarks>
    Private Sub PingReceived(ByVal sender As PingThread, ByVal SendDateTime As DateTime, ByVal Reply As PingReply)
      ' InvokeRequired is True here, so don't update any UI!

      SyncLock _ResultsLock
        Dim R As New PingResult

        ' Make a result. Set the status
        R.Status = Reply.Status

        ' Set the average response time IF IT IS VALID, else set it to RESPONSE_ERROR
        If Reply.Status = IPStatus.Success Then
          R.AverageResponseTime = Reply.RoundtripTime
          If R.AverageResponseTime = 0 Then
            R.AverageResponseTime = 1
          End If
        Else
          R.AverageResponseTime = PingResult.RESPONSE_ERROR
        End If

        ' Add the time stamp to the result
        R.TimeStamp = SendDateTime

        ' Add the result to the results list
        _Results.Add(R)

        ' TODO: clean up the last xxx hours of results
        'If _Results.Count > MAXSAMPLES Then
        '  _Results.RemoveRange(0, _Results.Count - MAXSAMPLES)
        'End If
      End SyncLock
    End Sub

    ''' <summary>
    ''' Handle ping errors here
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="SendDateTime"></param>
    ''' <param name="Exception"></param>
    ''' <remarks></remarks>
    Private Sub PingError(ByVal sender As PingThread, ByVal SendDateTime As DateTime, ByVal Exception As Exception)
      ' InvokeRequired is True here, so don't update any UI!

      SyncLock _ResultsLock
        Dim R As New PingResult

        ' Make a result. Set the status
        R.Status = IPStatus.Unknown ' TODO: doe hier iets anders
        ' Indicate an error in the response time
        R.AverageResponseTime = PingResult.RESPONSE_ERROR

        ' Add the time stamp to the result
        R.TimeStamp = SendDateTime

        ' Add the result to the results list
        _Results.Add(R)

        '' Clean up results TODO: based on time stamps!
        'If _Results.Count > MAXSAMPLES Then
        '  _Results.RemoveRange(0, _Results.Count - MAXSAMPLES)
        'End If
      End SyncLock
    End Sub

    ''' <summary>
    ''' Get an array averaged results between the specified date/times, using the specified interval.
    ''' If there was a timeout in an interval, the entire interval is set to timeout
    ''' </summary>
    ''' <param name="FromDateTime">The first date/time</param>
    ''' <param name="ToDateTime">The last date/time</param>
    ''' <param name="IntervalInSeconds">The interval between bins in seconds</param>
    ''' <returns>An array of PingResults</returns>
    ''' <remarks></remarks>
    Public Function RetrieveResults(ByVal FromDateTime As DateTime, ByVal ToDateTime As DateTime, ByVal IntervalInSeconds As Integer) As PingResult()
      ' If there are no results yet: exit
      If Me._Results.Count = 0 Then
        ReDim Me._LastResults(-1) ' 0 elements!
        Return Me._LastResults
      End If

      ' Round down FromDateTime with respect to IntervalInSeconds,
      ' to get clean date/time values for the bins
      Dim TotalSeconds As Integer = CInt(FromDateTime.TimeOfDay.TotalSeconds)
      ' Calculate the number of 'extra' seconds
      Dim SubtractSeconds As Integer = TotalSeconds Mod IntervalInSeconds
      ' Create a new clean start date/time
      FromDateTime = FromDateTime.Date.AddSeconds(TotalSeconds - SubtractSeconds)

      ' Figure out how many 'bins' we're going to need
      Dim Bins As Integer = CInt(Int(ToDateTime.Subtract(FromDateTime).TotalSeconds / IntervalInSeconds)) + 1
      ' Declare this number of bins
      Dim Data(Bins - 1) As PingResult
      ' Also keep a tab on the number of items in each bin
      Dim SampleCount(Bins - 1) As Integer

      For I As Integer = 0 To Bins - 1
        Data(I) = New PingResult
        Data(I).AverageResponseTime = PingResult.RESPONSE_NODATA
        Data(I).TimeStamp = FromDateTime.AddSeconds(I * IntervalInSeconds)
        SampleCount(I) = 0 ' No samples
      Next

      ' Store all available results into the Data array
      SyncLock Me._ResultsLock
        For Each Result As PingResult In Me._Results
          ' Calculate the bin to use
          Dim CurrentBin As Integer = CInt(Int(Result.TimeStamp.Subtract(FromDateTime).TotalSeconds / IntervalInSeconds))

          If CurrentBin >= 0 And CurrentBin < Bins Then
            ' Was this a successful Ping?
            If Result.IsSuccess() Then
              ' Update the total response time
              Data(CurrentBin).AverageResponseTime += Result.AverageResponseTime
              ' Increment the sample count
              SampleCount(CurrentBin) += 1
            Else
              ' The sample is an error! Do NOT count it,
              ' but upgrade the error status of the bin:
              If Data(CurrentBin).IsSuccess() Then
                ' Always overwrite a Success status
                Data(CurrentBin).Status = Result.Status
              ElseIf Data(CurrentBin).IsTimeout() Then
                ' Overwrite timeouts with errors
                If Result.IsError() Then
                  Data(CurrentBin).Status = Result.Status
                End If
              ElseIf Data(CurrentBin).IsError() Then
                ' Save the last status
                Data(CurrentBin).Status = Result.Status
              End If
            End If
          End If
        Next
      End SyncLock

      ' Now average the response times:
      For I As Integer = 0 To Bins - 1
        If SampleCount(I) > 0 Then
          Data(I).AverageResponseTime = CLng(Data(I).AverageResponseTime / SampleCount(I))
        End If
      Next

      Me._LastResults = Data

      Return Data
    End Function
  End Class
#End Region

#Region "Data members"
  ' Delay between successful pings
  Private Const PINGTIMEOUT As Integer = 1000

  ' The width of a single bar (including margin) in pixels
  Private Const BARWIDTH As Integer = 3

  ' The current number of seconds per bar in the chart
  Private _CurrentSecondsPerBar As Integer = -1
  ' The number of seconds to display in total
  Private _CurrentTimeScale As Integer = 0

  ' The current height of a graph and status control
  Private _CurrentGraphHeight As Integer = 0

  ' The date/time pinging was started
  Private _StartDateTime As DateTime = DateTime.MinValue

  ''' <summary>
  ''' The list of ping threads
  ''' </summary>
  ''' <remarks></remarks>
  Private _PingHosts As New List(Of PingHost)

  ''' <summary>
  ''' The last window state before minimizing
  ''' </summary>
  ''' <remarks></remarks>
  Private _LastWindowState As FormWindowState

  ' The current status
  Private _LastStatus As String = ""

  ' The list of time settings associated with the combo box
  Private _TimeSettings As New List(Of TimeSetting)
#End Region

#Region "Private methods"
  ''' <summary>
  ''' Attempt to load a list of hosts from a file
  ''' </summary>
  ''' <param name="XmlFileName"></param>
  ''' <returns>True if file loaded, else False</returns>
  ''' <remarks></remarks>
  Private Function AttemptLoadHosts(ByVal XmlFileName As String) As Boolean
    If File.Exists(XmlFileName) Then
      Try
        ' Load the file into an XML Document
        Dim XmlDoc As New XmlDocument()
        XmlDoc.Load(XmlFileName)

        ' TODO: read settings here
        ' TODO: validate the document here

        Me._PingHosts = New List(Of PingHost)

        Dim Hosts As XmlNodeList = XmlDoc.DocumentElement.SelectNodes("/mobzping/hosts/host")
        For Each Host As XmlNode In Hosts
          Dim Paused As Boolean = False
          If Host.Attributes("active") IsNot Nothing AndAlso Host.Attributes("active").Value = "false" Then
            Paused = True
          End If
          AddHost(Host.Attributes("name").Value, Paused)
        Next

        Return True
      Catch Ex As Exception
        MessageBox.Show("There was an error reading the file '" + XmlFileName + "': " + Ex.Message, "MOBZPing", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return False
      End Try
    Else
      Return False
    End If
  End Function

  ''' <summary>
  ''' Add a host to the collection. Create an associated status control
  ''' </summary>
  ''' <param name="HostName"></param>
  ''' <remarks></remarks>
  Private Sub AddHost(ByVal HostName As String, ByVal Paused As Boolean)
    Dim PH As New PingHost(HostName)

    Dim HostControl As New HostStatusControl
    HostControl.Size = New Size(10, 10)
    HostControl.Dock = DockStyle.Top
    HostControl.HostName = HostName
    HostControl.Color = Color.DarkGray
    HostControl.Percentage = 100
    HostControl.SetPaused(Paused)

    AddHandler HostControl.DeleteClicked, AddressOf Me.HostDeleted
    AddHandler HostControl.StartClicked, AddressOf Me.HostStarted
    AddHandler HostControl.StopClicked, AddressOf Me.HostStopped

    Me.MainSplitContainer.Panel1.Controls.Add(HostControl)
    HostControl.BringToFront()

    Me._PingHosts.Add(PH)

    PH.StatusControl = HostControl

    If Not Paused Then
      PH.Start()

      If Me._StartDateTime = DateTime.MinValue Then
        Me._StartDateTime = DateTime.Now
      End If
    End If
  End Sub

  ''' <summary>
  ''' Get the PingHost whose StatusControl is the specified HostStatusControl
  ''' </summary>
  ''' <param name="HostControl"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function PingHostFromControl(ByVal HostControl As HostStatusControl) As PingHost
    For Each P As PingHost In Me._PingHosts
      If P.StatusControl Is HostControl Then
        Return P
      End If
    Next

    Return Nothing
  End Function

  ''' <summary>
  ''' Enable/Disable the start/stop buttons
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub UpdateStartStopButtons()
    Dim RunningCount As Integer = 0
    Dim StoppedCount As Integer = 0

    For Each PH As PingHost In Me._PingHosts
      If PH.IsRunning Then
        RunningCount += 1
      Else
        StoppedCount += 1
      End If
    Next

    Me.StartButton.Enabled = StoppedCount > 0
    Me.StopButton.Enabled = RunningCount > 0

    Me.StartAllMenuItem.Enabled = Me.StartButton.Enabled
    Me.StopAllMenuItem.Enabled = Me.StopButton.Enabled
  End Sub

  ''' <summary>
  ''' Get the currently selected number of seconds per bar
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function GetSecondsPerBar() As Integer
    Return New Integer() {1, 10, 30, 60, 600, 1800, 3600}(Me.ResolutionComboBox.SelectedIndex)
  End Function

  ''' <summary>
  ''' Get the selected number of seconds
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function GetTimeScale() As Integer
    Return Me._TimeSettings(Me.TimeComboBox.SelectedIndex).NumberOfSeconds
  End Function

  ''' <summary>
  ''' Update the size of the graph picture box so the scrollbar is shown correctly
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub UpdateGraphSize()
    Const MINHOSTHEIGHT As Integer = 60

    If Me._PingHosts.Count = 0 OrElse Me.GraphPictureBox.PictureSize.Height = 0 Then
      ' Height may be 0 when coming out of minimized state!
      Me.GraphPictureBox.Size = Me.MainSplitContainer.Panel2.ClientSize
    Else
      ' Determine if our list of hosts fits inside the current *real* size of the picture box
      Dim H As Integer = CInt(Int(Me.GraphPictureBox.PictureSize.Height / Me._PingHosts.Count))
      Dim Overflow As Boolean = False

      If H < MINHOSTHEIGHT Then
        H = MINHOSTHEIGHT
        ' We don't fit!
        Overflow = True
      End If

      ' The number of visible items
      Dim VisibleCount As Integer = CInt(Int(Me.GraphPictureBox.PictureSize.Height / H))

      ' Determine how many pixels wide the picture box should be
      Dim W As Integer
      Select Case Me._CurrentTimeScale
        Case 0 ' All samples
          ' Calculate total number of seconds in all samples, divide by SecondsPerBar,
          ' multiply by BARWIDTH
          W = (CInt(Int(DateTime.Now.Subtract(Me._StartDateTime).TotalSeconds / Me._CurrentSecondsPerBar)) + 1) * BARWIDTH
        Case -1 ' Auto fit
          W = Me.GraphPictureBox.PictureSize.Width
        Case Else
          W = (CInt(Me._CurrentTimeScale / Me._CurrentSecondsPerBar) + 1) * BARWIDTH
      End Select

      If Overflow Then
        Me.MainSplitContainer.Panel1.VerticalScroll.LargeChange = VisibleCount * H
        Me.MainSplitContainer.Panel1.VerticalScroll.SmallChange = H
      End If

      ' TODO: try to keep the current scroll position!

      ' Then set the *virtual* size of the picture box
      Me.GraphPictureBox.VirtualSize = New Size(W, Me.GraphPictureBox.PictureSize.Height)

      ' Re-layout the status controls
      Me.MainSplitContainer.Panel1.SuspendLayout()
      For I As Integer = 0 To Me._PingHosts.Count - 1
        If Me._PingHosts(I).StatusControl IsNot Nothing Then
          Me._PingHosts(I).StatusControl.Height = H
        End If
      Next
      Me.MainSplitContainer.Panel1.ResumeLayout()

      ' Save the current graph height for later
      Me._CurrentGraphHeight = H

      InvalidateGraph()
    End If
  End Sub

  ''' <summary>
  ''' Invalidate the graph, causing it to get repainted
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub InvalidateGraph()
    Me.MainSplitContainer.Panel2.Invalidate(True)
  End Sub

  ''' <summary>
  ''' Update the status of the hosts. Sets a tooltip on the notify icon
  ''' and show a balloon tip of there was a change in the status
  ''' </summary>
  ''' <param name="Status"></param>
  ''' <param name="TipIcon"></param>
  ''' <remarks></remarks>
  Private Sub UpdateStatus(ByVal Status As String, ByVal TipIcon As ToolTipIcon)
    If Me._LastStatus <> Status Then
      Me._LastStatus = Status

      Const MAXICONTEXTLENGTH As Integer = 64

      ' Make sure the status if is not too long
      Dim ShortStatus As String = "MOBZPing status:" + Environment.NewLine + Status
      If ShortStatus.Length > MAXICONTEXTLENGTH Then
        ShortStatus = Status
        If ShortStatus.Length > MAXICONTEXTLENGTH Then
          ShortStatus = ShortStatus.Substring(0, MAXICONTEXTLENGTH)
        End If
      End If

      Me.MainNotifyIcon.Text = ShortStatus

      ' TODO: update the status in the caption of the form, too.
      ' This does not work well when the form is completely obscured by other windows,
      ' since status updates rely on the paint mechanism - which will not fire!
      ' Me.Text = Status + " - MOBZPing"

      Select Case TipIcon
        Case ToolTipIcon.Info
          Me.MainNotifyIcon.Icon = System.Drawing.Icon.FromHandle(My.Resources.Flag_greenHS.GetHicon())
        Case ToolTipIcon.Warning
          Me.MainNotifyIcon.Icon = System.Drawing.Icon.FromHandle(My.Resources.Flag_blueHS.GetHicon())
        Case ToolTipIcon.Error
          Me.MainNotifyIcon.Icon = System.Drawing.Icon.FromHandle(My.Resources.Flag_redHS.GetHicon())
        Case ToolTipIcon.None
          Me.MainNotifyIcon.Icon = My.Resources.App
      End Select

      If Me.MainNotifyIcon.Visible Then
        Me.MainNotifyIcon.ShowBalloonTip(1000, "MOBZPing", Status, TipIcon)
      End If

    End If
  End Sub

  ''' <summary>
  ''' Restore the window state to what it was
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub RestoreWindow()
    Me.Visible = True
    Me.WindowState = Me._LastWindowState

    Dim RestoreBounds As Rectangle = Me.RestoreBounds()

    Me.Location = RestoreBounds.Location
    Me.Size = RestoreBounds.Size

    Me.MainNotifyIcon.Visible = False
  End Sub

  ''' <summary>
  ''' This Sub is called by MyApplication when a new instance is started
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub RestoreWindowIfMinimized()
    If Me.MainNotifyIcon.Visible Then
      RestoreWindow()
    End If
  End Sub
#End Region

#Region "PingHost Event Handlers"
  ''' <summary>
  ''' Catch the HostDeleted event from some HostStatusControl
  ''' </summary>
  ''' <param name="HostControl"></param>
  ''' <remarks></remarks>
  Private Sub HostDeleted(ByVal HostControl As HostStatusControl)
    Dim PH As PingHost = PingHostFromControl(HostControl)
    If PH.IsRunning() Then
      PH.Abort()
    End If
    PH.StatusControl.Parent.Controls.Remove(PH.StatusControl)
    Me._PingHosts.Remove(PH)
    UpdateGraphSize()
  End Sub

  ''' <summary>
  ''' Catch the HostStarted event from some HostStatusControl
  ''' </summary>
  ''' <param name="HostControl"></param>
  ''' <remarks></remarks>
  Private Sub HostStarted(ByVal HostControl As HostStatusControl)
    Dim PH As PingHost = PingHostFromControl(HostControl)

    If Not PH.IsRunning() Then
      PH.Start()
    End If

    If Me._StartDateTime = DateTime.MinValue Then
      Me._StartDateTime = DateTime.Now
    End If

    UpdateStartStopButtons()
  End Sub

  ''' <summary>
  ''' Catch the HostStopped event from some HostStatusControl
  ''' </summary>
  ''' <param name="HostControl"></param>
  ''' <remarks></remarks>
  Private Sub HostStopped(ByVal HostControl As HostStatusControl)
    Dim PH As PingHost = PingHostFromControl(HostControl)

    If PH.IsRunning() Then
      PH.Abort()
    End If

    PH.StatusControl.Color = Color.DarkGray
    PH.StatusControl.Percentage = 100

    UpdateStartStopButtons()
  End Sub
#End Region

#Region "Form Event Handlers"
  ''' <summary>
  ''' The timer that drives the graphical display fired
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub PingTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PingTimer.Tick
    ' We need to repaint our graph
    If Me.MainNotifyIcon.Visible Then
      ' Call the update routine with a Nothing event argument. This will NOT paint
      ' TODO: solve this in less really, really ugly way!
      GraphPictureBox_VirtualPaint(GraphPictureBox, Nothing, New Point(0, 0))
    Else
      If Me._CurrentTimeScale = 0 Then ' All
        ' Dim D As DateTime = DateTimeFromPosition(Me.GraphPictureBox.Offset.X)
        UpdateGraphSize()
        ' Me.GraphPictureBox.
      End If
      InvalidateGraph()
    End If
  End Sub

  ''' <summary>
  ''' This event handler gets called when the hosts panel scrolls
  ''' </summary>
  ''' <param name="Sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub HostsScrolled(ByVal Sender As Object, ByVal e As ScrollEventArgs)
    InvalidateGraph()
  End Sub

  ''' <summary>
  ''' Paint the graphs
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub GraphPictureBox_VirtualPaint(ByVal sender As VirtualPictureBox, ByVal e As System.Windows.Forms.PaintEventArgs, ByVal Offset As Point) Handles GraphPictureBox.VirtualPaint
    ' Nothing to display yet?
    If Me._PingHosts.Count = 0 Then
      UpdateStatus("No data available.", ToolTipIcon.None)
      Exit Sub
    End If

    If e IsNot Nothing Then
      e.Graphics.TranslateTransform(-Offset.X, -Offset.Y + Me.MainSplitContainer.Panel1.AutoScrollPosition.Y)
    End If

    ' The number of hosts
    Dim HostCount As Integer = Me._PingHosts.Count
    Dim OffLineCount As Integer = 0
    Dim OnLineCount As Integer = 0
    Dim IgnoreCount As Integer = 0
    Dim LastOffLineHostName As String = ""

    For HostIndex As Integer = 0 To Me._PingHosts.Count - 1

      Const MARGIN As Integer = 3

      Dim MaxHeight As Integer = Me._CurrentGraphHeight - 2 * MARGIN
      Dim TopY As Integer = CInt((MaxHeight + 2 * MARGIN) * HostIndex) + MARGIN

      ' Get the results for the selected ping host

      ' Set the date/time of the first sample to read
      Dim FirstDateTime As DateTime

      If Me._CurrentTimeScale = 0 Then ' Show all (0)
        FirstDateTime = Me._StartDateTime
      ElseIf Me._CurrentTimeScale = -1 Then ' Fit to window (-1)
        ' Calculate how many bars will fit the width of the picture box
        ' Multiply by SecondsPerBar, subtract from Now
        FirstDateTime = DateTime.Now.Subtract(New TimeSpan(0, 0, (CInt(Int(Me.GraphPictureBox.PictureSize.Width / BARWIDTH)) - 1) * Me._CurrentSecondsPerBar))
      Else
        ' Show all samples
        FirstDateTime = DateTime.Now.Subtract(New TimeSpan(0, 0, Me._CurrentTimeScale))
      End If
      If FirstDateTime < Me._StartDateTime Then
        FirstDateTime = Me._StartDateTime
      End If

      Dim Results() As PingResult = Me._PingHosts(HostIndex).RetrieveResults(FirstDateTime, DateTime.Now, Me._CurrentSecondsPerBar)

      Dim LastColor As Drawing.Color = Color.Black
      Dim LastPercentage As Integer = 100

      ' If we have data...
      If Results.Length > 0 Then
        '' Determine the maximum round trip time:
        'Dim MaxRoundTripTime As Integer = -1
        'For Each R As PingResult In Results
        '  If R.AverageResponseTime <> PingResult.RESPONSE_ERROR AndAlso R.AverageResponseTime > MaxRoundTripTime Then
        '    MaxRoundTripTime = R.AverageResponseTime
        '  End If
        'Next

        '' Determine the height of a single ms in pixels.
        '' The maximum round trip time is full height
        Dim MilliSecondHeight As Double
        '' We only had timeouts or 0 ms (impossible?)
        'If MaxRoundTripTime <= 0 Then
        '  MilliSecondHeight = 1
        'Else
        '  MilliSecondHeight = CInt(Me.GraphPictureBox.Height / MaxRoundTripTime)
        'End If

        ' Set 100 ms for the whole graph
        MilliSecondHeight = MaxHeight / 100

        ' Plot the rectangles
        For N As Integer = 0 To Results.Length - 1
          Dim R As PingResult = Results(N)

          ' The height of the bar
          Dim H As Integer
          ' The color of the bar
          Dim B As Brush

          If R.IsError() Then
            H = 1
            B = Brushes.Red
            LastColor = Color.Red
            LastPercentage = 100
          ElseIf R.IsTimeout() Then
            ' Timeout - full size red bar
            H = MaxHeight
            B = Brushes.Red
            LastColor = Color.Red
            LastPercentage = 100
          ElseIf R.AverageResponseTime = PingResult.RESPONSE_NODATA Then ' No data
            H = 1
            B = Brushes.Yellow
            ' Do NOT save the last color or percentage - we need the last status
            ' LastColor = Color.Transparent
          Else
            H = CInt(R.AverageResponseTime * MilliSecondHeight)
            ' Always display something - height may not be 0 (or less)
            If H < 1 Then
              H = 1
            ElseIf H > MaxHeight Then
              H = MaxHeight
            End If
            B = Brushes.Green
            LastColor = Color.Green
            LastPercentage = CInt(H * 100 / MaxHeight)
          End If

          ' Skip drawing when we're outside the clip rectangle
          If e IsNot Nothing AndAlso Not ((N + 1) * BARWIDTH < e.ClipRectangle.Left + Offset.X OrElse N * BARWIDTH >= e.ClipRectangle.Right + Offset.X) Then
            ' Draw the bar
            e.Graphics.FillRectangle(B, N * BARWIDTH, TopY + MaxHeight - H, BARWIDTH - 1, H)
          End If
        Next

        If e IsNot Nothing Then
          ' Draw a cyan colored line right after the last sample
          e.Graphics.DrawLine(Pens.Cyan, Results.Length * BARWIDTH - 1, TopY, Results.Length * BARWIDTH - 1, TopY + MaxHeight - 1)
        End If

        If Me._PingHosts(HostIndex).IsRunning Then
          Dim HostControl As HostStatusControl = Me._PingHosts(HostIndex).StatusControl
          HostControl.Color = LastColor
          HostControl.Percentage = LastPercentage

          ' Update the status of this host
          If LastColor = Color.Green Then
            OnLineCount += 1
          ElseIf LastColor = Color.Yellow Then
            IgnoreCount += 1
          Else
            OffLineCount += 1
            LastOffLineHostName = Me._PingHosts(HostIndex).HostName
          End If
        Else
          ' Not running? Ignore
          IgnoreCount += 1
        End If

      Else
        ' No data
        IgnoreCount += 1
      End If
    Next

    If OffLineCount = 0 Then
      ' No hosts are off line.
      If IgnoreCount = 0 Then
        ' None paused, either? Then everyone is on line!
        UpdateStatus("All hosts are on line", ToolTipIcon.Info)
      ElseIf OnLineCount = 0 Then
        ' None on line, none off line: everyine paused
        UpdateStatus("All hosts are paused", ToolTipIcon.Warning)
      Else
        ' A mix of on line and paused
        UpdateStatus(String.Format("{0} hosts on line, {1} paused", OnLineCount, IgnoreCount), ToolTipIcon.Warning)
      End If
    Else
      ' We have off line hosts
      If OnLineCount = 0 Then
        ' None on line? 
        If IgnoreCount = 0 Then
          ' None paused? Everyone is off line
          UpdateStatus("All hosts are off line", ToolTipIcon.Error)
        Else
          If OffLineCount = 1 Then
            ' A mix of off line and paused
            UpdateStatus(String.Format("Host '{0}' is off line, {1} paused", LastOffLineHostName, IgnoreCount), ToolTipIcon.Error)
          Else
            ' A mix of off line and paused
            UpdateStatus(String.Format("{0} hosts off line, {1} paused", OffLineCount, IgnoreCount), ToolTipIcon.Error)
          End If
        End If
      Else
        ' We have some online, some offline
        If IgnoreCount = 0 Then
          ' Only offline and online
          If OffLineCount = 1 Then
            UpdateStatus("Host '" + LastOffLineHostName + "' is off line", ToolTipIcon.Error)
          Else
            UpdateStatus(String.Format("{0} of {1} hosts off line", OffLineCount, OffLineCount + OnLineCount), ToolTipIcon.Error)
          End If
        Else
          ' Offline and online and paused:
          If OffLineCount = 1 Then
            UpdateStatus(String.Format("Host '{0}' off line, {1} paused", LastOffLineHostName, IgnoreCount), ToolTipIcon.Error)
          Else
            UpdateStatus(String.Format("{0} host(s) off line, {1} paused", OffLineCount, IgnoreCount), ToolTipIcon.Error)
          End If
        End If
      End If
    End If
  End Sub

  ''' <summary>
  ''' The form is closed. Write out the list of hosts
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub PingForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    ' Save the lst of hosts for the current user:

    ' Ensure application data folder exists
    Dim AppPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    If Not Directory.Exists(AppPath) Then
      Directory.CreateDirectory(AppPath)
    End If

    Dim PathName As String = Path.Combine(AppPath, "MOBZystems")
    If Not Directory.Exists(PathName) Then
      Directory.CreateDirectory(PathName)
    End If

    ' Set up a file name
    Dim FileName As String = Path.Combine(PathName, "MOBZPing.xml")

    ' Create a writer
    Using W As New XmlTextWriter(FileName, System.Text.Encoding.UTF8)
      W.Formatting = Formatting.Indented

      W.WriteStartDocument()

      W.WriteStartElement("mobzping")
      W.WriteAttributeString("version", "1.0")

      W.WriteStartElement("hosts")

      For Each PH As PingHost In Me._PingHosts
        W.WriteStartElement("host")
        W.WriteAttributeString("name", PH.HostName)
        If PH.IsRunning = False Then
          W.WriteAttributeString("active", "false")
        End If
        W.WriteEndElement() ' host
      Next

      W.WriteEndElement() ' hosts

      W.WriteEndElement() ' mobzping
      W.WriteEndDocument()

      W.Close()
    End Using
  End Sub

  ''' <summary>
  ''' Update the graph size on change of size of the form or the container of the graph
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub MainSplitContainer_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainSplitContainer.Panel2.SizeChanged, MyBase.SizeChanged
    UpdateGraphSize()
  End Sub

  ''' <summary>
  ''' Adapt to the newly selected resolution
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub ResolutionComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResolutionComboBox.SelectedIndexChanged
    Me._CurrentSecondsPerBar = GetSecondsPerBar()
    UpdateGraphSize()
  End Sub

  ''' <summary>
  ''' Display status information about the sample under the mouse
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub GraphPictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GraphPictureBox.MouseMove
    If Me._CurrentGraphHeight = 0 Then
      Exit Sub
    End If

    ' Calculate host number
    Dim HostIndex As Integer = CInt(Int((e.Y - Me.MainSplitContainer.Panel1.AutoScrollPosition.Y) / Me._CurrentGraphHeight))

    If HostIndex >= 0 AndAlso HostIndex < Me._PingHosts.Count AndAlso Me._PingHosts(HostIndex).LastResults IsNot Nothing Then
      ' Calculate a bin number
      Dim Bin As Integer = CInt(e.X / BARWIDTH)
      If Bin >= 0 AndAlso Bin < Me._PingHosts(HostIndex).LastResults.Length Then
        Dim R As PingResult = Me._PingHosts(HostIndex).LastResults(Bin)

        Dim S As New System.Text.StringBuilder

        S.AppendFormat("{0} - {1}: ", Me._PingHosts(HostIndex).HostName, R.TimeStamp.ToString())

        If R.AverageResponseTime = PingResult.RESPONSE_ERROR Then
          S.Append("Timed out")
        ElseIf R.AverageResponseTime = 0 Then
          S.Append("No data")
        Else
          S.Append(R.AverageResponseTime.ToString() + " ms")
        End If
        Me.StatusStatusLabel.Text = S.ToString()
      Else
        Me.StatusStatusLabel.Text = ""
      End If
    End If
  End Sub

  ''' <summary>
  ''' Enable the AddHostButton if we have a non-empty host name
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub AddHostTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddHostTextBox.TextChanged
    Me.AddHostButton.Enabled = Me.AddHostTextBox.Text.Trim().Length > 0
  End Sub

  ''' <summary>
  ''' Add a host to the list and on screen
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub AddHostButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddHostButton.Click
    AddHost(Me.AddHostTextBox.Text, False)
    UpdateGraphSize()
  End Sub

  ''' <summary>
  ''' Perform a click of the AddHostButton when Enter is pressed inside the AddHostTextBox
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub AddHostTextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles AddHostTextBox.KeyPress
    If AscW(e.KeyChar) = 13 Then
      If Me.AddHostButton.Enabled Then
        Me.AddHostButton.PerformClick()
        Me.AddHostTextBox.SelectAll()
        e.Handled = True
      End If
    End If
  End Sub

  ''' <summary>
  ''' Open the home page for this tool when the link is clicked
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub LinkLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkLabel.Click
    Process.Start("http://www.mobzystems.com/Tools/MOBZPing.aspx")
  End Sub

  ''' <summary>
  ''' Restore the main window if the notify icon is doubleclicked or the balloon tip is clicked
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub MainNotifyIcon_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainNotifyIcon.DoubleClick, MainNotifyIcon.BalloonTipClicked
    RestoreWindow()
  End Sub

  ''' <summary>
  ''' Start all hosts that are paused now
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click, StartAllMenuItem.Click
    Me.StartButton.Enabled = False

    For Each P As PingHost In Me._PingHosts
      If Not P.IsRunning Then
        P.Start()
        P.StatusControl.SetPaused(False)

        If Me._StartDateTime = DateTime.MinValue Then
          Me._StartDateTime = DateTime.Now
        End If

      End If
    Next

    Me.StopButton.Enabled = True

    Me.StatusStatusLabel.Text = "Pinging..."
  End Sub

  ''' <summary>
  ''' Stop all hosts that are running now
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub StopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopButton.Click, StopAllMenuItem.Click
    Me.StopButton.Enabled = False

    For Each P As PingHost In Me._PingHosts
      If P.IsRunning Then
        P.Abort()
        P.StatusControl.Color = Color.DarkGray
        P.StatusControl.Percentage = 100
        P.StatusControl.SetPaused(True)
      End If
    Next

    Me.StartButton.Enabled = True

    Me.StatusStatusLabel.Text = "Stopped."
  End Sub

  ''' <summary>
  ''' The user clicked the 'Exit MOBZPing' menu item on the notify icon
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenuItem.Click
    ' RestoreWindow()
    Close()
  End Sub

  ''' <summary>
  ''' The user clicked the 'Open MOBZPing' context menu item on the notify icon
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub RestoreMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreMenuItem.Click
    RestoreWindow()
  End Sub

  ''' <summary>
  ''' Update the size of the graph when the time scale changes
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub TimeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeComboBox.SelectedIndexChanged
    Me._CurrentTimeScale = GetTimeScale()
    UpdateGraphSize()
  End Sub
#End Region

#Region "Overrides"
  ''' <summary>
  ''' Initialization of the form
  ''' </summary>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
    MyBase.OnLoad(e)

    Dim v As New Version(Application.ProductVersion)
    Me.LinkLabel.Text = Me.LinkLabel.Text.Replace("#", v.Major.ToString() + "." + v.Minor.ToString() + "." + v.Build.ToString())

    Me._TimeSettings.Add(New TimeSetting("(auto)", -1))
    ' Me._TimeSettings.Add(New TimeSetting("10 seconds", 10))
    ' Me._TimeSettings.Add(New TimeSetting("minute", 60))
    Me._TimeSettings.Add(New TimeSetting("15 minutes", 15 * 60))
    Me._TimeSettings.Add(New TimeSetting("hour", 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("2 hours", 2 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("3 hours", 3 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("6 hours", 6 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("12 hours", 12 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("day", 24 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("3 days", 3 * 24 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("week", 7 * 24 * 60 * 60))
    Me._TimeSettings.Add(New TimeSetting("(all)", 0))

    For N As Integer = 0 To Me._TimeSettings.Count - 1
      Me.TimeComboBox.Items.Add(Me._TimeSettings(N).DisplayName)
    Next

    ' Move the graph to position (0, 0)
    Me.GraphPictureBox.Location = New Point(0, 0)

    ' Set the resolution to 1 second
    Me.ResolutionComboBox.SelectedIndex = 0
    ' Set the display time to 'fit window'
    Me.TimeComboBox.SelectedIndex = 0

    ' Get the number of seconds per bar
    Me._CurrentSecondsPerBar = GetSecondsPerBar()
    Me._CurrentTimeScale = GetTimeScale()

    ' Set the size of the bottom filler panel below the host list
    Me.BottomFillPanel.Height = SystemInformation.HorizontalScrollBarHeight

    AddHandler Me.MainSplitContainer.Panel1.Scroll, AddressOf HostsScrolled

    ' Add a mouse move event handler
    ' AddHandler GraphPictureBox.PictureBox.MouseMove, AddressOf GraphPictureBox_MouseMove

    ' Find the list of hosts to load and launch:
    Dim HostsFile As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MOBZystems"), "MOBZPing.xml")
    If Not AttemptLoadHosts(HostsFile) Then
      AttemptLoadHosts(".\DefaultHosts.xml")
    End If

    Me.UpdateStartStopButtons()

    Me.GraphPictureBox.SmallChange = New Size(BARWIDTH, BARWIDTH)

    ' Update the graph size
    UpdateGraphSize()
  End Sub

  ''' <summary>
  ''' Hide ourselves and show the notify icon when minimized
  ''' </summary>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
    MyBase.OnSizeChanged(e)

    ' If we're minimized now, hide ourselves.
    If _LastWindowState <> Me.WindowState Then
      If Me.WindowState = FormWindowState.Minimized Then
        Me.MainNotifyIcon.Visible = True
        Me.Visible = False
      Else
        Me.Visible = True
        Me.MainNotifyIcon.Visible = False
      End If
    End If

    If Me.WindowState <> FormWindowState.Minimized Then
      Me._LastWindowState = Me.WindowState
      ' Me._LastWindowSize = Me.Size
    End If
  End Sub
#End Region

  Private Sub MainSplitContainer_Panel1_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MainSplitContainer.Panel1.DragOver
    ' Find out the control we're dragging
    Dim C As HostStatusControl = DirectCast(e.Data.GetData(GetType(HostStatusControl)), HostStatusControl)

    If C IsNot Nothing Then
      ' Is it us?
      Dim DropControl As HostStatusControl = DirectCast(Me.MainSplitContainer.Panel1.GetChildAtPoint(Me.MainSplitContainer.Panel1.PointToClient(New Point(e.X, e.Y))), HostStatusControl)
      ' If so, we can't drop
      If DropControl IsNot C AndAlso DropControl IsNot Nothing Then
        e.Effect = DragDropEffects.Move
      End If
    End If
  End Sub

  Private Sub MainSplitContainer_Panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MainSplitContainer.Panel1.DragDrop
    If e.Effect = DragDropEffects.Move Then
      Dim ParentControl As Control = Me.MainSplitContainer.Panel1

      ' Find out the control we're dragging
      Dim C As HostStatusControl = DirectCast(e.Data.GetData(GetType(HostStatusControl)), HostStatusControl)
      ' And the control we're dropping on
      Dim DropControl As HostStatusControl = DirectCast(ParentControl.GetChildAtPoint(ParentControl.PointToClient(New Point(e.X, e.Y))), HostStatusControl)

      If DropControl IsNot Nothing Then
        ParentControl.SuspendLayout()

        ' Dropping this...
        Dim PHDropSource As PingHost = PingHostFromControl(C)
        ' On that.
        Dim PHDropTarget As PingHost = PingHostFromControl(DropControl)

        If Me._PingHosts.IndexOf(PHDropSource) > Me._PingHosts.IndexOf(PHDropTarget) Then
          ' Dropping on a LOWER index. Insert the droppee in place of the droptarget
          Me._PingHosts.Remove(PHDropSource)
          Me._PingHosts.Insert(Me._PingHosts.IndexOf(PHDropTarget), PHDropSource)
        Else
          ' Dropping on a higher index control: insert at the desired position plus one
          Me._PingHosts.Remove(PHDropSource)
          Me._PingHosts.Insert(Me._PingHosts.IndexOf(PHDropTarget) + 1, PHDropSource)
        End If

        ' Remove all controls
        For Each PH As PingHost In Me._PingHosts
          ' ParentControl.Controls.Remove(PH.StatusControl)
          PH.StatusControl.BringToFront()
        Next

        ' Layout!
        ParentControl.ResumeLayout(True)

        InvalidateGraph()
      End If
    End If
  End Sub
End Class
