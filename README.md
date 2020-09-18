# How to bind the ListView data using RESTful API service in Xamarin.Forms (SfListView)

The Xamarin.Forms [SfListView](https://help.syncfusion.com/xamarin/listview/overview) supports to populate the fetched data from REST services. Refer Xamarin.Forms document about consuming RESTful web service before reading this [article](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/consuming/rest).

You can also refer the following article.

https://www.syncfusion.com/kb/11932/how-to-bind-the-listview-data-using-restful-api-service-in-xamarin-forms-sflistview

Please refer to the following steps to implement SfListView with REST services,

**STEP 1: Create Xamarin.Forms application with SfListView**

Define **SfListView** bound to the ViewModel collection.

``` xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:ListViewXamarin"
             xmlns:syncfusion="clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms"
             x:Class="ListViewXamarin.MainPage">
    <ContentPage.BindingContext>
        <local:ContactsViewModel/>
    </ContentPage.BindingContext>
	 <ContentPage.Content>
        <StackLayout>
            <syncfusion:SfListView x:Name="listView" AutoFitMode="Height" ItemSpacing="5" ItemsSource="{Binding UserInfo}">
                <syncfusion:SfListView.ItemTemplate >
                    <DataTemplate>
                        <Frame HasShadow="True" >
                            <Grid>
                                <Grid.RowDefinitions>
                                    <RowDefinition Height="Auto"/>
                                    <RowDefinition Height="Auto"/>
                                    <RowDefinition Height="Auto"/>
                                    <RowDefinition Height="Auto"/>
                                </Grid.RowDefinitions>
                                <Label Grid.Row="0" Text="{Binding [username]}" HorizontalOptions="Start" TextColor="Black" FontSize="16" />
                                <Label Grid.Row="1" Text="{Binding [website]}" HorizontalOptions="Start" TextColor="Black"/>
                                <Label Grid.Row="2" Text="{Binding [phone]}" HorizontalOptions="Start" TextColor="Black" FontSize="16" FontAttributes="Bold"/>
                                <Label Grid.Row="3" Text="{Binding [email]}" HorizontalOptions="Start" TextColor="Black" FontSize="16" FontAttributes="Bold"/>
                            </Grid>
                        </Frame>
                    </DataTemplate>
                </syncfusion:SfListView.ItemTemplate>
            </syncfusion:SfListView>
        </StackLayout>
    </ContentPage.Content>
</ContentPage>
```

**STEP 2:** Install the [ModernHttpClient](https://www.nuget.org/packages/modernhttpclient/) and [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) NuGet packages in the shared code project.

**STEP 3: Retrieve data from the REST service**

Define [HTTPClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync?view=netcore-3.1) to retrieve the information from the webservice for a specified url using [GetAsync](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync) method. You can read the retrieved data to string format using[HTTPContent.ReadAsStringAsync](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent.readasstringasync?view=netcore-3.1) method. Using, [JsonConvert.DeserializeObject](https://www.newtonsoft.com/json/help/html/Overload_Newtonsoft_Json_JsonConvert_DeserializeObject.htm) method, convert the JSON string to dynamic .

``` c#
namespace ListViewXamarin
{
    /// <summary>
    /// Implementation of RestService to be displayed.
    /// </summary>
    public class RestService
    { 
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        public dynamic Items { get; private set; }

        public string RestUrl { get; private set;  }

        public RestService()
        {
            RestUrl = "https://jsonplaceholder.typicode.com/users"; // Set your REST API url here
            client = new HttpClient();
        }

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
    }
}
```
**STEP 4: Populate data to collection**

In the ViewModel class, initialize the **RestService** and populate data by invoking the **GetDataAsync** method and set to the collection which bound to the **SfListView**.

```c#
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListViewXamarin
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        private dynamic userInfo;

        public static RestService DataServices { get; private set; }

        public dynamic UserInfo
        {
            get { return userInfo; }
            set
            {
                userInfo = value;
                RaisedOnPropertyChanged("UserInfo");
            }
        }

        public ContactsViewModel()
        {
            DataServices = new RestService();

            //Gets data from REST service and set it to the ItemsSource collection
            RetrieveDataAsync();
        }

        public async void RetrieveDataAsync()
        {
            UserInfo = await DataServices.GetDataAsync();
        }
    }
}
```
