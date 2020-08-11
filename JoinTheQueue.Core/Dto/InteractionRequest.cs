using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Dto
{
    public class InteractionRequest
    {
        public string type { get; set; }
        public string trigger_id { get; set; }
        public string response_url { get; set; }



    }
}