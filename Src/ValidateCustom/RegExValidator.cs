using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    public class RegExValidator : IValidator
    {
        public bool Validate(string value, string expression)
        {
            return true;
        }
    }
}
