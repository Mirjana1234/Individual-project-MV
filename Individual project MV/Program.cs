using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace IndividualProjectMV
{
    public class Program
    {
        static List<Todo> todoList = new List<Todo>();
        static string filePath = "todolist.json";

        public static void Main(string[] args)
        {
            LoadTodoList();

            while (true)
            {
                Console.Clear();
                int pendingTasks = todoList.Count(t => !t.IsDone);
                int doneTasks = todoList.Count(t => t.IsDone);

                Console.WriteLine(">> Welcome to ToDoLy");
                Console.WriteLine($">> You have {pendingTasks} tasks todo and {doneTasks} tasks are done!");
                Console.WriteLine(">> Pick an option:");
                Console.WriteLine(">> (1) Show Task List (by date or project)");
                Console.WriteLine(">> (2) Add New Task");
                Console.WriteLine(">> (3) Edit Task (update, mark as done, remove)");
                Console.WriteLine(">> (4) Save and Quit");
                Console.Write(">> Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowTaskListMenu();
                        break;
                    case "2":
                        AddTodo();
                        break;
                    case "3":
                        EditTaskMenu();
                        break;
                    case "4":
                        SaveTodoList();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void ShowTaskListMenu()
        {
            Console.Clear();
            Console.WriteLine("Show Task List");
            Console.WriteLine("1. Sort by Date");
            Console.WriteLine("2. Sort by Project");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewTodos(sortBy: "date");
                    break;
                case "2":
                    ViewTodos(sortBy: "project");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        static void AddTodo()
        {
            Console.Clear();
            Console.WriteLine("Add a new Todo");

            Console.Write("Enter todo title: ");
            string title = Console.ReadLine();

            Console.Write("Enter todo description: ");
            string description = Console.ReadLine();

            Console.Write("Enter due date (yyyy-mm-dd): ");
            DateTime dueDate;
            while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
            {
                Console.Write("Invalid date format. Please enter due date (yyyy-mm-dd): ");
            }

            Console.Write("Enter project name: ");
            string project = Console.ReadLine();

            int id = todoList.Count + 1;
            todoList.Add(new Todo { Id = id, Title = title, Description = description, DueDate = dueDate, Project = project, IsDone = false });
            Console.WriteLine("Todo added successfully.");
        }

        static void ViewTodos(string sortBy = "")
        {
            Console.Clear();
            Console.WriteLine("Todo List:");

            IEnumerable<Todo> sortedList = todoList;
            if (sortBy == "date")
            {
                sortedList = todoList.OrderBy(t => t.DueDate);
            }
            else if (sortBy == "project")
            {
                sortedList = todoList.OrderBy(t => t.Project);
            }

            if (!sortedList.Any())
            {
                Console.WriteLine("No todos available.");
            }
            else
            {
                foreach (var todo in sortedList)
                {
                    string status = todo.IsDone ? "Done" : "Pending";
                    Console.WriteLine($"Id: {todo.Id}, Title: {todo.Title}, Description: {todo.Description}, Due Date: {todo.DueDate:yyyy-MM-dd}, Project: {todo.Project}, Status: {status}");
                }
            }
        }

        static void EditTaskMenu()
        {
            Console.Clear();
            Console.WriteLine("Edit a Todo");
            Console.WriteLine("1. Update Todo");
            Console.WriteLine("2. Mark Todo as Done");
            Console.WriteLine("3. Remove Todo");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    EditTodo();
                    break;
                case "2":
                    MarkTodoAsDone();
                    break;
                case "3":
                    RemoveTodo();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        static void EditTodo()
        {
            Console.Clear();
            Console.WriteLine("Edit a Todo");

            Console.Write("Enter the Id of the todo to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var todo = todoList.Find(t => t.Id == id);
                if (todo != null)
                {
                    Console.Write("Enter new title (leave blank to keep current): ");
                    string title = Console.ReadLine();
                    if (!string.IsNullOrEmpty(title))
                    {
                        todo.Title = title;
                    }

                    Console.Write("Enter new description (leave blank to keep current): ");
                    string description = Console.ReadLine();
                    if (!string.IsNullOrEmpty(description))
                    {
                        todo.Description = description;
                    }

                    Console.Write("Enter new due date (yyyy-mm-dd, leave blank to keep current): ");
                    string dueDateInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(dueDateInput) && DateTime.TryParse(dueDateInput, out DateTime dueDate))
                    {
                        todo.DueDate = dueDate;
                    }

                    Console.Write("Enter new project name (leave blank to keep current): ");
                    string project = Console.ReadLine();
                    if (!string.IsNullOrEmpty(project))
                    {
                        todo.Project = project;
                    }

                    Console.WriteLine("Todo updated successfully.");
                }
                else
                {
                    Console.WriteLine("Todo not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id. Please try again.");
            }
        }

        static void MarkTodoAsDone()
        {
            Console.Clear();
            Console.WriteLine("Mark a Todo as Done");

            Console.Write("Enter the Id of the todo to mark as done: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var todo = todoList.Find(t => t.Id == id);
                if (todo != null)
                {
                    todo.IsDone = true;
                    Console.WriteLine("Todo marked as done successfully.");
                }
                else
                {
                    Console.WriteLine("Todo not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id. Please try again.");
            }
        }

        static void RemoveTodo()
        {
            Console.Clear();
            Console.WriteLine("Remove a Todo");

            Console.Write("Enter the Id of the todo to remove: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var todo = todoList.Find(t => t.Id == id);
                if (todo != null)
                {
                    todoList.Remove(todo);
                    Console.WriteLine("Todo removed successfully.");
                }
                else
                {
                    Console.WriteLine("Todo not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id. Please try again.");
            }
        }

        static void SaveTodoList()
        {
            string json = JsonSerializer.Serialize(todoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("Todo list saved successfully.");
        }

        static void LoadTodoList()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                todoList = JsonSerializer.Deserialize<List<Todo>>(json);
                Console.WriteLine("Todo list loaded successfully.");
            }
        }
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Project { get; set; }
        public bool IsDone { get; set; }
    }
}
