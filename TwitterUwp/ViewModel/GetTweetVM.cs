using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterUwp.Interfaces;

namespace TwitterUwp.ViewModel
{
    public class GetTweetVM
    {
        TwitterContext twitter;

        public ObservableCollection<Status> statusList { get; set; }
        public GetTweetVM()
        {
            statusList = new ObservableCollection<Status>();
            if (SharedState.Authorizer != null)
            {
                twitter = new TwitterContext(SharedState.Authorizer);
                GetTweet();
            }
            else
                return;
                
        }

        public async void GetTweet()
        {
            //base.GetTweet();
            var objectStatus = await (from status in twitter.Status
                                where status.Type == LinqToTwitter.StatusType.Home && status.Count==30
                                select status).ToListAsync();
            foreach(var item in objectStatus)
            {
                statusList.Add(item);
            }
        }
    }
}
