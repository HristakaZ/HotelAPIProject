# HotelAPIProject
A Hotel Project(Internal Hotel System) using REST API &amp; .NET Core 3.1.0.



Used Technologies:
`.NET Core Web API 2.`;\
`.NET Core 3.1.0.`;\
`Entity Framework Core 3.1.0.`;\
`Identity`;\
`Newtonsoft Json`;\
`jQuery Validate`.
# Skeleton of the project
The whole solution is made of three projects :
* `DataStructure` - Contains all of the Models (Employee, Position, Guest, Room, RoomType, Reservation & RoomReservation(bridge table for many-many)
relationship). The Employee and Position respectively inherit the IdentityUser and IdentityRole(in order to extend their functionality and make use of
Identity). Whenever a new Employee is created, a new User is created, and whenever a new Position is created, a new Role is created. In some of the
models we can see that the ```csharp[JsonIgnore]``` attribute is added, that is made in order to avoid a reference loop whenever the json data is displayed(whenever
we get the response).
* `DataAccess` - Contains the migrations added to the project, the Repositories which make CRUD operations linked to the 
models, the ApplicationDbContext which inherits from IdentityDbContext to make use of Identity and the Unit Of Work which only consists of a save method. 
Dependency injection is implemented in the project for the repositories, as well as for the services & mappers mentioned below.
* `Hotel API Project` - Here we can find the API Controllers, DTOs (Data Transfer Objects), Services, ViewModels, Extension Methods(Custom Attributes)
and Mappers. As mentioned in the `DataAccess` section, the mappers & services are made via dependency injection through interfaces, not relying on a
concrete implementation of business logic.
# Functionality of the project
* A basic CRUD internal system for a Hotel. A .NET Core 3.1.0 REST API is made and is consumed through a .NET Core 3.1.0. web application. The web
application consumes the REST API through ajax in the views and HttpClient where needed(mostly in the scaffolded Identity items).
A registered user(employee) can make reservations as well as other entities such as guests, rooms, specify the room types and create roles(positions). 
Whenever a new user registers, a new user is created in the database. And, as mentioned above, one role equals one position. This way, the project 
makes use of the identity system.
* Only an admin can edit other employees'(users') info.
* There are certain specific entries in the database(which are mentioned in the Seed section below), which make some of the functionalities possible. 
Such as - when deleting an Employee, his reservations remain intact, but the reservation's employee is changed to the employee with name "NoEmployee".
Whenever a guest is deleted, his reservations remain intact, but the reservation's guest is changed to the guest with name "No Guest!".
If a position is deleted, the employees that have this position now have the position with name "No Position!".
These actions are made as to prevent bugs when displaying null values.
* Seed Data: 
		`Employee(User): Username - "Admin" Password - "Admin1/", Position - Admin`\
		`Employee(User): Username - "NoEmployee"`\
		`Position(Role): Name - "Admin"`\
		`Position(Role): Name - "No Position!"`\
		`Guest: Name - "No Guest!"`
* Specific actions made in the project: As mentioned in the `DataStructure` section, the [JsonIgnore] attribute is added to avoid reference loops entirely.\
Also, in the Startup class, ConfigureServices method we can see this configuration:
 ```csharp
    .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
```
<!--at lines 86 & 87. This is made in order to avoid reference loop handling when SERIALIZING a json object(for example when sending a request with the 
`HttpClient` class).-->
* `Authentication & Authorization` - JWT Authentication and Authorization are used for this project. When using the [Authorize] attribute, the server
checks if the given token(which - in the client app - is stored in an HttpOnly Secure Cookie on the client side) is valid, if it is, the certain
operation could be done, if not, 401 Unauthorized is returned as a status code from the Web API and respectively the Login view in the client app.
The employee(which inherits from IdentityUser) is passed when generating the JWT. The position of the employee(which inherits from IdentityRole) is
passed as a claim when generating the JWT. Such claims are stored in the appsettings.json file(alongside the issuer & audience) and are obtained from
there. Whenever the token expires(5 minutes in the project's case), 401 Unauthorized is returned from the Web API and respectively the Login view 
in the client app. In both scenarios(if we are sending a request directly from a proxy client such as Postman/Fiddler - or if we are using the web
app - a new token will be requested). Of course, we could force emptying the JWT cookie on the client side, by manually clicking the Logout button.
If we want to test the JWT authentication from a proxy client such as Postman/Fiddler, we need to send a post request to /api/Authentication with keys 
"userName": "some username value", "normalPassword": "some unhashed(normal) password value".
*XSS protection: Whenever we receive the info(at the Get request), the list of entities is html encoded in order to avoid XSS attacks. This
operation is not done at the Post request, in order to save the info in the database as plain text, and not as encoded html. Example from the Employee
Controller:
```csharp
/*encoding(against xss) at the get request, so as to store the entity column in its plain form in the database*/
            if (employees != null)
            {
                employees.ForEach(x =>
                {
                    if (x != null)
                    {
                        string encodedEmployeeUserName = htmlEncoder.Encode(x.UserName);
                        x.UserName = encodedEmployeeUserName;
                        if (x.Position != null)
                        {
                            string encodedEmployeePosition = htmlEncoder.Encode(x.Position.Name);
                            x.Position.Name = encodedEmployeePosition;
                        }
                        if (x.Position == null)
                        {
                            x.Position = positions.Where(x => x.Name == "No Position!").FirstOrDefault();
                        }
                    }
                });
            }
```
* XSRF/CSRF protection: The REST API routes are protected with a ```csharp[ValidateAntiForgeryToken]``` attribute, which prevents a malicious user of 
making this attack. In the web application - a CSRF token is stored in a cookie and is used in the views(front end) to provide extra protection(sent
with an ajax request). The given below configurations are made in the Startup.cs ConfigureServices method: 
```csharp
	services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });
``` 
#### notice the header name, in the views, we are respectively sending it with that key("X-XSRF-TOKEN") for each web api method that requires the [ValidateAntiForgeryToken] attribute!
And respectively in the Configure method:
```csharp
	app.Use(next => context =>
            {
                string path = context.Request.Path.Value;
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                     new CookieOptions()
                     {
                         HttpOnly = false,
                         Secure = true
                     }); // set false if not using SSL });
                   return next(context);
            });
```
Also, if we want to send the XSRF token as a header from a proxy client such as Postman/Fiddler, we have to make a GET request to the XSRF Controller,
(and more specifically to the /api/XSRF route), obtain the XSRF token and header name, and put the header name as key & the XSRF TOKEN as value in the 
Authorization headers tab.
* CORS configuration:
In the ConfigureServices of the Startup.cs:
```csharp
	services.AddCors();
```
And respectively in the Configure method of the Startup.cs:
```csharp
	app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
``` 
We are giving free access to the endpoints, because this is not an official project, but in a real project we will have some restrictions, which
will most likely be part of a policy, and in the above given code, the policy will be used.
* Validation of the entities:
In the web application - we have front end validation, which comes from jquery validate.
On the back end - on the web api we have certain validations(made with Extension Methods/Custom DataAnnotations) - for example checking if the current
date isn't bigger than the end date(for a reservation).
On the create - we stop the employee from posting the data, but on put(update) - we let him update the entity, but if he didn't change anything,
nothing will be changed as an end result.
If an error occurs in the CRUD process, it will be displayed in an alert box.
Certain validation messages are also displayed near the input where the validation error occured(using `jQuery Validate`) - for example if we have an
error near a textbox and the data isn't valid, an error message near it will be displayed, containing the needed information for the given employee to
fix his input to be valid.
# - END OF DOCUMENTATION -