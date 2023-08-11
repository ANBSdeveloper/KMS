using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Infrastructure.EntityFramework.Seed
{
    public class DbInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            using (var transaction = context.Database.BeginTransaction())
            {
                await (new RolesCreator(context)).CreateAsync();
                await (new PermissionsCreator(context)).CreateAsync();
                await (new UsersCreator(context)).CreateAsync();
                await (new InvestmentSettingsCreator(context)).CreateAsync();
                await (new AppSettingsCreator(context)).CreateAsync();

                await EnsureStore(context);

                transaction.Commit();
            }
        }

        private static async Task EnsureStore(AppDbContext context)
        {
            
            string path = Path.Combine(Environment.CurrentDirectory, "Setup", "Scripts");
            foreach (var file in Directory.GetFiles(path, "*.sql").OrderBy(p => p))
            {
                string fileText = File.ReadAllText(file);
                fileText = fileText.Replace("{", "{{");
                fileText = fileText.Replace("}", "}}");
                var cmds = fileText.Split("GO", StringSplitOptions.None);
                cmds.ToList().ForEach(cmd => context.Database.ExecuteSqlRaw(cmd));

            }
        }
    }
}