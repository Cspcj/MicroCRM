using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Repositories
{
    public interface IClientRepository
    {
        // create client repository interface
        Task<IEnumerable<ClientModel>> GetClientsAsync();
        Task<ClientModel> GetClientByIdAsync(Guid id);
        Task CreateClientAsync(ClientModel client);
        Task<ClientModel> UpdateClientAsync(ClientModel client);
        Task<ClientModel> DeleteClientAsync(Guid id);
        Task<ClientModel> GetClientByIdentityUserIdAsync(Guid userId);
    }
}
