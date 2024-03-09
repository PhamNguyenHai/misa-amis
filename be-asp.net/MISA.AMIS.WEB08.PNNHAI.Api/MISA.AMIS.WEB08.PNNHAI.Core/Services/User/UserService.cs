using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UAParser;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class UserService : BaseService<User, UserModel, UserDto, UserCreateDto, UserUpdateDto>, IUserService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        protected readonly IUserRepository _userRepository;
        protected readonly ITokenRepository _tokenRepository;
        private readonly ILoginLogRepository _loginLogRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public UserService(IConfiguration configuration, IUserRepository userRepository, ITokenRepository tokenRepository, 
            ILoginLogRepository loginLogRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            : base(userRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _loginLogRepository = loginLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods
        public async Task<LoginedUserInfor> LoginAsync(UserLoginDto userLoginInfor)
        {
            // Kiểm tra thông tin đăng nhập có chính xác không
            var userToLogin = await _userRepository.FindUserByLoginInforAsync(userLoginInfor.EmailOrPhoneNumber, 
                userLoginInfor.Password);

            if(userToLogin == null)
            {
                throw new NotFoundException(Core.Resources.AppResource.LoginInforInvalidError);
            }
            else
            {
                // Generate token
                // Set thời gian hết hạn
                int jwtExpireTime = int.Parse(_configuration["AppSettings:JWTExpireHours"]);
                int refreshTokenExpireTime = int.Parse(_configuration["AppSettings:RefreshTokenExpireDays"]);

                DateTime jwtExpirationTime = DateTime.Now.AddMinutes(jwtExpireTime);
                DateTime refreshExpirationTime = DateTime.Now.AddDays(refreshTokenExpireTime);

                // Thời gian hết hạn cho access token (Thời gian tính theo local time)
                var accessToken = GenerateJwtToken(userToLogin, jwtExpirationTime);
                // Thời gian hết hạn cho refresh token (Thời gian tính theo local time)
                var refreshToken = GenerateRefreshToken();

                // Lấy thông tin thiết bị login
                var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgent);
                string deviceName = clientInfo.Device.Family; // Lấy tên thiết bị
                string operatingSystem = clientInfo.OS.Family; // Lấy hệ điều hành

                // Thông tin về login log đăng nhập
                var loginLog = new LoginLog()
                {
                    LoginId = Guid.NewGuid(),
                    MacAddress = GetMacAddress(),
                    IpAddress = GetIpAddress(),
                    OperatingSystem = operatingSystem,
                    DeviceName = deviceName,
                    DeviceType = DeviceType.Desktop,
                    UserId = userToLogin.UserId
                };

                // Thông tin về token đăng nhập
                var loginToken = new Token()
                {
                    TokenId = Guid.NewGuid(),
                    AccessToken = accessToken,
                    ExpirationDate = jwtExpirationTime,
                    RefreshToken = refreshToken,
                    RefreshTokenExpirationDate = refreshExpirationTime,
                    LoginId = loginLog.LoginId,
                };

                try
                {
                    // Tiến hành lưu thông tin vào db
                    _unitOfWork.BeginTransaction();

                    // Thêm nhật kí đăng nhập hiện tại vào db
                    await _loginLogRepository.InsertLoginLogAsync(loginLog);

                    // Thêm dữ liệu token hiện tại vào db
                    await _tokenRepository.InsertTokenAsync(loginToken);

                    // Commit
                    _unitOfWork.Commit();
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }

                // Token trả về
                var loginInfor = new LoginedUserInfor()
                {
                    AccessToken = accessToken,
                    UserId = userToLogin.UserId,
                    UserName = userToLogin.FullName,
                    UserRole = userToLogin.Role,
                };

                // Set cookie vào máy khách
                SetCookieToClient(_httpContextAccessor.HttpContext.Request, "refreshToken", refreshToken, refreshExpirationTime);
                return loginInfor;
            }
        }

        public async Task LogoutAsync()
        {
            // Lấy ra refresh token ở cookie client
            var refreshInTokenCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            // Check 1: Check xem có cookie gửi lên có chứa refresh token không
            if (string.IsNullOrEmpty(refreshInTokenCookieValue))
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenNotPostedError);
            }

            // Check 2: Check xem refresh token có trong db không
            var storedToken = await _tokenRepository.FindTokenByRefreshTokenAsync(refreshInTokenCookieValue);
            if (storedToken == null)
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenNotExistedError);
            }

            // Check 3: Check token đã bị thu hồi hay chưa
            if (storedToken.IsRevoked == true)
            {
                throw new ValidateException(Core.Resources.AppResource.TokenRevokedOrExpiredError);
            }

            // Check 4: Check refresh token đã hết hạn chưa
            if (storedToken.RefreshTokenExpirationDate < DateTime.Now)
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenExpiredError);
            }

            // Nếu thỏa mãn
            //      -> Thêm trường logout date ở LoginLog trong db thành hiện tại để đánh dấu đã logout
            //      -> Thu hồi token
            //      -> Xóa cookie máy khách

            try
            {
                // Mở transaction
                _unitOfWork.BeginTransaction();

                // Thu hồi token trong db
                await _tokenRepository.RevokeTokenByIdAsync(storedToken.TokenId);

                // Cập nhật thời gian đăng xuất là local time hiện tại
                await _loginLogRepository.ChangeLogoutDateByLoginIdAsync(storedToken.LoginId, DateTime.Now);

                // Commit
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            // Xóa cookie máy khách
            SetCookieToClient(_httpContextAccessor.HttpContext.Request, "refreshToken", "", DateTime.Now.AddDays(-1));
        }

        public async Task<string> RefreshTokenAsync()
        {
            // Check token
            // Check 1: Check xem access token và refresh token có tồn tại ở máy khách không
            string accessTokenInHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            // Lấy ra refresh token ở cookie client
            var refreshInTokenCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            string accessToken = "";

            // Kiểm tra xem có cookie gửi lên có chứa refresh token không
            if (string.IsNullOrEmpty(refreshInTokenCookieValue))
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenNotPostedError);
            }
            // Kiểm tra xem trong header có access token không
            if (string.IsNullOrEmpty(accessTokenInHeader))
            {
                throw new ValidateException(Core.Resources.AppResource.AccessTokenNotPostedError);
            }

            // Lấy access token bằng cách bỏ Bearer đi
            if (accessTokenInHeader.StartsWith("Bearer"))
            {
                accessToken = accessTokenInHeader.Replace("Bearer ", "");
            }

            // Check 2: Check xem access token có hợp lệ không
            ValidatedTokenResult? validatedTokenResult = ValidateJwtToken(accessToken);
            if (validatedTokenResult == null)
            {
                throw new ValidateException(Core.Resources.AppResource.TokenPostedInvalidError);
            }

            // Check 3: Check xem thuật toán mã hóa của access token có thỏa mãn không
            if (validatedTokenResult.SecurityToken is JwtSecurityToken jwtSecurityToken)
            {
                var algorithm = jwtSecurityToken.Header.Alg;
                if (!string.Equals(algorithm, SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ValidateException(Core.Resources.AppResource.TokenPostedInvalidError);
                }
            }

            // Check 4: Check xem accessToken có expire không
            // Ra số -> cần chuyển về thời gian
            long.TryParse(validatedTokenResult.ClaimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value, 
                out long expireDateTime);
            var expireDate = ConvertUnixTimeToDateTime(expireDateTime);
            if(expireDate > DateTime.Now)
            {
                throw new ValidateException(Core.Resources.AppResource.AccessTokenNotExpiredCantRefreshError);
            }

            // Check 5: Check xem refresh token có trong db không
            var storedToken = await _tokenRepository.FindTokenByRefreshTokenAsync(refreshInTokenCookieValue);
            if (storedToken == null)
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenNotExistedError);
            }

            // Check 6: Check xem access token và refresh token có phải là 1 cặp trong phiên làm việc trước không
            // Nhằm tránh trường hợp lấy access token cũ để refresh
            if(storedToken.AccessToken != accessToken)
            {
                throw new ValidateException(Core.Resources.AppResource.AccessTokenInvalidError);
            }

            // Check 7: Check token đã bị thu hồi hay chưa
            if (storedToken.IsRevoked == true)
            {
                throw new ValidateException(Core.Resources.AppResource.TokenRevokedOrExpiredError);
            }

            // Check 8: Check refresh token đã hết hạn chưa
            if (storedToken.RefreshTokenExpirationDate < DateTime.Now)
            {
                throw new ValidateException(Core.Resources.AppResource.RefreshTokenExpiredError);
            }


            // Đã qua các check -> Token hợp lệ -> Refresh

            // Generate access và refresh token mới
            Guid.TryParse(validatedTokenResult.ClaimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value, out Guid userId);
            var userModelToRefreshToken = await _userRepository.GetByIdAsync(userId);
            if (userModelToRefreshToken == null)
            {
                throw new NotFoundException(Core.Resources.AppResource.UserNotExistedError);
            }
            User userToRefreshToken = _mapper.Map<User>(userModelToRefreshToken);

            // Generate token mới
            // Set thời gian hết hạn cho access token mới
            int newJwtExpireTime = int.Parse(_configuration["AppSettings:JWTExpireHours"]);
            int newRefreshTokenExpireTime = int.Parse(_configuration["AppSettings:RefreshTokenExpireDays"]);

            // Thời gian hết hạn của access token
            DateTime newJwtExpirationTime = DateTime.Now.AddMinutes(newJwtExpireTime);
            // Thời gian hết hạn cho refresh token mới (Thời gian tính theo local time)
            DateTime newRefreshExpirationTime = DateTime.Now.AddDays(newRefreshTokenExpireTime);
            var newAccessToken = GenerateJwtToken(userToRefreshToken, newJwtExpirationTime);
            var newRefreshToken = GenerateRefreshToken();

            // Thông tin về token đăng nhập
            var loginToken = new Token()
            {
                TokenId = Guid.NewGuid(),
                AccessToken = newAccessToken,
                ExpirationDate = newJwtExpirationTime,
                RefreshToken = newRefreshToken,
                RefreshTokenExpirationDate = newRefreshExpirationTime,
                LoginId = storedToken.LoginId,
            };

            try
            {
                // Mở transaction
                _unitOfWork.BeginTransaction();

                // Thu hồi token trong db
                await _tokenRepository.RevokeTokenByIdAsync(storedToken.TokenId);

                // Thêm dữ liệu token hiện tại vào db
                await _tokenRepository.InsertTokenAsync(loginToken);

                // Commit
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

            // setCookie vào máy khách
            SetCookieToClient(_httpContextAccessor.HttpContext.Request, "refreshToken", newRefreshToken, newRefreshExpirationTime);

            // Trả về access token mới
            return newAccessToken;
        }

        public async Task ChangePasswordAsync(Guid id, UserPasswordChangeDto userPasswordChange)
        {
            // Check id có tồn tại không nếu ko -> exception
            await _userRepository.GetByIdAsync(id);

            // Check xem mật khẩu cũ có hợp lệ không
            var isPasswordMatched = await _userRepository.IsPasswordMatched(id, userPasswordChange.CurrentPassword);
            if (!isPasswordMatched)
            {
                throw new ValidateException(Core.Resources.AppResource.CurrentPasswordPostedNotMatchedError);
            }

            // Check xem mật khẩu muốn đổi có khác mật khẩu hiện tại không
            if (userPasswordChange.CurrentPassword == userPasswordChange.ChangePassword)
            {
                throw new ValidateException(Core.Resources.AppResource.NewPasswordMatchedCurrentPasswordError);
            }

            // Nếu thỏa mãn thì thực hiện đổi mật khẩu
            await _userRepository.ChangePasswordAsync(id, userPasswordChange.ChangePassword);
        }

        public async Task ResetPassword(Guid id)
        {
            // Check id có tồn tại không nếu ko -> exception
            await _userRepository.GetByIdAsync(id);

            // Nếu có tồn tại
            // Pass mặc định: Abc@123
            var passwordDefault = _configuration["AppSettings:ResetedPasswordDefault"];
            await _userRepository.ChangePasswordAsync(id, passwordDefault);
        }

        /// <summary>
        /// Hàm để lấy thông tin nhật kí đăng nhập, đăng xuất của người dùng
        /// </summary>
        /// <param name="userId">Id người dùng cần lấy thông tin</param>
        /// <returns>Chuỗi token sinh ra</returns>
        /// Author: PNNHai
        /// Date:
        public async Task<IEnumerable<LoginLogDto>> GetUserLoginLogsById(Guid userId)
        {
            var loginLogs = await _loginLogRepository.GetUserLoginLogsById(userId);

            var loginLogDtos = _mapper.Map<IEnumerable<LoginLogDto>>(loginLogs);
            return loginLogDtos;
        }

        /// <summary>
        /// Hàm để sinh jwt token
        /// </summary>
        /// <param name="user">User xác thực</param>
        /// <returns>Chuỗi token sinh ra</returns>
        /// Author: PNNHai
        /// Date:
        private string GenerateJwtToken(User user, DateTime jwtExpirationTime)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            // Lấy khóa bí mật
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);

            var creadentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), 
                SecurityAlgorithms.HmacSha512);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("fullName", user.FullName),
                    new Claim(ClaimTypes.Role, ((int)user.Role).ToString()),

                }),
                Expires = jwtExpirationTime,
                SigningCredentials = creadentials,
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var tokenString = jwtTokenHandler.WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// Hàm để sinh refresh token
        /// </summary>
        /// <returns>Chuỗi refresh token</returns>
        /// Author: PNNHai
        /// Date:
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Hàm lấy địa chỉ MAC của thiết bị
        /// </summary>
        /// <returns>Địa chỉ MAC</returns>
        /// Author: PNNHai
        /// Date:
        private string GetMacAddress()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string macAddress = string.Empty;

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = networkInterface.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddress;
        }

        /// <summary>
        /// Hàm lấy địa chỉ IP của thiết bị
        /// </summary>
        /// <returns>Địa chỉ IP</returns>
        /// Author: PNNHai
        /// Date:
        private string GetIpAddress()
        {
            string ipAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Lấy địa chỉ IP của card mạng không ảo và có trạng thái hoạt động
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        // Lấy địa chỉ IPv4
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.Address.ToString();
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ipAddress))
                    break;
            }

            return ipAddress;
        }

        /// <summary>
        /// Hàm thực hiện convert giờ utc từ dạng long -> date time utc
        /// </summary>
        /// <param name="expireDate">Giờ utc dạng long</param>
        /// <returns>Giờ local time dạng date time</returns>
        /// Author: PNNHai
        /// Date
        private DateTime ConvertUnixTimeToDateTime(long expireDate)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime expirationDateTime = unixEpoch.AddSeconds(expireDate);

            // Chuyển đổi thành giờ local time
            DateTime expDateTime = expirationDateTime.ToLocalTime();
            return expDateTime;
        }

        /// <summary>
        /// Hàm thực hiện set cookie cho máy khách
        /// </summary>
        /// <param name="name">Tên key cookie cần set</param>
        /// <param name="value">Giá trị cookie cần set</param>
        /// <param name="dateTimeOffset">Thời gian hết hạn</param>
        /// Author: PNNHai
        /// Date
        private void SetCookieToClient(HttpRequest request, string name, string value, DateTimeOffset dateTimeOffset)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = dateTimeOffset,
                HttpOnly = true,
                Secure = true, // (Optional) Set to true if the cookie should only be sent over HTTPS
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value, cookieOptions);
        }

        /// <summary>
        /// Hàm thực hiện validate jwt token có hợp lệ không
        /// </summary>
        /// <param name="token">jwt token</param>
        /// <returns>kết quả sau khi validate nếu hợp lệ nếu không trả về null</returns>
        /// Author: PNNHai
        /// Date
        private ValidatedTokenResult? ValidateJwtToken(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                // Lấy khóa bí mật
                var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);

                var tokenValidateParam = new TokenValidationParameters
                {
                    // Tự cấp token ko sử dụng của các bên cấp token
                    ValidateLifetime = false,      // Không kiểm tra token hết hạn (ko vào exception để bóc tách token ra)
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                };

                var tokenInVerification = jwtTokenHandler.ValidateToken(token, tokenValidateParam, out SecurityToken validatedToken);
                var validatedTokenResult = new ValidatedTokenResult()
                {
                    ClaimsPrincipal = tokenInVerification,
                    SecurityToken = validatedToken,
                };

                return validatedTokenResult;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            
        }

        // ============================================ VALIDATE NGHIỆP VỤ =================================================
        public override async Task ValidateForInserting(UserCreateDto entityCreateDto)
        {
            // Check email chưa tồn tại ko -> nếu đã tồn tại -> exception
            await CheckUserNotExistedByEmail(entityCreateDto.Email);

            // Check sđt chưa tồn tại không -> nếu đã tồn tại -> exception
            await CheckUserNotExistedByPhoneNumber(entityCreateDto.PhoneNumber);

        }

        public override async Task ValidateForUpdating(Guid id, UserUpdateDto entityUpdateDto)
        {
            // Check xem id có tồn tại không. Nếu ko tồn tại -> exception
            var employeeExist = await GetByIdAsync(id);

            // Validate cập nhật email.
            // Kiểm tra email muốn cập nhật có thay đổi so với ban đầu không. Nếu đã thay đổi -> kiểm tra
            // xem nó có được cập nhật sang email chưa tồn tại không (nếu có throw exception)
            if (employeeExist.Email != entityUpdateDto.Email)
            {
                await CheckUserNotExistedByEmail(entityUpdateDto.Email);
            }

            // Validate cập nhật sđt
            // Kiểm tra SĐT muốn cập nhật có thay đổi so với ban đầu không. Nếu đã thay đổi -> kiểm tra
            // xem nó có được cập nhật sang SĐT chưa tồn tại không (nếu có throw exception)
            if (employeeExist.PhoneNumber != entityUpdateDto.PhoneNumber)
            {
                await CheckUserNotExistedByPhoneNumber(entityUpdateDto.PhoneNumber);
            }
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra xem Email chưa tồn tại không
        /// </summary>
        /// <param name="email">Email cần check</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">Tồn tại người dùng có email như đã nhập vào</exception>
        /// Author: PNNHai
        /// Date
        private async Task CheckUserNotExistedByEmail(string email)
        {
            var userExistedByEmail = await _userRepository.FindUserByEmail(email);

            if (userExistedByEmail != null)
            {
                throw new ValidateException(Core.Resources.AppResource.EmailExistedError);
            }
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra xem SĐT chưa tồn tại không
        /// </summary>
        /// <param name="phoneNumber">SĐT cần check</param>
        /// <exception cref="ValidateException">Tồn tại người dùng có sđt như đã nhập</exception>
        /// Author: PNNHai
        /// Date
        private async Task CheckUserNotExistedByPhoneNumber(string phoneNumber)
        {
            var userExistedByPhoneNumber = await _userRepository.FindUserByPhoneNumber(phoneNumber);

            if (userExistedByPhoneNumber != null)
            {
                throw new ValidateException(Core.Resources.AppResource.PhoneNumberExistedError);
            }
        }
        #endregion
    }
}