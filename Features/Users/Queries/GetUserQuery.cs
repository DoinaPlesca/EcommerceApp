using EcommerceApp.Models.DTOs;
using MediatR;

namespace EcommerceApp.Features.Users.Queries;

public class GetUserQuery : IRequest<UserProfileDto>
{
    public string UserId { get; set; }
}