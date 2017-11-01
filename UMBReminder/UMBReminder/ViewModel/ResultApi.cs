using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMBReminder.ViewModel
{

    public class ResultApi <T>
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }
    }

    public class ResultApi
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object Payload { get; set; }
    }
}
