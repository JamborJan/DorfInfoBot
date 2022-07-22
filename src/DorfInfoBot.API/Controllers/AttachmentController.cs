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

        [HttpGet]
        public IActionResult GetAttachment(int newsId)
        {
            try
            {
                
                if (!_newsRepository.NewsExists(newsId))
                {
                    _logger.LogInformation($"News with id {newsId} wasn't found when accessing attachments.");
                    return NotFound();
                }

                var attachmentForNews = _newsRepository.GetAttachmentForNews(newsId);
                return Ok(_mapper.Map<IEnumerable<AttachmentDto>>(attachmentForNews));
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
