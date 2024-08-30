using System.Text.Json;
using TaskLibrary.Models;
using DoneTool.Models;
using DoneTool.Data;

namespace TaskLibrary.Services
{
    public class JsonTaskService
    {
        private readonly DoneToolContext _context;

        public JsonTaskService(DoneToolContext context)
        {
            _context = context;
        }

        public TaskInfo ReadTaskFromJson(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<TaskInfo>(jsonString);
        }

        public void WriteTaskToJson(TaskInfo taskInfo, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(taskInfo, options);
            File.WriteAllText(filePath, jsonString);
        }

        public List<Checks> GetChecksForTask(TaskInfo taskInfo)
        {
            var taskChecklistEntry = _context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID);

            if (taskChecklistEntry != null)
            {
                return GetChecksForExistingTask(taskChecklistEntry);
            }
            else
            {
                return CreateNewRelationships(taskInfo);
            }
        }

        private List<Checks> GetChecksForExistingTask(TaskChecklist taskChecklistEntry)
        {
            var taskChecksIDs = _context.TaskChecklist
                .Where(tc => tc.TaskID == taskChecklistEntry.TaskID)
                .Select(tc => tc.TaskChecksID)
                .ToList();

            var checkIDs = taskChecksIDs.Select(id => _context.TaskChecks
                .Where(c => c.ID == id)
                .Select(c => c.CheckID)
                .FirstOrDefault())
                .Where(checkID => checkID != Guid.Empty)
                .ToList();

            return _context.Checks
                .Where(c => checkIDs.Contains(c.ID))
                .ToList();
        }

        private List<Checks> CreateNewRelationships(TaskInfo taskInfo)
        {
            var taskChecksIDs = _context.TaskChecks
                .Where(tc => tc.TaskType == taskInfo.TaskType)
                .Select(tc => tc.ID)
                .ToList();

            foreach (var taskChecksID in taskChecksIDs)
            {
                var newTaskChecklistEntry = new TaskChecklist
                {
                    ID = Guid.NewGuid(),
                    TaskID = taskInfo.TaskID,
                    TaskChecksID = taskChecksID,
                    Status = DoneTool.Models.TaskStatus.TODO,
                    Guard = taskInfo.Guard
                };

                _context.TaskChecklist.Add(newTaskChecklistEntry);
            }

            _context.SaveChanges();

            return GetChecksForExistingTask(_context.TaskChecklist.FirstOrDefault(tc => tc.TaskID == taskInfo.TaskID));
        }
    }
}
