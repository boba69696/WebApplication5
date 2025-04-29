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
                if (string.IsNullOrEmpty(userData.Name) ||
                    string.IsNullOrEmpty(userData.Login) ||
                    string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Name, login and password are required");
                }

                User newUser = new User
                {
                    Name = userData.Name,
                    Login = userData.Login,
                    Password = userData.Password,
                    Age = userData.Age
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
                if (userData.Id <= 0 ||
                    string.IsNullOrEmpty(userData.Name) ||
                    string.IsNullOrEmpty(userData.Login) ||
                    string.IsNullOrEmpty(userData.Password))
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

                existingUser.Name = userData.Name;
                existingUser.Login = userData.Login;
                existingUser.Password = userData.Password;
                existingUser.Age = userData.Age;

                await existingUser.Update<User>();

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

                await userToDelete.Delete<User>(); // Явное указание типа
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Добавляем в WeatherForecastController.cs

        // Получение списка всех городов
        [HttpGet("GetAllCities", Name = "GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var result = await _supabaseContext.GetAllCities(_supabaseClient);
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
        [HttpPost("InsertCity", Name = "InsertCity")]
        public async Task<ActionResult> InsertCity([FromBody] CityData cityData)
        {
            try
            {
                if (string.IsNullOrEmpty(cityData.Name) || cityData.Population <= 0)
                {
                    return BadRequest("Название города и население обязательны");
                }

                City newCity = new City
                {
                    Name = cityData.Name,
                    Population = cityData.Population,
                    CreatedAt = DateTime.UtcNow
                };

                bool result = await _supabaseContext.InsertCity(_supabaseClient, newCity);
                return result ? Ok("Город успешно добавлен") : BadRequest("Ошибка при добавлении города");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("UpdateCity", Name = "UpdateCity")]
        public async Task<ActionResult> UpdateCity([FromBody] CityUpdateData cityData)
        {
            try
            {
                if (cityData.Id <= 0 || string.IsNullOrEmpty(cityData.Name) || cityData.Population <= 0)
                {
                    return BadRequest("Некорректные данные для обновления");
                }

                var existingCity = await _supabaseClient.From<City>()
                    .Where(x => x.Id == cityData.Id)
                    .Single();

                if (existingCity == null)
                {
                    return NotFound("Город не найден");
                }

                existingCity.Name = cityData.Name;
                existingCity.Population = cityData.Population;
                await existingCity.Update<City>();

                return Ok("Данные города успешно обновлены");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
        [HttpDelete("DeleteCity", Name = "DeleteCity")]
        public async Task<ActionResult> DeleteCity(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Некорректный ID города");
                }

                bool result = await _supabaseContext.DeleteCity(_supabaseClient, id);

                if (!result)
                {
                    return NotFound("Город не найден или ошибка при удалении");
                }

                return Ok("Город успешно удален");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }

    public class UserData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }
    }
    public class CityData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }
    }

    public class CityUpdateData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }
    }
}