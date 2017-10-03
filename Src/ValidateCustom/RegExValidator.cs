using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    public class RegExValidator : IValidator
    {
        public bool Validate(string value, string expression)
        {
            if (string.IsNullOrEmpty(value)) return false;
            Match match = Regex.Match(value, expression);

            if(match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
