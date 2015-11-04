using SNDotNetSDK.Configuration;
using SNDotNetSDK.Service;
using SNDotNetSDK.ServiceImpl;

namespace SNDotNetSDK
{
    public class CudaSign
    {
        private Config config;

        public CudaSign(IConfig config)
        {
            this.config = (Config)config;
            InitServices();
        }

        private void InitServices()
        {
            UserService = new UserService(config);
            AuthenticationService = new OAuth2TokenService(config);
            DocumentService = new DocumentService(config);
        }

        public IUserService UserService { get; private set; }

        public IAuthenticationService AuthenticationService { get; private set; }

        public IDocumentService DocumentService { get; private set; }

    }
}
