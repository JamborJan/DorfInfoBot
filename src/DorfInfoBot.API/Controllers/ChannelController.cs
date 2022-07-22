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
    [Route("api/channel")]
    public class ChannelController : ControllerBase
    {
        private readonly ILogger<ChannelController> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public ChannelController(ILogger<ChannelController> logger, 
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

        [HttpGet]
        public IActionResult GetChannel()
        {
            var channelEntities = _channelRepository.GetChannel();

            var results = new List<ChannelDto>();

            return Ok(_mapper.Map<IEnumerable<ChannelDto>>(channelEntities));

        }

        [HttpGet("{id}", Name = "GetChannel")]
        public IActionResult GetChannel(int id)
        {   
            var channel = _channelRepository.GetChannel(id);

            if (channel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ChannelDto>(channel));
        }

        [HttpGet("{id}/oldestunsentnews")]
        public IActionResult GetOldestUnsentNewsByChannel(int id)
        {   

            if (!_channelRepository.ChannelExists(id))
            {
                return new ContentResult() { Content = $"No channel with ID {id} found.", StatusCode = 404 };
            }

            var oldestUnsentNewsByChannel = _channelRepository.GetOldestUnsentNewsByChannel(id);

            if (oldestUnsentNewsByChannel == null)
            {
                return new ContentResult() { Content = $"No unsent news for channel with ID {id} found.", StatusCode = 404 };
            }

            return Ok(_mapper.Map<NewsDto>(oldestUnsentNewsByChannel));
        }

        [HttpPost]
        public IActionResult CreateChannel([FromBody] ChannelCreationDto channel)
        {
            // Data validation is done by ApiController, according to annotations in Models
            // 500: Bad Request Response is handled by ApiController

            var finalChannel = _mapper.Map<Entities.Channel>(channel);

            _channelRepository.AddChannel(finalChannel);

            _channelRepository.Save();

            var createdChannelToReturn = _mapper
                .Map<Models.ChannelDto>(finalChannel);

            return CreatedAtRoute(
                "GetChannel",
                new { id = createdChannelToReturn.Id },
                createdChannelToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChannel(int id,
            [FromBody] ChannelUpdateDto channel)
        {
            var channelEntity = _channelRepository
                .GetChannel(id);
            if (channelEntity == null)
            {
                return NotFound();
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(channel, channelEntity);
            _channelRepository.UpdateChannel(channelEntity);
            _channelRepository.Save();

            return Ok(_mapper.Map<ChannelDto>(channelEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateChannel(int id,
            [FromBody] JsonPatchDocument<ChannelUpdateDto> patchDoc)
        {
            var channelEntity = _channelRepository
                .GetChannel(id);
            if (channelEntity == null)
            {
                return NotFound();
            }

            var channelToPatch = _mapper
                .Map<ChannelUpdateDto>(channelEntity);

            patchDoc.ApplyTo(channelToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(channelToPatch, channelEntity);
            _channelRepository.UpdateChannel(channelEntity);
            _channelRepository.Save();

            return Ok(_mapper.Map<ChannelDto>(channelEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChannel(int id)
        {

            var channelEntity = _channelRepository
                .GetChannel(id);
            if (channelEntity == null)
            {
                return NotFound();
            }

            _channelRepository.DeleteChannel(channelEntity);
            _channelRepository.Save();

            return NoContent();
        }
    }
}