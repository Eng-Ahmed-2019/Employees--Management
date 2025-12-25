using MediatR;

namespace Employees.Application.Queries
{
    public class GetUserByIdQuery : IRequest<string?>
    {
        public int Id { get; }
        public bool IsPdf { get; }

        public GetUserByIdQuery(int id, bool isPdf)
        {
            this.Id = id;
            this.IsPdf = isPdf;
        }
    }
}