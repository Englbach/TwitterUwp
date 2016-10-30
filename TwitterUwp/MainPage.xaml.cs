using System.Collections.Generic;
using TwitterUwp.Control;
using TwitterUwp.Model;
using TwitterUwp.Views;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TwitterUwp.ViewModel;
using TwitterUwp.Interfaces;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TwitterUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static MainPage Current = null;
        Tools tools = new Tools();
        GetTweetVM Timeline = new GetTweetVM();

        public List<NavMenuItem> NavTopMenu { get; } = new List<NavMenuItem>(new[]
        {
            new NavMenuItem() {name="Home", symbol=Symbol.Home, destPage=typeof(MasterPages), arguments=typeof(HomePages) },
            new NavMenuItem() {name="Notification",symbol=Symbol.Mail,destPage=typeof(MasterPages),arguments=typeof(NotificationsPages) },
            new NavMenuItem() {name="Messages", symbol=Symbol.Message,destPage=typeof(MasterPages),arguments=typeof(MessagesPages) },

        });

        public List<NavMenuItem> NavBotMenu { get; } = new List<NavMenuItem>(new[]
        {
            new NavMenuItem() {name="Tweet", symbol=Symbol.Comment,destPage=typeof(MasterPages),arguments=typeof(TweetPages) },
            new NavMenuItem() {name="Search",symbol=Symbol.Find,destPage=typeof(MasterPages), arguments=typeof(SearchPages) }
        });

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            //Timeline.GetTweet();

            if (SharedState.Authorizer == null)
            {
                tools.TwitterAuthorizer();
            }
            else
            {
                return;
            }
        }

        public Frame AppFrame => AppShellFrame;

        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.BackRequested(ref handled);
            e.Handled = handled;
        }
        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current frame so that we can inspect the app back stack.

            if (this.AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (this.AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                this.AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavTopMenu_ItemInvoked(object sender, ListViewItem e)
        {

            NavBotMenuList.SelectedIndex = -1;
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(e);
            if (item != null)
            {
                AppFrame.Navigate(typeof(MasterPages), item.arguments);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavTopMenu_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) => UpdateAutomationName<NavMenuItem>(args, ((NavMenuItem)args.Item).name);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavBotMenuList_ItemInvoked(object sender, ListViewItem e)
        {
            NavTopMenuList.SelectedIndex = -1;
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(e);
            if (item != null)
            {
                AppFrame.Navigate(typeof(MasterPages), item.arguments);
            }
        }

        private void NavBotMenuList_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) => UpdateAutomationName<NavMenuItem>(args, ((NavMenuItem)args.Item).name);

        #endregion

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        private void UpdateAutomationName<T>(ContainerContentChangingEventArgs args, string value)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is T)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, value);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        /// 
        public Rect TogglePaneButtonRect { get; set; }
        public event TypedEventHandler<MainPage, Rect> TogglePaneButtonChanged;
        private void CheckTogglePaneButonSizeChanged()
        {
            TogglePaneButtonRect = RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay
                ? TogglePanelButton.TransformToVisual(this).TransformBounds(new Rect(0, 0, TogglePanelButton.ActualWidth, TogglePanelButton.ActualHeight))
                : new Rect();
            TogglePaneButtonChanged?.Invoke(this, this.TogglePaneButtonRect);
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => CheckTogglePaneButonSizeChanged();

        private void AppShellFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShellFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Control_Loaded;
            }
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Control_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(SharedState.Authorizer==null)
            {
                tools.TwitterAuthorizer();
            }
            else
            {
                return;
            }
        }
    }
}
