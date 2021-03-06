﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XamarinFormsChatUI.Model;

namespace XamarinFormsChatUISample.Model
{
    public class DummyChatProvider : ObservableCollection<ITextModel>, IChatProvider
    {
        public IProfile CurrentProfile { get; private set; }

        public DummyChatProvider(Profile currentProfile)
        {
            CurrentProfile = currentProfile;
        }

        public async Task LoadAllAsync()
        {
            Add(new Message { FromProfile = CurrentProfile, Text = "Hi.", MessageDate = DateTime.Now });
            Add(new Message { FromProfile = new Profile { EmailAddress = "test@test.com", ProfileName = "Test Guy" }, Text = "Hi. How are you?", MessageDate = DateTime.Now });
            Add(new Message { FromProfile = CurrentProfile, Text = "I'm alright.", MessageDate = DateTime.Now });
            Add(new Message { FromProfile = CurrentProfile, Text = "I'm doing some coding.", MessageDate = DateTime.Now });
        }

        public void SendMessage(IMessage message)
        {
            Add(message);
        }
    }
}
