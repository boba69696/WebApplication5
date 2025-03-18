using System.Security;

namespace WebApplication5
{
    public class SupaBaseContext
    {

    
        public async Task<List<User>> GetUser(Supabase.Client _supabaseClient)
        {
            var ruesult = await _supabaseClient.From<User>().Get();
            return ruesult.Models;
        }

        public async Task<bool> InsertUser(Supabase.Client _supabaseClient, User user)
        {
            try
            {
                await _supabaseClient.From<User>().Insert(user);
                return true;
            }
            catch
            {
                return false;
            }
   
        }
        public async Task<bool> UpdateUserName(Supabase.Client _supabaseClient, int id, string newName, string Oldname)
        {
            //Логика метода
            return true;

        }
    } 
}