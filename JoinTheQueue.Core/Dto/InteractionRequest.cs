using System.Collections.Generic;

namespace JoinTheQueue.Core.Dto
{
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string team_id { get; set; }
    }

    public class Container
    {
        public string type { get; set; }
        public string message_ts { get; set; }
        public string channel_id { get; set; }
        public bool is_ephemeral { get; set; }
    }

    public class Team
    {
        public string id { get; set; }
        public string domain { get; set; }
    }

    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Text
    {
        public string type { get; set; }
        public string text { get; set; }
        public bool verbatim { get; set; }
    }

    public class Text2
    {
        public string type { get; set; }
        public string text { get; set; }
        public bool emoji { get; set; }
    }

    public class Element
    {
        public string type { get; set; }
        public string action_id { get; set; }
        public Text2 text { get; set; }
        public string value { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string subtype { get; set; }
        public string text { get; set; }
        public string ts { get; set; }
        public string bot_id { get; set; }
        public List<Block> blocks { get; set; }
    }

    public class Text3
    {
        public string type { get; set; }
        public string text { get; set; }
        public bool emoji { get; set; }
    }

    public class Action
    {
        public string action_id { get; set; }
        public string block_id { get; set; }
        public Text3 text { get; set; }
        public string value { get; set; }
        public BlockElement selected_option { get; set; }
        public string type { get; set; }
        public string action_ts { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public User user { get; set; }
        public string api_app_id { get; set; }
        public string token { get; set; }
        public Container container { get; set; }
        public string trigger_id { get; set; }
        public Team team { get; set; }
        public Channel channel { get; set; }
        public Message message { get; set; }
        public string response_url { get; set; }
        public List<Action> actions { get; set; }
    }
}