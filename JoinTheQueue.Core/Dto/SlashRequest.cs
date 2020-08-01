using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Dto
{
    public class SlashRequest
    {
        public string Token { get; set; }

        public string Team_Id { get; set; }

        public string Team_Domain { get; set; }

        public string Enterprise_Id { get; set; }

        public string Enterprise_Name { get; set; }

        public string Channel_Id { get; set; }

        public string Channel_Name { get; set; }

        public string User_Id { get; set; }

        public string User_Name { get; set; }

        public string Command { get; set; }

        public string Text { get; set; }

        public string Response_Url { get; set; }

        public string Trigger_Id { get; set; }
    }
}