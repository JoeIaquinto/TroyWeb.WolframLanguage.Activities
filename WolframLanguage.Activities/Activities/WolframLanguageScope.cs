using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using WolframLanguage.Activities.Properties;
using System.Threading.Tasks;
using System.Threading;

namespace WolframLanguage.Activities
{
    [LocalizedDescription(nameof(Resources.ParentScopeDescription))]
    [LocalizedDisplayName(nameof(Resources.ParentScope))]
    public class WolframLanguageScope : NativeActivity
    {
        #region Properties

        [Browsable(false)]
        public ActivityAction<Application> Body { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParentScopePathDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopePathDescription))]
        [RequiredArgument]
        public InArgument<string> KernelPath { get; set; }

        [LocalizedDisplayName(nameof(Resources.ParentScopeArgsDisplayName))]
        [LocalizedDescription(nameof(Resources.ParentScopeArgsDescription))]
        [RequiredArgument]
        public InArgument<string[]> KernelArgs { get; set; }

        internal static string ParentContainerPropertyTag => "WolframLanguageScope";

        #endregion


        #region Constructors

        public WolframLanguageScope()
        {
            Body = new ActivityAction<Application>
            {
                Argument = new DelegateInArgument<Application>(ParentContainerPropertyTag),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        #endregion


        #region Private Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
        {
            var kernelPath = KernelPath.Get(context);
            var kernelArgs = KernelArgs.Get(context);
            var application = new Application(kernelPath, kernelArgs);
            Task.Run(() => application.Initialization);
            if (Body != null)
            {
                while (application == null || !application.Ready)
                {
                    Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                    Thread.Sleep(100);
                }
                
                context.Properties.Add(@"Application", application);
                context.ScheduleAction<Application>(Body, application, OnCompleted, OnFaulted);
            }
        }

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
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
