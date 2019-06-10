using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using System.Linq;
using WolframLanguage.Activities.Properties;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Markup;
using Microsoft.Win32;
using Wolfram.NETLink;

namespace WolframLanguage.Activities.Activities
{
    [LocalizedDescription(nameof(Resources.ParentScopeDescription))]
    [LocalizedDisplayName(nameof(Resources.ParentScope))]
    public class WolframLanguageScope : NativeActivity
    {
        #region Properties

        [Browsable(false)]
        public ActivityAction<Application> Body { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParentScopeKernelDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeKernelDescription))]
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.InputKernel))]
        [OverloadGroup("ExistingKernel")]
        public InOutArgument<IKernelLink> Kernel { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.ParentScopePathDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopePathDescription))]
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.InputOpenKernel))]
        [OverloadGroup("NewKernel")]
        public InArgument<string> KernelPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParentScopeArgsDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeArgsDescription))]
        [DependsOn(nameof(KernelPath))]
        [LocalizedCategory(nameof(Resources.InputOpenKernel))]
        [OverloadGroup("NewKernel")]
        public InArgument<string[]> KernelArgs { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.ParentScopeStartupSleepDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeStartupSleepDescription))]
        [DefaultValue(typeof(int),@"100")]
        [OverloadGroup("NewKernel")]
        [LocalizedCategory(nameof(Resources.InputOpenKernel))]
        [DependsOn(nameof(KernelPath))]
        public InArgument<int> StartupSleep { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EnableObjectReferencesDisplayName))]
        [LocalizedDescription(nameof(Resources.EnableObjectReferencesDescription))]
        [LocalizedCategory(nameof(Resources.InputOpenKernel))]
        [OverloadGroup("NewKernel")]
        [DependsOn(nameof(KernelPath))]
        [RequiredArgument]
        public InArgument<bool> EnableObjectReferences { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.CloseKernelOnFinishDisplayName))]
        [LocalizedDescription(nameof(Resources.CloseKernelOnFinishDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<bool> CloseKernelOnFinish { get; set; }

        internal static string ParentContainerPropertyTag => "WolframLanguageScope";
        private Application _kernel;
        
        #endregion


        #region Constructors

        #region Overrides of NativeActivity

        protected override void Abort(NativeActivityAbortContext context)
        {
            _kernel?.Dispose();
            base.Abort(context);
        }

        protected override void Cancel(NativeActivityContext context)
        {
            _kernel?.Dispose();
            base.Cancel(context);
        }

        #endregion

        public WolframLanguageScope()
        {
            Body = new ActivityAction<Application>();
            Body.Argument = new DelegateInArgument<Application>(ParentContainerPropertyTag);
            Body.Handler = new Sequence {DisplayName = @"Do"};
        }

        #endregion


        #region Private Methods
        
        protected override void Execute(NativeActivityContext context)
        {
            var kernel = Kernel.Get(context);
            if (kernel != null)
            {
                _kernel = new Application(kernel);
            }
            else
            {
                var kernelPath = KernelPath.Get(context);
                var kernelArgs = KernelArgs.Get(context);
                var startupSleep = StartupSleep.Get(context);
                var enableObjectReferences = EnableObjectReferences.Get(context);
                _kernel = new Application(kernelPath, kernelArgs, enableObjectReferences);
                
                Task.Run(() => _kernel.Initialization);
                if (Body == null) return;
                while (!_kernel.Ready)
                {
                    Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                    Thread.Sleep(startupSleep);
                }
            }
            
            Kernel.Set(context, _kernel.KernelLink);
            context.Properties.Add(@"Application", _kernel);
            context.ScheduleAction(Body, _kernel,
                OnCompleted, OnFaulted);
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
