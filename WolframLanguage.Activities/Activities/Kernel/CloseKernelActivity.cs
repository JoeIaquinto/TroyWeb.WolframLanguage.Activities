using System.Activities;
using WolframLanguage.Activities.Properties;
using Wolfram.NETLink;

namespace WolframLanguage.Activities.Activities.Kernel
{
    [LocalizedDescription(nameof(Resources.CloseKernelDescription))]
    [LocalizedDisplayName(nameof(Resources.CloseKernelDisplayName))]
    public class CloseKernelActivity : NativeActivity
    {
        #region Properties

        [LocalizedDisplayName(nameof(Resources.KernelOutputDisplayName))]
        [LocalizedDescription(nameof(Resources.KernelOutputDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<IKernelLink> Kernel { get; set; }
        
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
            var kernel = Kernel.Get(context);
            kernel?.Close();
        }

        #endregion


        #region Helpers

        #endregion
    }
}
