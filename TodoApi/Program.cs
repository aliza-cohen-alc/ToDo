using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.AspNetCore.Http;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options => options.UseMySql
(builder.Configuration.GetConnectionString("ToDoDB"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

builder.Services.AddCors(options => options.AddPolicy("Everything", policy =>
{
    policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
}));

var app = builder.Build();
//עובד
app.MapGet("/a", async (ToDoDbContext t) => {return await t.Items.ToListAsync();});

app.MapPut("/{id}",async (HttpContext httpContext,ToDoDbContext t) => {
    int id=int.Parse(httpContext.Request.RouteValues["id"].ToString());
        var item = await t.Items.FindAsync(id);
        if (item != null) {
            // קריאה לגוף הבקשה כדי לקבל את הנתונים החדשים
            var updatedItem = await httpContext.Request.ReadFromJsonAsync<Item>();

            if (updatedItem != null) {
                // עדכון השדות של הפריט
                item.IsComplete = updatedItem.IsComplete; // דוגמה לשדה
                await t.SaveChangesAsync();
                return Results.Ok(item);
            }
            return Results.BadRequest("Invalid item data.");
        }
        return Results.NotFound();
});

app.MapGet("/", () => "Hello World!");

app.MapPost("/", async (HttpContext httpContext, ToDoDbContext t) => {
    var itemJson = await httpContext.Request.ReadFromJsonAsync<Item>();
    if (itemJson != null)
    {
        t.Items.Add(itemJson);
        await t.SaveChangesAsync();
        return Results.Created($"/{itemJson.Id}", itemJson);
    }
    return Results.BadRequest("Invalid item data.");
});

//עובד
app.MapDelete("/{id}", (HttpContext httpContext, ToDoDbContext t) =>{
    int id=int.Parse(httpContext.Request.RouteValues["id"].ToString());
    var entity = t.Items.Find(id); // מצא את היישות לפי ה-ID
    if (entity != null)
    {
        t.Items.Remove(entity); // מחק את היישות
        t.SaveChanges(); // שמור את השינויים
    }
 return entity;
 });
app.UseCors("Everything");
app.Run();
