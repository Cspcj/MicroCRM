using MicroCRM.Data;
using MicroCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace MicroCRM.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IClientRepository> _logger;

        public ClientRepository(ILogger<IClientRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<ClientModel>> GetClientsAsync()
        {
            _logger.LogInformation("GetListAsync triggered");
            return await _context.Clients.ToListAsync();
        }
        public async Task<ClientModel> GetClientByIdAsync(Guid id)
        {
            _logger.LogInformation("GetClientAsync triggered");
            return await _context.Clients.FirstOrDefaultAsync(x => x.ClientID == id);
        }

        public async Task CreateClientAsync(ClientModel client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }


        public async Task<ClientModel> DeleteClientAsync(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientID == id);
            if (client != null)
            {
                _context.Remove(client);
                await _context.SaveChangesAsync();
                return client;
            }
            return null;
        }

        public async Task<bool> ClientExistsAsync(ClientModel Client)
        {
            return await _context.Clients.AnyAsync(x => x.ClientID == Client.ClientID);
        }

        public async Task<ClientModel> UpdateClientAsync(ClientModel client)
        {
            if (client != null)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return client;
            }

            return null;
        }

        public async Task<ClientModel> GetClientByIdentityUserIdAsync(Guid userId)
        {
            _logger.LogInformation("GetClientAsync triggered");
            return await _context.Clients.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}

