using System.Linq;
using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;

namespace JoinTheQueue.Core.Services
{
    public interface IBlockCreationService
    {
        Task<QueueBlockDto> CurrentQueue(QueueDto queue);
    }

    public class BlockCreationService : IBlockCreationService
    {
        public Task<QueueBlockDto> CurrentQueue(QueueDto queue)
        {
            var block = new QueueBlockDto
            {
                Blocks = new[]
                {
                    new Block
                    {
                        Type = BlockTypes.section,
                        Text = new BlockText
                        {
                            Type = TextTypes.mrkdwn,
                            Text = queue.Name + " Queue"
                        }
                    },
                    new Block
                    {
                        Type = BlockTypes.divider
                    },
                    new Block
                    {
                        Type = BlockTypes.section,
                        Text = new BlockText
                        {
                            Type = TextTypes.mrkdwn,
                            Text =
                                "The queue is empty"
                        }
                    },
                    new Block
                    {
                        Type = BlockTypes.divider
                    },
                    new Block
                    {
                        Type = BlockTypes.actions,
                        Elements = new BlockElement[]
                        {
                            new BlockElement
                            {
                                Type = ElementTypes.button,
                                Text = new BlockText
                                {
                                    Type = TextTypes.plain_text,
                                    Text = "Join"
                                },
                                Value = "JoinAction"
                            },
                            new BlockElement
                            {
                                Type = ElementTypes.button,
                                Text = new BlockText
                                {
                                    Type = TextTypes.plain_text,
                                    Text = "Leave"
                                },
                                Value = "LeaveAction"
                            },
                            new BlockElement
                            {
                                Type = ElementTypes.overflow,
                                Options = new BlockElement[]
                                {
                                    new BlockElement
                                    {
                                        Type = null,
                                        Text = new BlockText
                                        {
                                            Type = TextTypes.plain_text,
                                            Text = "Nudge the Leader"
                                        },
                                        Value = "NudgeAction"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var queueAsText = "";
            foreach (var person in queue.Queue)
            {
                queueAsText += $"@{person}";
                if (!queue.Queue.LastOrDefault().Equals(person))
                {
                    queueAsText += " \n";
                }
            }

            if (!string.IsNullOrEmpty(queueAsText))
            {
                block.Blocks[2].Text.Text = queueAsText;
            }

            return Task.FromResult(block);
        }
    }
}