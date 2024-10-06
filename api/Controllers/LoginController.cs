using api.Helpers;
using entities;
using entities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using repository.Interface;
using System.Net;

namespace api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string Module = "Login";
        private readonly IUnitOfWork _iuw;
        private readonly TokenService _tokenService;

        private readonly ILogger<LoginController> _logger;

        public LoginController(IUnitOfWork iuw, TokenService tokenService, ILogger<LoginController> logger)
        {
            _iuw = iuw;
            _logger = logger;
            _tokenService = tokenService;
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(LoginViewModel login)
        {

            if (login == null)
                return BadRequest();

            var loginResult = new LoginResultViewModel();
            try
            {
                LoginHelper loginHelper = new LoginHelper(_iuw);

                User? user = loginHelper.GetPermissions(login);

                if (user == null || user.Roles.Count == 0)
                {
                    loginResult.Successful = false;
                    loginResult.Error = "Invalid username or password, or you don't have access to the application";
                    loginResult.Token = "";

                    _logger.LogInformation($"Invalid login attempt by user {login}");
                    return Unauthorized(loginResult);
                }


                loginResult.Token = _tokenService.CreateToken(user);
                loginResult.Successful = true;
                loginResult.GenerationDate = DateTime.Now;

                _logger.LogInformation($"Login performed by user {login}");
            }
            catch (Exception e)
            {
                loginResult.Successful = false;
                loginResult.Error = e.ToString();
                loginResult.Token = "";

                _logger.LogError(e, "Error during login");

                return new JsonResult(loginResult);
            }

            return new JsonResult(loginResult);
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

                var result = _iuw.Login.GetAll(filter);

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
                var entity = _iuw.Login.Get(id);

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

        [HttpPost]
        [Route("Add")]
        public JsonResult Post(Login entity)
        {
            string method = "Post";
            try
            {
                entity.RecordUser = User?.Identity?.Name;

                _iuw.Login.Add(entity);
                _iuw.Login.SaveChanges();

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
        public ActionResult Put(Login entity)
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

                _iuw.Login.Update(entity);
                _iuw.Login.SaveChanges();

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
