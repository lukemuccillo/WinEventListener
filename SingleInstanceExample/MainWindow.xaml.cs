using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using SharedComponents;
using SharedComponents.Constants;

namespace SingleInstanceExample
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WmManager _wtsHandler;

        private IEnumerable<WtsMessage> WatchedWTSEvents
        {
            get
            {
                return new[]
                {
                    WtsMessage.SESSION_LOCK,
                    WtsMessage.SESSION_UNLOCK,
                    WtsMessage.REMOTE_CONNECT,
                    WtsMessage.REMOTE_DISCONNECT,
                    WtsMessage.SESSION_REMOTE_CONTROL
                };
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var presentationSource = PresentationSource.FromVisual(this) as HwndSource;

            try
            {
                _wtsHandler = new WmManager((int)WindowsMessages.WM_WTSSESSION_CHANGE, presentationSource);

                foreach (var wEvent in WatchedWTSEvents)
                {
                    var cntEvent = wEvent;
                    _wtsHandler.AddEventCallback((int)cntEvent, () => Log(cntEvent.ToString()));
                    Log("Callback added on " + cntEvent);
                }

                Log("Session Event Listener has initialized successfully");
            }
            catch (Exception ex)
            {
                Log("An error occured, the application was unable to start the event watcher.");
                Log(ex.ToString());
            }
        }

        private void Log(string value)
        {
            LbxLog.Dispatcher.Invoke(new Action(() => LbxLog.Items.Add(String.Format("{0}: {1}", DateTime.Now.ToString("hh:mm:ss"), value))));
        }
    }
}
