using MediatR;

namespace Employees.Application.Commands
{
    public class UpdateUserFilesCommand : IRequest<bool>
    {
        public int Id { get; }
        public string FileName { get; }

        public UpdateUserFilesCommand(int id,string filename)
        {
            this.Id = id;
            this.FileName = filename;
        }
    }
}