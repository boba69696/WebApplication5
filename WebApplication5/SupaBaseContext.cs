using System.Security;

namespace WebApplication5
{
    public class SupaBaseContext
    {
        public async Task<List<User>> GetUser(Supabase.Client _supabaseClient)
        {
            var result = await _supabaseClient.From<User>().Get();
            return result.Models;
        }

        public async Task<bool> InsertUser(Supabase.Client _supabaseClient, User user)
        {
            try
            {
                var response = await _supabaseClient.From<User>().Insert(user);
                return response.ResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(Supabase.Client _supabaseClient, int id, string newLogin, string newPassword, string newAge)
        {
            try
            {
                var user = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id) // Здесь x.Id должен соответствовать имени в БД
                    .Single();

                if (user != null)
                {
                    user.Login = newLogin;
                    user.Password = newPassword;
                    user.Age = newAge;
                    await user.Update<User>();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUser(Supabase.Client _supabaseClient, int id)
        {
            try
            {
                var user = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id)
                    .Single();

                if (user != null)
                {
                    await user.Delete<User>(); // Явное указание типа
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}