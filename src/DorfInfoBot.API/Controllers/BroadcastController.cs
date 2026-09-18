using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using DorfInfoBot.API.Models;
using DorfInfoBot.API.Services;
using System.Linq;
using Newtonsoft.Json;

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

        [HttpGet(Name = "GetAllBroadcasts")]
        [HttpHead]
        public IActionResult GetBroadcast([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var broadcastEntities = _channelRepository.GetBroadcast();

                // Calculate the number of items to skip and take
                var itemsToSkip = (pageNumber - 1) * pageSize;
                var itemsToTake = pageSize;

                // Apply pagination to the data
                var pagedData = broadcastEntities.Skip(itemsToSkip).Take(itemsToTake);

                var results = new List<BroadcastDto>();

                // Map the paged data to DTOs
                results = (List<BroadcastDto>)_mapper.Map<IEnumerable<BroadcastDto>>(pagedData);

                // Create a pagination header
                var paginationHeader = new
                {
                    currentPage = pageNumber,
                    pageSize = pageSize,
                    totalCount = broadcastEntities.Count(),
                    totalPages = (int)Math.Ceiling((double)broadcastEntities.Count() / pageSize)
                };

                // Add the pagination header to the response headers
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

                // Get the total count of news entities
                var count = broadcastEntities.Count();
                // Create a count object
                var countObject = new { count = count };
                // Add the count object to the response headers
                Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject(countObject));
                
                return Ok(_mapper.Map<IEnumerable<BroadcastDto>>(broadcastEntities));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting all broadcasts", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetOneBroadcast")]
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
