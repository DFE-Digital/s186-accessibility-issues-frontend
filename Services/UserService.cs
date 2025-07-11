using S186Statements.Web.Models;
using System;

namespace S186Statements.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IStrapiApiService _strapiApiService;
        private readonly ILogger<UserService> _logger;

        public UserService(IStrapiApiService strapiApiService, ILogger<UserService> logger)
        {
            _strapiApiService = strapiApiService;
            _logger = logger;
        }

        public async Task<User> EnsureUserExistsAsync(string email, string firstName, string lastName, string entraId)
        {
            try
            {
                _logger.LogInformation($"Starting EnsureUserExistsAsync for email: {email}");

                // Check if user already exists by email
                _logger.LogInformation("Calling GetUserByEmailAsync...");
                var (existingUser, userId) = await _strapiApiService.GetUserByEmailAsync(email);
                _logger.LogInformation($"GetUserByEmailAsync result - User: {(existingUser != null ? "Found" : "Not found")}, ID: {userId}");

                if (existingUser != null && userId.HasValue)
                {
                    // User exists, check if we need to update their profile
                    var needsUpdate = false;
                    var updateReason = new List<string>();

                    if (!string.IsNullOrEmpty(firstName) && existingUser.FirstName != firstName)
                    {
                        needsUpdate = true;
                        updateReason.Add($"firstName: '{existingUser.FirstName}' -> '{firstName}'");
                    }

                    if (!string.IsNullOrEmpty(lastName) && existingUser.LastName != lastName)
                    {
                        needsUpdate = true;
                        updateReason.Add($"lastName: '{existingUser.LastName}' -> '{lastName}'");
                    }

                    if (!string.IsNullOrEmpty(entraId) && existingUser.EntraId != entraId)
                    {
                        needsUpdate = true;
                        updateReason.Add($"entraId: '{existingUser.EntraId}' -> '{entraId}'");
                    }

                    if (needsUpdate)
                    {
                        _logger.LogInformation($"User {email} exists, updating profile: {string.Join(", ", updateReason)}");

                        // Update the user record with only the fields that have values
                        if (!string.IsNullOrEmpty(firstName))
                            existingUser.FirstName = firstName;
                        if (!string.IsNullOrEmpty(lastName))
                            existingUser.LastName = lastName;
                        if (!string.IsNullOrEmpty(entraId))
                            existingUser.EntraId = entraId;
                        existingUser.UpdatedAt = DateTime.UtcNow;

                        // Update the user in the database
                        _logger.LogInformation("Calling UpdateUserAsync...");
                        var updatedUser = await _strapiApiService.UpdateUserAsync(userId.Value, existingUser);
                        _logger.LogInformation($"UpdateUserAsync completed successfully");
                        return updatedUser;
                    }
                    else
                    {
                        _logger.LogInformation($"User {email} profile is up to date, no changes needed");
                        return existingUser;
                    }
                }
                else
                {
                    // Create new user
                    _logger.LogInformation($"User {email} does not exist, creating new user");
                    var newUser = new User
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EntraId = entraId,
                        Username = email, // Use email as username
                        Provider = "azure-ad",
                        Confirmed = true,
                        Blocked = false,
                        Role = "authenticated", // Default role
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _logger.LogInformation("Calling CreateUserAsync...");
                    var createdUser = await _strapiApiService.CreateUserAsync(newUser);
                    _logger.LogInformation($"Created new user {email} with Entra ID {entraId}");

                    return createdUser;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error ensuring user exists for email {email}");
                throw;
            }
        }
    }
}