using Microsoft.JSInterop;
using StudentServiceApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace StudentServiceApp.Services
{
    public class StudentService
    {
        private List<Student> students = new List<Student>();
        private int nextId = 1;
        private readonly IJSRuntime js;
        public StudentService(IJSRuntime js)
        {
            this.js = js;
        }
        public async Task InitializeAsync()
        {
            await LoadFromSessionAsync();
        }
        public List<Student> GetAll() => students;
        public Student? GetById(int id) => students.FirstOrDefault(s => s.Id == id);
        public async Task AddAsync(Student student)
        {
            student.Id = nextId++;
            students.Add(student);
            await SaveToSessionAsync();
        }
        public async Task UpdateAsync(Student student)
        {
            var existing = GetById(student.Id);
            if (existing != null)
            {
                existing.FirstName = student.FirstName;
                existing.LastName = student.LastName;
                existing.Email = student.Email;
                existing.Major = student.Major;
                await SaveToSessionAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var student = GetById(id);
            if (student != null)
            {
                students.Remove(student);
                await SaveToSessionAsync();
            }
        }
        private async Task SaveToSessionAsync()
        {
            var json = JsonSerializer.Serialize(students);
            await js.InvokeVoidAsync("sessionStudentService.save", json);
        }
        private async Task LoadFromSessionAsync()
        {
            var json = await js.InvokeAsync<string>("sessionStudentService.load");
            if (!string.IsNullOrEmpty(json))
            {
                var loaded = JsonSerializer.Deserialize<List<Student>>(json);
                if (loaded != null)
                {
                    students = loaded;
                    nextId = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
                }
            }
        }
    }
}
