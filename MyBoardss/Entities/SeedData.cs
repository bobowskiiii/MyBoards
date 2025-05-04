// using Bogus;
// using MyBoardss.Entities;
//
// public static class SeedData
// {
//     public static void SeedDatabase(MyBoardsContext dbContext)
//     {
//         Console.WriteLine("Checking if users exist...");
//         if (dbContext.Users.Any())
//             return;
//
//         // Seedowanie użytkowników i adresów
//         var addressFaker = new Faker<Address>()
//             .RuleFor(a => a.Id, f => f.Random.Guid())
//             .RuleFor(a => a.Country, f => f.Address.Country())
//             .RuleFor(a => a.City, f => f.Address.City())
//             .RuleFor(a => a.Street, f => f.Address.StreetAddress())
//             .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());
//
//         var userFaker = new Faker<User>()
//             .RuleFor(u => u.Id, f => f.Random.Guid())
//             .RuleFor(u => u.FullName, f => f.Name.FullName())
//             .RuleFor(u => u.Email, f => f.Internet.Email())
//             .RuleFor(u => u.Address, f => addressFaker.Generate());
//
//         var users = userFaker.Generate(30);
//         dbContext.Users.AddRange(users);
//         dbContext.SaveChanges();
//         dbContext.ChangeTracker.Clear();
//
//         // Seedowanie stanów pracy
//         var workStates = new List<WorkState>
//         {
//             new WorkState { Id = Guid.NewGuid(), State = "To Do" },
//             new WorkState { Id = Guid.NewGuid(), State = "Doing" },
//             new WorkState { Id = Guid.NewGuid(), State = "Done" },
//             new WorkState { Id = Guid.NewGuid(), State = "On Hold" }
//         };
//         dbContext.WorkStates.AddRange(workStates);
//         dbContext.SaveChanges();
//         dbContext.ChangeTracker.Clear();
//
//         // Seedowanie tagów
//         var tags = new List<Tag>
//         {
//             new Tag { Id = 1, Value = "Web" },
//             new Tag { Id = 2, Value = "UI" },
//             new Tag { Id = 3, Value = "Desktop" },
//             new Tag { Id = 4, Value = "API" },
//             new Tag { Id = 5, Value = "Service" }
//         };
//         dbContext.Tags.AddRange(tags);
//         dbContext.SaveChanges();
//         dbContext.ChangeTracker.Clear();
//
//         // Seedowanie epików
//         var epicFaker = new Faker<Epic>()
//             .RuleFor(e => e.Id, (f, e) => f.Random.Guid())
//             .RuleFor(e => e.Area, (f, e) => f.Commerce.Department())
//             .RuleFor(e => e.IterationPath, (f, e) => f.Commerce.ProductName())
//             .RuleFor(e => e.Priority, (f, e) => f.Random.Int(1, 5))
//             .RuleFor(e => e.StartDate, (f, e) => f.Date.Past())
//             .RuleFor(e => e.EndDate, (f, e) => f.Date.Future())
//             .RuleFor(e => e.State, (f, e) => f.PickRandom(workStates))
//             .RuleFor(e => e.AuthorId, (f, e) => f.PickRandom(users).Id);
//
//         var issueFaker = new Faker<Issue>()
//             .RuleFor(i => i.Id, (f, i) => f.Random.Guid())
//             .RuleFor(i => i.Area, (f, i) => f.Commerce.Department())
//             .RuleFor(i => i.IterationPath, (f, i) => f.Commerce.ProductName())
//             .RuleFor(i => i.Priority, (f, i) => f.Random.Int(1, 5))
//             .RuleFor(i => i.Efford, (f, i) => f.Random.Decimal(1, 100))
//             .RuleFor(i => i.State, (f, i) => f.PickRandom(workStates))
//             .RuleFor(i => i.AuthorId, (f, i) => f.PickRandom(users).Id);
//
//         var taskFaker = new Faker<MyBoardss.Entities.Task>()
//             .RuleFor(t => t.Id, (f, t) => f.Random.Guid())
//             .RuleFor(t => t.Area, (f, t) => f.Commerce.Department())
//             .RuleFor(t => t.IterationPath, (f, t) => f.Commerce.ProductName())
//             .RuleFor(t => t.Priority, (f, t) => f.Random.Int(1, 5))
//             .RuleFor(t => t.Activity, (f, t) => f.Hacker.Verb())
//             .RuleFor(t => t.RemaningWork, (f, t) => f.Random.Decimal(1, 50))
//             .RuleFor(t => t.State, (f, t) => f.PickRandom(workStates))
//             .RuleFor(t => t.AuthorId, (f, t) => f.PickRandom(users).Id);
//
//         var epics = epicFaker.Generate(10);
//         var issues = issueFaker.Generate(20);
//         var tasks = taskFaker.Generate(30);
//
//         dbContext.Epics.AddRange(epics);
//         dbContext.Issues.AddRange(issues);
//         dbContext.Tasks.AddRange(tasks);
//         dbContext.SaveChanges();
//         dbContext.ChangeTracker.Clear();
//
//         // Seedowanie komentarzy
//         var allWorkItems = epics.Cast<WorkItem>().Concat(issues).Concat(tasks).ToList();
//
//         var commentFaker = new Faker<Comment>()
//             .RuleFor(c => c.Id, (f, c) => f.Random.Guid())
//             .RuleFor(c => c.Message, (f, c) => f.Lorem.Sentence())
//             .RuleFor(c => c.CreatedDate, (f, c) => f.Date.Past())
//             .RuleFor(c => c.AuthorId, (f, c) => f.PickRandom(users).Id)
//             .RuleFor(c => c.WorkItemId, (f, c) => f.PickRandom(allWorkItems).Id);
//
//         var comments = commentFaker.Generate(50);
//         dbContext.Comments.AddRange(comments);
//         dbContext.SaveChanges();
//         dbContext.ChangeTracker.Clear();
//
//         Console.WriteLine($"Seeded {users.Count} users, {epics.Count} epics, {comments.Count} comments.");
//     }
// }