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
        #region Fields

        private dynamic userInfo;
        #endregion

        #region Properties
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
        #endregion

        #region Constructor

        public ContactsViewModel()
        {
            DataServices = new RestService();

            //Gets data from REST service and set it to the ItemsSource collection
            RetrieveDataAsync();
        }

        #endregion

        #region Method

        public async void RetrieveDataAsync()
        {
            UserInfo = await DataServices.GetDataAsync();
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        #endregion
    }
}
