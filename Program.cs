var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

List<Order> repo = 
[
  new Order(1,new(2023,11,11),"123","132","123","123","132","123")   
];


app.MapGet("/", () => repo);
app.MapGet("/create", ([AsParameters]Order dto) => repo.Add(dto));
app.MapGet("/update", ([AsParameters] UpdateOrderDTO dto) =>
{
    var order = repo.Find(x => x.Number == dto.Number);
    if (order == null)
        return;
    if (dto.Status != order.Status && dto.Status != "");

    if (dto.DescProblem != "")
        order.DescProblem = dto.DescProblem;
    if (dto.Master != "")
        order.Master = dto.Master;

    int CompliteCount() => repo.FindAll(x =>  x.Status == "Завершено").Count;

    Dictionary<string, int> GetProblemTypeStat() =>
    repo.GroupBy(x => x.DescProblem).Select(x => x.EndDate.Value.DayNumber - x.StartDate.DayNumber).Sum() / CompliteCount();

    
        
});
app.Run();

record class UpdateOrderDTO(int Number, string DescProblem, string Master, string Status);

class Order(int number, DateOnly startDate, string typeEquipmet, string model, string descProblem, string fIO, string numbPhone, string status)
{
    public int Number { get; set; } = number;
    public DateOnly StartDate { get; set; } = startDate;
    public string TypeEquipmet { get; set; } = typeEquipmet;
    public string Model { get; set; } = model;
    public string DescProblem { get; set; } = descProblem;
    public string FIO { get; set; } = fIO;
    public string NumbPhone { get; set; } = numbPhone;
    public string Status { get; set; } = status;
    public string? Master {  get; set; } = "Не назначен";
    public DateOnly? EndDate { get; set; } = null;
}


