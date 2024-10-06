
using entities;
using entities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using repository.Interface;
using System.Net;

namespace api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ParameterInternalController : ControllerBase
    {
        private readonly string Module = "Parameter";
        private readonly IUnitOfWork _iuw;
        private readonly ILogger<ParameterInternalController> _logger;

        public ParameterInternalController(IUnitOfWork iuw, ILogger<ParameterInternalController> logger)
        {
            _iuw = iuw;
            _logger = logger;
        }

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

                var result = _iuw.ParameterInternal.GetAll(filter);

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Get(Guid id)
        {
            string method = "Get";

            try
            {
                var entity = _iuw.ParameterInternal.Get(id);

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Post(ParameterInternal entity)
        {
            string method = "Post";
            try
            {
                entity.RecordUser = User?.Identity?.Name;
                entity.RecordDate = DateTime.Now;

                _iuw.ParameterInternal.Add(entity);
                _iuw.ParameterInternal.SaveChanges();

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

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Put(ParameterInternal entity)
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

                _iuw.ParameterInternal.Update(entity);
                _iuw.ParameterInternal.SaveChanges();

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
    }
}
