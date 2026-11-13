using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using DorfInfoBot.API.Models;
using DorfInfoBot.API.Services;
using Newtonsoft.Json;

namespace DorfInfoBot.API.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsController(ILogger<NewsController> logger, 
            INewsRepository newsRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _newsRepository = newsRepository ?? 
                throw new ArgumentNullException(nameof(newsRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        
        [HttpGet(Name = "GetAllNews")]
        [HttpHead]
        public IActionResult GetNews([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var newsEntities = _newsRepository.GetNews();

            // Calculate the number of items to skip and take
            var itemsToSkip = (pageNumber - 1) * pageSize;
            var itemsToTake = pageSize;

            // Apply pagination to the data
            var pagedData = newsEntities.Skip(itemsToSkip).Take(itemsToTake);

            var results = new List<NewsWithoutAttachmentDto>();

            // Map the paged data to DTOs
            results = (List<NewsWithoutAttachmentDto>)_mapper.Map<IEnumerable<NewsWithoutAttachmentDto>>(pagedData);

            // Create a pagination header
            var paginationHeader = new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalCount = newsEntities.Count(),
                totalPages = (int)Math.Ceiling((double)newsEntities.Count() / pageSize)
            };

            // Add the pagination header to the response headers
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

            // Get the total count of news entities
            var count = newsEntities.Count();
            // Create a count object
            var countObject = new { count = count };
            // Add the count object to the response headers
            Response.Headers.Add("X-Total-Count", JsonConvert.SerializeObject(countObject));
            
            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetOneNews")]
        public IActionResult GetNews(int id, bool includeAttachments = false)
        {   
            var news = _newsRepository.GetNews(id, includeAttachments);

            if (news == null)
            {
                return NotFound();
            }

            if (includeAttachments)
            {
                return Ok(_mapper.Map<NewsDto>(news));
            } 
            else
            {
                return Ok(_mapper.Map<NewsWithoutAttachmentDto>(news));
            }
        }

        [HttpGet("{id}/broadcasts")]
        public IActionResult GetBroadcastsForNews(int id)
        {   
            if (!_newsRepository.NewsExists(id))
            {
                return NotFound();
            }

            var broadcasts = _newsRepository.GetBroadcastsForNews(id);

            if (broadcasts == null || broadcasts.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BroadcastDto>>(broadcasts));
        }

        [HttpPost]
        public IActionResult CreateNews([FromBody] NewsCreationDto news)
        {
            // Data validation is done by ApiController, according to annotations in Models
            // 500: Bad Request Response is handled by ApiController

            var finalNews = _mapper.Map<Entities.News>(news);

            // Check, if there is already a news entry identified by the external key
            // If one exists, don't create new news entry, answer with the existing one
            var newsEntity = _newsRepository
                .NewsExistsByExternalKey(news.ExternalKey);
            if (newsEntity != null)
            {
                return Ok(_mapper.Map<NewsDto>(newsEntity));
            }

            _newsRepository.AddNews(finalNews);

            _newsRepository.Save();

            var createdNewsToReturn = _mapper
                .Map<Models.NewsDto>(finalNews);

            return CreatedAtRoute(
                "GetNews",
                new { id = createdNewsToReturn.Id },
                createdNewsToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNews(int id,
            [FromBody] NewsUpdateDto news,
            bool includeAttachments = false)
        {
            var newsEntity = _newsRepository
                .GetNews(id, includeAttachments);
            if (newsEntity == null)
            {
                return NotFound();
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(news, newsEntity);
            _newsRepository.UpdateNews(newsEntity);
            _newsRepository.Save();

            return Ok(_mapper.Map<NewsDto>(newsEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateNews(int id,
            [FromBody] JsonPatchDocument<NewsUpdateDto> patchDoc,
            bool includeAttachments = false)
        {
            var newsEntity = _newsRepository
                .GetNews(id,includeAttachments);
            if (newsEntity == null)
            {
                return NotFound();
            }

            var newsToPatch = _mapper
                .Map<NewsUpdateDto>(newsEntity);

            patchDoc.ApplyTo(newsToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(newsToPatch, newsEntity);
            _newsRepository.UpdateNews(newsEntity);
            _newsRepository.Save();

            return Ok(_mapper.Map<NewsDto>(newsEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNews(int id)
        {

            var newsEntity = _newsRepository
                .GetNews(id, false);
            if (newsEntity == null)
            {
                return NotFound();
            }

            _newsRepository.DeleteNews(newsEntity);
            _newsRepository.Save();

            return NoContent();
        }
    }
}