using Newtonsoft.Json;
using SQLPLUS.Builder.Tags;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SQLPlusExtension.Services
{
    /// <summary>
    /// Enumeration for query SubscriptionTypes
    /// </summary>
    public enum SubscriptionTypes : System.Byte
    {
        /// <summary>
        /// Enumerated value for IndividualCommunityMonthly
        /// </summary>
        IndividualCommunityMonthly = 1,

        /// <summary>
        /// Enumerated value for IndividualProfessionalMonthly
        /// </summary>
        IndividualProfessionalMonthly = 2,

        /// <summary>
        /// Enumerated value for IndividualProfessionalYearly
        /// </summary>
        IndividualProfessionalYearly = 3,

        /// <summary>
        /// Enumerated value for TeamProfessionalMonthly
        /// </summary>
        TeamProfessionalMonthly = 4,

        /// <summary>
        /// Enumerated value for TeamProfessionalYearly
        /// </summary>
        TeamProfessionalYearly = 5
    }

    public enum LoginStatuses
    {
        BadRequest = 0,
        InvalidEmailPassCombo = 1,
        Ok = 2
    }

    public enum SubscriptionStatuses
    {
        NoSubscription = 0,
        BalanceDue = 1,
        UpToDate = 2
    }


    public class Customer
    {
        public int CustomerId { set; get; }
        public string Email { set; get; }
        public DateTime LoginValidTill { set; get; }
        public SubscriptionTypes SubscriptionType { set; get; }
        public LoginStatuses LoginStatus { set; get; }
        public SubscriptionStatuses SubscriptionStatus { set; get; }
        public string Token { set; get; }
        public int RenderCount { set; get; }

        public void SetToken()
        {
            Token = CreateToken();
        }

        private bool TokenIsValid()
        {
            return Token == CreateToken();
        }

        private string CreateToken()
        {
            string hash = $"{CustomerId}{Email}{LoginValidTill}{SubscriptionType}{LoginStatus}{SubscriptionStatus}".GetHashCode().ToString();
            return $"sqlp4v_key_{hash}";
        }

        public static Customer DefaultCustomer()
        {
            return new Customer
            {
                CustomerId = 0,
                Email = null,
                LoginValidTill = DateTime.Now.AddDays(-1),
                SubscriptionType = 0,
                LoginStatus = LoginStatuses.BadRequest,
                SubscriptionStatus = SubscriptionStatuses.NoSubscription
            };
        }

        public bool RequiresAuthentication()
        {
            if(CustomerId == 0)
            {
                return true;
            }

            if(LoginStatus != LoginStatuses.Ok)
            {
                return true;
            }

            if(SubscriptionStatus != SubscriptionStatuses.UpToDate)
            {
                return true;
            }

            return false;
        }

    }

    public class Login
    {
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { set; get; }

        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { set; get; }
    }

    public class RenderTrack
    {
        public int CustomerId { set; get; }
        
        public SubscriptionTypes SubscriptionType { set; get; }

        public int FilesCreated { set; get; }

        public string Token { set; get; }

        public DateTime TrackDate { set; get; }

        public void SetToken()
        {
            Token = CreateToken();
        }

        private bool TokenIsValid()
        {
            return Token == CreateToken();
        }

        private string CreateToken()
        {
            string hash = $"{FilesCreated}{TrackDate}{SubscriptionType}{CustomerId}".GetHashCode().ToString();
            return $"sqlp4v_key_{hash}";
        }

        public static RenderTrack DefaultRenderTrack()
        {
            return new RenderTrack()
            {
                FilesCreated = 0,
                TrackDate = DateTime.Now.AddDays(1),
                SubscriptionType = SubscriptionTypes.IndividualCommunityMonthly
            };
        }

        public void InitializeDate()
        {
            TrackDate = DateTime.Now.AddDays(1);
        }

        public void IncrementCount(int filesCreated)
        {
            FilesCreated = FilesCreated + filesCreated;
        }

        public int FilesRemaining()
        {
            if (TrackDate <= DateTime.Now)
            {
                TrackDate = DateTime.Now.AddDays(1);
                FilesCreated = 0;
            }

            if (SubscriptionType == SubscriptionTypes.IndividualCommunityMonthly)
            {
                int filesRemaing = 100 - FilesCreated;

                if(filesRemaing < 0)
                {
                    return 0;
                }
                return filesRemaing;
            }
            return int.MaxValue;
        }
    }
        
    public class CustomerService
    {
        private const string SQL_PLUS_CUSTOMER_FILE_NAME = "customer";
        private const string SQL_PLUS_RENDER_TRACK_FILE_NAME = "render";
        private const string SQL_PLUS_FOLDER_NAME = "SQLPLUS4";

#if DEBUG
        private const string SQL_PLUS_LOGIN_URL = "https://localhost:7166/api/Auth/Login";
#else
        private const string SQL_PLUS_LOGIN_URL = "https://www.SQLPlus.net/api/Auth/Login";
#endif

        public Customer ExistingCustomerOrDefault()
        {
            if (File.Exists(SQLPlusCustomerPath()))
            {
                string json = File.ReadAllText(SQLPlusCustomerPath());
                return JsonConvert.DeserializeObject<Customer>(json);
            }
            return Customer.DefaultCustomer();
        }

        public void SaveCustomer(Customer customer)
        {
            CreateDirectoryIfNotExists();
            string json = JsonConvert.SerializeObject(customer);
            File.WriteAllText(SQLPlusCustomerPath(), json);
        }

        private string SQLPlusFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SQL_PLUS_FOLDER_NAME);
        }

        private string SQLPlusCustomerPath()
        {
            return Path.Combine(SQLPlusFolderPath(), SQL_PLUS_CUSTOMER_FILE_NAME);
        }
        
        private string SQLPlusRenderTrackPath()
        {
            return Path.Combine(SQLPlusFolderPath(), SQL_PLUS_RENDER_TRACK_FILE_NAME);
        }

        private void CreateDirectoryIfNotExists()
        {
            if (!Directory.Exists(SQLPlusFolderPath()))
            {
                Directory.CreateDirectory(SQLPlusFolderPath());
            }
        }

        public async Task<Customer> Login(string email, string password)
        {
            try
            {
                var client = new HttpClient();
                var result = await client.PostAsync(new Uri(SQL_PLUS_LOGIN_URL), LoginContent(email, password));
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resultJson = await result.Content.ReadAsStringAsync();
                    Customer customer = JsonConvert.DeserializeObject<Customer>(resultJson);
                    return customer;
                }

                return Customer.DefaultCustomer();
            }
            catch(Exception ex)
            {
                return Customer.DefaultCustomer();
            }
        }

        private StringContent LoginContent(string email, string password)
        {
            Login postLogin = new Login() { Email = email, Password = password };
            string json = JsonConvert.SerializeObject(postLogin);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public void DeleteCustomer()
        {
            if(File.Exists(SQLPlusCustomerPath()))
            {
                File.Delete(SQLPlusCustomerPath());
            }
        }

        public RenderTrack ExistingRenderTrackOrDefault()
        {
            if (File.Exists(SQLPlusRenderTrackPath()))
            {
                string json = File.ReadAllText(SQLPlusRenderTrackPath());
                var renderTrack = JsonConvert.DeserializeObject<RenderTrack>(json);
                return renderTrack;
            }
            return RenderTrack.DefaultRenderTrack();
        }

        public void SaveRenderTrack(RenderTrack renderTrack)
        {
            CreateDirectoryIfNotExists();
            renderTrack.SetToken();
            string json = JsonConvert.SerializeObject(renderTrack);
            File.WriteAllText(SQLPlusRenderTrackPath(), json);
        }
    }
}
