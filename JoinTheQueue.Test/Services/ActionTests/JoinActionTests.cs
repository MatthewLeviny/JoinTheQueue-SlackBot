using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Repository;
using JoinTheQueue.Core.Services;
using JoinTheQueue.Core.Services.Actions;
using Moq;
using StructureMap.AutoMocking.Moq;
using Xunit;

namespace JoinTheQueue.Test.Services.ActionTests
{
    public class JoinActionTests : MoqAutoMocker<JoinActionService>
    {
        [Fact]
        public void KeyCheck()
        {
            //Arrange
            //Act
            var key = ClassUnderTest.Key;
            //Assert
            key.Should().Be("JoinAction");
        }

        [Theory]
        [MemberData(nameof(RequestAndResponseJoin))]
        public async Task JoinQueue(Root request,
            QueueDto databaseResponseGet, QueueDto databaseResponseUpdate, ResponseCount responseCount)
        {
            //arrange
            Mock.Get(Get<IQueueDatabase>()).Setup(database => database.GetQueue(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(databaseResponseGet == null
                    ? null
                    : new QueueDto
                    {
                        Queue = databaseResponseGet.Queue,
                        ChannelId = databaseResponseGet.ChannelId
                    }));

            Mock.Get(Get<IQueueDatabase>()).Setup(database => database.UpdateQueue(It.IsAny<QueueDto>()))
                .Returns(Task.FromResult(new QueueDto
                {
                    Queue = databaseResponseUpdate?.Queue,
                    ChannelId = databaseResponseUpdate?.ChannelId
                }));
            Mock.Get(Get<IBlockCreationService>()).Setup(web => web.CurrentQueue(It.IsAny<QueueDto>()))
                .Returns(Task.FromResult(new QueueBlockDto()));
            Mock.Get(Get<IWebHookService>()).Setup(web => web.TriggerWebHook(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(true));

            //act
            await ClassUnderTest.PerformAction(request);
            //assert
            Mock.Get(Get<IQueueDatabase>()).Verify(mock => mock.GetQueue(It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(responseCount.QueueGet));

            Mock.Get(Get<IQueueDatabase>()).Verify(mock => mock.UpdateQueue(It.IsAny<QueueDto>()),
                Times.Exactly(responseCount.QueueUpdate));
            Mock.Get(Get<IWebHookService>())
                .Verify(mock => mock.TriggerWebHook(It.IsAny<string>(), It.IsAny<object>()),
                    Times.Exactly(responseCount.WebService));
        }

        public static IEnumerable<object[]> RequestAndResponseJoin()
        {
            //No Queue
            yield return new object[]
            {
                //request
                new Root()
                {
                    channel = new Channel
                    {
                        id = "123"
                    }
                },
                //databaseResponseGet
                null,
                //databaseResponseUpdate
                null,
                new ResponseCount
                {
                    QueueGet = 1,
                    QueueUpdate = 0,
                    WebService = 1
                }
            };
            //happy user already in queue
            yield return new object[]
            {
                //request
                new Root()
                {
                    user = new User
                    {
                        username = "123"
                    },
                    channel = new Channel
                    {
                        id = "123"
                    }
                },
                //databaseResponseGet
                new QueueDto
                {
                    ChannelId = "123",
                    Queue = new Queue<string>(new List<string> {"123"})
                },
                //databaseResponseUpdate
                null,
                new ResponseCount
                {
                    QueueGet = 1,
                    QueueUpdate = 0,
                    WebService = 2
                }
            };
            //happy add user
            yield return new object[]
            {
                //request
                new Root()
                {
                    user = new User
                    {
                        username = "123"
                    },
                    channel = new Channel
                    {
                        id = "123"
                    }
                },
                //databaseResponseGet
                new QueueDto
                {
                    ChannelId = "123",
                    Queue = new Queue<string>()
                },
                //databaseResponseUpdate
                new QueueDto
                {
                    ChannelId = "123",
                    Queue = new Queue<string>(new List<string>{"123"})
                },
                new ResponseCount
                {
                    QueueGet = 1,
                    QueueUpdate = 1,
                    WebService = 2
                }
            };
        }
    }

    public class ResponseCount
    {
        public int WebService { get; set; }
        public int QueueGet { get; set; }
        public int QueueUpdate { get; set; }
    }
}