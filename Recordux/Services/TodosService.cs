using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace Recordux.Services
{
    public class TodosService
    {
        public async Task Save(ImmutableList<string> todos)
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            await using var outputFile = new StreamWriter(Path.Combine(docPath, "todos.txt"));
            foreach (var todo in todos)
            {
                await outputFile.WriteLineAsync(todo);
            }
        }
    }
}
