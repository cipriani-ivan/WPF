namespace InterviewAssessment.Model
{
    public class Entity
{
    public Entity() { }

    public Entity(int id, string name, int x, int y)
    {
        Id = id;
        Name = name;
        X = x;
        Y = y;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public int X { get; set; }

    public int Y { get; set; }
}
}
