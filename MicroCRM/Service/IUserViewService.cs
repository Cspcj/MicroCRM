using MicroCRM.ViewModels;


namespace MicroCRM.Service
{
    public interface IUserViewService
    {
        Task<IEnumerable<UserViewModel>> GetUsersAsync();
        Task CreateNewUser(string userEmail, string phoneNumber, string RoleId);
        Task<UserViewModel> GetUserByIdAsync(Guid id);
        //Task<IdentityUser> UpdateUserAsync(IdentityUser user);
        //Task<IdentityUser> CreateUserAsync(IdentityUser user);
        //Task<IdentityUser> DeleteUserAsync(Guid id);
    }
}
