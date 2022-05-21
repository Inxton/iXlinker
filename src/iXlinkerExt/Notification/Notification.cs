using Microsoft.VisualStudio.Shell;
using System;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft;
using Microsoft.VisualStudio.Threading;

namespace iXlinkerExt
{
    public static class Notification
    {
        public static IServiceProvider ServiceProvider;

        public static async Task UseStatusBarProgressAsync(string message, uint currentSteps, uint numberOfSteps)
        {
            uint cookie = 0;

            var pb = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar;

            Assumes.Present(pb);

            if (pb.IsFrozen(out int frozen) == 1 && frozen == 1)
            {
                pb.FreezeOutput(0);
            }
            pb.Clear();
            pb.SetText("");
            pb.Progress(ref cookie, 1, message + $" {currentSteps} of {numberOfSteps} completed", currentSteps, numberOfSteps);

            if (currentSteps == numberOfSteps && currentSteps > 0)
            {
                await Task.Delay(1000);
                pb.Progress(ref cookie, 0, message + $" {currentSteps} of {numberOfSteps} completed", currentSteps, numberOfSteps);
            }
        }

        public static async Task ShowInStatusBar(string message)
        {

            var sb = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar;

            Assumes.Present(sb);

            if (sb.IsFrozen(out int frozen) == 1 && frozen == 1)
            {
                sb.FreezeOutput(0);
            }
            sb.Clear();
            sb.SetText(message);
        }
    }
}
