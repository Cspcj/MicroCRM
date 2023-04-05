using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroCRM.Service
{
    public interface IClientService
    {
        // create client service interface
        Task<IEnumerable<ClientModel>> GetClientsAsync();
        Task<ClientModel> GetClientByIdAsync(Guid id);
        Task<ClientModel> UpdateClientAsync(ClientModel client);
        Task<ClientModel> CreateNewClientAsync(ClientModel client);
        Task<ClientModel> CreateClientFromExistingAsync(ClientModel client, string id);
        Task<ClientModel> DeleteClientAsync(Guid id);

        Task<ClientModel> GetClientByIdentityUserIdAsync(Guid userId);
    }
}
