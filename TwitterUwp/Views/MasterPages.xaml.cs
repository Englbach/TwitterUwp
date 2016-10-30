using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitterUwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterPages : Page
    {
        public MasterPages()
        {
            this.InitializeComponent();

            DetailFrame.Navigating += DetailFrame_Navigating;
            DetailFrame.Loaded += DetailFrame_Loaded;

            


        }

        private void DetailFrame_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            DetailFrame.Visibility = Visibility.Visible;
            LoadingProgressBar.Visibility = Visibility.Collapsed;
        }

        private void DetailFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            // In-app navigation, so hide the WebView control and display the progress 
            // animation until the page load is completed.
            DetailFrame.Visibility = Visibility.Collapsed;
            LoadingProgressBar.Visibility = Visibility.Visible;
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewType = e.Parameter as Type;
            if(viewType!=null && MasterFrame.CurrentSourcePageType!=viewType)
            {
                MasterFrame.Navigate(viewType);
            }

            UpdateForVisualState(AdaptiveStates.CurrentState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState=null)
        {
            var isNarrow = newState == NarrowState;

            if(isNarrow && oldState==DefaultState)
            {
                //Resize down to the detail item
                Frame.Navigate(typeof(DetailPages), null, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }
        }
    }
}
