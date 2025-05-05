using System.Text.Json;
using EcommerceApp.Features.Users.Queries;
using EcommerceApp.Models;
using EcommerceApp.Models.DTOs;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Users.Handlers;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserProfileDto>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public GetUserHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<UserProfileDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.UserId}";

        return await _cache.GetOrSetAsync<UserProfileDto>(
            cacheKey,
            async () =>
            {
                var users = _mongo.GetCollection<User>("Users");
                var user = await users.Find(x => x.Id == request.UserId).FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("User not found");

                return new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Rating = user.Rating
                };
            },
            TimeSpan.FromMinutes(10)
        );
    }
}