using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListViewXamarin
{
    /// <summary>
    /// Implementation of RestService to be displayed.
    /// </summary>
    public class RestService
    {
        #region Fields 

        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        #endregion

        #region Properties 

        public dynamic Items { get; private set; }

        public string RestUrl { get; private set;  }
        #endregion

        #region Constructor
        public RestService()
        {
            RestUrl = "https://jsonplaceholder.typicode.com/users"; // Set your REST API url here
            client = new HttpClient();
        }

        #endregion

        #region Methods

        public async Task<dynamic> GetDataAsync()
        {
            try
            {
                //Sends request to retrieve data from the web service for the specified Uri
                var response = await client.GetAsync(RestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(); //Returns the response as JSON string
                    Items = JsonConvert.DeserializeObject<dynamic>(content); //Converts JSON string to dynamic
                }
            }
            catch (Exception ex)
            {
                 Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return Items;
        }
        #endregion
    }
}
