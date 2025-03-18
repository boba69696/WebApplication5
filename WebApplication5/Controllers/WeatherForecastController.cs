using IOTMPO.ExecutiveUnits.DataUserUnit;
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
        public async Task<string> GetAllUsers()
        {
            try
            {
                var result = await _supabaseContext.GetUser(_supabaseClient);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [HttpPost("InsertUser", Name = "InsertUser")]
        public async Task<ActionResult> InsertUser([FromBody] UserData userData)
        {
            try
            {
                if (string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Или логин или пароль пустой");
                }
               
            

                    User newUser = new User
                    {
                        Name = userData.Login,
                        Password = userData.Password,
                    };
                    bool result = await _supabaseContext.InsertUser(_supabaseClient, newUser);
                    if (result == true)
                    {
                        return Ok("Регистрация прошла успешно");
                    }
                    else
                    {
                        return BadRequest("Не удалось добавить пользователя в БД");
                    }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Неизвестная ошибка");
            }
        }
}
       /* [HttpPut("UpdateUserName", Name = "UpdateUserName")]
        public async Task<ActionResult> UpdateUserName([FromBody] UserData userData)
        {


        }*/
             


          
    }
    public class UserData
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    public class UserDataOnUpdate
    {

    }





