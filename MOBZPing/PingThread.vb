Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Net.NetworkInformation

''' <summary>
''' This class represents a thead that sends ping request to a host continuously.
''' When a reply is recieved (either with a round trip time or a timeout or some
''' other error), the PingReceived event is raised.
''' </summary>
''' <remarks></remarks>
Public Class PingThread
  ' The host to ping. Supplied in constructor
  Private _HostName As String
  ' The ping timeout in milliseconds. Also supplied in constructor.
  Private _TimeOutMilliSeconds As Integer

  ' The internal worker object
  Private _Worker As BackgroundWorker

  ' This event is raised when a ping request returns
  Public Event PingReceived(ByVal sender As PingThread, ByVal SendDateTime As DateTime, ByVal Reply As PingReply)

  ' This event is raised when a ping request failed
  Public Event PingError(ByVal sender As PingThread, ByVal SendDateTime As DateTime, ByVal Exception As Exception)

  ''' <summary>
  ''' Constructor with host name
  ''' </summary>
  ''' <param name="HostName"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal HostName As String, ByVal TimeOutMilliSeconds As Integer)
    _HostName = HostName
    _TimeOutMilliSeconds = TimeOutMilliSeconds
  End Sub

  ''' <summary>
  ''' Get the host name associated with this object
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property HostName() As String
    Get
      Return _HostName
    End Get
  End Property

  ''' <summary>
  ''' The main routine for the background worker
  ''' </summary>
  ''' <param name="Sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub DoWork(ByVal Sender As Object, ByVal e As DoWorkEventArgs)
    Do
      Dim P As New Ping

      ' Sample the time the request was sent
      Dim T As DateTime = DateTime.Now
      Dim R As PingReply = Nothing

      Try
        ' Send out the PING
        R = P.Send(Me._HostName)

        ' Are we canceling? Then abort here
        If _Worker Is Nothing OrElse _Worker.CancellationPending Then
          Exit Do
        End If

        ' Notify listeners of the reply, supplying the time the request was sent
        RaiseEvent PingReceived(Me, T, R)

        ' Sleep a bit, so we don't flood the host
        ' but only if we didn't time out and we had a reply!
        If R.Status = IPStatus.Success Then
          System.Threading.Thread.Sleep(Me._TimeOutMilliSeconds)
        End If

      Catch ex As Exception
        ' Notify listeners that there was an error
        RaiseEvent PingError(Me, T, ex)
      End Try

      ' Keep going until we need to cancel
    Loop Until _Worker Is Nothing OrElse _Worker.CancellationPending
  End Sub

  ''' <summary>
  ''' This event is raised when the worker thread completes. Cleans up the worker object
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub WorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
    ' Detach ourselves from the worker object
    RemoveHandler _Worker.DoWork, AddressOf DoWork
    RemoveHandler _Worker.RunWorkerCompleted, AddressOf WorkerCompleted

    ' Clean up the worker
    _Worker.Dispose()

    _Worker = Nothing
  End Sub

  ''' <summary>
  ''' Start the worker
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Start()
    ' Create a new worker
    _Worker = New BackgroundWorker

    _Worker.WorkerSupportsCancellation = True

    ' Attach event handlers
    AddHandler _Worker.DoWork, AddressOf DoWork
    AddHandler _Worker.RunWorkerCompleted, AddressOf WorkerCompleted

    ' Start it
    _Worker.RunWorkerAsync()
  End Sub

  ''' <summary>
  ''' Abort the worker
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Abort()
    ' Cancel the worker
    _Worker.CancelAsync()
  End Sub
End Class
