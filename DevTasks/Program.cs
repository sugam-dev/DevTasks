using DevTasks;
using DevTasks.Data;
using DevTasks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with the ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Test database connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.CanConnect(); // Test connection
        Console.WriteLine("Database connection successful.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Shows detailed errors in development
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication is enabled
app.UseAuthorization();

app.MapGet("/api/state", async (ApplicationDbContext dbContext) =>
{
    var states = await dbContext.States.ToListAsync();
    return Results.Ok(states);
});

app.MapGet("/api/state/{id}", async (int id, ApplicationDbContext dbContext) =>
{
    var state = await dbContext.States.FindAsync(id);
    return state is not null ? Results.Ok(state) : Results.NotFound();
});

app.MapPost("/api/state", async (HttpRequest request, ApplicationDbContext dbContext) =>
{
    try
    {
        var state = await request.ReadFromJsonAsync<State>();
        if (state == null || string.IsNullOrEmpty(state.Name))
            return Results.BadRequest(new { title = "State name cannot be empty." });

        dbContext.States.Add(state);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/api/state/{state.Id}", state);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred: {ex.Message}");
        return Results.Problem("An error occurred while saving the state.");
    }
});

app.MapPut("/api/state/{id}", async (int id, State updatedState, ApplicationDbContext dbContext) =>
{
    try
    {
        var state = await dbContext.States.FindAsync(id);
        if (state is null) return Results.NotFound();

        state.Name = updatedState.Name;
        dbContext.States.Update(state);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while updating data: {ex.Message}");
        return Results.Problem("An error occurred while updating the state.");
    }
});

app.MapDelete("/api/state/{id}", async (int id, ApplicationDbContext dbContext) =>
{
    try
    {
        var state = await dbContext.States.FindAsync(id);
        if (state is null) return Results.NotFound();

        dbContext.States.Remove(state);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while deleting data: {ex.Message}");
        return Results.Problem("An error occurred while deleting the state.");
    }
});


app.MapGet("/api/district", async (ApplicationDbContext dbContext) =>
{
    var districts = await dbContext.Districts.Include(d => d.State).ToListAsync();
    return Results.Ok(districts.Select(d => new
    {
        d.Id,
        d.Name,
        d.StateId,
        StateName = d.State.Name
    }));
});

app.MapPost("/api/district", async (HttpRequest request, ApplicationDbContext dbContext) =>
{
    try
    {
        var district = await request.ReadFromJsonAsync<District>();
        if (district == null || string.IsNullOrEmpty(district.Name))
            return Results.BadRequest(new { title = "District name cannot be empty." });

        var entity = new District
        {
            Name = district.Name,
            StateId = district.StateId
        };

        dbContext.Districts.Add(entity);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/api/district/{entity.Id}", entity);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred: {ex.Message}");
        return Results.Problem("An error occurred while saving the district.");
    }
});

app.MapPut("/api/district/{id}", async (int id, District updatedDistrict, ApplicationDbContext dbContext) =>
{
    try
    {
        var district = await dbContext.Districts.FindAsync(id);
        if (district is null) return Results.NotFound();

        district.Name = updatedDistrict.Name;
        district.StateId = updatedDistrict.StateId;
        dbContext.Districts.Update(district);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while updating data: {ex.Message}");
        return Results.Problem("An error occurred while updating the district.");
    }
});

app.MapDelete("/api/district/{id}", async (int id, ApplicationDbContext dbContext) =>
{
    try
    {
        var district = await dbContext.Districts.FindAsync(id);
        if (district is null) return Results.NotFound();

        dbContext.Districts.Remove(district);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while deleting data: {ex.Message}");
        return Results.Problem("An error occurred while deleting the district.");
    }
});


app.MapGet("/api/customer", async (ApplicationDbContext dbContext) =>
{
    var customers = await dbContext.Customers
        .Include(c => c.State)
        .Include(c => c.District)
        .ToListAsync();
    return Results.Ok(customers.Select(d => new
    {
        d.Id,
        d.Name,
        d.GenderId,
        d.StateId,
        d.DistrictId,
        StateName = d.State.Name,
        DistrictName = d.District.Name
    }));
});

app.MapGet("/api/customer/{id}", async (int id, ApplicationDbContext dbContext) =>
{
    var customer = await dbContext.Customers.Select(d => new
    {
        d.Id,
        d.Name,
        d.GenderId,
        d.StateId,
        d.DistrictId,
        StateName = d.State.Name,
        DistrictName = d.District.Name
    }).FirstOrDefaultAsync(c => c.Id == id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.MapPost("/api/customer", async (HttpRequest request, ApplicationDbContext dbContext) =>
{
    try
    {
        var customer = await request.ReadFromJsonAsync<Customer>();
        if (customer == null)
            return Results.BadRequest(new { title = "Customer data is missing." });

        // Validate the customer data
        if (string.IsNullOrEmpty(customer.Name) || customer.GenderId <= 0 || customer.StateId <= 0 || customer.DistrictId <= 0)
            return Results.BadRequest(new { title = "Invalid customer data." });

        if (!await dbContext.States.AnyAsync(s => s.Id == customer.StateId))
            return Results.BadRequest(new { title = "Invalid StateId." });

        if (!await dbContext.Districts.AnyAsync(d => d.Id == customer.DistrictId))
            return Results.BadRequest(new { title = "Invalid DistrictId." });

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/api/customers/{customer.Id}", customer);
    }
    catch (Exception ex)
    {
        // Log the exception and return a generic error message
        Console.WriteLine($"Exception occurred: {ex.Message}");
        return Results.Problem("An error occurred while saving the customer.");
    }
});


app.MapPut("/api/customer/{id}", async (int id, Customer updatedCustomer, ApplicationDbContext dbContext) =>
{
    try
    {
        var customer = await dbContext.Customers.FindAsync(updatedCustomer.Id);

        if (customer is null) return Results.NotFound();

        customer.Name = updatedCustomer.Name;
        customer.GenderId = updatedCustomer.GenderId;
        customer.StateId = updatedCustomer.StateId;
        customer.DistrictId = updatedCustomer.DistrictId;

        dbContext.Customers.Update(customer);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while updating data: {ex.Message}");
        return Results.Problem("An error occurred while updating the customer.");
    }
});

app.MapDelete("/api/customer/{id}", async (int id, ApplicationDbContext dbContext) =>
{
    try
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer is null) return Results.NotFound();

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred while deleting data: {ex.Message}");
        return Results.Problem("An error occurred while deleting the customer.");
    }
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
