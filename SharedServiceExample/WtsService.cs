using System.ServiceProcess;
using log4net;
using log4net.Config;

namespace SharedServiceExample
{
    public partial class WtsService : ServiceBase
    {
        private readonly ILog _log;
        public WtsService()
        {
            InitializeComponent();
            this.CanHandleSessionChangeEvent = true;
            XmlConfigurator.Configure(); // Log4Net Initialization.
            _log = LogManager.GetLogger("DEBUGLOG");
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    _log.Info(changeDescription.SessionId + " logon");
                    break;
                case SessionChangeReason.SessionLogoff:
                    _log.Info(changeDescription.SessionId + " logoff");
                    break;
                case SessionChangeReason.ConsoleConnect:
                    _log.Info(changeDescription.SessionId + " console_connect");
                    break;
                case SessionChangeReason.ConsoleDisconnect:
                    _log.Info(changeDescription.SessionId + " console.disconnect");
                    break;
                case SessionChangeReason.RemoteConnect:
                    _log.Info(changeDescription.SessionId + " remote.connect");
                    break;
                case SessionChangeReason.RemoteDisconnect:
                    _log.Info(changeDescription.SessionId + " remote.disconnect");
                    break;
                case SessionChangeReason.SessionLock:
                    _log.Info(changeDescription.SessionId + " lock");
                    break;
                case SessionChangeReason.SessionUnlock:
                    _log.Info(changeDescription.SessionId + " unlock");
                    break;
            }

            base.OnSessionChange(changeDescription);
        }

    }
}
