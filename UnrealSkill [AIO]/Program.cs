using System;
using EloBuddy;
using EloBuddy.SDK.Events;
namespace EloBuddy
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Chat.Print(Player.Instance.ChampionName);
            switch (Player.Instance.ChampionName)
            {
                case "Gangplank":
                    new UnrealSkill.Gangplank();
                    break;
                case "Shen":
                    new EloBuddy.Shen();
                    break;
                case "XinZhao":
                    new EloBuddy.XinZhao();
                    break;
                case "Vladimir":
                    new EloBuddy.Vladimir1();
                    break;
                case "Zed":
                    //new EloBuddy.Zed2();
                    break;
                case "Draven":
                    new EloBuddy.Dravvenn();
                    break;
                case "Rengar":
                    //new EloBuddy.Rengar();
                    break;
                case "Katarina":
                    new EloBuddy.Katarina();
                    break;
            }
        }
    }
}
