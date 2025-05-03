using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Dal.Entities;

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public string? Description { get; set; }

    public List<GameGenre> GameGenres { get; set; }
    public List<GamePlatform> GamePlatforms { get; set; }
}
