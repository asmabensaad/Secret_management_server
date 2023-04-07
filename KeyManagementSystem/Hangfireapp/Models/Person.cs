namespace Hangfireapp.Models;

public class Person
{
    public Person(int id, string name, string lastName, string sexe)
    {
        Id = id;
        Name = name;
        this.lastName = lastName;
        this.sexe = sexe;
    }

    public int Id  { get; set; }
    public string Name { get; set; } 
    public string lastName { get; set; }
    public string sexe { get; set; }
}
