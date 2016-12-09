using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    interface IMessageReader
    {
        string ReadValue(Stream msg, string expression);
    }
}   
