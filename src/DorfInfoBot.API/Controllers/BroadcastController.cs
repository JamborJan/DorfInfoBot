using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DorfInfoBot.API.Models;
using DorfInfoBot.API.Services;

namespace DorfInfoBot.API.Controllers
{
    [ApiController]
    [Route("api/broadcast")]
    public class BroadcastController : ControllerBase
    {
        private readonly ILogger<BroadcastController> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public BroadcastController(ILogger<BroadcastController> logger, 
            IChannelRepository channelRepository,
            INewsRepository newsRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _channelRepository = channelRepository ??
                throw new ArgumentNullException(nameof(channelRepository));
            _newsRepository = newsRepository ??
                throw new ArgumentNullException(nameof(newsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // TODO: do we really wan't to provide a GET which gets all Broadcasts? MAybe add a TOP 100 or so.
        [HttpGet]
        public IActionResult GetBroadcast()
        {
            try
            {
                var broadcastEntities = _channelRepository.GetBroadcast();
                var results = new List<BroadcastDto>();
                return Ok(_mapper.Map<IEnumerable<BroadcastDto>>(broadcastEntities));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting all broadcasts", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetBroadcast")]
        public IActionResult GetBroadcast(int id)
        {
            var broadcast = _channelRepository.GetBroadcast(id);

            if (broadcast == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BroadcastDto>(broadcast));
        }

        [HttpPost]
        public IActionResult CreateBroadcast(
            [FromBody] BroadcastCreationDto broadcast)
        {
            
            // Data validation is done by ApiController, according to annotations in Models
            // 500: Bad Request Response is handled by ApiController

            var finalBroadcast = _mapper.Map<Entities.Broadcast>(broadcast);

            // 404: check if news entry is there before try to add an broadcast
            if (!_newsRepository.NewsExists(finalBroadcast.NewsId))
            {
                //return NotFound();
                return new ContentResult() { Content = $"News with ID {finalBroadcast.NewsId} does not exist.", StatusCode = 404 };
            }

            // 404: check if news entry is there before try to add an broadcast
            if (!_channelRepository.ChannelExists(finalBroadcast.ChannelId))
            {
                //return NotFound();
                return new ContentResult() { Content = $"Channel with ID {finalBroadcast.ChannelId} does not exist.", StatusCode = 404 };
            }

            // I'm enforcing this rule on purpose here and not on the DB level, because:
            // 1) it's not possible in EF core with annotations: https://stackoverflow.com/questions/41246614/entity-framework-core-add-unique-constraint-code-first/41257827
            // 2) I'm not yet sure, if I really want that or if it is useful in the future to resend news
            if (_channelRepository.BroadcastExists(finalBroadcast.ChannelId, finalBroadcast.NewsId))
            {
                return new ContentResult() { Content = $"Broadcast with Channel ID {finalBroadcast.ChannelId} and News ID {finalBroadcast.NewsId} already exists.", StatusCode = 409 };
            }

            _channelRepository.AddBroadcast(finalBroadcast);

            _channelRepository.Save();

            var createdBroadcastToReturn = _mapper
                .Map<Models.BroadcastDto>(finalBroadcast);

            return CreatedAtRoute(
                "GetBroadcast",
                new { finalBroadcast.NewsId, id = createdBroadcastToReturn.Id },
                createdBroadcastToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBroadcast(int id)
        {
            var broadcastEntity = _channelRepository
                .GetBroadcast(id);
            if (broadcastEntity == null)
            {
                return NotFound();
            }

            _channelRepository.DeleteBroadcast(broadcastEntity);
            _channelRepository.Save();

            return NoContent();
        }
    }
}
