using JoinTheQueue.Core.Enum;

namespace JoinTheQueue.Core.Dto
{
    public class SlackResponseDto
    {
        public BasicResponseTypes response_type { get; set; }
        public string text { get; set; }
    }
}