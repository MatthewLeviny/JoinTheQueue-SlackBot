﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;
using JoinTheQueue.Core.Services;
using Moq;
using StructureMap.AutoMocking.Moq;
using Xunit;

namespace JoinTheQueue.Test.Services
{
    public class QueueServicesTests : MoqAutoMocker<QueueServices>
    {
        [Theory]
        [MemberData(nameof(RequestAndResponseJoin))]
        public async Task JoinQueue(SlashRequest request, SlackResponseDto expectedResponse,
            QueueDto databaseResponseGet, QueueDto databaseResponseUpdate)
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

            //act
            var response = await ClassUnderTest.JoinQueue(request);
            //assert
            response.response_type.Should().Be(expectedResponse.response_type);
            response.text.Should().Be(expectedResponse.text);
        }

        [Theory]
        [MemberData(nameof(RequestAndResponseLeave))]
        public async Task LeaveQueue(SlashRequest request, SlackResponseDto expectedResponse,
            QueueDto databaseResponseGet, QueueDto databaseResponseUpdate)
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

            //act
            var response = await ClassUnderTest.LeaveQueue(request);
            //assert
            response.response_type.Should().Be(expectedResponse.response_type);
            response.text.Should().Be(expectedResponse.text);
        }

        public static IEnumerable<object[]> RequestAndResponseJoin()
        {
            //No Queue
            yield return new object[]
            {
                new SlashRequest
                {
                    Channel_Id = "123",
                    Enterprise_Id = "123"
                },
                new SlackResponseDto
                {
                    text = "Queue Does not exist",
                    response_type = BasicResponseTypes.ephemeral
                },
                //databaseResponseGet
                null,
                //databaseResponseUpdate
                null
            };
            //Join Queue - Happy Path
            yield return new object[]
            {
                new SlashRequest
                {
                    Channel_Id = "123",
                    Enterprise_Id = "123",
                    User_Id = "123"
                },
                new SlackResponseDto
                {
                    text = "@123 has joined the queue",
                    response_type = BasicResponseTypes.in_channel
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
                    Queue = new Queue<string>(new List<string> {"123"})
                }
            };
        }

        public static IEnumerable<object[]> RequestAndResponseLeave()
        {
            //No Queue
            yield return new object[]
            {
                new SlashRequest
                {
                    Channel_Id = "123",
                    Enterprise_Id = "123"
                },
                new SlackResponseDto
                {
                    text = "Queue Does not exist",
                    response_type = BasicResponseTypes.ephemeral
                },
                //databaseResponseGet
                null,
                //databaseResponseUpdate
                null
            };
            //Happy Path - Leave Queue
            yield return new object[]
            {
                new SlashRequest
                {
                    Channel_Id = "123",
                    Enterprise_Id = "123",
                    User_Id = "123"
                },
                new SlackResponseDto
                {
                    text = $"@123 has left the queue" + "\n" +
                           $"@321 ITS GO TIME",
                    response_type = BasicResponseTypes.in_channel
                },
                //databaseResponseGet
                new QueueDto
                {
                    ChannelId = "123",
                    Queue = new Queue<string>(new List<string> {"123", "321"})
                },
                //databaseResponseUpdate
                new QueueDto
                {
                    ChannelId = "123",
                    Queue = new Queue<string>(new List<string> {"321"})
                }
            };
        }
    }
}