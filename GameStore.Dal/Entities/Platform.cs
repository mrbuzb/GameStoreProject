using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Dal.Entities;

public class Platform
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public List<GamePlatform> GamePlatforms { get; set; }
}
