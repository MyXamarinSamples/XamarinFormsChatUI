﻿using XamarinFormsChatUI.Model;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsChatUI.Controls
{
    public class ChatBox : ScrollView
    {
        public static BindableProperty ChatProviderProperty = BindableProperty.Create("ChatProvider", typeof(IChatProvider), typeof(ChatBox));
        public static Color BackgroundColour = Color.FromRgb(224, 224, 224);

        private StackLayout _MainLayout = new StackLayout { Orientation = StackOrientation.Vertical };
        private IChatProvider _ChatProvider;
        private bool _IsRefreshing;

        public IChatProvider ChatProvider
        {
            get => _ChatProvider;

            set
            {
                _ChatProvider = value;
                _ChatProvider.CollectionChanged += _ChatProvider_CollectionChanged;

                //TODO: Kick off refresh here?
                _ChatProvider.LoadAllAsync();
            }
        }

        private async void _ChatProvider_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await RefreshAsync();
        }

        public ChatBox()
        {
            BackgroundColor = BackgroundColour;
            Content = _MainLayout;
        }

        private async Task RefreshAsync()
        {
            //TODO: This is not good handling. If a new item is added while refresh is going on, it might be missed. Fixed this
            if (_IsRefreshing)
            {
                return;
            }
            _IsRefreshing = true;

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    _MainLayout.Children.Clear();

                    if (_ChatProvider == null)
                    {
                        return;
                    }

                    //TODO: This is just a dummy indicator for now
                    //_MainLayout.Children.Add(new IndicationNoteItem { BindingContext = new IndicationNote { Text = DateTime.Now.ToString() } });

                    foreach (var item in _ChatProvider)
                    {
                        var message = item as IMessage;
                        var indicationNote = item as IIndicationNote;

                        if (message == null && indicationNote == null)
                        {
                            throw new Exception($"Item type {item.GetType()} not supported");
                        }

                        if (message != null)
                        {
                            ProcessMessage(message);
                        }
                    }
                });
            }
            catch (Exception ex)
            {

            }
            _IsRefreshing = false;
        }

        private void ProcessMessage(IMessage message)
        {
            var isCurrentProfile = message.FromProfile.Equals(ChatProvider.CurrentProfile);

            var messageItem = new MessageItem()
            {
                BindingContext = message,
                HorizontalOptions = isCurrentProfile ? LayoutOptions.End : LayoutOptions.Start
            };
            _MainLayout.Children.Add(messageItem);

        }
    }
}
