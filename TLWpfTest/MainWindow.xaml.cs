using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TLWpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TLCode();
        }


        private async void TLCode()
        {
            try
            {

                int api_id = 110553;
                string api_hash = "9f3a9f8bd417e76ce805d394537c3f03";

                string phone = "79850156275";
                string guid = "9A5EF0EA-76A5-49AB-ABEF-F412848A706B";
                string fullPath = Directory.GetCurrentDirectory() + "\\" + guid + ".dat";


                #region Connect

                var client = new TelegramClient(api_id, api_hash, new FileSessionStore(), fullPath);
                bool conect = await client.ConnectAsync();

                //var hash = await client.SendCodeRequestAsync(phone);
                //var code = "51971";

                //var user = await client.MakeAuthAsync(phone, hash, code);

                #endregion

                #region GetCotact

                var absDialogs = (TLDialogs) await client.GetUserDialogsAsync();
                //TLDialogs dialogs = (TLDialogs) absDialogs;

                //var chat = absDialogs.Chats.ToList()
                //    .Where(c => c.GetType() == typeof(TLChannel))
                //    .Cast<TLChannel>()
                //    .FirstOrDefault(c => c.Title == "Йобатряд_Mint");

                var chatWars = absDialogs.Users.ToList()
                    .Where(c => c.GetType() == typeof(TLUser))
                    .Cast<TLUser>()
                    .FirstOrDefault(c => c.FirstName == "Chat Wars");

                #endregion

                #region SendMessage

                //await client.SendTypingAsync(new TLInputPeerUser() { UserId = chatWars.Id });
                Thread.Sleep(300);

                //Using TLInputPeerUser with access hash:
                //try
                //{
                //    await client.SendMessageAsync(new TLInputPeerUser() { UserId = chatWars.Id, AccessHash = chatWars.AccessHash.Value }, "🌲Лес");

                //}
                //catch (Exception e)
                //{
                //    Debug.Print("Using TLInputPeerUser with access hash:");
                //    Debug.Print(e.Message);
                //}

                TLMessagesSlice x = (TLMessagesSlice) await client.GetHistoryAsync(new TLInputPeerUser() { UserId = chatWars.Id, AccessHash = chatWars.AccessHash.Value }, 0, Int32.MaxValue, 100);
                var x_new = x.Messages.ToList().Where(c => c.GetType() == typeof(TLMessage));
                foreach (TLMessage a in x_new)
                {
                    string s = String.Join(";", a.Id.ToString(), a.FromId, a.Message, a.Date);
                }

                Debug.Print("-------------------------");
                #endregion

            }
            catch (Exception e)
            {
                Debug.Print("------Error Message------");
                Debug.Print(e.Message);
                Debug.Print("-------------------------");
                Debug.Print("----------Error----------");
                Debug.Print(e.ToString());
                Debug.Print("-------------------------");
            }

        }
    }
}
