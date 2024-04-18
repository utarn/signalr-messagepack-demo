using Microsoft.AspNetCore.SignalR;

using TestSignalR.Data;

namespace TestSignalR.Hubs;

public class DataHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly static Dictionary<string, string> ConnectionUser = new Dictionary<string, string>();

    public DataHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddPerson(string name)
    {
        var person = new Person() { Name = name };
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
        return person.Id;
        // await Clients.All.SendAsync("ReceiveData", people);
    }

    public async Task<AddPersonDto> AddPersonDto(AddPersonDto dto)
    {
        var person = new Person() { Name = dto.Name };
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
        // await Clients.All.SendAsync("notification", person);
        for (int i = 0; i < 100; i++)
        {
            await Clients.Groups("ActiveUser").SendAsync("notification", $"{DateTime.Now} New person added! {i}");
        }
        return dto;
    }

    public async Task RegisterUser(string connectionId, string userId)
    {
        ConnectionUser[connectionId] = userId;
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", "You are connected as " + userId);
    }
    
    public async Task RegisterGroup(string connectionId)
    {
        await Groups.AddToGroupAsync(connectionId, "ActiveUser");
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", "You are connected to group ActiveUser");
    }
}

public class AddPersonDto
{
    public string Name { get; set; }
}