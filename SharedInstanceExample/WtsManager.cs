using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using SharedComponents.Constants;

namespace SharedInstanceExample
{
    public class WtsManager : IDisposable
    {
        [DllImport("WtsApi32.dll")]
        private static extern bool WTSRegisterSessionNotification(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)]int dwFlags);
        [DllImport("WtsApi32.dll")]
        private static extern bool WTSUnRegisterSessionNotification(IntPtr hWnd);

        private List<KeyValuePair<int, Action>> _callbacks;
        private readonly int _wmMessage;
        protected readonly HwndSource PresentationSource;
        protected bool IsHookActive = false;

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == _wmMessage && _callbacks != null)
            {
                var message = (int)wparam;

                var actions = _callbacks.Where(c => c.Key == message).Select(a => a.Value);

                foreach (var action in actions)
                {
                    action.Invoke();
                }
            }

            return IntPtr.Zero;
        }

        public WtsManager(int wmMessage, HwndSource presentationSource)
        {
            _wmMessage = wmMessage;
            PresentationSource = presentationSource;
            InstallHooksAndRegister();
        }

        private void InstallHooksAndRegister()
        {
            PresentationSource.AddHook(WndProc);
            if (!WTSRegisterSessionNotification(PresentationSource.Handle,
               (int)WtsConstants.NOTIFY_FOR_ALL_SESSIONS))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            IsHookActive = true;
        }

        public void RemoveHooksAndUnregister()
        {
            WTSUnRegisterSessionNotification(PresentationSource.Handle);
            PresentationSource.RemoveHook(WndProc);
            IsHookActive = false;
        }

        public void AddEventCallback(int message, Action callback)
        {
            if (_callbacks == null)
            {
                _callbacks = new List<KeyValuePair<int, Action>>();
            }

            _callbacks.Add(new KeyValuePair<int, Action>(message, callback));
        }

        public void Dispose()
        {
            if (IsHookActive)
            {
                RemoveHooksAndUnregister();
            }
        }
    }

}