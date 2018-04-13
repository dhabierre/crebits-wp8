
namespace WP8.Crebits
{
    using Microsoft.Xna.Framework;

    using System;
    using System.Windows;
    using System.Windows.Threading;

    // System.InvalidOperationException: 
    //FrameworkDispatcher.Update has not been called. Regular FrameworkDispatcher.Update calls are necessary for fire and forget sound effects and framework events to function correctly. See http://go.microsoft.com/fwlink/?LinkId=193853 for details.
    //at Microsoft.Xna.Framework.FrameworkDispatcher.AddNewPendingCall(ManagedCallType callType, UInt32 arg)
    //at Microsoft.Xna.Framework.UserAsyncDispatcher.AsyncDispatcherThreadFunction()
    //at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
    //at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
    //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
    //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
    //at System.Threading.ThreadHelper.ThreadStart()

    // Fix: http://msdn.microsoft.com/library/ff842408.aspx

    public class XNAFrameworkDispatcherService : IApplicationService
    {
        private DispatcherTimer _frameworkDispatcherTimer = null;

        public XNAFrameworkDispatcherService()
        {
            _frameworkDispatcherTimer = new DispatcherTimer();
            _frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
            _frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;

            FrameworkDispatcher.Update();
        }

        private void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        #region [ IApplicationService Implementation ]

        public void StartService(ApplicationServiceContext context)
        {
            _frameworkDispatcherTimer.Start();
        }

        public void StopService()
        {
            _frameworkDispatcherTimer.Stop();
        }

        #endregion
    }
}
