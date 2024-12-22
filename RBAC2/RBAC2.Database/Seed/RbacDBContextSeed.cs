using RBAC2.Database.Entities;

namespace RBAC2.Database
{
    [Obsolete(message:"Klasa była na potrzeby testowania z SQLlite")]
    public static class RbacDbContextSeed
    {
        public static void Seed(RbacDbContext context)
        {
            // Sprawdzamy, czy użytkownicy o określonych loginach już istnieją
            var existingUsers = context.Users
                .Select(u => u.Login)
                .ToHashSet();

            var usersToAdd = new List<User>
            {
                new User
                {
                    Login = "domain\\user1",
                    CzyAktywny = true,
                    //CosUser = "Admin",
               
                },
                new User
                {
                    Login = "domain\\user2",
                    CzyAktywny = false,
                    //CosUser = "Developer",
                   
                }
            };

            // Filtrujemy użytkowników, którzy już istnieją
            var newUsers = usersToAdd.Where(u => !existingUsers.Contains(u.Login)).ToList();

            if (newUsers.Any())
            {
                context.Users.AddRange(newUsers);
            }

            // Sprawdzamy, czy istnieją projekty o określonych nazwach
            var existingProjects = context.Projects
                .Select(p => p.Name)
                .ToHashSet();

            var projectsToAdd = new List<Project>
            {
                new Project
                {
                    Name = "Project Alpha",
                    Description = "Main application development"
                },
                new Project
                {
                    Name = "Project Beta",
                    Description = "Beta testing phase"
                }
            };

            // Filtrujemy projekty, które już istnieją
            var newProjects = projectsToAdd.Where(p => !existingProjects.Contains(p.Name)).ToList();

            if (newProjects.Any())
            {
                context.Projects.AddRange(newProjects);
            }

            context.SaveChanges();

            // Dodawanie zadań, jeśli nie istnieją
            var existingTasks = context.Tasks
                .Select(t => t.Title)
                .ToHashSet();

            var tasksToAdd = new List<Tasks>
            {
                new Tasks
                {
                    Title = "Setup project repository",
                    Description = "Create repository on GitHub and configure CI/CD pipeline",
                    IsCompleted = true,
                    CosTask = "Setup",
                    UserId = context.Users.FirstOrDefault(u => u.Login == "domain\\user1")?.UserId ?? 0,
                    ProjectId = context.Projects.FirstOrDefault(p => p.Name == "Project Alpha")?.ProjectId ?? 0
                },
                new Tasks
                {
                    Title = "Develop user authentication",
                    Description = "Implement login functionality",
                    IsCompleted = false,
                    CosTask = "Auth",
                    UserId = context.Users.FirstOrDefault(u => u.Login == "domain\\user2")?.UserId ?? 0,
                    ProjectId = context.Projects.FirstOrDefault(p => p.Name == "Project Alpha")?.ProjectId ?? 0
                },
                new Tasks
                {
                    Title = "Prepare testing scenarios",
                    Description = "Create test cases for beta version",
                    IsCompleted = false,
                    CosTask = "Testing",
                    UserId = context.Users.FirstOrDefault(u => u.Login == "domain\\user2")?.UserId ?? 0,
                    ProjectId = context.Projects.FirstOrDefault(p => p.Name == "Project Beta")?.ProjectId ?? 0
                }
            };

            // Filtrujemy zadania, które już istnieją
            var newTasks = tasksToAdd.Where(t => !existingTasks.Contains(t.Title)).ToList();

            if (newTasks.Any())
            {
                context.Tasks.AddRange(newTasks);
            }

            context.SaveChanges();
        }
    }
}
