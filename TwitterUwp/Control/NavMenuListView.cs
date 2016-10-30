using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace TwitterUwp.Control
{
    public class NavMenuListView :ListView
    {
        private SplitView SplitViewHost;

        public NavMenuListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.IsItemClickEnabled = true;
            this.ItemClick += NavMenuListView_ItemClick;

            //locate the hosting splitview control
            this.Loaded += (s, a) =>
            {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is Page) && !(parent is SplitView))
                {
                    parent = VisualTreeHelper.GetParent(parent);

                }
                if (parent != null && parent is SplitView)
                {
                    this.SplitViewHost = parent as SplitView;
                    SplitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (sender, args) =>
                    {
                        this.OnPaneToggled();

                    });
                    this.OnPaneToggled();
                }
            };
        }



        /// <summary>
        /// Mark the <paramref name="item"/> as selected and ensures everything else is not.
        /// If the <paramref name="item"/> is null then everything is unselected.
        /// </summary>
        /// <param name="item"></param>
        /// 
        public void SetSelectedItem(ListViewItem item)
        {
            int index = -1;
            if(item!=null)
            {
                index = this.IndexFromContainer(item);
            }

            for(int i=0;i<this.Items.Count;i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if(i!=index)
                {
                    lvi.IsSelected = false;
                }
                else if(i==index)
                {
                    lvi.IsSelected = true;
                }
            }
        }

        private void OnPaneToggled()
        {
            if (this.ItemsPanelRoot == null) return;
            if(this.SplitViewHost.IsPaneOpen)
            {
                this.ItemsPanelRoot.ClearValue(FrameworkElement.WidthProperty);
                this.ItemsPanelRoot.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            }
            else if(this.SplitViewHost.DisplayMode==SplitViewDisplayMode.CompactInline||
                this.SplitViewHost.DisplayMode==SplitViewDisplayMode.CompactOverlay)
            {
                this.ItemsPanelRoot.SetValue(FrameworkElement.WidthProperty, this.SplitViewHost.CompactPaneLength);
                this.ItemsPanelRoot.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //remove the entrance animation on the item containers.
            for(int i=0;i<this.ItemContainerTransitions.Count;i++)
            {
                if(this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Occurs when an item has been selected
        /// </summary>
        /// 
        public event EventHandler<ListViewItem> ItemInvoked;

        private void InvokeItem(object focusedItem)
        {
            this.SetSelectedItem(focusedItem as ListViewItem);
            this.ItemInvoked?.Invoke(this, focusedItem as ListViewItem);
            if(this.SplitViewHost==null|| this.SplitViewHost.IsPaneOpen)
            {
                if(this.SplitViewHost!=null &&
                    (this.SplitViewHost.DisplayMode==SplitViewDisplayMode.CompactOverlay || 
                    this.SplitViewHost.DisplayMode==SplitViewDisplayMode.Overlay))
                {
                    this.SplitViewHost.IsPaneOpen = false;
                }
                if(focusedItem is ListViewItem)
                {
                    ((ListViewItem)focusedItem).Focus(FocusState.Programmatic);
                }
            }
        }

        private void NavMenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //triggered when the item is selected using something other than a keyboard
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }
    }
}
