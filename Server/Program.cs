using Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

//builder.Services
//    .AddDbContext<ChatDbContext>()
//    .AddGraphQL(sp =>
//        SchemaBuilder.New()
//            .AddServices(sp)
//            .AddQueryType(d => d.Name("Query"))
//            .AddType<PersonQueries>()
//            .AddMutationType(d => d.Name("Mutation"))
//            .AddType<MessageMutations>()
//            .AddType<PersonMutations>()
//            .AddType<UserMutations>()
//            .AddSubscriptionType(d => d.Name("Subscription"))
//            .AddType<MessageSubscriptions>()
//            .AddType<PersonSubscriptions>()
//            .AddType<MessageExtension>()
//            .AddType<PersonExtension>()
//            .AddAuthorizeDirectiveType()
//            .BindClrType<string, StringType>()
//            .BindClrType<Guid, IdType>()
//            .Create(),
//        new QueryExecutionOptions { ForceSerialExecution = true });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();

