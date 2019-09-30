using Discord;
using Discord.Commands;
using DiscordBot.Data;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class WaifuRandomizerModule : ModuleBase<SocketCommandContext>
    {
        private IWriteWaifus _waifuRepo;
        public WaifuRandomizerModule(IWriteWaifus waifuRepo)
        {
            _waifuRepo = waifuRepo;
        }

        [Command("waifu")]
        public async Task RandomWaifuAsync()
        {
            Random rand = new Random();
            int n = rand.Next(waifuList.Count);
            Waifu randomWaifu = waifuList[n];
            _waifuRepo.AddNewWaifu(randomWaifu);
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(randomWaifu.Name)
                .WithThumbnailUrl(randomWaifu.Image)
                .WithDescription(randomWaifu.WittyRemark)
                .WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }


        public static Waifu makotoNiijima = new Waifu
        {
            Name = "Makoto Niijima",
            Image = "https://images-na.ssl-images-amazon.com/images/I/61TBFQyVpiL._SY679_.jpg",
            Anime = "Persona5",
            WittyRemark = "Finsh your homework first!"
        };

        public static Waifu annTakamaki = new Waifu
        {
            Name = "Ann Takamaki",
            Image = "https://media.gamestop.com/i/gamestop/11052006/Persona-5-Ann-Takamaki-Phantom-Thief-Nendoroid-Figure?$zoom$",
            Anime = "Persona4",
            WittyRemark = "Like, omg"
        };

        public static Waifu futabaSakura = new Waifu
        {
            Name = "FutabaSakura",
            Image = "https://images-na.ssl-images-amazon.com/images/I/61ZXoWQ0AXL._SY606_.jpg",
            Anime = "Persona3",
            WittyRemark = "Nerf this!"
        };

        public static List<Waifu> waifuList = new List<Waifu>
        {
            makotoNiijima, annTakamaki, futabaSakura

        };
    }
}