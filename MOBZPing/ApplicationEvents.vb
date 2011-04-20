Namespace My

  ' The following events are available for MyApplication:
  ' 
  ' Startup: Raised when the application starts, before the startup form is created.
  ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
  ' UnhandledException: Raised if the application encounters an unhandled exception.
  ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
  ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
  Partial Friend Class MyApplication
    Protected Overrides Sub OnStartupNextInstance(ByVal eventArgs As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs)
      ' If we're minimized, restore ourselves
      Dim F As PingForm = DirectCast(Application.MainForm, PingForm)
      F.RestoreWindowIfMinimized()

      eventArgs.BringToForeground = True
      MyBase.OnStartupNextInstance(eventArgs)

      'F.BringToFront()
      'F.Focus()
      'F.Activate()
    End Sub
  End Class

End Namespace

