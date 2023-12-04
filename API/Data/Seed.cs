using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                await context.Users.AddAsync(user);

                // Initialize the Photos collection if it's null
                user.Photos ??= new List<Photo>();

                // Check if the user has photos
                if (user.Photos.Any())
                {
                    foreach (var photo in user.Photos)
                    {
                        // Ensure each photo has a URL
                        if (string.IsNullOrEmpty(photo.Url))
                        {
                            // Set a default URL or handle accordingly
                            // photo.Url = "default_photo_url.jpg";
                        }
                    }
                }
            }
            await context.SaveChangesAsync();


        }
    }


}