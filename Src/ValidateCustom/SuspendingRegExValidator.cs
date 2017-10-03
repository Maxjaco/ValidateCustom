using Microsoft.BizTalk.Component.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.BizTalk.Message.Interop;
using System.Runtime.InteropServices;
using BizTalkComponents.Utils;
using System.ComponentModel;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [Guid("903CA73C-70FD-4F72-A2A3-B9EC6213029A")]
    public partial class SuspendingRegExValidator : IComponent, IBaseComponent, IComponentUI, IPersistPropertyBag
    {
        private readonly IMessageReader _reader = null;
        private readonly IValidator _validator = null;

        private const string XpathPropertyName = "Xpath";
        private const string RegexPropertyName = "Regex";

        [RequiredRuntime]
        [DisplayName("Xpath")]
        [Description("Xpath to the node to validate in the incomming message.")]
        public string Xpath { get; set; }

        [RequiredRuntime]
        [DisplayName("Regex")]
        [Description("Regex to validate the value from the incomming message with.")]
        public string Regex{ get; set; }



        public SuspendingRegExValidator()
        {
            _reader = new XmlMessageReader();
            _validator = new RegExValidator();
        }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            var msg = pInMsg.BodyPart.GetOriginalDataStream();
            var value = _reader.ReadValue(msg, Xpath);
            var valid = _validator.Validate(value, Regex);

            if(!valid)
            {
                pInMsg.Context.Write(new ContextProperty("SuspendAsNonResumable", "http://schemas.microsoft.com/BizTalk/2003/system-properties"), true);
                pInMsg.Context.Write(new ContextProperty("SuppressRoutingFailureDiagnosticInfo", "http://schemas.microsoft.com/BizTalk/2003/system-properties"), true);

                throw new Exception(string.Format("{0} is not a valid value for the current node", value));
            }

            return pInMsg;
        }

        

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            Xpath = PropertyBagHelper.ReadPropertyBag(propertyBag, XpathPropertyName, Xpath);
            Regex = PropertyBagHelper.ReadPropertyBag(propertyBag, RegexPropertyName, Regex);
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PropertyBagHelper.WritePropertyBag(propertyBag, XpathPropertyName, Xpath);
            PropertyBagHelper.WritePropertyBag(propertyBag, RegexPropertyName, Regex);
        }
    }
}
