using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Bll.Dto_s;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentGenreId { get; set; }
}

