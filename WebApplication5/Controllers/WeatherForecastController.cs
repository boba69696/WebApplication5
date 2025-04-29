using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly SupaBaseContext _supabaseContext;

        public WeatherForecastController(Supabase.Client supabaseClient, SupaBaseContext supaBaseContext)
        {
            _supabaseClient = supabaseClient;
            _supabaseContext = supaBaseContext;
        }

        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _supabaseContext.GetUser(_supabaseClient);
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("InsertUser", Name = "InsertUser")]
        public async Task<ActionResult> InsertUser([FromBody] UserData userData)
        {
            try
            {
                if (string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Login and password are required");
                }

                User newUser = new User
                {
                    Login = userData.Login,
                    Password = userData.Password,
                    Age = userData.Age // ƒобавлено поле Age
                };

                bool result = await _supabaseContext.InsertUser(_supabaseClient, newUser);
                return result ? Ok("User registered successfully") : BadRequest("Failed to add user to database");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser", Name = "UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateData userData)
        {
            try
            {
                if (userData.Id <= 0 || string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Invalid data for update");
                }

                var existingUser = await _supabaseClient.From<User>()
                    .Where(x => x.Id == userData.Id)
                    .Single();

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                existingUser.Login = userData.Login;
                existingUser.Password = userData.Password;
                existingUser.Age = userData.Age;

                await existingUser.Update<User>(); // явное указание типа

                return Ok("User data updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteUser/{id}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user ID");
                }

                var userToDelete = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id)
                    .Single();

                if (userToDelete == null)
                {
                    return NotFound("User not found");
                }

                await userToDelete.Delete<User>(); // явное указание типа
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class UserData
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("age")]
        public string Age { get; set; }
    }

    public class UserUpdateData
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("age")]
        public string Age { get; set; }
    }
}