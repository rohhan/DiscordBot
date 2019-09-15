using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public enum GuildUserActionEnum
    {
        Unknown = 0,
        Joined,
        Left,
        Kicked,
        Banned
    }
}
