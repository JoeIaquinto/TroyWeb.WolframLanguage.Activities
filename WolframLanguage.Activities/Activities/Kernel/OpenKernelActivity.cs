using System;
using System.Activities;
using System.ComponentModel;
using WolframLanguage.Activities.Properties;
using System.Threading.Tasks;
using System.Threading;
using Wolfram.NETLink;

namespace WolframLanguage.Activities.Activities.Kernel
{
    [LocalizedDescription(nameof(Resources.OpenKernelDescription))]
    [LocalizedDisplayName(nameof(Resources.OpenKernelDisplayName))]
    public class OpenKernelActivity : NativeActivity
    {
        #region Properties

        [LocalizedDisplayName(nameof(Resources.ParentScopePathDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopePathDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<string> KernelPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParentScopeArgsDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeArgsDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<string[]> KernelArgs { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.ParentScopeStartupSleepDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeStartupSleepDescription))]
        [DefaultValue(typeof(int), @"100")]
        [LocalizedCategory(nameof(Resources.Input))]

        public InArgument<int> StartupSleep { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EnableObjectReferencesDisplayName))]
        [LocalizedDescription(nameof(Resources.EnableObjectReferencesDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [DefaultValue(false)]
        public InArgument<bool> EnableObjectReferences { get; set; }

        [LocalizedDisplayName(nameof(Resources.KernelOutputDisplayName))]
        [LocalizedDescription(nameof(Resources.KernelOutputDescription))]
        [LocalizedCategory(nameof(Resources.Output))]
        [RequiredArgument]
        public OutArgument<IKernelLink> Kernel { get; set; }
        
        #endregion


        #region Constructors

        #region Overrides of NativeActivity

        protected override void Abort(NativeActivityAbortContext context)
        {
            Kernel.Get(context)?.Close();
            base.Abort(context);
        }

        protected override void Cancel(NativeActivityContext context)
        {
            Kernel.Get(context)?.Close();
            base.Cancel(context);
        }
        
        #endregion

        #endregion


        #region Private Methods
        
        protected override void Execute(NativeActivityContext context)
        {
            var kernelPath = KernelPath.Get(context);
            var kernelArgs = KernelArgs.Get(context);
            var startupSleep = StartupSleep.Get(context);
            var enableObjectReferences = EnableObjectReferences.Get(context);
            var application = new Application(kernelPath, kernelArgs, enableObjectReferences, false);
            
            Task.Run(() => application.Initialization);
            
            while (!application.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(startupSleep);
            }
            
            Kernel.Set(context, application.KernelLink);
        }

        private static void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            faultContext.DataContext.Dispose();
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            Console.WriteLine(Resources.WolframLanguageScope_OnCompleted_Parent_Scope_complete_);
        }

        #endregion


        #region Helpers

        #endregion
    }
}
