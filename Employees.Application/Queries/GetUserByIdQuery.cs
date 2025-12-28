using MediatR;
using Employees.Domain.UserEntity;

namespace Employees.Application.Queries
{
    public class GetUserByIdQuery : IRequest<User?>
    {
        public int Id { get; }

        public GetUserByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}