namespace GameStore.Bll.Dto_s;

public class GenreCreateDto
{
    public string Name { get; set; }
    public Guid? ParentGenreId { get; set; }
}
