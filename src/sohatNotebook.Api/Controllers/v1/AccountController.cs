using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SohatNotebook.Authentication.Configuration;
using SohatNotebook.Authentication.Models.Generic;
using SohatNotebook.Authentication.Models.Incomming;
using SohatNotebook.Authentication.Models.Outgoing;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sohatNotebook.Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public AccountController(
            IUnitOfWork unitOfWork,
            IOptions<JwtConfig> options,
            TokenValidationParameters tokenValidationParameters,
            UserManager<IdentityUser> userManager,
            IMapper mapper) : base(unitOfWork, userManager, mapper) 
        {
            _jwtConfig = options.Value;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserRegistrationResponseDTO
                {
                    Success = false,
                    Errors = new List<string>([
                        "Input Model is not valid"
                    ])
                });
            }

            var userExists = await _userManager.FindByEmailAsync(userRegistration.Email);
            if (userExists != null) 
            {
                return BadRequest(new UserRegistrationResponseDTO 
                {
                    Success = false,
                    Errors = new List<string> 
                    {
                        "Email Already Exists"
                    }
                });
            }

            // Add new user
            var newUser = new IdentityUser()
            {
                Email = userRegistration.Email,
                UserName = userRegistration.Email
            };
            var isCreated = await _userManager.CreateAsync(newUser, userRegistration.Password);
            if (!isCreated.Succeeded)
            {
                return BadRequest(new UserRegistrationResponseDTO
                {
                    Success = isCreated.Succeeded,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                });
            }

            var _user = new User();
            _user.Id = new Guid(newUser.Id);
            _user.FirstName = userRegistration.FirstName;
            _user.LastName = userRegistration.LastName;
            _user.Email = userRegistration.Email;
            _user.Phone = "";
            _user.Country = "";
            _user.Address = "";
            _user.MobileNumber = "";
            _user.Sex = "";
            _user.Datebirth = DateTime.UtcNow;
            _user.Status = 1;
            await _unitOfWork.Users.Add(_user);
            await _unitOfWork.CompleteAsync();

            // generate jwt token
            var token = await this.GenerateJwtToken(newUser);

            var response = new UserRegistrationResponseDTO
            {
                Success = true,
                Token = token.Token,
                RefreshToken = token.RefreshToken
            };
            return Ok(response);

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequestDTO userLogin)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new UserLoginResponseDTO
                {
                    Success = false,
                    Errors = new List<string>([
                        "Invalid payload"
                    ])
                });
            }

            var _user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (_user == null)
            {
                return BadRequest(new UserLoginResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> {
                        "Email or Password is incorrect"
                    }
                });
            }

            var isCorrect = await _userManager.CheckPasswordAsync(_user, userLogin.Password);

            if (!isCorrect) 
            {
                return BadRequest(new UserLoginResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> {
                        "Email or Password is incorrect"
                    }
                });
            }

            // genrate token
            var jwtToken = await GenerateJwtToken(_user);

            var respoonse = new UserLoginResponseDTO 
            {
                Success = true,
                Token = jwtToken.Token,
                RefreshToken = jwtToken.RefreshToken,
            };

            return Ok(respoonse);
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new UserLoginResponseDTO
                {
                    Success = false,
                    Errors = new List<string>([
                        "Invalid payload"
                    ])
                });
            }

            var result = await ValidateToken(refreshTokenRequest);

            if(result == null)
            {
                return BadRequest(new AuthResult
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Unable to generate token"
                    }
                }); 
            }

            return Ok(result);
        }

        private async Task<AuthResult> ValidateToken(RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var principle = jwtTokenHandler.ValidateToken(refreshTokenRequest.Token, _tokenValidationParameters, out var ValidToken);

                if (ValidToken is JwtSecurityToken securityToken)
                {
                    var hasCorrectAlgo = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!hasCorrectAlgo) 
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Token is not valid"
                            }
                        };
                    }

                    // check if jwt token is still valid
                    var expDate = securityToken.Payload.Expiration;
                    if (expDate == null) 
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Token is not valid"
                            }
                        };
                    }
                    var expDateTime = DateTimeOffset.FromUnixTimeSeconds((long)expDate).UtcDateTime;
                    if (expDateTime > DateTime.UtcNow)
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "JWT Token is still valid, so cannot be regenerated"
                            }
                        };
                    }

                    // check if refresh token is valid

                    var refreshTokenData = await _unitOfWork.RefreshTokens.GetByRefreshToken(refreshTokenRequest.RefreshToken);
                    if (refreshTokenData == null) 
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh Token is not valid"
                            }
                        };
                    }

                    //check if refreshToken is belongs to given jwt token
                    if(refreshTokenData.JwtId != securityToken.Id)
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh Token does not belong to jwt token"
                            }
                        };
                    }

                    // check if refresh token is still valid

                    if (refreshTokenData.Expiry < DateTime.UtcNow)
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh Token expired, Make sure to Login again"
                            }
                        };
                    }

                    // check if token is used or not

                    if(refreshTokenData.IsUsed == true)
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh Token is used, login again"
                            }
                        };
                    }

                    // check if token is revoked

                    if(refreshTokenData.IsRevocked == true)
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh Token is revoked, login again"
                            }
                        };
                    }

                    // fetch user by email

                    var user = await _userManager.FindByIdAsync(principle.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value);
                    if (user == null) 
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "User data is not valid"
                            }
                        };
                    }

                    // now we are sure that everything is good, so generate new token.
                    var tokens = await GenerateJwtToken(user);

                    var response = new AuthResult()
                    {
                        Success = true,
                        Token = tokens.Token,
                        RefreshToken = tokens.RefreshToken,
                    };

                    return response;
                }
                else
                {
                    return new AuthResult
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token is not valid"
                        }
                    };
                }

            }
            catch (Exception ex) 
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }

        private async Task<TokenData> GenerateJwtToken(IdentityUser user)
        {
            var _tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpireTimeFrame),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = _tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = $"{GenerateRandomString(32)}_{Guid.NewGuid()}",
                Expiry = DateTime.UtcNow.AddMonths(6),
                IsUsed = false,
                IsRevocked = false,
                Status = 1,
                JwtId = token.Id,

            };

            await _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync();

            var tokenData = new TokenData
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            char[] randomChars = new char[length];
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < length; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(randomChars);
        }
        
    }
}
