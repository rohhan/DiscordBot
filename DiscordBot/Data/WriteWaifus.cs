using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot.Models;

namespace DiscordBot.Data
{
    public class WriteWaifus : IWriteWaifus
    {
        private DiscordBotDbContext _context;
        public WriteWaifus(DiscordBotDbContext context)
        {
            _context = context;
        }
        public async Task AddNewWaifu(Waifu waifuToAdd)
        {
            var alreadyExists = _context.Waifus.FirstOrDefault(w => w.Name == w.Name);

            if (alreadyExists != null)
            {
                return;
            }

            var waifu = new Waifu
            {
                Name = waifuToAdd.Name,
                Image = waifuToAdd.Image,
                Anime = waifuToAdd.Anime,
                WittyRemark = waifuToAdd.WittyRemark
            };

            await _context.Waifus.AddAsync(waifu);

            await _context.SaveChangesAsync();
        }
    }
}
