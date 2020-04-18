using ApiApp_Male.Models;
using ApiApp_Male.Models.entities;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp_Male.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryContext dbcontext;
        private readonly IMapper map;
        private readonly IConfiguration config;

        public UserRepository(LibraryContext dbcontext, IMapper map,IConfiguration config)
        {
            this.dbcontext = dbcontext;
            this.map = map;
            this.config = config;
        }

        public AuthResult Registration(UserAddDTO userDTO)
        {
            if (dbcontext.Users.Any(x => x.UserName.Equals(userDTO.UserName)))
            {
                return new AuthResult { Success = false, ErrorCode = "Usr001" };
            }
            if (dbcontext.Users.Any(x => x.Email.Equals(userDTO.Email)))
            {
                return new AuthResult { Success = false, ErrorCode = "Usr002" };
            }

            var CurrentUser = map.Map<Users>(userDTO);
            byte[] hash, salt;
            GererateHash(userDTO.Password, out hash, out salt);
            CurrentUser.Passwordhash = hash;
            CurrentUser.Passwordsalt = salt;

            dbcontext.Users.Add(CurrentUser);

            dbcontext.SaveChanges();

            return new AuthResult { Success = true, UserId = CurrentUser.UserId, UserName = CurrentUser.UserName };
        }

        public AuthResult UserLogin(UserLoginDTO loginDTO)
        {
            var CurUser=dbcontext.Users.Where(x => x.UserName.Equals(loginDTO.UserName)).SingleOrDefault();
            if(CurUser == null){
                return new AuthResult { Success = false, ErrorCode = "Usr003" };
            }

            if(!ValidateHash(loginDTO.Password, CurUser.Passwordhash, CurUser.Passwordsalt))
            {
                return new AuthResult { Success = false, ErrorCode = "Usr004" };
            }
            //generate JWT Token
            var key = config.GetValue<String>("JWTSecret");
            var KeyByte = Encoding.ASCII.GetBytes(key);
            var Desc = new SecurityTokenDescriptor { 
                    Expires=DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(KeyByte),SecurityAlgorithms.HmacSha512Signature),
                    Subject=new ClaimsIdentity(
                        new Claim[] { 
                            new Claim(JwtRegisteredClaimNames.Sub,CurUser.UserName),
                            new Claim(JwtRegisteredClaimNames.Email,CurUser.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim("UserId",CurUser.UserId)
                        }
                        )
            };
            var handler = new JwtSecurityTokenHandler();
            var token=handler.CreateToken(Desc);


            return new AuthResult { Success=true,UserId=CurUser.UserId,UserName=CurUser.UserName,
                                        Token= handler.WriteToken(token)
            };

        }

        private Boolean ValidateHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using(var hash=new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes( password));
                for (int i = 0; i < newPassHash.Length; i++)
                    if (newPassHash[i] != passwordhash[i])
                        return false;
            }
            return true;
        }

        public void GererateHash(String Password,out byte[] PasswordHash,out byte[] PasswordSalt)
        {
            using(var hash=new System.Security.Cryptography.HMACSHA512())
            {
                PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
                PasswordSalt = hash.Key;
            }
        }
    }
}
