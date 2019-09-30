using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public class Waifu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Anime { get; set; }
        public string WittyRemark { get; set; }
    }
}