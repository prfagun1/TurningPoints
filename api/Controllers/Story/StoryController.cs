using entities;
using entities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using repository.Interface;
using System.Net;

namespace api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly string Module = "Story";
        private readonly IUnitOfWork _iuw;
        private readonly ILogger<StoryController> _logger;

        public StoryController(IUnitOfWork iuw, ILogger<StoryController> logger)
        {
            _iuw = iuw;
            _logger = logger;
        }


        [EnableRateLimiting("Default")]
        [HttpPost]
        [Route("GetListOpen")]
        public ActionResult GetListOpen(SearchFilter filter)
        {
            string method = "GetListOpen";
            try
            {
                filter.PageSize = filter.PageSize != 0 ? filter.PageSize : _iuw.ParameterInternal.GetItemsPerPage();
                _logger.LogInformation($"{nameof(Get)}: {JsonConvert.SerializeObject(filter)} returned");

                var result = _iuw.Story.GetAllOpen(filter);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                string message = $"Error retrieving list, {e}";
                _logger.LogError(e, $"{nameof(Get)}: Error retrieving entity");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }


        [EnableRateLimiting("Default")]
        [HttpGet]
        [Route("GetOpen")]
        public ActionResult GetOpen(Guid id)
        {
            string method = "GetOpen";

            try
            {
                var entity = _iuw.Story.Get(id);

                _logger.LogInformation($"{nameof(Get)}: {JsonConvert.SerializeObject(entity)} returned");

                return new JsonResult(entity);
            }
            catch (Exception e)
            {
                string message = $"Error retrieving parameter: {e}";

                _logger.LogError(e, $"{nameof(Get)}: Error retrieving entity");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }


        [EnableRateLimiting("Default")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("GetList")]
        public ActionResult GetList(SearchFilter filter)
        {
            string method = "GetList";
            try
            {
                filter.PageSize = filter.PageSize != 0 ? filter.PageSize : _iuw.ParameterInternal.GetItemsPerPage();
                _logger.LogInformation($"{nameof(Get)}: {JsonConvert.SerializeObject(filter)} returned");

                var result = _iuw.Story.GetAll(filter);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                string message = $"Error retrieving list, {e}";
                _logger.LogError(e, $"{nameof(Get)}: Error retrieving entity");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }

        [EnableRateLimiting("Default")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Get(Guid id)
        {
            string method = "Get";

            try
            {
                var entity = _iuw.Story.Get(id);

                _logger.LogInformation($"{nameof(Get)}: {JsonConvert.SerializeObject(entity)} returned");

                return new JsonResult(entity);
            }
            catch (Exception e)
            {
                string message = $"Error retrieving parameter: {e}";

                _logger.LogError(e, $"{nameof(Get)}: Error retrieving entity");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }


        [EnableRateLimiting("Default")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Post(Story entity)
        {
            string method = "Post";
            try
            {
                entity.RecordUser = User?.Identity?.Name;
                entity.RecordDate = DateTime.Now;

                _iuw.Story.Add(entity);
                _iuw.Story.SaveChanges();

                _logger.LogDebug($"{nameof(Post)}: {JsonConvert.SerializeObject(entity)} created");

                return new JsonResult(entity);
            }
            catch (Exception e)
            {
                string message = $"Error creating initial parameters: {e}";

                _logger.LogError(e, $"{nameof(Get)}: {message}");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }


        [EnableRateLimiting("Default")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Put(Story entity)
        {
            string method = "Put";
            try
            {
                if (entity == null)
                {
                    string message = $"Attempted to update a record that does not exist in the database.";

                    Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
                }

                entity.RecordUser = User?.Identity?.Name;
                entity.RecordDate = DateTime.Now;

                _iuw.Story.Update(entity);
                _iuw.Story.SaveChanges();

                _logger.LogDebug($"{nameof(Put)}: {JsonConvert.SerializeObject(entity)} updated");

                return new JsonResult(entity);
            }
            catch (Exception e)
            {
                string message = $"Error updating the record with id {entity.Id}: {e}";

                _logger.LogError(e, $"{nameof(Get)}: {message}");

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }



        [EnableRateLimiting("Default")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            string method = "Delete";
            try
            {
                var entity = _iuw.Story.Get(id);
                if (entity == null)
                {
                    string message = $"Attempt to delete the record {id} that does not exist in the database.";
                    _logger.LogTrace($"{nameof(Get)}: {message}");

                    Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
                }

                _iuw.Story.Delete(entity);
                _iuw.Story.SaveChanges();

                _logger.LogDebug($"{nameof(Put)}: {JsonConvert.SerializeObject(entity)} deleted");

                return new JsonResult(entity);
            }
            catch (Exception e)
            {
                string message = $"Error updating the record with id {id}";
                _logger.LogError(e, $"{nameof(Get)}: {message} - {e}");
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new ApiErrorReturn(message, DateTime.Now, $"{Module}, Method: {method}"));
            }
        }
    }
}
