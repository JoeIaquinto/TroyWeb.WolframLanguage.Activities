using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using WolframLanguage.Activities.Activities;
using WolframLanguage.Activities.Activities.Evaluate;
using WolframLanguage.Activities.Activities.Kernel;
using WolframLanguage.Activities.Activities.Low_Level.Packet_Management;
using WolframLanguage.Activities.Activities.Low_Level.Get_Data;
using WolframLanguage.Activities.Activities.Low_Level.Send_Data;
using WolframLanguage.Activities.Design.Properties;

namespace WolframLanguage.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var parentScopeCategoryAttribute = new CategoryAttribute($"{Resources.Category}");
            var lowLevelScopeCategoryAttribute = new CategoryAttribute(parentScopeCategoryAttribute.Category + "." + $"{Resources.LowLevelScopeCategory}");
            var evaluateScopeCategoryAttribute = new CategoryAttribute(parentScopeCategoryAttribute.Category + "." + $"{Resources.EvaluateScopeCategory}");
            var kernelScopeCategoryAttribute = new CategoryAttribute(parentScopeCategoryAttribute.Category + "." + $"{Resources.KernelScopeCategory}");
            var lowLevelPacketScopeCategoryAttribute = new CategoryAttribute(lowLevelScopeCategoryAttribute.Category + "." + $"{Resources.LowLevelPacketScopeCategory}");
            var lowLevelGetDataScopeCategoryAttribute = new CategoryAttribute(lowLevelScopeCategoryAttribute.Category + "." + $"{Resources.LowLevelGetDataScopeCategory}");
            var lowLevelPutDataScopeCategoryAttribute = new CategoryAttribute(lowLevelScopeCategoryAttribute.Category + "." + $"{Resources.LowLevelPutDataScopeCategory}");
            
            builder.AddCustomAttributes(typeof(WolframLanguageScope), parentScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new DesignerAttribute(typeof(ParentScopeDesigner)));
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(OpenKernelActivity), kernelScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(OpenKernelActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(OpenKernelActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(CloseKernelActivity), kernelScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(CloseKernelActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(CloseKernelActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(EvaluateExprActivity), evaluateScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateExprActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateExprActivity), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), evaluateScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), evaluateScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), evaluateScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(EndPacketActivity), lowLevelPacketScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(EndPacketActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EndPacketActivity), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(FlushActivity), lowLevelPacketScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(FlushActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(FlushActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(GetActivity<>), lowLevelGetDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(GetActivity<>), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(GetActivity<>), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(GetExpressionActivity), lowLevelGetDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(GetExpressionActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(GetExpressionActivity), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(NewPacketActivity), lowLevelPacketScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(NewPacketActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(NewPacketActivity), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(PeekExpressionActivity), lowLevelGetDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PeekExpressionActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PeekExpressionActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutArgCountActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutArgCountActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutArgCountActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutDataActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutDataActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutDataActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutNextActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutNextActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutNextActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutReferenceActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutReferenceActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutReferenceActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutSymbolActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutSymbolActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutSymbolActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(WaitForPacketActivity), lowLevelPacketScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(WaitForPacketActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(WaitForPacketActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(PutFunctionActivity), lowLevelPutDataScopeCategoryAttribute);
            builder.AddCustomAttributes(typeof(PutFunctionActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(PutFunctionActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
