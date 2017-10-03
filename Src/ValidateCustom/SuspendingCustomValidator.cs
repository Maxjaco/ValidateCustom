using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{

        [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
        [ComponentCategory(CategoryTypes.CATID_Any)]
        [Guid("085F34E0-82D5-485C-A835-1432764BA783")]
        public partial class SuspendingCustomValidator : IComponent, IBaseComponent, IComponentUI, IPersistPropertyBag
        {
            private readonly IMessageReader _reader = null;
            private readonly IValidator _validator = null;

            private const string XpathPropertyName = "Xpath";
            private const string CustomPropertyName = "Custom";

            [RequiredRuntime]
            [DisplayName("Xpath")]
            [Description("Xpath to the node to validate in the incomming message.")]
            public string Xpath { get; set; }

            [RequiredRuntime]
            [DisplayName("Custom")]
            [Description("Custom value to validate the value from the incomming message with.")]
            public string Custom { get; set; }



            public SuspendingCustomValidator()
            {
                _reader = new XmlMessageReader();
                _validator = new XpathValidator();
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
                var valid = _validator.Validate(value, Custom);

                if (!valid)
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
                Custom = PropertyBagHelper.ReadPropertyBag(propertyBag, CustomPropertyName, Custom);
            }

            public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
            {
                PropertyBagHelper.WritePropertyBag(propertyBag, XpathPropertyName, Xpath);
                PropertyBagHelper.WritePropertyBag(propertyBag, CustomPropertyName, Custom);
            }

        }

    }
