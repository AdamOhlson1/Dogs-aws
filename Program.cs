using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var dogs = new List<Dog>
{
    new Dog { Id = 1, Name = "Buddy", Breed = "Labrador" },
    new Dog { Id = 2, Name = "Max", Breed = "German Shepherd" },
    new Dog { Id = 3, Name = "Enzo", Breed = "Labrador" },
    new Dog { Id = 4, Name = "Peter", Breed = "Golden Retriver" },
    new Dog { Id = 5, Name = "John", Breed = "Tax" }
};

app.MapGet("/", () => GetDogs());

app.MapGet("/dogs/{id}", (int id) => GetDogById(id));

app.MapPost("/dogs", (HttpContext context) => AddDog(context));

app.MapDelete("/dogs/{id}", (int id) => DeleteDogById(id));

app.Run();

string GetDogs()
{
    return JsonSerializer.Serialize(dogs);
}

string GetDogById(int id)
{
    var dog = dogs.FirstOrDefault(d => d.Id == id);
    if (dog != null)
    {
        return JsonSerializer.Serialize(dog);
    }
    else
    {
        return "Hund hittades inte.";
    }
}

string AddDog(HttpContext context)
{
    try
    {
        using (var reader = new StreamReader(context.Request.Body))
        {
            var requestBody = reader.ReadToEnd();
            var newDog = JsonSerializer.Deserialize<Dog>(requestBody);
            newDog.Id = dogs.Count + 1;
            dogs.Add(newDog);
            return JsonSerializer.Serialize(newDog);
        }
    }
    catch (Exception ex)
    {
        return $"Fel vid läsning av inkommande data: {ex.Message}";
    }
}

string DeleteDogById(int id)
{
    var dogToRemove = dogs.FirstOrDefault(d => d.Id == id);
    if (dogToRemove != null)
    {
        dogs.Remove(dogToRemove);
        return $"Hund med ID {id} raderad.";
    }
    else
    {
        return "Hund hittades inte.";
    }
}

class Dog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
}