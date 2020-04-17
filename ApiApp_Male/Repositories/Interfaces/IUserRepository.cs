using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;

namespace ApiApp_Male.Repositories
{
    public interface IUserRepository
    {
        AuthResult Registration(UserAddDTO userDTO);
        AuthResult UserLogin(UserLoginDTO loginDTO);
    }
}