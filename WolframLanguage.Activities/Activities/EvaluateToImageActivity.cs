using System;
using System.Activities;
using System.Threading;
using System.Drawing;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities
{
    [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateToImageActivityDescription))]
    public class EvaluateToImageActivity : NativeActivity<Image>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Expression { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityWidthDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityWidthDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> Width { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityHeightDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityHeightDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> Height { get; set; }
        
        protected override void Execute(NativeActivityContext context)
        {
            var expression = Expression.Get(context);
            var w = Width.Get(context);
            var h = Height.Get(context);
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            Result.Set(context, client.EvaluateToImage(expression, w, h));
        }
    }
}
