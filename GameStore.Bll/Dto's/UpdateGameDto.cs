using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Bll.Dto_s;

public class UpdateGameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public List<Guid> GenreIds { get; set; }
    public List<Guid> PlatformIds { get; set; }
}

