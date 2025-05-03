using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Dal.Entities;

public class GamePlatform
{
    public Guid GameId { get; set; }
    public Guid PlatformId { get; set; }

    public Game Game { get; set; }
    public Platform Platform { get; set; }
}
