using BizTalkComponents.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    public partial class SuspendingCustomValidator
    {

        public string Name { get { return "SuspendingCustomValidator"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "This component is used to validate a specific value from an incomming xml message."; } }

        public void InitNew() { }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }

        public void GetClassID(out Guid classid)
        {
            classid = new Guid("1CD10318-CF64-48D0-A1B8-E3E5EA4AC7D8");
        }

    }
}
