using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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
    [Route("api/news/{newsId}/attachment")]
    public class AttachmentController : ControllerBase
    {
        private readonly ILogger<AttachmentController> _logger;
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public AttachmentController(ILogger<AttachmentController> logger, 
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

        [HttpGet(Name = "GetAllAttachments")]
        [HttpHead]
        public IActionResult GetAttachment(int newsId , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                
                if (!_newsRepository.NewsExists(newsId))
                {
                    _logger.LogInformation($"News with id {newsId} wasn't found when accessing attachments.");
                    return NotFound();
                }

                var attachmentForNews = _newsRepository.GetAttachmentForNews(newsId);

                // Calculate the number of items to skip and take
                var itemsToSkip = (pageNumber - 1) * pageSize;
                var itemsToTake = pageSize;

                // Apply pagination to the data
                var pagedData = attachmentForNews.Skip(itemsToSkip).Take(itemsToTake);
                
                var results = new List<AttachmentDto>();

                // Map the paged data to DTOs
                results = (List<AttachmentDto>)_mapper.Map<IEnumerable<AttachmentDto>>(pagedData);

                // Create a pagination header
                var paginationHeader = new
                {
                    currentPage = pageNumber,
                    pageSize = pageSize,
                    totalCount = attachmentForNews.Count(),
                    totalPages = (int)Math.Ceiling((double)attachmentForNews.Count() / pageSize)
                };

                // Add pagination metadata to the response header
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

                // Get the total count of attachments entities
                var count = attachmentForNews.Count();
                // create a count object
                var countObject = new { count = count };
                // Add the count object to the response header
                Response.Headers.Add("X-Count", JsonConvert.SerializeObject(countObject));

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting attachments for news with id {newsId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetAttachment")]
        public IActionResult GetAttachment(int newsId, int id)
        {
            if (!_newsRepository.NewsExists(newsId))
            {
                return NotFound();
            }

            var attachment = _newsRepository.GetAttachmentForNews(newsId, id);

            if (attachment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AttachmentDto>(attachment));
        }

        [HttpPost]
        public IActionResult CreateAttachment(int newsId,
            [FromBody] AttachmentCreationDto attachment)
        {
            
            // Data validation is done by ApiController, according to annotations in Models
            // 500: Bad Request Response is handled by ApiController

            // 404: check if news entry is there before try to add an attachment
            if (!_newsRepository.NewsExists(newsId))
            {
                return NotFound();
            }

            var finalAttachment = _mapper.Map<Entities.Attachment>(attachment);

            _newsRepository.AddAttachmentForNews(newsId, finalAttachment);

            _newsRepository.Save();

            var createdAttachmentToReturn = _mapper
                .Map<Models.AttachmentDto>(finalAttachment);

            return CreatedAtRoute(
                "GetAttachment",
                new { newsId, id = createdAttachmentToReturn.Id },
                createdAttachmentToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAttachment(int newsId, int id,
            [FromBody] AttachmentUpdateDto attachment)
        {

            if (!_newsRepository.NewsExists(newsId))
            {
                return NotFound();
            }

            var attachmentEntity = _newsRepository
                .GetAttachmentForNews(newsId,id);
            if (attachmentEntity == null)
            {
                return NotFound();
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(attachment, attachmentEntity);
            _newsRepository.UpdateAttachmentForNews(newsId, attachmentEntity);
            _newsRepository.Save();

            return Ok(_mapper.Map<AttachmentDto>(attachmentEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateAttachment(int newsId, int id,
            [FromBody] JsonPatchDocument<AttachmentUpdateDto> patchDoc)
        {
            if (!_newsRepository.NewsExists(newsId))
            {
                return NotFound();
            }

            var attachmentEntity = _newsRepository
                .GetAttachmentForNews(newsId,id);
            if (attachmentEntity == null)
            {
                return NotFound();
            }

            var attachmentToPatch = _mapper
                .Map<AttachmentUpdateDto>(attachmentEntity);

            patchDoc.ApplyTo(attachmentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Auto mapper overwrites destination object with source
            _mapper.Map(attachmentToPatch, attachmentEntity);
            _newsRepository.UpdateAttachmentForNews(newsId, attachmentEntity);
            _newsRepository.Save();

            return Ok(_mapper.Map<AttachmentDto>(attachmentEntity)); // I rather like that for automated consumer processes
            // return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttachment(int newsId, int id)
        {
            if (!_newsRepository.NewsExists(newsId))
            {
                return NotFound();
            }

            var attachmentEntity = _newsRepository
                .GetAttachmentForNews(newsId,id);
            if (attachmentEntity == null)
            {
                return NotFound();
            }

            _newsRepository.DeleteAttachment(attachmentEntity);
            _newsRepository.Save();

            return NoContent();
        }
    }
}
