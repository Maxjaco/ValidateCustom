﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkComponents.PipelineComponents.ValidateCustom
{
    [Serializable]
    public class InvalidMessageException : Exception
    {
        public InvalidMessageException(string message):base(message)
        {

        }
    }
}
